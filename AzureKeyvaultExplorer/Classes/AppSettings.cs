namespace AzureKeyvaultExplorer.Classes
{
    internal class AppSettings
    {
        public string TenantID { get; set; } = "00000000-0000-0000-0000-000000000000";
        public string ClientID { get; set; } = "00000000-0000-0000-0000-000000000000";
        public int ClearClipboardAfterSeconds { get; set; } = 10;
        public string TOTP { get; set; } = "TOTP";
        public bool AutoConnect { get; set; } = true;

        // Remembered between sessions (not shown in Preferences).
        public string? LastSubscriptionId { get; set; }
    }
}