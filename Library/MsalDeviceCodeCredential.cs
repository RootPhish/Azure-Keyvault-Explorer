using Azure.Core;
using Microsoft.Identity.Client;

namespace Library
{
    public class MsalDeviceCodeCredential : TokenCredential
    {
        private readonly IPublicClientApplication _app;

        public MsalDeviceCodeCredential(string clientId, string tenantId = "common")
        {
            _app = PublicClientApplicationBuilder
                .Create(clientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
                .WithRedirectUri("http://localhost") // must match app registration
                .Build();

            MsalTokenCache.Register(_app.UserTokenCache);
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
                result = await _app.AcquireTokenWithDeviceCode(scopes, deviceCodeCallback =>
                {
                    Console.WriteLine(deviceCodeCallback.Message);
                    return Task.CompletedTask;
                }).ExecuteAsync(cancellationToken);
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
