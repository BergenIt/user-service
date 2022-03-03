using System;
using System.Collections.Generic;

namespace UserService.Core.SenderInteraces
{
    /// <summary>
    /// При большом потоке сообщений склеивает их в 1
    /// </summary>
    public interface INotificationLoadBalancer
    {
        /// <summary>
        /// Добавить сообщение в обработку
        /// </summary>
        /// <param name="senderContracts"></param>
        /// <param name="sendAction">Действие которое выполняется для отправки</param>
        void AddToHandle(IEnumerable<SenderContract> senderContracts, Action<SenderContract> sendAction);

        /// <summary>
        /// Добавить сообщение в обработку
        /// </summary>
        /// <param name="senderContract"></param>
        /// <param name="sendAction">Действие которое выполняется для отправки</param>
        void AddToHandle(SenderContract senderContract, Action<SenderContract> sendAction);
    }
}
