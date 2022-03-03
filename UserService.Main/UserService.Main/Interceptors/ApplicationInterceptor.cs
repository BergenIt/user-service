using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using DatabaseExtension.Translator;

using Google.Protobuf;

using Grpc.Core;
using Grpc.Core.Interceptors;

using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using UserService.Core.AuditPackage.AuditException;
using UserService.Core.DataInterfaces;
using UserService.Core.JwtGenerator;
using UserService.Proto;

namespace UserService.Main.Interceptors
{
    /// <summary>
    /// Interceptor для grpc подключений
    /// </summary>
    public class ApplicationInterceptor : Interceptor
    {
        private static readonly IDictionary<Type, Func<IMessage, string>> s_tokenFreeRequestMaps = new Dictionary<Type, Func<IMessage, string>>()
        {
            { typeof(ForgotPasswordRequest), req => (req as ForgotPasswordRequest).UserName },
            { typeof(ChangeForgotPasswordCommand), req => (req as ChangeForgotPasswordCommand).UserName },
            { typeof(LoginRequest), req => (req as LoginRequest).UserName },
            { typeof(GetCaptchaImageRequest), _ => string.Empty },
            { typeof(CreateNotificationCommand), _ => string.Empty },
        };

        private static readonly IEnumerable<Type> s_tokenFreeRequests = s_tokenFreeRequestMaps.Select(p => p.Key);

        private static readonly IEnumerable<Type> s_jtiIgnoreRequests = new List<Type>(s_tokenFreeRequests)
        {
            typeof(ChangePasswordCommand),
        };

        private readonly IAuditWorker _auditWorker;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly ITranslator _translator;

        /// <summary>
        /// Interceptor для grpc подключений
        /// </summary>
        /// <param name="auditWorker"></param>
        /// <param name="jwtGenerator"></param>
        /// <param name="translator"></param>
        public ApplicationInterceptor(IAuditWorker auditWorker, IJwtGenerator jwtGenerator, ITranslator translator)
        {
            _auditWorker = auditWorker;
            _jwtGenerator = jwtGenerator;
            _translator = translator;
        }

        /// <summary>
        /// Обработка вызовов клиент-сервер grpc
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            Serilog.Log.Logger.ForContext(context.Host, context.Method).Debug("Start request with data {request}", request);

            StringValues jwt = context.GetHttpContext().Request.Headers[HeaderNames.Authorization];

            Serilog.Log.Logger.ForContext(context.Host, context.Method).Debug("Start request with token {jwt}", jwt);

            bool hasJwt = !string.IsNullOrWhiteSpace(jwt);

            if (!hasJwt && !s_tokenFreeRequests.Contains(typeof(TRequest)))
            {
                throw new RpcException(new(StatusCode.Unauthenticated, "Unauthenticated"));
            }

            TokenMeta tokenMeta = hasJwt
                ? _jwtGenerator.GetTokenData(jwt)
                : new TokenMeta(string.Empty, Guid.Empty, string.Empty, DateTime.MinValue, false);

            try
            {
                if (tokenMeta.HasJti && !s_jtiIgnoreRequests.Contains(typeof(TRequest)))
                {
                    throw new JtiUserException();
                }

                tokenMeta.AddToMeta(context.RequestHeaders);

                TResponse response = await continuation(request, context);

                Serilog.Log.Logger.ForContext(context.Host, context.Method).Debug("Response to request with {response}", response);

                return response;
            }
            catch (AuditException exception)
            {
                Serilog.Log.Logger.ForContext<AuditException>().Error("Exception while runnig unary server handler: {exception}", exception);

                StatusCodeAttribute statusCodeAttribute = exception.GetType().GetCustomAttribute<StatusCodeAttribute>();

                StringValues clientIp = context.GetHttpContext().Request.Headers[JwtGenerator.ClientIp];

                string detail = GetExceptionMessage(exception);

                string userExceptionDetail = _translator.GetUserText<AuditException>(exception.AuditType);

                Core.Models.AuditCreateCommand auditCreateCommand = hasJwt
                    ? new(tokenMeta.UserName, userExceptionDetail, nameof(AuditException))
                    : new(s_tokenFreeRequestMaps[typeof(TRequest)]((IMessage)request), userExceptionDetail, nameof(AuditException));

                if (tokenMeta.UserId != Guid.Empty || !string.IsNullOrWhiteSpace(auditCreateCommand.UserName))
                {
                    await _auditWorker.CreateAudit(auditCreateCommand);
                }

                throw new RpcException(new(statusCodeAttribute.StatusCode, $"{userExceptionDetail}:\n{detail}"));
            }
            catch (Exception exception)
            {
                Serilog.Log.Logger.ForContext<Interceptor>().Error("Exception while runnig unary server handler: {exception}", exception);

                throw new RpcException(new(StatusCode.Unknown, GetExceptionMessage(exception)));
            }
        }

        private static string GetExceptionMessage(Exception exception)
        {
            Exception inner = exception;

            string message = $"{exception.GetType().Name}: {exception.Message}";

            byte i = 0;

            while (inner.InnerException != null && i < 5)
            {
                inner = inner.InnerException;

                message += $"\n{inner.GetType().Name}: {inner.Message}";

                ++i;
            }

            return message;
        }
    }
}
