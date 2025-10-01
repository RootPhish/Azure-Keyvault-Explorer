using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.KeyVault;

namespace Library
{
    public class AzureKeyvaultService(MsalTokenCredential credential)
    {
        private MsalTokenCredential _credential = credential;

        public async IAsyncEnumerable<string> GetKeyvaultsAsync(SubscriptionItem subItem)
        {
            var arm = new ArmClient(_credential);
            var sub = arm.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions/{subItem.ResourceId?.Name}"));
            var kvCollection = sub.GetKeyVaultsAsync();

            await foreach (var kv in kvCollection)
            {
                yield return new string(kv.Data.Name);
            }
        }
    }
}
