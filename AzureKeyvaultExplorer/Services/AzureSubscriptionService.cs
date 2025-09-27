using Azure.ResourceManager;
using AzureKeyvaultExplorer.Classes;

namespace AzureKeyvaultExplorer.Services
{
    internal class AzureSubscriptionService
    {
        private MsalTokenCredential _credential;

        public AzureSubscriptionService(MsalTokenCredential credential)
        {
            _credential = credential;
        }

        public async IAsyncEnumerable<SubscriptionItem> GetSubscriptionsAsync()
        {
            var result = new List<SubscriptionItem>();
            var arm = new ArmClient(_credential);
            var subs = arm.GetSubscriptions().GetAllAsync();

            await foreach (var sub in subs)
            {
                yield return new SubscriptionItem
                {
                    Name = sub.Data.DisplayName,
                    ResourceId = sub.Id,
                };
            }
        }
    }
}
