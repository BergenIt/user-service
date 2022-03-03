using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UserService.Core.SenderInteraces;

namespace UserService.Core.Senders
{

    public class WebhookSender : IWebhookSender
    {
        private readonly IWebhookClientProvider _httpClient;

        public WebhookSender(IWebhookClientProvider webhookDelegate)
        {
            _httpClient = webhookDelegate;
        }

        public async void Send(IEnumerable<SenderContract> senderContracts)
        {
            await Task.WhenAll(senderContracts
                .Select(contract =>
                    SendContract(contract)
                )
            );
        }

        public async void Send(SenderContract senderContract)
        {
            await SendContract(senderContract);
        }

        private Task SendContract(SenderContract senderContract)
        {
            return Task.WhenAll(senderContract
                .Receivers
                .SelectMany(r =>
                    senderContract.Msgs
                    .Select(m =>
                        _httpClient.SendPostAsJsonAsync(r, m)
                    )
                )
            );
        }
    }
}
