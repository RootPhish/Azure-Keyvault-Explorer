using Azure.ResourceManager;

namespace Library
{
    public class AzureSubscriptionService(MsalTokenCredential credential)
    {
        private MsalTokenCredential _credential = credential;

        public async IAsyncEnumerable<SubscriptionItem> GetSubscriptionsAsync()
        {
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
