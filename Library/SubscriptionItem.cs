namespace Library
{
    public class SubscriptionItem
    {
        public string Name { get; set; } = "";
        public Azure.Core.ResourceIdentifier? ResourceId { get; set; }

        public override string ToString() => Name;
    }
}
