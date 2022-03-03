namespace UserService.Core.CaptchaGenerator
{
    /// <summary>
    /// Результат работы генератора капчи
    /// </summary>
    public record CaptchaResult(byte[] CaptchaByteData, string HashCode)
    {
        public const string ContentType = "image/png";
    }

}
