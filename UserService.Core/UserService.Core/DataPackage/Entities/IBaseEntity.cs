using System;

namespace UserService.Core.Entity
{
    /// <summary>
    /// Базовый интерфейс сущности
    /// </summary>
    public interface IBaseEntity
    {
        Guid Id { get; set; }

        /// <summary>
        /// Имя объекта, которое должно быть понятно юзеру, используется в аудите
        /// </summary>
        string AuditName { get; }
    }
}
