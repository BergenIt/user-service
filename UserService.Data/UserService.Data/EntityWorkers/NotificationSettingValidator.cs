using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using UserService.Core.AuditPackage.AuditException;
using UserService.Core.Entity;
using UserService.Core.NotificationPackage.ContractProfileValidator;

namespace UserService.Data.EntityWorkers
{
    public class NotificationSettingValidator : INotificationSettingValidator
    {
        private readonly IInternalDataGetter _dataGetter;

        public NotificationSettingValidator(IInternalDataGetter dataGetter)
        {
            _dataGetter = dataGetter;
        }

        public async ValueTask Validate<TSetting>(TSetting notificationSetting, Expression<Func<TSetting, bool>> userPredicate) where TSetting : NotificationSetting, new()
        {
            if (!notificationSetting.Enable)
            {
                return;
            }

            string eventType = await _dataGetter.GetQueriable<ContractProfile>()
                .Where(p => p.Id == notificationSetting.ContractProfileId)
                .Select(p => p.NotifyEventType)
                .SingleAsync();

            IEnumerable<TargetNotify> existTargetNotifies = await _dataGetter.GetQueriable<TSetting>()
                .Where(s => s.Id != notificationSetting.Id)
                .Where(s => s.Enable)
                .Where(s => s.ContractProfile.NotifyEventType == eventType)
                .Where(userPredicate)
                .Select(s => s.TargetNotifies)
                .ToArrayAsync()
                .ContinueWith(t => t.Result.SelectMany(x => x));

            foreach (TargetNotify targetNotify in notificationSetting.TargetNotifies)
            {
                if (existTargetNotifies.Contains(targetNotify))
                {
                    throw new ValidateNotificationSettingException();
                }
            }
        }
    }
}
