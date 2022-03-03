using System.Threading.Tasks;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using UserService.Core.Authorizer;
using UserService.Core.JwtGenerator;
using UserService.Proto;

namespace UserService.Main
{
    /// <summary>
    /// Сервис авторизации
    /// </summary>
    public class AuthorizeServices : AuthorizeService.AuthorizeServiceBase
    {
        private readonly IAuthorizer _authorizer;

        /// <summary>
        /// Сервис авторизации
        /// </summary>
        /// <param name="authorizer"></param>
        public AuthorizeServices(IAuthorizer authorizer)
        {
            _authorizer = authorizer;
        }

        /// <summary>
        /// Логин пользователя в системе
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Token> Login(LoginRequest request, ServerCallContext context)
        {
            string token = await _authorizer.LoginAsync(request.UserName, request.Password);

            return new Token() { JwtToken = token };
        }

        /// <summary>
        /// Выход пользователя из системы
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Empty> Logout(Empty request, ServerCallContext context)
        {
            await _authorizer.LogoutAsync(context.RequestHeaders.Get(JwtGenerator.UserName).Value);

            return new();
        }

        /// <summary>
        /// Обновление токена пользователя
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Token> UpdateToken(Empty request, ServerCallContext context)
        {
            StringValues jwt = context.GetHttpContext().Request.Headers[HeaderNames.Authorization];

            string token = await _authorizer.UpdateTokenAsync(jwt);

            return new Token() { JwtToken = token };
        }
    }
}
