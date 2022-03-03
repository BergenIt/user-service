using System;

using AutoMapper;

using DatabaseExtension.Translator;
namespace UserService.Main.Automapper
{
    /// <summary>
    /// Конвертер обеспечивающий перевод перечислений
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public class EnumReverseConverter<TEnum> : IValueConverter<string, TEnum> where TEnum : struct, Enum
    {
        private readonly ITranslator _translator;

        /// <summary>
        /// Конвертер обеспечивающий перевод перечислений
        /// </summary>
        /// <param name="translator"></param>
        public EnumReverseConverter(ITranslator translator)
        {
            _translator = translator;
        }

        /// <summary>
        /// Конвертер обеспечивающий перевод перечислений
        /// </summary>
        /// <param name="sourceMember"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public TEnum Convert(string sourceMember, ResolutionContext context)
        {
            return _translator.GetSourceElementFromEnumText<TEnum>(sourceMember);
        }
    }
}
