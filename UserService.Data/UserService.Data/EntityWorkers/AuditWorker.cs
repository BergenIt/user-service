using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension;
using DatabaseExtension.Translator;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using UserService.Core.AuditPackage;
using UserService.Core.Authorizer;
using UserService.Core.DataInterfaces;
using UserService.Core.Elasticsearch;
using UserService.Core.Entity;
using UserService.Core.JwtGenerator;
using UserService.Core.Models;

namespace UserService.Data.EntityWorkers
{
    public class AuditWorker : IAuditWorker
    {
        private readonly string _ip;

        private readonly TokenMeta _tokenMeta;

        private readonly UserServiceContext _userServiceContext;

        private readonly IElasticsearchWorker _timeseriesInserter;
        private readonly IElasticsearchGetter _timeseriesGetter;

        private readonly IAuditActionGetter _actionGetter;
        private readonly ITranslator _translator;

        public AuditWorker(IAuditActionGetter auditActionGetter, IElasticsearchGetter timeseriesGetter, IElasticsearchWorker timeseriesInserter, IContextManager contextManager, IJwtGenerator jwtGenerator, IHttpContextAccessor httpContextAccessor, ITranslator translator)
        {
            _actionGetter = auditActionGetter;
            _timeseriesGetter = timeseriesGetter;
            _timeseriesInserter = timeseriesInserter;
            _translator = translator;

            _userServiceContext = contextManager.CreateDbContext();

            _ip = httpContextAccessor.HttpContext?.Request.Headers[JwtGenerator.ClientIp];

            StringValues? jwt = httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.Authorization];

            if (!string.IsNullOrWhiteSpace(jwt))
            {
                Serilog.Log.Logger.ForContext<IAuditWorker>().Debug("Parse token {jwt}", jwt);

                _tokenMeta = jwtGenerator.GetTokenData(jwt);
            }
        }

        public Task CreateAudit(AuditCreateCommand auditCreateCommand)
        {
            return CreateAudit(_ip, auditCreateCommand);
        }

        public async Task CreateAudit(string ip, AuditCreateCommand auditCreateCommand)
        {
            if (string.IsNullOrEmpty(auditCreateCommand.UserName) && _tokenMeta is null)
            {
                return;
            }

            string userName = string.IsNullOrEmpty(auditCreateCommand.UserName)
                ? _tokenMeta.UserName
                : auditCreateCommand.UserName;

            string actionDescription = _actionGetter.GetActionText(auditCreateCommand.Action);

            Audit audit = await _userServiceContext
                .Set<User>()
                .Where(user => user.UserName == userName)
                .Select(u => new Audit
                {
                    Subdivision = u.Subdivision.Name,
                    IndexKey = auditCreateCommand.Action,
                    Action = actionDescription,
                    Message = auditCreateCommand.Message,
                    UserName = u.UserName,
                    FullName = u.FullName,
                    IpAddress = ip,
                    Position = u.Position.Name,
                    Timestamp = DateTime.UtcNow,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToArray(),
                })
                .SingleOrDefaultAsync();

            audit ??= new()
            {
                Subdivision = string.Empty,
                IndexKey = auditCreateCommand.Action,
                Action = actionDescription,
                Message = auditCreateCommand.Message,
                UserName = auditCreateCommand.UserName,
                FullName = string.Empty,
                IpAddress = ip,
                Position = string.Empty,
                Timestamp = DateTime.UtcNow,
                Roles = Array.Empty<string>(),
            };

            await _timeseriesInserter.InsertAsync(audit);
        }

        public Task<IPageItems<Audit>> GetAudits(FilterContract filter)
        {
            return _timeseriesGetter.GetEntities<Audit>(filter);
        }

        record SystemSubdivisionSelector(string Name, IEnumerable<string> UserNames);

