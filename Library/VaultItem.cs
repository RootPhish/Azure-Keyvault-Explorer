namespace Library
{
    public sealed class VaultItem
    {
        public required string Name { get; init; }
        public required string SubscriptionId { get; init; }
        public required Uri VaultUri { get; init; }

        public override string ToString() => Name;
    }
}
