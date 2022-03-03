using System.Threading.Tasks;

using Grpc.Core;

using UserService.Core.CaptchaGenerator;
using UserService.Proto;

namespace UserService.Main
{
    /// <summary>
    /// Сервис работы с капчой
    /// </summary>
    public class CaptchaServices : CaptchaService.CaptchaServiceBase
    {
        private const int ImageHeight = 200;
        private const int ImageWeidth = 480;

        private readonly ICaptchaGenerator _captchaGenerator;

        /// <summary>
        /// Сервис работы с капчой
        /// </summary>
        /// <param name="captchaGenerator"></param>
        public CaptchaServices(ICaptchaGenerator captchaGenerator)
        {
            _captchaGenerator = captchaGenerator;
        }

        /// <summary>
        /// Получить изображение капчи
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CaptchaImage> GetCaptchaImage(GetCaptchaImageRequest request, ServerCallContext context)
        {
            CaptchaResult captchaResult = _captchaGenerator.GenerateCaptchaImage(ImageWeidth, ImageHeight);

            await context.WriteResponseHeadersAsync(new() { { Startup.CaptchaCode, captchaResult.HashCode } });

            CaptchaImage captchaImage = new()
            {
                CaptchaImage_ = Google.Protobuf.ByteString.CopyFrom(captchaResult.CaptchaByteData),
                ContentType = CaptchaResult.ContentType,
            };

            return captchaImage;
        }
    }
}
