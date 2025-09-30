using Azure.Core;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;

namespace AzureKeyvaultExplorer.Classes
{
    public class MsalDeviceCodeCredential : TokenCredential
    {
        private readonly IPublicClientApplication _app;
        private readonly MsalCacheHelper _cacheHelper;

        public MsalDeviceCodeCredential(string clientId, string tenantId = "common")
        {
            _app = PublicClientApplicationBuilder
                .Create(clientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
                .WithRedirectUri("http://localhost") // must match app registration
                .Build();

            var storageProps = new StorageCreationPropertiesBuilder(
                    cacheFileName: "azurekeyvaultexplorer.msalcache",
                    cacheDirectory: MsalCacheHelper.UserRootDirectory)
                .WithLinuxKeyring(
                    schemaName: "com.contoso.azurekeyvaultexplorer",
                    collection: "default",
                    secretLabel: "MSAL token cache for Azure Keyvault Explorer",
                    attribute1: new KeyValuePair<string, string>("Version", "1"),
                    attribute2: new KeyValuePair<string, string>("Product", "AzureKeyvaultExplorer"))
                .WithMacKeyChain(
                    serviceName: "com.contoso.azurekeyvaultexplorer",
                    accountName: "AzureKeyvaultExplorer")
                .Build();

            var cacheHelperTask = MsalCacheHelper.CreateAsync(storageProps, null);
            _cacheHelper = cacheHelperTask.GetAwaiter().GetResult();
            _cacheHelper.RegisterCache(_app.UserTokenCache);
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
