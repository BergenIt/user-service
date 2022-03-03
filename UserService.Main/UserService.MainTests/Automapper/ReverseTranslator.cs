using System;
using System.Collections.Generic;
using System.Linq;

using DatabaseExtension.Translator;

namespace UserService.Main.Automapper_Tests
{
    public class ReverseTranslator : ITranslator
    {
        public IDictionary<Enum, string> GetEnumText(Type enumType)
        {
            return new Dictionary<Enum, string>();
        }

        public string GetSourceElementFromUserText(string className, string userText)
        {
            return userText;
        }

        public string GetSourceElementFromUserText<TClass>(string userText)
        {
            return userText;
        }

        public string GetUserText(string className, string elementName)
        {
            return elementName;
        }

        public string GetUserText<TClass>(string elementName)
        {
            return elementName;
        }

        public IEnumerable<string> GetUserText(string className)
        {
            return new string[] { className };
        }

        public IEnumerable<string> GetUserText<TClass>()
        {
            return Array.Empty<string>();
        }

        IDictionary<TEnum, string> ITranslator.GetEnumText<TEnum>()
        {
            return Enum.GetValues<TEnum>().ToDictionary(
                  k => k,
                  k => k.ToString()
              );
        }

        TEnum ITranslator.GetSourceElementFromEnumText<TEnum>(string userText)
        {
            return Enum.TryParse(userText, out TEnum @enum) ? @enum : default;
        }
    }
}

