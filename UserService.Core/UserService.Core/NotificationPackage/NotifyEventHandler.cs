using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UserService.Core.ContractConfigParser;
using UserService.Core.DataInterfaces;
using UserService.Core.Entity;
using UserService.Core.Models;
using UserService.Core.NotifyEventTypeGetter;
using UserService.Core.SenderInteraces;

namespace UserService.Core.NotificationPackage
{
    public class NotifyEventHandler : INotifyEventHandler
    {
        private readonly INotifyEventTypeGetter _notifyEventTypeGetter;

        private readonly INotificationManager _notificationManager;
        private readonly IContractProfileGetter _contractProfileWorker;
        private readonly IContractConfigParser _contractConfigParser;

        private readonly ISender _emailSender;
        private readonly ISender _websocketSender;
        private readonly ISender _webhookSender;

        public NotifyEventHandler(INotifyEventTypeGetter notifyEventTypeGetter, INotificationManager notificationManager, IContractProfileGetter contractProfileWorker, IContractConfigParser contractConfigParser, IEmailSender emailSender, IWebsocketSender websocketSender, IWebhookSender webhookSender)
        {
            _notifyEventTypeGetter = notifyEventTypeGetter;
            _notificationManager = notificationManager;
            _contractProfileWorker = contractProfileWorker;
            _contractConfigParser = contractConfigParser;
            _emailSender = emailSender;
            _websocketSender = websocketSender;
            _webhookSender = webhookSender;
        }

        public async Task NotifyEventHandle(Notification notification)
        {
            IEnumerable<ContractProfile> contractProfiles = await _contractProfileWorker
                .GetContractProfilesWithWebhooks(notification.NotifyEventType);

            IEnumerable<UserSendView> userSendViews = await _contractProfileWorker
                .GetUsersFromContractProfiles(contractProfiles.Select(c => c.Id), notification.ObjectId);

            await _notificationManager
                .CreateNotification(notification, userSendViews.Select(u => u.UserName));

            IEnumerable<IGrouping<Guid, UserSendView>> userSendViewsGropped = userSendViews.GroupBy(u => u.ContractProfileId);

            IDictionary<Guid, IEnumerable<KeyValuePair<string, string>>> rawMessages = _contractConfigParser.BuildRawStringArray(contractProfiles, notification);

            IDictionary<Guid, string> messages = _contractConfigParser.GetMessageFromContractProfiles(rawMessages, WebHookContractType.StringArray);
            IDictionary<Guid, string> jsons = _contractConfigParser.GetMessageFromContractProfiles(rawMessages, WebHookContractType.Json);

            string subject = _notifyEventTypeGetter.GetTranslatedNotifyEventType(notification.NotifyEventType);

            foreach (IGrouping<Guid, UserSendView> sendViews in userSendViewsGropped)
            {
                string message = messages[sendViews.Key];

                SendCommonMessage(sendViews, TargetNotify.Email, s => s.Email, message, subject, _emailSender);
                SendCommonMessage(sendViews, TargetNotify.Socket, s => s.UserName, message, subject, _websocketSender);

                ContractProfile contractProfile = contractProfiles.Single(p => p.Id == sendViews.Key);

                if (contractProfile.WebHooks.Any())
                {
                    SendWebhookMessage(contractProfile.WebHooks, WebHookContractType.StringArray, message);

                    if (jsons.TryGetValue(contractProfile.Id, out string json))
                    {
                        SendWebhookMessage(contractProfile.WebHooks, WebHookContractType.Json, json);
                    }
                }
            }
        }

        private void SendWebhookMessage(IEnumerable<WebHook> webHooks, WebHookContractType webHookContractType, string message)
        {
            IEnumerable<string> msgUrls = webHooks
                .Where(h => h.WebHookContractType == webHookContractType)
                .Select(h => h.Url);

            if (msgUrls.Any())
            {
                SenderContract senderContract = new(message, msgUrls);
                _webhookSender.Send(senderContract);
            }
        }

        private void SendCommonMessage(IEnumerable<UserSendView> sendViews, TargetNotify targetNotify, Func<UserSendView, string> selector, string message, string subject, ISender sender)
        {
            IEnumerable<string> userNames = sendViews.Where(v => v.TargetNotifies.Contains(targetNotify)).Select(selector);

            if (userNames.Any())
            {
                SenderContract hubSenderContract = new(message, userNames, subject);
                sender.Send(hubSenderContract);
            }
        }
    }
}
