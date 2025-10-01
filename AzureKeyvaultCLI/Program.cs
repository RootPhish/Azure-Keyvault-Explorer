// See https://aka.ms/new-console-template for more information
using Azure.Security.KeyVault.Secrets;
using Library;
using System.CommandLine;

namespace AzureKeyvaultCLI
{
    internal static class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
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
                parseResult.GetValue(tenantIdOption)!,
                parseResult.GetValue(clientIdOption)!,
                parseResult.GetValue(keyvaultOption)!,
                parseResult.GetValue(secretOption)!));

            return rootCommand.Parse(args).Invoke();
        }

        static private void GetSecret(string tenantId, string clientId, string keyvaultName, string secretName)
        {
            var credential = new MsalDeviceCodeCredential(clientId, tenantId);
            var secretService = new AzureSecretService(credential);
            string value = secretService.GetSecretValue(keyvaultName, secretName);
            Console.WriteLine(value);
        }
    }
}
