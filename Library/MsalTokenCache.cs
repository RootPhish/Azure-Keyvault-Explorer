using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;

namespace Library
{
    /// <summary>
    /// Persistent, encrypted (DPAPI on Windows) MSAL token cache shared by the GUI and the CLI,
    /// so users don't have to sign in again on every start.
    /// </summary>
    internal static class MsalTokenCache
    {
        private static readonly Lazy<MsalCacheHelper> _helper = new(CreateHelper);

        public static void Register(ITokenCache tokenCache) => _helper.Value.RegisterCache(tokenCache);

        private static MsalCacheHelper CreateHelper()
        {
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

            // Run on the thread pool so blocking here can't deadlock on a UI SynchronizationContext.
            return Task.Run(() => MsalCacheHelper.CreateAsync(storageProps)).GetAwaiter().GetResult();
        }
    }
}
