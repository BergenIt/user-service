using System;
using System.Collections.Generic;
using System.Linq;

using DatabaseExtension.Translator;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsTranslator : ITranslator
    {
        private const string Default = "defaultString";

        public IDictionary<Enum, string> GetEnumText(Type enumType)
        {
            return new Dictionary<Enum, string> { { default, Default } };
        }

        public string GetSourceElementFromUserText(string className, string userText)
        {
            return Default;
        }

        public string GetSourceElementFromUserText<TClass>(string userText)
        {
            return Default;
        }

        public string GetUserText(string className, string elementName)
        {
            return Default;
        }

        public string GetUserText<TClass>(string elementName)
        {
            return Default;
        }

        public IEnumerable<string> GetUserText(string className)
        {
            return new string[] { Default };
        }

        public IEnumerable<string> GetUserText<TClass>()
        {
            return new string[] { Default };
        }

        IDictionary<TEnum, string> ITranslator.GetEnumText<TEnum>()
        {
            return new Dictionary<TEnum, string> { { Enum.GetValues<TEnum>().First(), Default } };
        }

        TEnum ITranslator.GetSourceElementFromEnumText<TEnum>(string userText)
        {
            return default;
        }
    }
}
