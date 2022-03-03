using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using UserService.Core.Entity;

namespace UserService.Core.NotificationPackage.ContractProfileValidator
{
    public interface INotificationSettingValidator
    {
        ValueTask Validate<TSetting>(TSetting notificationSetting, Expression<Func<TSetting, bool>> userPredicate) where TSetting : NotificationSetting, new();
    }
}
