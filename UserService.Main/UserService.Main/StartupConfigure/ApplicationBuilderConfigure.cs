using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.DependencyInjection;

using UserService.Core;
using UserService.Core.AuditPackage;
using UserService.Core.Elasticsearch;
using UserService.Core.NotifyEventTypeGetter;
using UserService.Data;
using UserService.Main.HealthCheck;

namespace UserService.Main.StartupConfigure
{
    /// <summary>
    /// Методы расширения для настройки IApplicationBuilder
    /// </summary>
    public static class ApplicationBuilderConfigure
    {
        /// <summary>
        /// Получить инстансты контекстов и запустить миграции
        /// </summary>
        /// <param name="app"></param>
        /// <param name="projectOptions"></param>
        /// <returns></returns>
        public static IApplicationBuilder MigrateAsync(this IApplicationBuilder app, ProjectOptions projectOptions)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

            DatabaseContext dbContextPsql = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
            CacheContext dbContextSqlite = serviceScope.ServiceProvider.GetRequiredService<CacheContext>();

            INotifyEventTypeGetter notifyEventTypeGetter = serviceScope.ServiceProvider.GetRequiredService<INotifyEventTypeGetter>();

            dbContextPsql.MigrateAsync(notifyEventTypeGetter, projectOptions).GetAwaiter().GetResult();
            dbContextSqlite.MigrateAsync(notifyEventTypeGetter, projectOptions).GetAwaiter().GetResult();

            IContextManager contextManager = serviceScope.ServiceProvider.GetRequiredService<IContextManager>();

            contextManager.SynchronizeCacheWithPsql().GetAwaiter().GetResult();

            IElasticsearchMigrator elasticsearchMigrator = serviceScope.ServiceProvider.GetRequiredService<IElasticsearchMigrator>();

            IAuditActionGetter auditActionGetter = serviceScope.ServiceProvider.GetRequiredService<IAuditActionGetter>();

            elasticsearchMigrator.MigrateDataStreamAsync<Core.Entity.Audit>(auditActionGetter.GetActions()).GetAwaiter().GetResult();
            elasticsearchMigrator.MigrateDataStreamAsync<Core.Entity.ScreenTime>(new string[] { string.Empty }).GetAwaiter().GetResult();
            elasticsearchMigrator.MigrateNotifyAliasesAsync(notifyEventTypeGetter.GetAllNotifyEventTypes()).GetAwaiter().GetResult();

            return app;
        }

        /// <summary>
        /// Добавить endpoint сокета
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder MapHub(this IApplicationBuilder app) => app.UseEndpoints(endpoints =>
        {
            _ = endpoints.MapHub<Websocket.Websocket>(Websocket.Websocket.MethodName, options =>
            {
                options.Transports = HttpTransportType.ServerSentEvents;
            });
        });

        /// <summary>
        /// Добавить endpoint HealthChecks
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder MapHealthCheck(this IApplicationBuilder app) => app.UseEndpoints(endpoints =>
        {
            _ = endpoints.MapGrpcService<GrpcHealthCheck>();

            _ = endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            });

            _ = endpoints.MapHealthChecks("/health");
        });

        /// <summary>
        /// Добавить Grpc сервисы
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder MapGrpcServices(this IApplicationBuilder app) => app.UseEndpoints(endpoints =>
        {
            _ = endpoints.MapGrpcService<AuthorizeServices>();
            _ = endpoints.MapGrpcService<UserManagerServices>();
            _ = endpoints.MapGrpcService<RoleServices>();
            _ = endpoints.MapGrpcService<SubdivisionServices>();
            _ = endpoints.MapGrpcService<PermissionServices>();
            _ = endpoints.MapGrpcService<PositionServices>();
            _ = endpoints.MapGrpcService<EnumServices>();
            _ = endpoints.MapGrpcService<AuditServices>();
            _ = endpoints.MapGrpcService<NotificationServices>();
            _ = endpoints.MapGrpcService<ContractProfileServices>();
            _ = endpoints.MapGrpcService<RoleNotifySettingServices>();
            _ = endpoints.MapGrpcService<UserNotifySettingServices>();
            _ = endpoints.MapGrpcService<CaptchaServices>();
            _ = endpoints.MapGrpcService<PasswordServices>();
            _ = endpoints.MapGrpcService<ServiceSettingServices>();
            _ = endpoints.MapGrpcService<LifecycleIndexServices>();
            _ = endpoints.MapGrpcService<UserAccessObjectServices>();
            _ = endpoints.MapGrpcService<WebhookServices>();
        });
    }
}
