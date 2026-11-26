using Azure.ResourceManager;

namespace Library
{
    public class AzureSubscriptionService(ArmClient arm)
    {
        private readonly ArmClient _arm = arm;

        public async IAsyncEnumerable<SubscriptionItem> GetSubscriptionsAsync()
        {
            await foreach (var sub in _arm.GetSubscriptions().GetAllAsync())
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
