using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using UserService.Core.Elasticsearch;
using UserService.Core.Entity;
using UserService.Core.SenderInteraces;

namespace UserService.Main.Websocket
{
    /// <summary>
    /// Класс вебсокета
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Websocket : Hub, IWebsocket
    {
        private readonly IElasticsearchWorker _elasticsearchWorker;

        /// <summary>
        /// Класс вебсокета
        /// </summary>
        /// <param name="elasticsearchWorker"></param>
        public Websocket(IElasticsearchWorker elasticsearchWorker)
        {
            _elasticsearchWorker = elasticsearchWorker;
        }

        /// <summary>
        /// Имя подключения
        /// </summary>
        public const string MethodName = "/notify";

        private const string Type = nameof(DateTime);

        /// <summary>
        /// Отправить сообщения пользователям
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public ValueTask SendUsersAsync(SenderContract contract)
        {
            IClientProxy clientProxy = Clients?.Users(contract.Receivers);

            if (clientProxy is not null)
            {
                return new ValueTask(clientProxy.SendAsync(contract.Subject, contract.Msgs));
            }

            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// Отключение от сокета клиента
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Claim timeClaim = Context.User.Claims.Single(c => c.Type == Type);

            string userName = Context.User.Identity.Name;

            if (!string.IsNullOrWhiteSpace(userName) && DateTime.TryParse(timeClaim.Value, out DateTime startSession))
            {
                TimeSpan duration = DateTime.UtcNow - startSession;

                ScreenTime screenTime = new()
                {
                    Id = Guid.NewGuid(),
                    Duration = duration,
                    Timestamp = startSession,
                    UserName = userName,
                };

                await _elasticsearchWorker.InsertAsync(screenTime);
            }

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Подключение к сокету клиента
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            Context.User.AddIdentity(new(new Claim[] { new Claim(Type, DateTime.UtcNow.ToString()) }));

            return base.OnConnectedAsync();
        }
    }
}
