using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Azure;
using Azure.Core;
using Azure.Security.KeyVault.Secrets;

namespace Library
{
    public class AzureSecretService(TokenCredential credential)
    {
        private readonly TokenCredential _credential = credential;
        private readonly ConcurrentDictionary<Uri, SecretClient> _clients = new();

        public static Uri VaultUriFromName(string vaultName) => new($"https://{vaultName}.vault.azure.net/");

        private SecretClient GetClient(Uri vaultUri) =>
            _clients.GetOrAdd(vaultUri, uri => new SecretClient(uri, _credential));

        /// <summary>
        /// Streams secret names one page at a time (Key Vault returns at most 25 per page and pages
        /// must be fetched sequentially), so callers can show results while the rest is still loading.
        /// </summary>
        public async IAsyncEnumerable<IReadOnlyList<string>> GetSecretPagesAsync(
            Uri vaultUri,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var pages = GetClient(vaultUri).GetPropertiesOfSecretsAsync(cancellationToken).AsPages();
            await foreach (Page<SecretProperties> page in pages.WithCancellation(cancellationToken))
            {
                yield return page.Values.Select(p => p.Name).ToList();
            }
        }

        public async Task<string> GetSecretValueAsync(Uri vaultUri, string secretName, CancellationToken cancellationToken = default)
        {
            var secret = await GetClient(vaultUri).GetSecretAsync(secretName, cancellationToken: cancellationToken);
            return secret.Value.Value;
        }

        /// <summary>Synchronous lookup by vault name (used by the CLI).</summary>
        public string GetSecretValue(string vaultName, string secretName)
        {
            var secret = GetClient(VaultUriFromName(vaultName)).GetSecret(secretName);
            return secret.Value.Value;
        }
    }
}
