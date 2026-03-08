using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MobileClient.AuthStateProvider
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await SecureStorage.GetAsync("accessToken");

            if (string.IsNullOrWhiteSpace(token))
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        public void NotifyUserAuthentication(string token)
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void NotifyUserLogout()
        {
            SecureStorage.Remove("accessToken");
            SecureStorage.Remove("refreshToken");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = Convert.FromBase64String(PadBase64(payload));
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            //return keyValuePairs.Select(kvp =>
            //    new Claim(kvp.Key, kvp.Value.ToString()));

            var claims = new List<Claim>();

            foreach (var kvp in keyValuePairs)
            {
                switch (kvp.Key)
                {
                    case JwtRegisteredClaimNames.Sub:
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, kvp.Value.ToString()));
                        break;
                    case "role":    //Role pretvaramo u ClaimTypes.Role kako bi ih se moglo prepoznati na fronti
                        claims.Add(new Claim(ClaimTypes.Role, kvp.Value.ToString()));
                        break;
                    default:
                        claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
                        break;
                }
                //if (kvp.Key == "role")
                //{
                //    //Role pretvaramo u ClaimTypes.Role kako bi ih se moglo prepoznati na fronti
                //    claims.Add(new Claim(ClaimTypes.Role, kvp.Value.ToString()));
                //}
                //else
                //{
                //    claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
                //}
            }

            return claims;
        }

        private string PadBase64(string base64)
        {
            return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        }
    }
}
