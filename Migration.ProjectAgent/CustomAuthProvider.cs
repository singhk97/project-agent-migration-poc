using Microsoft.Teams.Api.Auth;
using Microsoft.Teams.Common.Http;

namespace Migration.ProjectAgent
{
    // <summary>
    // Custom authentication provider for Teams AI that uses a token service and client credentials.
    // Showcases how PA auth provider can be hooked right into Teams AI.
    // </summary>
    public class CustomAuthProvider : IHttpCredentials
    {
        TokenService _tokenService;
        ClientCredentials _clientCredentials;

        public CustomAuthProvider(TokenService tokenService, ClientCredentials clientCredentials) 
        { 
            _tokenService = tokenService;
            _clientCredentials = clientCredentials;
        }

        public async Task<ITokenResponse> Resolve(IHttpClient client, string[] scopes, CancellationToken cancellationToken = default)
        {
            // PLACEHOLDER FOR PA:
            //string accessToken = await _tokenService.GetAppTokenAsync(cancellationToken);

            //return new TokenResponse { 
            //    TokenType = "Bearer",
            //    AccessToken = accessToken,
            //    // ExpiresIn = 3000 // ExpiresIn being get only can be problematic
            //};
            
            return await _clientCredentials.Resolve(client, scopes, cancellationToken);
        }
    }
}
