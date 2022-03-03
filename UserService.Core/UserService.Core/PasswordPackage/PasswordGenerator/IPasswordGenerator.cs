namespace UserService.Core.PasswordGenerator
{
    /// <summary>
    /// Генератор паролей
    /// </summary>
    public interface IPasswordGenerator
    {
        /// <summary>
        /// Генерирует пароль
        /// </summary>
        /// <returns></returns>
        string GeneratePassword();
    }
}
