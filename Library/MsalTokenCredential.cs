using Azure.Core;
using Microsoft.Identity.Client;

namespace Library
{
    public class MsalTokenCredential : TokenCredential
    {
        private readonly IPublicClientApplication _app;

        public MsalTokenCredential(string clientId, string tenantId = "common")
        {
            _app = PublicClientApplicationBuilder
                .Create(clientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
                .WithRedirectUri("http://localhost") // must match app registration
                .Build();
        }

        public override async ValueTask<AccessToken> GetTokenAsync(
            TokenRequestContext requestContext,
            CancellationToken cancellationToken)
        {
            var scopes = requestContext.Scopes;
            AuthenticationResult result;

            try
            {
                // Try silent first
                var accounts = await _app.GetAccountsAsync();
                result = await _app.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                                   .ExecuteAsync(cancellationToken);
            }
            catch (MsalUiRequiredException)
            {
                // Interactive fallback if consent is required for this resource
                result = await _app.AcquireTokenInteractive(scopes)
                                   .WithPrompt(Prompt.SelectAccount)
                                   .ExecuteAsync(cancellationToken);
            }

            return new AccessToken(result.AccessToken, result.ExpiresOn);
        }

        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            // Call the async version and block
            return GetTokenAsync(requestContext, cancellationToken).GetAwaiter().GetResult();
        }
    }
}
