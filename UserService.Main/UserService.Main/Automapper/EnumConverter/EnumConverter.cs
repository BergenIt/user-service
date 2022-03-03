using System;

using AutoMapper;

using DatabaseExtension.Translator;

namespace UserService.Main.Automapper
{
    /// <summary>
    /// Конвертер обеспечивающий перевод перечислений
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public class EnumConverter<TEnum> : IValueConverter<TEnum, string> where TEnum : struct, Enum
    {
        private readonly ITranslator _translator;

        /// <summary>
        /// Конвертер обеспечивающий перевод перечислений
        /// </summary>
        /// <param name="translator"></param>
        public EnumConverter(ITranslator translator)
        {
            _translator = translator;
        }

        /// <summary>
        /// Конвертер обеспечивающий перевод перечислений
        /// </summary>
        /// <param name="sourceMember"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Convert(TEnum sourceMember, ResolutionContext context)
        {
            return _translator.GetUserText<TEnum>(sourceMember.ToString());
        }
    }
}
