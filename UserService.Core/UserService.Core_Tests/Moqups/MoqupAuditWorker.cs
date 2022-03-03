using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;
using UserService.Core.Models;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupAuditWorker : IAuditWorker
    {
        private readonly ObjectFabric _objectFabric = new();

        public Task CreateAudit(AuditCreateCommand audit)
        {
            Assert.IsTrue(!string.IsNullOrWhiteSpace(audit.Action));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(audit.Message));

            return Task.CompletedTask;
        }

        public Task CreateAudit(Guid userId, string ip, AuditCreateCommand audit)
        {
            Assert.IsTrue(userId != default);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(ip));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(audit.Action));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(audit.Message));

            return Task.CompletedTask;
        }

        public Task CreateAudit(string ip, AuditCreateCommand auditCreateCommand)
        {
            Assert.IsTrue(!string.IsNullOrWhiteSpace(ip));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(auditCreateCommand.Action));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(auditCreateCommand.Message));

            return Task.CompletedTask;
        }

        public Task<IPageItems<Audit>> GetAudits(FilterContract filter)
        {
            List<Audit> audits = new();

            for (int i = 0; i < 5; i++)
            {
                Audit audit = _objectFabric.CreateInstance<Audit>(2);
                audits.Add(audit);
            }

            PageItems<Audit> pageItems = new(audits, 20);

            return Task.FromResult(pageItems as IPageItems<Audit>);
        }

        public Task<IPageItems<SystemAuditRecord>> GetSystemAudits(FilterContract filter)
        {
            List<SystemAuditRecord> audits = new();

            for (int i = 0; i < 5; i++)
            {
                SystemAuditRecord audit = _objectFabric.CreateInstance<SystemAuditRecord>(2);
                audits.Add(audit);
            }

            PageItems<SystemAuditRecord> pageItems = new(audits, 20);

            return Task.FromResult(pageItems as IPageItems<SystemAuditRecord>);
        }

        public Task<IPageItems<UserAuditRecord>> GetUserAudits(IEnumerable<string> userNames, IEnumerable<Guid> subdivisionIds, FilterContract filter)
        {
            throw new NotImplementedException();
        }

        Task<IPageItems<IGrouping<string, SubdivisionAuditRecord>>> IAuditWorker.GetSubdivisionAudits(IEnumerable<Guid> subdivisionIds, FilterContract filter)
        {
            throw new NotImplementedException();
        }
    }
}