        public async Task<IPageItems<SystemAuditRecord>> GetSystemAudits(FilterContract filter)
        {
            IAsyncEnumerable<SystemSubdivisionSelector> subdivisions = _userServiceContext
                .Set<Subdivision>()
                .Select(s => new SystemSubdivisionSelector(s.Name, s.Users.Select(u => u.UserName)))
                .AsAsyncEnumerable();

            ConcurrentBag<SystemAuditRecord> collection = new();

            await foreach (SystemSubdivisionSelector systemSubdivision in subdivisions)
            {
                SearchFilter searchFilterAction = new()
                {
                    Value = _translator.GetUserText<IAuthorizer>(nameof(IAuthorizer.LoginAsync)),
                    ColumnName = nameof(Audit.Message)
                };

                SearchFilter searchFilterSubdivision = new()
                {
                    Value = systemSubdivision.Name,
                    ColumnName = nameof(Audit.Subdivision)
                };

                FilterContract filterContract = new()
                {
                    SearchFilters = new List<SearchFilter> { searchFilterAction, searchFilterSubdivision },
                    SortFilters = new List<SortFilter>(),
                    PaginationFilter = new(),
                    TimeFilter = filter.TimeFilter,
                };

                long countSubdivisonLogin = await _timeseriesGetter.GetCountEntities<Audit>(filterContract);

                IEnumerable<Task<IPageItems<ScreenTime>>> tasks = systemSubdivision.UserNames.Select(u =>
                {
                    SearchFilter searchFilterUserName = new()
                    {
                        Value = u,
                        ColumnName = nameof(Audit.UserName)
                    };

                    FilterContract userAllFilter = new()
                    {
                        TimeFilter = filter.TimeFilter,
                        SearchFilters = new List<SearchFilter>() { searchFilterUserName },
                        SortFilters = new List<SortFilter>(),
                        PaginationFilter = new()
                        {
                            PageNumber = 1,
                            PageSize = 9999,
                        },
                    };

                    return _timeseriesGetter.GetEntities<ScreenTime>(userAllFilter, a => a.Duration);
                });

                IPageItems<ScreenTime>[] pageItems = await Task.WhenAll(tasks);

                SystemAuditRecord systemAuditRecord = new()
                {
                    Subdivision = systemSubdivision.Name,
                    CountUsers = systemSubdivision.UserNames.Count(),
                    CountLogin = countSubdivisonLogin,
                    ScreenTime = new(pageItems.SelectMany(i => i.Items.Select(s => s.Duration.Ticks)).Sum()),
                };

                collection.Add(systemAuditRecord);
            }

            IEnumerable<SystemAuditRecord> enumerable = collection
                .Search(filter.SearchFilters);

            int count = enumerable.Count();

            IEnumerable<SystemAuditRecord> systemAuditRecords = enumerable
                .Sort(filter.SortFilters)
                .Paginations(filter.PaginationFilter);

            return new PageItems<SystemAuditRecord>(systemAuditRecords, count);
        }

        private record UserAuditSelector(string UserName, string FullName, string PositionName, string SubdivisionName, IEnumerable<string> RoleNames);
        public async Task<IPageItems<IGrouping<string, SubdivisionAuditRecord>>> GetSubdivisionAudits(IEnumerable<Guid> subdivisionIds, FilterContract filter)
        {
            IAsyncEnumerable<UserAuditSelector> users = _userServiceContext.Set<User>()
                .Where(u => subdivisionIds.Contains(u.SubdivisionId))
                .Select(u => new UserAuditSelector(
                    u.UserName,
                    u.FullName,
                    u.Position.Name,
                    u.Subdivision.Name,
                    u.UserRoles.Select(r => r.Role.Name)
                ))
                .AsAsyncEnumerable();

            ConcurrentBag<SubdivisionAuditRecord> collection = new();

            await foreach (UserAuditSelector user in users)
            {
                SearchFilter searchFilterAction = new()
                {
                    Value = _translator.GetUserText<IAuthorizer>(nameof(IAuthorizer.LoginAsync)),
                    ColumnName = nameof(Audit.Message)
                };

                SearchFilter searchFilterUserName = new()
                {
                    Value = user.UserName,
                    ColumnName = nameof(Audit.UserName)
                };

                FilterContract filterContract = new()
                {
                    SearchFilters = new List<SearchFilter>() { searchFilterAction, searchFilterUserName },
                    SortFilters = new List<SortFilter>(),
                    PaginationFilter = new()
                    {
                        PageNumber = 1,
                        PageSize = 9999,
                    },
                    TimeFilter = filter.TimeFilter
                };

                long countSubdivisonLogin = await _timeseriesGetter.GetCountEntities<Audit>(filterContract);

                FilterContract userAllFilter = new()
                {
                    TimeFilter = filter.TimeFilter,
                    SearchFilters = new List<SearchFilter>() { searchFilterUserName },
                    SortFilters = new List<SortFilter>(),
                    PaginationFilter = new()
                    {
                        PageNumber = 1,
                        PageSize = 9999,
                    },
                };

                IPageItems<Audit> ipAddress = await _timeseriesGetter.GetEntities<Audit>(userAllFilter, a => a.IpAddress);
                IPageItems<ScreenTime> screenTime = await _timeseriesGetter.GetEntities<ScreenTime>(userAllFilter, a => a.Duration);

                SubdivisionAuditRecord subdivisionAudit = new()
                {
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Position = user.PositionName,
                    ConnectionType = user.RoleNames,
                    CountLogin = countSubdivisonLogin,
                    Ips = ipAddress.Items.Select(ip => ip.IpAddress).Distinct().Where(s => !string.IsNullOrWhiteSpace(s)),
                    ScreenTime = new(screenTime.Items.Select(s => s.Duration.Ticks).Sum()),
                    Subdivision = user.SubdivisionName,
                };

                collection.Add(subdivisionAudit);
            }

            IEnumerable<SubdivisionAuditRecord> enumerable = collection
                .Search(filter.SearchFilters);

            int count = enumerable.Count();

            IEnumerable<SubdivisionAuditRecord> subdivisionAuditRecords = enumerable
                .Sort(filter.SortFilters)
                .OrderBy(s => s.Subdivision)
                .Paginations(filter.PaginationFilter);

            return new PageItems<IGrouping<string, SubdivisionAuditRecord>>(subdivisionAuditRecords.GroupBy(s => s.Subdivision), count);
        }

