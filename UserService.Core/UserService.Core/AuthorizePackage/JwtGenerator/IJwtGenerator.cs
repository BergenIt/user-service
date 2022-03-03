using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace UserService.Core.JwtGenerator
{
    /// <summary>
    /// Парсер jwt токена
    /// </summary>
    public interface IJwtGenerator
    {
        /// <summary>
        /// Генерирует jwt токен
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userEmail"></param>
        /// <param name="userKey"></param>
        /// <param name="role"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        string CreateToken(Guid userId, string userName, string userEmail, string userKey, IEnumerable<string> role, IEnumerable<Claim> access);

        /// <summary>
        /// Получает данные из токена
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        TokenMeta GetTokenData(string token);
    }
}
