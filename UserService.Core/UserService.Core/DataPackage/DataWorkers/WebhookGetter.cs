using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Core.DataPackage.DataWorkers
{
    public class WebhookGetter : IWebhookGetter
    {
        private readonly IDataGetter _dataGetter;

        public WebhookGetter(IDataGetter dataGetter)
        {
            _dataGetter = dataGetter;
        }

        public Task<WebHook> GetWebhook(Guid webookId)
        {
            return _dataGetter.GetSingleEntityAsync<WebHook>(webookId);
        }

        public Task<IPageItems<WebHook>> GetWebhooks(Guid contractProfileId, FilterContract filterContract)
        {
            filterContract.SearchFilters = new List<SearchFilter>(filterContract.SearchFilters)
            {
                new SearchFilter
                {
                    ColumnName = nameof(NotificationSetting.ContractProfileId),
                    Value = contractProfileId.ToString(),
                }
            };

            return _dataGetter.GetPage<WebHook>(filterContract);
        }
    }
}
