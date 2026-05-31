using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ACE_PC.BlazorServer.Providers
{
    public class CustomeAuthenticationProvider : AuthenticationStateProvider
    {

        private readonly ProtectedLocalStorage _localStorage;

        public CustomeAuthenticationProvider(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = (await _localStorage.GetAsync<string>("jwtAuth")).Value;

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(jwt.Claims,"jwt")
                );

            return new AuthenticationState(user);
        }

        public async Task MarkUserAsAuthenticated(string token) //login
        {
            await _localStorage.SetAsync("jwtAuth", token);

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwt.Claims,"jwt");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task MaskUserAsLogout()  // logout
        {
            await _localStorage.DeleteAsync("jwtAuth");
            var user = new ClaimsPrincipal(
                new ClaimsIdentity());

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
