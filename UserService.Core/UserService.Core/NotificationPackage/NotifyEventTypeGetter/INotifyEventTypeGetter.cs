using System.Collections.Generic;

namespace UserService.Core.NotifyEventTypeGetter
{
    /// <summary>
    /// Предоставляет досуп к типам уведомлений
    /// </summary>
    public interface INotifyEventTypeGetter
    {
        /// <summary>
        /// Получить все типы уведомлений
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetAllNotifyEventTypes();

        /// <summary>
        /// Получить стандартный шаблон контракта для типа
        /// </summary>
        /// <param name="notifyEventType"></param>
        /// <returns></returns>
        string GetNotifyEventTypeDefaultUserTemplate(string notifyEventType);

        /// <summary>
        /// Получить список json свойств для типа
        /// </summary>
        /// <param name="notifyEventType"></param>
        /// <returns></returns>
        IDictionary<string, string> GetNotifyEventTypePropperties(string notifyEventType);

        /// <summary>
        /// Получить перевод типа
        /// </summary>
        /// <param name="eventTypeKey"></param>
        /// <returns></returns>
        string GetTranslatedNotifyEventType(string eventTypeKey);

        /// <summary>
        /// Получить исходник типа по переводу
        /// </summary>
        /// <param name="eventTypeTranslated"></param>
        /// <returns></returns>
        string GetSourceNotifyEventType(string eventTypeTranslated);
    }
}
