namespace AzureKeyvaultExplorer.Classes
{
    internal class KeyvaultItem
    {
        public string Name { get; set; } = "";
        public string VaultUri { get; set; } = "";
        public Azure.Core.ResourceIdentifier? ResourceId { get; set; }

        public override string ToString() => Name;
    }
}
