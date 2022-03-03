using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UserService.Core.Entity;

namespace UserService.Core.DataInterfaces
{
    public interface IWebhookManager
    {
        Task<IEnumerable<WebHook>> AddWebHook(IEnumerable<WebHook> webHooks);
        Task<WebHook> AddWebHook(WebHook webHook);
        Task<WebHook> RemoveWebHook(Guid webHookId);
        Task<IEnumerable<WebHook>> RemoveWebHook(IEnumerable<Guid> webHookIds);
        Task<IEnumerable<WebHook>> UpdateWebHook(IEnumerable<WebHook> webHooks);
        Task<WebHook> UpdateWebHook(WebHook webHook);
    }
}
