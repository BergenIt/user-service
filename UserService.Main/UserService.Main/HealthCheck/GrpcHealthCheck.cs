using System.Threading.Tasks;

using Grpc.Core;
using Grpc.Health.V1;

namespace UserService.Main.HealthCheck
{
    /// <summary>
    /// Класс для определения HealthCheck по grpc
    /// </summary>
    public class GrpcHealthCheck : Health.HealthBase
    {
        /// <summary>
        /// Класс для определения HealthCheck по grpc
        /// </summary>
        public GrpcHealthCheck() : base() { }

        /// <summary>
        /// Проверка доступности сервиса по grpc
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context)
        {
            HealthCheckResponse result = new()
            {
                Status = HealthCheckResponse.Types.ServingStatus.Serving
            };

            return Task.FromResult(result);
        }
    }
}
