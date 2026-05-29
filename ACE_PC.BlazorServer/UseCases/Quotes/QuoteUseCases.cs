using ACE_PC.BlazorServer.Dtos.Quotes;
using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Models.Quotes;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
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
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }


        //All
        public async Task<ResultModel<QuotesResponse>> GetAllQuotes()
        {
            await this.SetAuthHeader();
            var response = await _httpClient.GetFromJsonAsync<ResultModel<QuotesResponse>>("/api/quotes");
            return response!;
        }

        //CreateQuote
        public async Task<ResultModel<CreateQuoteResponse>> CreateQuote(CreateQuoteDto dto)
        {
            await this.SetAuthHeader();

            var modal = new CreateQuoteRequest
            {
                Title = dto.Title,
                Content = dto.Content,
                CategoryId = dto.CategoryId,
                UserId = dto.UserId
            };

            var response = await _httpClient.PostAsJsonAsync("/api/quotes", modal);
            if (response.IsSuccessStatusCode)
            {
                var resStr = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResultModel<CreateQuoteResponse>>(resStr);
                if (result is null) return default!;
                return result;

            }
            return default!;
        }

        //Delete
        public async Task<ResultModel<DeleteQuoteResponse>> DeleteAsync(int id)
        {
            await this.SetAuthHeader();
            var response = await _httpClient.DeleteFromJsonAsync<ResultModel<DeleteQuoteResponse>>($"/api/quotes/{id}");
            return response!;
        }

        //getById
        public async Task<ResultModel<QuoteResponse>> GetByIdAsync(int id)
        {
            await this.SetAuthHeader();
            var response = await _httpClient.GetFromJsonAsync<ResultModel<QuoteResponse>>($"/api/quotes/{id}");
            return response!;
        }

        //UpdateQuote
        public async Task<ResultModel<UpdateQuoteResponse>> UpdateAsync(int id, UpdateQuoteRequest request)
        {
            await this.SetAuthHeader();
            var response = await _httpClient.PutAsJsonAsync($"/api/quotes/{id}", request);
            if (response.IsSuccessStatusCode)
            {
                var resStr = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResultModel<UpdateQuoteResponse>>(resStr);
                return result!;
            }
            return default!;
        }


        //getBySearchKey
        public async Task<ResultModel<QuotesResponse>> GetBySearchKey(QuoteSearchRequest searchReq, QuotePaginationRequest? paginationReq = default)
        {
            await this.SetAuthHeader();

            // Fallback to defaults if paginationReq is null to prevent 400 Bad Request
            int pageNumber = paginationReq?.PageNumber ?? 1;
            int pageCount = paginationReq?.PageCount ?? 10;

            var queryParams = new Dictionary<string, string?>
            {
                // Guaranteed to pass valid integers now
                { "PageNumber", pageNumber.ToString() },
                { "PageCount", pageCount.ToString() },

                // Search Parameters
                { "QuoteTitle", string.IsNullOrWhiteSpace(searchReq.QuoteTitle) ? null : searchReq.QuoteTitle },
                { "AuthorId", searchReq.AuthorId >= 1 ? searchReq.AuthorId.ToString() : null },
                { "AuthorName", string.IsNullOrWhiteSpace(searchReq.AuthorName) ? null : searchReq.AuthorName },
                { "CategoryId", searchReq.CategoryId >= 1 ? searchReq.CategoryId.ToString() : null },
                { "CategoryName", string.IsNullOrWhiteSpace(searchReq.CategoryName) ? null : searchReq.CategoryName }
            };
            string requestUri = QueryHelpers.AddQueryString("/api/quotes/search", queryParams);
            var response = await _httpClient.GetFromJsonAsync<ResultModel<QuotesResponse>>(requestUri);
            Console.WriteLine($"Requesting URL: {requestUri}");
            return response ?? ResultModel<QuotesResponse>.ValidationError(500, "Received empty response from server.");
        }
    }
}
