using System;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.Entity;

namespace UserService.Core.DataInterfaces
{
    public interface IWebhookGetter
    {
        Task<WebHook> GetWebhook(Guid webookId);
        Task<IPageItems<WebHook>> GetWebhooks(Guid contractProfileId, FilterContract filterContract);
    }
}
