namespace AzureKeyvaultExplorer.Classes
{
    internal class SecretItem
    {
        public string Name { get; set; } = "";
        public string Value { get; set; } = "";

        public override string ToString() => Name;
    }
}
