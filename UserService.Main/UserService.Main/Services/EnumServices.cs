using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension.Translator;

using Grpc.Core;

using UserService.Core.Entity;
using UserService.Core.NotifyEventTypeGetter;
using UserService.Proto;

namespace UserService.Main
{
    /// <summary>
    /// Сервис для работы с перечислениями сервиса
    /// </summary>
    public class EnumServices : EnumService.EnumServiceBase
    {
        private readonly ITranslator _translator;
        private readonly INotifyEventTypeGetter _notifyEventTypeGetter;

        /// <summary>
        /// Сервис для работы с перечислениями сервиса
        /// </summary>
        /// <param name="translator"></param>
        /// <param name="notifyEventTypeGetter"></param>
        public EnumServices(ITranslator translator, INotifyEventTypeGetter notifyEventTypeGetter)
        {
            _translator = translator;
            _notifyEventTypeGetter = notifyEventTypeGetter;
        }

        /// <summary>
        /// Получить перечисление NotifyEventType
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<EnumResponse> GetNotifyEventTypes(EnumRequest request, ServerCallContext context)
        {
            IEnumerable<string> notifyEvenTypes = _notifyEventTypeGetter
                .GetAllNotifyEventTypes()
                .Select(e => _translator.GetUserText(nameof(Proto.Notification.NotifyEventType), e));

            return Task.FromResult(new EnumResponse
            {
                Values = { notifyEvenTypes }
            });
        }

        /// <summary>
        /// Получить перечисление UserState
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<EnumResponse> GetUserStates(EnumRequest request, ServerCallContext context) => Task.FromResult(new EnumResponse
        {
            Values = { GetEnumValuesList<UserState>() }
        });

        /// <summary>
        /// Получить перечисление PermissionAssert
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<EnumResponse> GetPermissionAsserts(EnumRequest request, ServerCallContext context) => Task.FromResult(new EnumResponse
        {
            Values = { GetEnumValuesList<PermissionAssert>() }
        });

        /// <summary>
        /// Получить перечисление TargetNotify
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<EnumResponse> GetTargetNotifies(EnumRequest request, ServerCallContext context) => Task.FromResult(new EnumResponse
        {
            Values = { GetEnumValuesList<TargetNotify>() }
        });

        /// <summary>
        /// Получить перечисление WebHookContractType
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<EnumResponse> GetWebHookContractTypes(EnumRequest request, ServerCallContext context) => Task.FromResult(new EnumResponse
        {
            Values = { GetEnumValuesList<WebHookContractType>() }
        });

        private IEnumerable<string> GetEnumValuesList<TEnum>() where TEnum : struct, Enum =>
            _translator.GetEnumText<TEnum>().Select(e => e.Value);
    }
}
