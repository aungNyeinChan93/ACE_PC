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


        //Login
        public async Task<ResultModel<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login",request);

            if (!response.IsSuccessStatusCode)
            {
                var resStr = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResultModel<LoginResponse>>(resStr);
                result!.ResponseMessage = "Credential Wrong!";
                result.ResponseCode = 200;
                result.IsSuccess = false;
                return result;
            }

            if (response.IsSuccessStatusCode)
            {

                var resStr = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResultModel<LoginResponse>>(resStr);
               
                await ((CustomeAuthenticationProvider)_AuthStateProvider).MarkUserAsAuthenticated(result!.Data!.Token);

                return result!;
            }

            return default!;
        }
        

        //Logout
        public async Task Logout()
        {
            await ((CustomeAuthenticationProvider)_AuthStateProvider).MaskUserAsLogout();
        }

        //Register
        public async Task<ResultModel<RegisterResponse>> Register(RegisterRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/register",request);
            if (response.IsSuccessStatusCode)
            {
                var resStr = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResultModel<RegisterResponse>>(resStr);
                return result!;
            }
            return default!;
        }
    }
}
