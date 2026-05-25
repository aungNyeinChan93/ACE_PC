using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Formats.Asn1;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;

namespace ACE_PC.BlazorServer.Middleware
{
    public class SetJwtTokenMiddleware : IMiddleware
    {
        private readonly ProtectedLocalStorage _localStorage;
        private readonly HttpClient _httpClient;
        private readonly ILogger<SetJwtTokenMiddleware> _logger;

        public SetJwtTokenMiddleware(ProtectedLocalStorage localStorage, IHttpClientFactory factory, ILogger<SetJwtTokenMiddleware> logger)
        {
            _localStorage = localStorage;
            _httpClient = factory.CreateClient("client");
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            _logger.LogInformation("[SetJwtTokenMiddleware] Hit the Middleware!");
            var token = (await _localStorage.GetAsync<string>("jwtAuth")).Value;

            if (token is not null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            }
            await next(context);
        }
    }
}
