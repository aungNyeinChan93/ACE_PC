using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Models.Comments;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;

namespace ACE_PC.BlazorServer.UseCases.Comments
{
    public class CommentUseCases
    {
        private readonly HttpClient _httpClient;

        private readonly ProtectedLocalStorage _localStorage;

        public CommentUseCases(IHttpClientFactory factory, ProtectedLocalStorage localStorage)
        {
            _httpClient = factory.CreateClient("client");
            _localStorage = localStorage;
        }

        public async Task SetAuthHeader()
        {
            var token = (await _localStorage.GetAsync<string>("jwtAuth")).Value;
            if (token is not null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
            }
        }

        public async Task<ResultModel<CreateCommentResponse>> CreateAsync(CreateCommentRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/comments", request);
            if (response.IsSuccessStatusCode)
            {
                var resStr = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResultModel<CreateCommentResponse>>(resStr);
                return result!;
            }
            return default!;
        }
    }
}
