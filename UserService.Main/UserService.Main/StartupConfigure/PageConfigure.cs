using DatabaseExtension.Configure;

using UserService.Core.Entity;

namespace UserService.Main.StartupConfigure
{
    /// <summary>
    /// Конфигуратор поиска и сортировка
    /// </summary>
    public class PageConfigure : PageConfigurator
    {
        /// <summary>
        /// Конфигуратор поиска и сортировка
        /// </summary>
        public PageConfigure()
        {
            _ = Config.InitConfigProfile<Proto.Audit, Audit>()
                .AddCustomRoute(a => a.Timestamp, "@timestamp");

            _ = Config.InitConfigValueProfile<User>()
                .AddCustomValueRoute(a => a.UserState);

            _ = Config.InitConfigValueProfile<WebHook>()
                .AddCustomValueRoute(a => a.WebHookContractType);

            _ = Config.InitConfigValueProfile<RoleClaim>()
                .AddCustomValueRoute(a => a.PermissionAssert);
        }
    }
}
