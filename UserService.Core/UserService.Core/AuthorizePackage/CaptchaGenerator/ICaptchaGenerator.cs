namespace UserService.Core.CaptchaGenerator
{
    /// <summary>
    /// Генератор и валидатор капчи
    /// </summary>
    public interface ICaptchaGenerator
    {
        /// <summary>
        /// Генерирует изображение капчи
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        CaptchaResult GenerateCaptchaImage(int width, int height);

        /// <summary>
        /// Валидирует ввод юзера на капчу
        /// </summary>
        /// <param name="userInputCaptcha">Текст ведвенный юезром</param>
        /// <param name="hashCode">Хеш, который шел вместе с капчей</param>
        void ValidateCaptchaCode(string userInputCaptcha, string hashCode);
    }
}
