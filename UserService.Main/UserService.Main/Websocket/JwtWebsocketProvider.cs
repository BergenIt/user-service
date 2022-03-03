using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

namespace UserService.Main.Websocket
{
    /// <summary>
    /// Класс реализующий получение UserId (UserName) из jwt tokena при подключения пользователя к хабу
    /// </summary>
    public class JwtWebsocketProvider : IUserIdProvider
    {
        /// <summary>
        /// Имя query, где находится токен
        /// </summary>
        public const string AccessToken = "access_token";

        /// <summary>
        /// Метод для получения UserId (UserName) из jwt tokena при подключения пользователя к хабу
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public string GetUserId(HubConnectionContext connection)
        {
            string jwt = GetJwt(connection);

            if (string.IsNullOrEmpty(jwt))
            {
                return default;
            }

            SecurityToken securityToken = new JwtSecurityTokenHandler().ReadToken(jwt);

            Claim claimUserName = (securityToken as JwtSecurityToken)
                .Claims
                .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName);

            return claimUserName?.Value;
        }

        private static string GetJwt(HubConnectionContext c)
        {
            return c.GetHttpContext().Request.Query[AccessToken];
        }
    }
}
