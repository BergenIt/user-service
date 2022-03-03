using Microsoft.AspNetCore.Http;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsHttpContextAccessor : IHttpContextAccessor
    {
        private HttpContext _httpContext = new DefaultHttpContext();
        HttpContext IHttpContextAccessor.HttpContext { get => _httpContext; set => _httpContext = value; }
    }
}
