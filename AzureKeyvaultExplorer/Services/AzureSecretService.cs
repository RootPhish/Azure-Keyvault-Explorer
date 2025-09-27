using Azure.ResourceManager;
using Azure.Security.KeyVault.Secrets;
using AzureKeyvaultExplorer.Classes;

namespace AzureKeyvaultExplorer.Services
{
    internal class AzureSecretService
    {
        private MsalTokenCredential _credential;

        public AzureSecretService(MsalTokenCredential credential)
        {
            _credential = credential;
        }

        public async IAsyncEnumerable<string> GetSecretsAsync(KeyvaultItem vault)
        {
            var result = new List<SubscriptionItem>();
            var arm = new ArmClient(_credential);
            var subs = arm.GetSubscriptions().GetAllAsync();

            var secretClient = new SecretClient(new Uri(vault.VaultUri), _credential);
            await foreach (SecretProperties props in secretClient.GetPropertiesOfSecretsAsync())
            {
                yield return props.Name;
            }
        }

        public string GetSecretValue(KeyvaultItem vault, string secretName)
        {
            var secretClient = new SecretClient(new Uri(vault.VaultUri), _credential);
            var secret = secretClient.GetSecret(secretName);
            return secret.Value.Value;
        }
    }
}
