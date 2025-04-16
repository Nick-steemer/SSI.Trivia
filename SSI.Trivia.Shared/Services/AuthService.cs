using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace SSI.Trivia.Shared.Services
{
    public class AuthService(AuthenticationStateProvider authStateProvider)
    {
        public async Task<bool> IsUserAuthenticated()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            return authState.User.Identity?.IsAuthenticated ?? false;
        }

        public async Task<string?> GetUserDisplayName()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated != true)
                return null;

            // Try to get display name from claims in order of preference
            return user.FindFirst(ClaimTypes.Name)?.Value ??
                   user.FindFirst("name")?.Value ??
                   user.FindFirst(ClaimTypes.GivenName)?.Value ??
                   user.FindFirst(ClaimTypes.Email)?.Value ??
                   user.Identity.Name;
        }

        public async Task<string?> GetUserEmail()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated != true)
                return null;

            return user.FindFirst(ClaimTypes.Email)?.Value ??
                   user.FindFirst("preferred_username")?.Value;
        }
    }
}
