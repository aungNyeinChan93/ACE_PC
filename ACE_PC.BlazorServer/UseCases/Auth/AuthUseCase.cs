using ACE_PC.BlazorServer.Providers;
using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Models.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;



namespace ACE_PC.BlazorServer.UseCases.Auth
{
    public class AuthUseCase
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _AuthStateProvider;

        public AuthUseCase(IHttpClientFactory factory, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = factory.CreateClient("client");
            _AuthStateProvider = authStateProvider;
        }

        public async Task<ResultModel<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login",request);

            if (response.IsSuccessStatusCode)
            {

                var resStr = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResultModel<LoginResponse>>(resStr);
               
                await ((CustomeAuthenticationProvider)_AuthStateProvider).MarkUserAsAuthenticated(result!.Data!.Token);

                return result!;
            }
            return default!;
        }

        public async Task Logout()
        {
            await ((CustomeAuthenticationProvider)_AuthStateProvider).MaskUserAsLogout();
        }
    }
}
