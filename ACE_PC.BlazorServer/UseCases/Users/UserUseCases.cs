using ACE_PC.BlazorServer.Providers;
using ACE_PC.Domain.Entity;
using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Models.Users;
using Microsoft.AspNetCore.Components.Authorization;
using System.ClientModel.Primitives;
using System.Runtime.Serialization;
using System.Security.Claims;

namespace ACE_PC.BlazorServer.UseCases.Users
{
    public class UserUseCases
    {
        private readonly AuthenticationStateProvider _stateProvider;

        private readonly HttpClient _httpClient;

        public UserUseCases(AuthenticationStateProvider stateProvider, IHttpClientFactory factory)
        {
            _stateProvider = stateProvider;
            _httpClient = factory.CreateClient("client");
        }

        public async Task<User> GetAuthUser()
        {
            var authState = await ((CustomeAuthenticationProvider)_stateProvider).GetAuthenticationStateAsync();
            var email = authState.User.FindFirstValue(ClaimTypes.Email);
            var response = await _httpClient.GetFromJsonAsync<ResultModel<UserResponse>>($"/api/users/{email}");
            if (response is not null || response!.IsSuccess)
            {
                return response.Data!.User;
            }
            return default!;
        } 
    }
}
