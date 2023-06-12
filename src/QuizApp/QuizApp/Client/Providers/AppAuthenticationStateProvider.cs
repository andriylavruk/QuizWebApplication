using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace QuizApp.Client.Providers;

public class AppAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

    public AppAuthenticationStateProvider(ILocalStorageService localStorageService,
        HttpClient httpClient)
    {
        _localStorageService = localStorageService;
        _httpClient = httpClient;
    }
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            string savedToken = await _localStorageService.GetItemAsync<string>("bearerToken");

            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);
            JwtSecurityToken jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);
            DateTime expiry = jwtSecurityToken.ValidTo;

            if (expiry < DateTime.UtcNow)
            {
                await _localStorageService.RemoveItemAsync("bearerToken");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = ParseClaims(jwtSecurityToken);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            return new AuthenticationState(user);
        }
        catch (Exception)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    private IList<Claim> ParseClaims(JwtSecurityToken jwtSecurityToken)
    {
        IList<Claim> claims = jwtSecurityToken.Claims.ToList();

        return claims;
    }

    internal async Task SignIn()
    {
        string savedToken = await _localStorageService.GetItemAsync<string>("bearerToken");
        JwtSecurityToken jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);
        var claims = ParseClaims(jwtSecurityToken);
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

        Task<AuthenticationState> authenticationState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authenticationState);
    }

    internal void SignOut()
    {
        ClaimsPrincipal nobody = new ClaimsPrincipal(new ClaimsIdentity());

        Task<AuthenticationState> authenticationState = Task.FromResult(new AuthenticationState(nobody));
        NotifyAuthenticationStateChanged(authenticationState);
    }
}
