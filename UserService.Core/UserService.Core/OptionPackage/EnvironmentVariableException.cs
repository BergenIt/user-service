using System;

namespace UserService.Core
{
    public class EnvironmentVariableException : Exception
    {
        private const string ExceptionText = "Не найдена переменная окружения: ";
        public EnvironmentVariableException(string variableName)
            : base($"{ExceptionText}{variableName}") { }
    }
}