        public async Task<IPageItems<UserAuditRecord>> GetUserAudits(IEnumerable<string> userNames, IEnumerable<Guid> subdivisionIds, FilterContract filter)
        {
            IAsyncEnumerable<UserAuditRecord> userAuditRecords = _userServiceContext
                .Set<User>()
                .Where(u => userNames.Contains(u.UserName) || subdivisionIds.Contains(u.SubdivisionId))
                .Select(u => new UserAuditRecord()
                {
                    RegistredDate = u.RegistredDate,
                    ConnectionType = u.UserRoles.Select(r => r.Role.Name),
                    Email = u.Email,
                    FullName = u.FullName,
                    LastLogin = u.LastLogin,
                    Position = u.Position.Name,
                    UserName = u.UserName,
                    Subdivision = u.Subdivision.Name,
                })
                .AsAsyncEnumerable();

            ConcurrentBag<UserAuditRecord> collection = new();

            await foreach (UserAuditRecord userAuditRecord in userAuditRecords)
            {
                SearchFilter searchFilterAction = new()
                {
                    Value = _translator.GetUserText<IAuthorizer>(nameof(IAuthorizer.LoginAsync)),
                    ColumnName = nameof(Audit.Message)
                };

                SearchFilter searchFilterUserName = new()
                {
                    Value = userAuditRecord.UserName,
                    ColumnName = nameof(Audit.UserName)
                };

                FilterContract filterContract = new()
                {
                    TimeFilter = filter.TimeFilter,
                    SearchFilters = new List<SearchFilter>() { searchFilterAction, searchFilterUserName },
                    SortFilters = new List<SortFilter>(),
                    PaginationFilter = new()
                    {
                        PageNumber = 1,
                        PageSize = 9999,
                    },
                };

                long countLogin = await _timeseriesGetter.GetCountEntities<Audit>(filterContract);

                FilterContract userAllFilter = new()
                {
                    TimeFilter = filter.TimeFilter,
                    SearchFilters = new List<SearchFilter>() { searchFilterUserName },
                    SortFilters = new List<SortFilter>(),
                    PaginationFilter = new()
                    {
                        PageNumber = 1,
                        PageSize = 9999,
                    },
                };

                IPageItems<Audit> ipAddress = await _timeseriesGetter.GetEntities<Audit>(userAllFilter, a => a.IpAddress);
                IPageItems<ScreenTime> screenTime = await _timeseriesGetter.GetEntities<ScreenTime>(userAllFilter, a => a.Duration);

                userAuditRecord.CountLogin = countLogin;
                userAuditRecord.Ips = ipAddress.Items.Select(ip => ip.IpAddress).Distinct().Where(s => !string.IsNullOrWhiteSpace(s));
                userAuditRecord.ScreenTime = new(screenTime.Items.Select(s => s.Duration.Ticks).Sum());

                collection.Add(userAuditRecord);
            }

            IEnumerable<UserAuditRecord> enumerable = collection
                .Search(filter.SearchFilters);

            int count = enumerable.Count();

            IEnumerable<UserAuditRecord> userAudit = enumerable
                .Sort(filter.SortFilters)
                .Paginations(filter.PaginationFilter);

            return new PageItems<UserAuditRecord>(userAudit, count);
        }
    }
}
