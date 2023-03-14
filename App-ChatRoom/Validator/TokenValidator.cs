using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
namespace App_ChatRoom.Validator
{
        public interface ITokenValidator
        {
            Task<bool> IsTokenValidAsync(string token);
        }

        public class TokenValidator : ITokenValidator
        {
            private readonly NavigationManager _navigationManager;
            private readonly IJSRuntime _jsRuntime;

            public TokenValidator(NavigationManager navigationManager, IJSRuntime jsRuntime)
            {
                _navigationManager = navigationManager;
                _jsRuntime = jsRuntime;
            }

            public async Task<bool> IsTokenValidAsync(string token)
            {
                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }

                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadJwtToken(token);

                    if (jsonToken.ValidTo < DateTime.UtcNow)
                    {
                        return false;
                    }

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public async Task<bool> IsTokenValidFromLocalStorageAsync()
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "token");
                return await IsTokenValidAsync(token);
            }
        
    }
}
