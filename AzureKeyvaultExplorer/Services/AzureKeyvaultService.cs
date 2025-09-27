using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.KeyVault;
using AzureKeyvaultExplorer.Classes;

namespace AzureKeyvaultExplorer.Services
{
    internal class AzureKeyvaultService
    {
        private MsalTokenCredential _credential;

        public AzureKeyvaultService(MsalTokenCredential credential)
        {
            _credential = credential;
        }

        public async IAsyncEnumerable<KeyvaultItem> GetKeyvaultsAsync(SubscriptionItem subItem)
        {
            var result = new List<KeyvaultItem>();
            var arm = new ArmClient(_credential);
            var sub = arm.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions/{subItem.ResourceId?.Name}"));
            var kvCollection = sub.GetKeyVaultsAsync();

            await foreach (var kv in kvCollection)
            {
                yield return new KeyvaultItem
                {
                    Name = kv.Data.Name,
                    ResourceId = kv.Id,
                    VaultUri = kv.Data.Properties.VaultUri.ToString(),
                };
            }
        }
    }
}
