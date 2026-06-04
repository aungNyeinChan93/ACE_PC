using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Models.Likes;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;

namespace ACE_PC.BlazorServer.UseCases.Like
{
    public class LikesUseCase
    {

        private readonly ProtectedLocalStorage _localStorage;

        private readonly HttpClient _httpClient;

        public LikesUseCase(ProtectedLocalStorage localStorage, IHttpClientFactory factory)
        {
            _localStorage = localStorage;
            _httpClient = factory.CreateClient("client");
        }

        public async Task SetAuthHeader()
        {
            var token = (await _localStorage.GetAsync<string>("jwtAuth")).Value;
            if (token is not null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<ResultModel<string>> LikeToogle(LikeCreateRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/likes/toogle",request);

            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResultModel<string>>(str);
                return result!;
            }
            return default!;
        }
    }
}
