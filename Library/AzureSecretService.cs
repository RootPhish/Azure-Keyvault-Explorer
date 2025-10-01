using Azure.Core;
using Azure.ResourceManager;
using Azure.Security.KeyVault.Secrets;

namespace Library
{
    public class AzureSecretService(TokenCredential credential)
    {
        private TokenCredential _credential = credential;

        public async IAsyncEnumerable<string> GetSecretsAsync(string vault)
        {
            var secretClient = new SecretClient(new Uri($"https://{vault}.vault.azure.net/"), _credential);
            await foreach (SecretProperties props in secretClient.GetPropertiesOfSecretsAsync())
            {
                yield return props.Name;
            }
        }

        public string GetSecretValue(string vault, string secretName)
        {
            var secretClient = new SecretClient(new Uri($"https://{vault}.vault.azure.net/"), _credential);
            var secret = secretClient.GetSecret(secretName);
            return secret.Value.Value;
        }
    }
}
