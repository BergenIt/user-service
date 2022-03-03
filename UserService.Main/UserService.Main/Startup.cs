
using DatabaseExtension;
using DatabaseExtension.Configure;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using UserService.Core;
using UserService.Core.Authorizer;
using UserService.Core.CaptchaGenerator;
using UserService.Core.ContractConfigParser;
using UserService.Core.DataInterfaces;
using UserService.Core.DataPackage.DataWorkers;
using UserService.Core.Elasticsearch;
using UserService.Core.Entity;
using UserService.Core.JwtGenerator;
using UserService.Core.NotificationPackage;
using UserService.Core.NotificationPackage.ContractProfileValidator;
using UserService.Core.NotificationPackage.UserNotificationGetter;
using UserService.Core.NotifyEventTypeGetter;
using UserService.Core.PasswordGenerator;
using UserService.Core.PasswordManager;
using UserService.Core.PolindromHasher;
using UserService.Core.SenderInteraces;
using UserService.Core.Senders;
using UserService.Core.ServiceSettings;
using UserService.Core.UserManager;
using UserService.Data.EntityWorkers;
using UserService.Data.IdentityManagersProvider;
using UserService.Main.Interceptors;
using UserService.Main.StartupConfigure;
using UserService.Main.SenderProvider;

namespace UserService.Main
{
    /// <summary>
    /// Конфигурация сервиса
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Имя CORS политики по умолчанию
        /// </summary>
        public const string CorsName = "CorsPolicy";

        /// <summary>
        /// Имя хедера с кодом валидации капчи
        /// </summary>
        public const string CaptchaCode = "CaptchaCode";

        private readonly IConfiguration _configuration;
        private readonly ProjectOptions _projectOptions;

        /// <summary>
        /// Конфигурация сервиса
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

            JsonConvert.DefaultSettings = () =>
            {
                JsonSerializerSettings settings = new()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Include,
                    DefaultValueHandling = DefaultValueHandling.Include,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                };

                CamelCaseNamingStrategy camelCaseNamingStrategy = new();

                settings.Converters.Add(new StringEnumConverter { NamingStrategy = camelCaseNamingStrategy });

                return settings;
            };

            _projectOptions = new(_configuration);
        }

        /// <summary>
        /// Конфигурация сервиса
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            _ = services
                .AddLdapCert(_projectOptions);

            _ = services
                .AddHealthCheck()
                .AddControllers().Services
                .AddHttpContextAccessor()
                .AddCorsPolicy()
                .AddAuthorizationPolicy()
                .AddJwtBearer(_projectOptions)
                .AddHubOptions()
                .AddGrpc(a => a.Interceptors.Add<ApplicationInterceptor>());

            _ = services
                .AddSingleton(_projectOptions)
                .AddIdentityContext(_projectOptions)
                .AddSmtpClient()
                .AddTranslator()
                .AddLoadBalanser()
                .AddLdapConnector()
                .AddElasticClient();

            _ = services
                .AddScoped<ILifecycleIndexManager, LifecycleIndexManager>()
                .AddSingleton<INotifyEventTypeGetter, NotifyEventTypeGetter>()
                .AddScoped<IContractConfigParser, ContractConfigParser>()
                .AddTransient<IJwtGenerator, JwtGenerator>()
                .AddTransient<ICaptchaGenerator, CaptchaGenerator>()
                .AddTransient<IWebhookClientProvider, WebhookClientProvider>()
                .AddTransient<IPasswordGenerator, PasswordGenerator>()
                .AddTransient<IPasswordHasher, PolinomialHasher>()
                .AddTransient<IPasswordHasher<User>, PasswordHasher>();

            _ = services
                .AddScoped<IEmailSender, EmailSender>()
                .AddScoped<IWebhookSender, WebhookSender>()
                .AddScoped<IWebsocketSender, WebsocketSender>();

            _ = services
                .AddScoped<IIdentityManagersProvider, IdentityManagersProvider>()
                .AddScoped<IDataGetter, DataGetter>()
                .AddScoped<IInternalDataGetter, DataGetter>()
                .AddScoped<IDataWorker, DataWorker>();

            _ = services
                .AddEntityManager<Position>()
                .AddEntityManager<Permission>()
                .AddEntityManager<Subdivision>();

            _ = services
                .AddScoped<INotificationSettingValidator, NotificationSettingValidator>()
                .AddScoped<IContractProfileGetter, ContractProfileGetter>()
                .AddScoped<IRoleClaimManager, RoleClaimManager>()
                .AddScoped<INotificationSettingGetter<UserNotificationSetting>, NotificationSettingGetter<UserNotificationSetting>>()
                .AddScoped<INotificationSettingGetter<RoleNotificationSetting>, NotificationSettingGetter<RoleNotificationSetting>>()
                .AddScoped<IWebhookManager, WebhookManager>()
                .AddScoped<IWebhookGetter, WebhookGetter>()
                .AddScoped<IUserGetter, UserGetter>()
                .AddScoped<IRoleGetter, RoleGetter>()
                .AddScoped<IAccessObjectManager, AccessObjectManager>()
                .AddScoped<IUserAccessGetter, UserAccessGetter>()
                .AddScoped<IPermissionGetter, PermissionGetter>();

            _ = services
                .AddScoped<IServiceSettingManager, ServiceSettingManager>()
                .AddScoped<IPermissionManager, PermissionManager>()
                .AddScoped<IUserManager, UserManager>()
                .AddScoped<IRoleManager, RoleManager>()
                .AddScoped<IAuditWorker, AuditWorker>()
                .AddScoped<INotificationManager, NotificationManager>()
                .AddScoped<INotificationSettingManager<RoleNotificationSetting>, RoleNotificationSettingManager>()
                .AddScoped<INotificationSettingManager<UserNotificationSetting>, UserNotificationSettingManager>()
                .AddScoped<IContractProfileManager, ContractProfileManager>()
                .AddScoped<IContractProfileValidator, ContractProfileValidator>();

            _ = services
                .AddScoped<IUserNotificationGetter, UserNotificationGetter>()
                .AddScoped<IAuthorizer, Authorizer>()
                .AddScoped<IPasswordManager, PasswordManager>()
                .AddScoped<INotifyEventHandler, NotifyEventHandler>();

            _ = services
                .AddAutoMapper(typeof(Startup));
        }

        /// <summary>
        /// Конфигурация сервиса
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
            _ = app
                .UseDeveloperExceptionPage()
                .UseRouting();

            _ = app
                .UseAuthentication()
                .UseAuthorization()
                .UseCors(CorsName);

            _ = app
                .MapHealthCheck()
                .MapHub()
                .MapGrpcServices();

            _ = app
                .UsePageConfigurator(typeof(Startup).Assembly)
                .MigrateAsync(_projectOptions);
        }
    }
}
