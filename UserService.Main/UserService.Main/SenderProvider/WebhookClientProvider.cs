using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using UserService.Core.SenderInteraces;

namespace UserService.Main.SenderProvider
{
    /// <summary>
    /// Класс, предоставляющий отправителю webhook http клиента
    /// </summary>
    public class WebhookClientProvider : IWebhookClientProvider
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Класс, предоставляющий отправителю webhook http клиента
        /// </summary>
        public WebhookClientProvider()
        {
            _httpClient = GetClient();
        }

        /// <summary>
        /// Отправить сообщение по url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public Task SendPostAsJsonAsync(string url, string msg)
        {
            try
            {
                return _httpClient.PostAsJsonAsync(url, msg);
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.ForContext<IWebhookClientProvider>().Error("Exception while send webhook as json: {ex}", ex);

                return Task.CompletedTask;
            }
        }

        private static HttpClient GetClient()
        {
            HttpClientHandler httpClientHandler = new()
            {
                UseDefaultCredentials = true
            };

            return new(httpClientHandler);
        }
    }
}
