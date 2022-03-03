using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.DataInterfaces;
using UserService.Core.Entity;

namespace UserService.Core.DataPackage.DataWorkers
{
    public class WebhookManager : BasePackOperation, IWebhookManager
    {
        private readonly IDataWorker _dataWorker;
        private readonly IDataGetter _dataGetter;

        public WebhookManager(IDataWorker dataWorker, IDataGetter dataGetter)
        {
            _dataWorker = dataWorker;
            _dataGetter = dataGetter;
        }

        public async Task<WebHook> RemoveWebHook(Guid webHookId)
        {
            WebHook entity = await _dataWorker.RemoveAsync<WebHook>(webHookId);
            await _dataWorker.SaveChangesAsync();

            return entity;
        }

        public async Task<WebHook> AddWebHook(WebHook webHook)
        {
            webHook.ContractProfile = null;

            webHook.Id = Guid.NewGuid();

            _ = await _dataWorker.AddAsync(webHook);
            await _dataWorker.SaveChangesAsync();

            return webHook;
        }

        public async Task<WebHook> UpdateWebHook(WebHook webHook)
        {
            WebHook entity = await _dataGetter.GetSingleEntityAsync<WebHook>(webHook.Id);

            entity.Enable = webHook.Enable;
            entity.Comment = webHook.Comment;
            entity.Name = webHook.Name;
            entity.Url = webHook.Url;

            entity.WebHookContractType = webHook.WebHookContractType;

            _ = _dataWorker.Update(entity);
            await _dataWorker.SaveChangesAsync();

            return entity;
        }

        public Task<IEnumerable<WebHook>> UpdateWebHook(IEnumerable<WebHook> webHooks) =>
            EntityPackOperationAsync<WebHook>(webHooks, UpdateWebHook);

        public Task<IEnumerable<WebHook>> AddWebHook(IEnumerable<WebHook> webHooks) =>
            EntityPackOperationAsync<WebHook>(webHooks, AddWebHook);

        public Task<IEnumerable<WebHook>> RemoveWebHook(IEnumerable<Guid> webHookIds) =>
            EntityPackOperationAsync(webHookIds, RemoveWebHook);

    }
}
