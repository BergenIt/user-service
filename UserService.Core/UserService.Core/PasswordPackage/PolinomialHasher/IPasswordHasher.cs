namespace UserService.Core.PolindromHasher
{
    /// <summary>
    /// Сервис хеша паролей
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Валидация количества одинаковых символов в старом и новом паролях юзера
        /// </summary>
        /// <param name="userName">Имя юзера</param>
        /// <param name="hashPassword">Хеш старого пароля</param>
        /// <param name="newPassword">Новый пароль</param>
        /// <returns>true - проверка пройдена, false - проверка провалена</returns>
        bool ComparePassword(string userName, string hashPassword, string newPassword);

        /// <summary>
        /// Получить хеш пароля
        /// </summary>
        /// <param name="userName">Имя юзера</param>
        /// <param name="password">Пароль для хеширования</param>
        /// <returns>Хеш пароля</returns>
        string HashPassword(string userName, string password);

        /// <summary>
        /// Сравнивает хеш пароля и данные введенные юзером
        /// </summary>
        /// <param name="userName">Имя юзера</param>
        /// <param name="hashedPassword">Хеш пароля</param>
        /// <param name="providedPassword">Ведвенный пароль</param>
        /// <returns></returns>
        PasswordVerificationResult VerifyHashedPassword(string userName, string hashedPassword, string providedPassword);
    }
}
