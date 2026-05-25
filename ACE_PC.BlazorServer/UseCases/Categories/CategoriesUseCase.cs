using ACE_PC.Domain.Entity;
using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Models.Categories;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;

namespace ACE_PC.BlazorServer.UseCases.Categories
{
    public class CategoriesUseCase 
    {
        private readonly HttpClient _httpClient;
        private readonly ProtectedLocalStorage _localStorage;

        public CategoriesUseCase(IHttpClientFactory factory, ProtectedLocalStorage localStorage)
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

        public async Task<ResultModel<CategoriesResponse>> GetAllAsync()
        {
            await this.SetAuthHeader();

            var response = await _httpClient.GetFromJsonAsync<ResultModel<CategoriesResponse>>("/api/categories");

            if (response is null) return default!;

            if (response.IsError) return default!;

            return response;
            
        }

        public async Task<ResultModel<bool>> DeleteCategory(int id)
        {
            await this.SetAuthHeader();
            var response = await _httpClient.DeleteFromJsonAsync<ResultModel<bool>>($"/api/categories/{id}");
            
            return response!;
        }
    }
}
