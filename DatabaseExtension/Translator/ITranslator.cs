using System;
using System.Collections.Generic;

namespace DatabaseExtension.Translator
{
    public interface ITranslator
    {
        string GetSourceElementFromUserText(string className, string userText);
        string GetSourceElementFromUserText<TClass>(string userText);

        TEnum GetSourceElementFromEnumText<TEnum>(string userText) where TEnum : struct, Enum;

        IDictionary<Enum, string> GetEnumText(Type enumType);
        IDictionary<TEnum, string> GetEnumText<TEnum>() where TEnum : struct, Enum;

        string GetUserText(string className, string elementName);
        string GetUserText<TClass>(string elementName);

        IEnumerable<string> GetUserText(string className);
        IEnumerable<string> GetUserText<TClass>();
    }
}
