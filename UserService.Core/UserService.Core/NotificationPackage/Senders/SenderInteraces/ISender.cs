using System.Collections.Generic;

namespace UserService.Core.SenderInteraces
{
    /// <summary>
    /// Отправитель
    /// </summary>
    public interface ISender
    {
        /// <summary>
        /// Отправляет сообщения
        /// </summary>
        /// <param name="senderContracts"></param>
        void Send(IEnumerable<SenderContract> senderContracts);

        /// <summary>
        /// Отправляет сообщение
        /// </summary>
        /// <param name="senderContract"></param>
        void Send(SenderContract senderContract);
    }
}
