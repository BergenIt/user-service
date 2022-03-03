using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using DatabaseExtension.Translator;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

using MimeKit;

using Novell.Directory.Ldap;

using UserService.Core;
using UserService.Core.AuditPackage;
using UserService.Core.Authorizer;
using UserService.Core.DataInterfaces;
using UserService.Core.DataPackage;
using UserService.Core.Elasticsearch;
using UserService.Core.Entity;
using UserService.Core.SenderInteraces;
using UserService.Core.Senders;
using UserService.Data;
using UserService.Data.Elasticsearch;
using UserService.Main.Ldap;
using UserService.Main.SenderProvider;
using UserService.Main.Websocket;

namespace UserService.Main.StartupConfigure
{
    /// <summary>
    /// Методы расширения для конфигурации DI
    /// </summary>
    public static class ServiceCollectionConfigure
    {
        /// <summary>
        /// Добавить Ldap коннектора
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddLdapConnector(this IServiceCollection services) => services
            .AddTransient<ILdapConnectionFabric, LdapConnectionFabric>()
            .AddTransient<ILdapConnection>(f => f.GetRequiredService<ILdapConnectionFabric>().CreateLdapConnection())
            .AddScoped<ILdapConnector, LdapConnector>();

        /// <summary>
        /// Добавляет балансировщика нагрузки на почту пользователей
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddLoadBalanser(this IServiceCollection services) => services
            .AddSingleton<INotificationLoadBalancer>(f => new NotificationLoadBalancer(f.GetRequiredService<ProjectOptions>().LoadBalanserDelay));

        /// <summary>
        /// Добавить при необходимости протокол для подключения к ad
        /// </summary>
        /// <param name="services"></param>
        /// <param name="projectOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddLdapCert(this IServiceCollection services, ProjectOptions projectOptions)
        {
            if (!string.IsNullOrWhiteSpace(projectOptions.LdapCertPath))
            {
                using X509Store store = new(StoreName.Root, StoreLocation.CurrentUser, OpenFlags.ReadWrite);
                {
                    using X509Certificate2 certificate = new(
                        projectOptions.LdapCertPath,
                        projectOptions.LdapCertPassword,
                        X509KeyStorageFlags.MachineKeySet
                    );

                    store.Add(certificate);
                }
            }

            return services;
        }

        /// <summary>
        ///  Добавление проверки health по http
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHealthCheck(this IServiceCollection services) => services
            .AddHealthChecks()
            .AddDbContextCheck<DatabaseContext>()
            .Services;

        /// <summary>
        /// Добавление базового EntityManager
        /// </summary>
        /// <typeparam name="TBaseEntity"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddEntityManager<TBaseEntity>(this IServiceCollection services) where TBaseEntity : class, IBaseEntity, new() =>
            services.AddScoped<IEntityManager<TBaseEntity>, EntityManager<TBaseEntity>>();

        /// <summary>
        /// Добавление SignalR
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHubOptions(this IServiceCollection services) => services
            .AddScoped<IWebsocket, Websocket.Websocket>()
            .AddSignalR()
            .AddHubOptions<Websocket.Websocket>(options => options.EnableDetailedErrors = true)
            .AddJsonProtocol(options => { options.PayloadSerializerOptions.PropertyNamingPolicy = null; })
            .Services
            .AddSingleton<IUserIdProvider, JwtWebsocketProvider>();

        /// <summary>
        /// Добавление обработчика jwt при подключение по сокету
        /// </summary>
        /// <param name="services"></param>
        /// <param name="projectOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtBearer(this IServiceCollection services, ProjectOptions projectOptions) => services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(projectOptions.IdentitySecret)),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false
                };

                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        StringValues accessToken = context.Request.Query[JwtWebsocketProvider.AccessToken];

                        if (!string.IsNullOrEmpty(accessToken) && context.HttpContext.Request.Path.StartsWithSegments(Websocket.Websocket.MethodName))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            })
            .Services;

        /// <summary>
        /// Добавление политики авторизации при подключение по сокету
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorizationPolicy(this IServiceCollection services) => services.AddAuthorization(options =>
        {
            AuthorizationPolicy authorizationPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, authorizationPolicy);
        });

        /// <summary>
        /// Добавление политики CORS
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services) => services.AddCors(o =>
        {
            o.AddPolicy(Startup.CorsName, builder =>
            {
                _ = builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(host => true)
                    .AllowCredentials()
                    .WithExposedHeaders(HeaderNames.Authorization, Startup.CaptchaCode, HeaderNames.CacheControl);
            });
        });

        /// <summary>
        /// Добавление переводчика
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddTranslator(this IServiceCollection services) => services
            .AddSingleton<ITranslator, Translator>(s => new Translator());

        /// <summary>
        /// Добавляет в DI классы для работы с эластиком
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddElasticClient(this IServiceCollection services) => services
            .AddScoped<Nest.IElasticClient>(p =>
            {
                ProjectOptions projectOptions = p.GetRequiredService<ProjectOptions>();

                System.Uri uri = new(projectOptions.ElasticsearchUri);

                Elasticsearch.Net.SingleNodeConnectionPool connectionPool = new(uri);

                Nest.ConnectionSettings connectionSettings = new(connectionPool);

                connectionSettings = connectionSettings.DisableDirectStreaming();

                if (!string.IsNullOrWhiteSpace(projectOptions.ElasticsearchToken))
                {
                    System.Collections.Specialized.NameValueCollection headers = new();

                    headers.Add(HeaderNames.Authorization, projectOptions.ElasticsearchToken);

                    _ = connectionSettings.GlobalHeaders(headers);
                }

                Nest.ElasticClient elasticClient = new(connectionSettings);

                return elasticClient;
            })
            .AddSingleton<IAuditActionGetter, AuditActionGetter>()
            .AddScoped<IElasticsearchMigrator, ElasticsearchMigrator>()
            .AddScoped<IElasticsearchWorker, ElasticsearchWorker>()
            .AddScoped<IElasticsearchGetter, ElasticsearchGetter>();

        /// <summary>
        /// Добавление Smtp клиента
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSmtpClient(this IServiceCollection services) => services
            .AddScoped<InternetAddress>(f =>
            {
                ProjectOptions projectOptions = f.GetRequiredService<ProjectOptions>();

                return new MailboxAddress(Encoding.UTF8, projectOptions.SmtpSenderName, projectOptions.SmtpSenderAddress);
            })
            .AddSingleton<IMailTransportFabric, MailTransportFabric>();

        /// <summary>
        /// Добавление DbContext (Ef core) и IdentityServer
        /// </summary>
        /// <param name="services"></param>
        /// <param name="projectOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityContext(this IServiceCollection services, ProjectOptions projectOptions)
        {
            _ = services.AddDbContext<CacheContext>(
                optionsAction: o => o.UseSqlite($"Filename={nameof(CacheContext)}"),
                contextLifetime: ServiceLifetime.Scoped,
                optionsLifetime: ServiceLifetime.Singleton
            );

            _ = services.AddDbContext<DatabaseContext>(
                optionsAction: o => o.UseNpgsql(projectOptions.PsqlDbConnectionString),
                contextLifetime: ServiceLifetime.Scoped,
                optionsLifetime: ServiceLifetime.Singleton
            );

            _ = services.AddScoped<IContextManager, ContextManager>();

            _ = services
                .AddIdentity<User, Role>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
