using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Models.Quotes;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.ClientModel.Primitives;
using System.Net.Http.Headers;

namespace ACE_PC.BlazorServer.UseCases.Quotes
{
    public class QuoteUseCases
    {
        private HttpClient _httpClient;
        private readonly ProtectedLocalStorage _localStorage;

        public QuoteUseCases(IHttpClientFactory factory, ProtectedLocalStorage localStorage)
        {
            _httpClient = factory.CreateClient("client");
            _localStorage = localStorage;
        }

        public async Task SetAuthHeader()
        {
            var token = (await _localStorage.GetAsync<string>("jwtAuth")).Value;

            if (token is not null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            }
        }

        public async Task<ResultModel<QuotesResponse>> GetAllQuotes()
        {
            await this.SetAuthHeader();
            var response =await _httpClient.GetFromJsonAsync<ResultModel<QuotesResponse>>("/api/quotes");
            return response!;
        }

    }
}
