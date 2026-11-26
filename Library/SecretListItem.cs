namespace Library
{
    public sealed class SecretListItem
    {
        public required string Name { get; init; }
        public bool Enabled { get; init; } = true;

        public override string ToString() => Enabled ? Name : $"{Name} (disabled)";
    }
}
