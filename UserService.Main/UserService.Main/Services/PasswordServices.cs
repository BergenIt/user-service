using System.Threading.Tasks;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using UserService.Core.AuditPackage.AuditException;
using UserService.Core.CaptchaGenerator;
using UserService.Core.JwtGenerator;
using UserService.Core.PasswordManager;
using UserService.Proto;

namespace UserService.Main
{
    /// <summary>
    /// Сервис работы с паролями
    /// </summary>
    public class PasswordServices : PasswordService.PasswordServiceBase
    {
        private readonly IPasswordManager _passwordManager;
        private readonly ICaptchaGenerator _captchaGenerator;

        /// <summary>
        /// Сервис работы с паролями
        /// </summary>
        /// <param name="passwordManager"></param>
        /// <param name="captchaGenerator"></param>
        public PasswordServices(IPasswordManager passwordManager, ICaptchaGenerator captchaGenerator)
        {
            _passwordManager = passwordManager;
            _captchaGenerator = captchaGenerator;
        }

        /// <summary>
        /// Сменить пароль
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Empty> ChangePassword(ChangePasswordCommand request, ServerCallContext context)
        {
            Metadata.Entry userName = context.RequestHeaders.Get(JwtGenerator.UserName);

            if (string.IsNullOrWhiteSpace(request.NewPassword) && request.AutogenerateToEmail is null)
            {
                throw new PasswordInvalidChangeException(PasswordInvalidChangeVariant.BasePolicy);
            }

            string password = request.AutogenerateToEmail is null ? request.NewPassword : null;

            await _passwordManager.ChangeUserPassword(userName.Value, password);

            return new();
        }

        /// <summary>
        /// Запросить токен для смены пароля (на email)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Empty> GetForgotPasswordToken(ForgotPasswordRequest request, ServerCallContext context)
        {
            IHeaderDictionary requestHeaders = context.GetHttpContext().Request.Headers;

            StringValues captchaHash = requestHeaders[Startup.CaptchaCode];
            StringValues applicationUrl = requestHeaders[HeaderNames.Origin];

            _captchaGenerator.ValidateCaptchaCode(request.InputCaptcha, captchaHash);

            await _passwordManager.CreateForgotPasswordToken(request.UserName, applicationUrl);

            return new();
        }

        /// <summary>
        /// Сменить забытый пароль
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Empty> SetForgotPassword(ChangeForgotPasswordCommand request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.NewPassword) && request.AutogenerateToEmail is null)
            {
                throw new PasswordInvalidChangeException(PasswordInvalidChangeVariant.BasePolicy);
            }

            string password = request.AutogenerateToEmail is null ? request.NewPassword : null;

            await _passwordManager.ChangeForgotPasswordToken(request.UserName, request.Token, password);

            return new();
        }
    }
}
