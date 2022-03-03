using System.Collections.Generic;

using UserService.Core.SenderInteraces;

namespace UserService.Core.Senders
{
    public class WebsocketSender : IWebsocketSender
    {
        private readonly IWebsocket _websocket;

        public WebsocketSender(IWebsocket websocket)
        {
            _websocket = websocket;
        }

        public async void Send(IEnumerable<SenderContract> senderContracts)
        {
            foreach (SenderContract senderContract in senderContracts)
            {
                await _websocket.SendUsersAsync(senderContract);
            }
        }

        public async void Send(SenderContract senderContract)
        {
            await _websocket.SendUsersAsync(senderContract);
        }
    }
}
