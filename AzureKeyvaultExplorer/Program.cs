using Azure.Security.KeyVault.Secrets;
using System.CommandLine;
using System.CommandLine.Parsing;

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
        static int Main(string[] args)
        {
            if (args.Length > 0)
            {
                return RunConsole(args);
            } else
            {
                HideConsole();
                ApplicationConfiguration.Initialize();
                Application.Run(new MainForm());
                return 0;
            }
        }

        static int RunConsole(string[] args)
        {
            bool consoleAttached = AttachConsole(ATTACH_PARENT_PROCESS);
            if (!consoleAttached)
            {
                AllocConsole();
            }

            Option<string> tenantIdOption = new("--tenantid", "-t")
            {
                Description = "The ID of the Azure Tenant in which the App Registration lives.",
                Required = true
            };
            Option<string> clientIdOption = new("--clientid", "-c")
            {
                Description = "The Client ID of the App Registration.",
                Required = true
            };
            Option<string> keyvaultOption = new("--keyvault", "-k")
            {
                Description = "The name of the Keyvault in which the secret resides.",
                Required = true
            };
            Option<string> secretOption = new("--secret", "-s")
            {
                Description = "The name of the Secret you want to read.",
                Required = true
            };

            RootCommand rootCommand = new("Azure Keyvault Explorer");
            rootCommand.Options.Add(tenantIdOption);
            rootCommand.Options.Add(clientIdOption);
            rootCommand.Options.Add(keyvaultOption);
            rootCommand.Options.Add(secretOption);

            rootCommand.SetAction(parseResult => GetSecret(
                parseResult.GetValue(tenantIdOption),
                parseResult.GetValue(clientIdOption),
                parseResult.GetValue(keyvaultOption),
                parseResult.GetValue(secretOption)));

            return rootCommand.Parse(args).Invoke();
        }

        static private void GetSecret(string tenantId, string clientId, string keyvaultName, string secretName)
        {
            var credential = new Classes.MsalDeviceCodeCredential(clientId, tenantId);
            var client = new SecretClient(new Uri($"https://{keyvaultName}.vault.azure.net/"), credential);
            KeyVaultSecret secret = client.GetSecret(secretName);
            Console.WriteLine(secret.Value);
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