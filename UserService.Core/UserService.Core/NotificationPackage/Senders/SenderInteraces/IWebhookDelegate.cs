using System.Threading.Tasks;

namespace UserService.Core.SenderInteraces
{
    /// <summary>
    /// Дает доступ к http client
    /// </summary>
    public interface IWebhookClientProvider
    {
        /// <summary>
        /// Отправить как json сообщение по url HttpClient-ом
        /// </summary>
        /// <param name="url"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task SendPostAsJsonAsync(string url, string msg);
    }
}
