using ACE_PC.BlazorServer.Providers;
using ACE_PC.Domain.Dtos.Users;
using ACE_PC.Domain.Entity;
using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Models.Users;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.WebUtilities;
using System.ClientModel.Primitives;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Security.Claims;

namespace ACE_PC.BlazorServer.UseCases.Users
{
    public class UserUseCases
    {
        private readonly AuthenticationStateProvider _stateProvider;

        private readonly HttpClient _httpClient;

        private readonly ProtectedLocalStorage _localStorage;

        public UserUseCases(AuthenticationStateProvider stateProvider, IHttpClientFactory factory, ProtectedLocalStorage localStorage)
        {
            _stateProvider = stateProvider;
            _httpClient = factory.CreateClient("client");
            _localStorage = localStorage;
        }

        //SetAuthHeader
        public async Task SetAuthHeader()
        {
            var token = (await _localStorage.GetAsync<string>("jwtAuth")).Value;

            if (token is not null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        //GetAuthUserByEmail
        public async Task<UserDto> GetAuthUser()
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

        //GetAllUsers
        public async Task<ResultModel<UsersResposne>> GetAllUsers(UserPaginationRequest? pagination = default)
        {
            await this.SetAuthHeader();
            var queryParams = new Dictionary<string, string?>
            {
                {"PageCount" ,pagination?.PageCount.ToString() ?? "10" },
                {"PageNumber",pagination?.PageNumber.ToString() ?? "1" }
            };

            var requestUri = QueryHelpers.AddQueryString("/api/users", queryParams);

            var response = await _httpClient.GetFromJsonAsync<ResultModel<UsersResposne>>(requestUri);

            return response!;
        }

        //GetUserById
        public async Task<ResultModel<UserResponse>> GetUserById(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ResultModel<UserResponse>>($"/api/users/{id}");
            return response!;
        }



    }
}
