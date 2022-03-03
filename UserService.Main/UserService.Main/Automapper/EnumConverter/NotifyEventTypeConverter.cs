
using AutoMapper;

using DatabaseExtension.Translator;

using UserService.Core.Entity;

namespace UserService.Main.Automapper
{
    /// <summary>
    /// Конвертер обеспечивающий перевод NotifyEventType
    /// </summary>
    public class NotifyEventTypeConverter : IValueConverter<string, string>
    {
        private readonly ITranslator _translator;

        /// <summary>
        /// Конвертер обеспечивающий перевод NotifyEventType
        /// </summary>
        /// <param name="translator"></param>
        public NotifyEventTypeConverter(ITranslator translator)
        {
            _translator = translator;
        }

        /// <summary>
        /// Конвертер обеспечивающий перевод NotifyEventType
        /// </summary>
        /// <param name="sourceMember"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Convert(string sourceMember, ResolutionContext context)
        {
            return _translator.GetUserText(nameof(Notification.NotifyEventType), sourceMember);
        }
    }
}
