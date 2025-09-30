using Azure.Core;
using Azure.ResourceManager.KeyVault;
using AzureKeyvaultExplorer.Classes;
using AzureKeyvaultExplorer.Services;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace AzureKeyvaultExplorer
{
    internal static class Program
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);
        
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int ATTACH_PARENT_PROCESS = -1;
        private const int SW_HIDE = 0;

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                RunConsole(args);
            } else
            {
                HideConsole();
                ApplicationConfiguration.Initialize();
                Application.Run(new MainForm());
            }
        }

        static void RunConsole(string[] args)
        {
            bool consoleAttached = AttachConsole(ATTACH_PARENT_PROCESS);
            if (!consoleAttached)
            {
                AllocConsole();
            }
            
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: AzureKeyvaultExplorer <Subscription> <KeyVaultName> <SecretName>\n");
                return;
            }

            string tenantId = Properties.Settings.Default.TenantID;
            string clientId = Properties.Settings.Default.ClientID;
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(clientId))
            {
                Console.WriteLine("TenantID and ClientID must be set in the application settings.");
                return;
            }

            string subscription = args[0];
            string keyVaultName = args[1];
            string secretName = args[2];
            var credential = new Classes.MsalTokenCredential(clientId, tenantId);

            var arm = new Azure.ResourceManager.ArmClient(credential);
            var sub = arm.GetSubscriptions().Where(s => s.Data.DisplayName.Equals(subscription, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            var kv = sub.GetKeyVaults().Where(v => v.Data.Name.Equals(keyVaultName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            AzureSecretService secretService = new AzureSecretService(credential);
            KeyvaultItem kvItem = new KeyvaultItem
            {
                Name = kv.Data.Name,
                ResourceId = kv.Id,
                VaultUri = kv.Data.Properties.VaultUri.ToString(),
            };
            var secret = secretService.GetSecretValue(kvItem, secretName);
            Console.WriteLine(secret);
            FreeConsole();
        }

        static void HideConsole()
        {
            var handle = GetConsoleWindow();
            if (handle != IntPtr.Zero)
            {
                ShowWindow(handle, SW_HIDE);
            }
        }

    }
}