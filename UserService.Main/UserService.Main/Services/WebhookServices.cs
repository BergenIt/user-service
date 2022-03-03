using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using DatabaseExtension;

using Grpc.Core;

using UserService.Core.DataInterfaces;
using UserService.Proto;

namespace UserService.Main
{
    /// <summary>
    /// Сервис для настройки вебхуков
    /// </summary>
    public class WebhookServices : WebhookService.WebhookServiceBase
    {
        private readonly IMapper _mapper;

        private readonly IWebhookManager _webhookManager;
        private readonly IWebhookGetter _webhookGetter;

        /// <summary>
        /// Сервис для настройки вебхуков
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="webhookManager"></param>
        /// <param name="webhookGetter"></param>
        public WebhookServices(IMapper mapper, IWebhookManager webhookManager, IWebhookGetter webhookGetter)
        {
            _mapper = mapper;
            _webhookManager = webhookManager;
            _webhookGetter = webhookGetter;
        }

        /// <summary>
        /// Создать настройки вебхуков
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Webhooks> CreateWebhooks(WebhooksCreateCommand request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.WebHook> inputWebhooks = _mapper.Map<IEnumerable<Core.Entity.WebHook>>(request.CreateWebhookList);

            IEnumerable<Core.Entity.WebHook> webHooks = await _webhookManager.AddWebHook(inputWebhooks);

            return new()
            {
                WebhookList = { _mapper.Map<IEnumerable<Proto.Webhook>>(webHooks) },
            };
        }

        /// <summary>
        /// Обновить настройки вебхуков
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Webhooks> UpdateWebhooks(WebhooksUpdateCommand request, ServerCallContext context)
        {
            IEnumerable<Core.Entity.WebHook> inputWebhooks = _mapper.Map<IEnumerable<Core.Entity.WebHook>>(request.UpdateWebhookList);

            IEnumerable<Core.Entity.WebHook> webHooks = await _webhookManager.UpdateWebHook(inputWebhooks);

            return new()
            {
                WebhookList = { _mapper.Map<IEnumerable<Proto.Webhook>>(webHooks) },
            };
        }

        /// <summary>
        /// Удалить настройки вебхуков
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Webhooks> RemoveWebhooks(WebhooksRemoveCommand request, ServerCallContext context)
        {
            IEnumerable<Guid> rmIds = _mapper.Map<IEnumerable<Guid>>(request.RemoveWebhookIds);

            IEnumerable<Core.Entity.WebHook> webHooks = await _webhookManager.RemoveWebHook(rmIds);

            return new()
            {
                WebhookList = { _mapper.Map<IEnumerable<Proto.Webhook>>(webHooks) },
            };
        }

        /// <summary>
        /// Получить настройки вебхука по id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Proto.Webhook> GetWebhookById(WebhookGetRequest request, ServerCallContext context)
        {
            Guid id = Guid.Parse(request.Id);

            Core.Entity.WebHook webHook = await _webhookGetter.GetWebhook(id);

            return _mapper.Map<Proto.Webhook>(webHook);
        }

        /// <summary>
        /// Получить страницу настроек вебхуков по id contractProfile
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<WebhookPage> GetWebhooks(WebhooksGetRequest request, ServerCallContext context)
        {
            FilterContract filterContract = request.Filter.FromProtoFilter<Proto.Webhook, Core.Entity.WebHook>();

            Guid contractProfileId = Guid.Parse(request.ContractProfileId);

            IPageItems<Core.Entity.WebHook> webHooks = await _webhookGetter.GetWebhooks(contractProfileId, filterContract);

            return new()
            {
                WebhookList = { _mapper.Map<IEnumerable<Proto.Webhook>>(webHooks.Items) },
                CountItems = (int)webHooks.CountItems,
            };
        }
    }
}
