using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TSJ_CRI.Model;

namespace TSJ_CRI.Authentication
{
    public class CustomAuth : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private ClaimsPrincipal _anon = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuth(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSessionStorageResult = await _sessionStorage.GetAsync<UserSession>("UserSession");
                var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
                if (userSession == null)
                {
                    return await Task.FromResult(new AuthenticationState(_anon));
                }
                var ClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.Username),
                    new Claim(ClaimTypes.Role, userSession.Roles),
                    new Claim("userid", userSession.Userid.ToString()),
                    new Claim("username", userSession.Username),
                    new Claim("email", userSession.Email),
                    new Claim("roles", userSession.Roles),
                    new Claim("org_id", userSession.OrgId),
                    new Claim("status", userSession.Status)
                }, "CustomAuth"));
                return await Task.FromResult(new AuthenticationState(ClaimsPrincipal));
            }
            catch (System.Exception)
            {
                return await Task.FromResult(new AuthenticationState(_anon));
            }
        }

        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            ClaimsPrincipal claimsPrincipal;
            if (userSession != null)
            {
                await _sessionStorage.SetAsync("UserSession", userSession);
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.Username),
                    new Claim(ClaimTypes.Role, userSession.Roles),
                    new Claim("userid", userSession.Userid.ToString()),
                    new Claim("username", userSession.Username),
                    new Claim("email", userSession.Email),
                    new Claim("roles", userSession.Roles),
                    new Claim("org_id", userSession.OrgId),
                    new Claim("status", userSession.Status)
                }, "CustomAuth"));
                NotifyAuthenticationStateChanged();
            }
            else
            {
                await _sessionStorage.DeleteAsync("UserSession");
                claimsPrincipal = _anon;
                NotifyAuthenticationStateChanged();
            }
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

    }
}