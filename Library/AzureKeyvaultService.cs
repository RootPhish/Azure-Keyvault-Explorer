using System.Text.Json;
using Azure.ResourceManager;
using Azure.ResourceManager.ResourceGraph;
using Azure.ResourceManager.ResourceGraph.Models;
using Azure.ResourceManager.Resources;

namespace Library
{
    public class AzureKeyvaultService(ArmClient arm)
    {
        // Resource Graph returns only the columns we need, for all subscriptions in a single query,
        // instead of the full vault definitions (access policies etc.) per subscription.
        // 'id' must be projected for Resource Graph paging (skipToken) to work.
        private const string VaultQuery =
            "resources " +
            "| where type =~ 'microsoft.keyvault/vaults' " +
            "| project id, name, subscriptionId, vaultUri = tostring(properties.vaultUri)";

        // Resource Graph accepts at most 1000 subscriptions per request.
        private const int MaxSubscriptionsPerQuery = 1000;

        private readonly ArmClient _arm = arm;

        /// <summary>
        /// Returns the key vaults of all given subscriptions, keyed by subscription id (case-insensitive),
        /// each list sorted by name. Every requested subscription has an entry, even if it has no vaults.
        /// </summary>
        public async Task<Dictionary<string, List<VaultItem>>> GetKeyvaultsBySubscriptionAsync(
            IEnumerable<string> subscriptionIds,
            CancellationToken cancellationToken = default)
        {
            var ids = subscriptionIds.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            var result = ids.ToDictionary(id => id, _ => new List<VaultItem>(), StringComparer.OrdinalIgnoreCase);
            if (ids.Count == 0)
                return result;

            // The Resource Graph endpoint is not tenant-scoped in its URL (the token decides the tenant),
            // so any TenantResource works as the entry point.
            TenantResource? tenant = null;
            await foreach (var t in _arm.GetTenants().GetAllAsync(cancellationToken))
            {
                tenant = t;
                break;
            }
            if (tenant == null)
                throw new InvalidOperationException("No Azure tenant is accessible with the current account.");

            foreach (var chunk in ids.Chunk(MaxSubscriptionsPerQuery))
            {
                string? skipToken = null;
                do
                {
                    var content = new ResourceQueryContent(VaultQuery)
                    {
                        Options = new ResourceQueryRequestOptions
                        {
                            ResultFormat = ResultFormat.ObjectArray,
                            SkipToken = skipToken,
                        },
                    };
                    foreach (var id in chunk)
                        content.Subscriptions.Add(id);

                    ResourceQueryResult page = await tenant.GetResourcesAsync(content, cancellationToken);

                    using (var doc = JsonDocument.Parse(page.Data.ToMemory()))
                    {
                        foreach (var row in doc.RootElement.EnumerateArray())
                        {
                            string name = row.GetProperty("name").GetString() ?? "";
                            string subId = row.GetProperty("subscriptionId").GetString() ?? "";
                            string? uri = row.TryGetProperty("vaultUri", out var u) ? u.GetString() : null;

                            if (name.Length == 0 || !result.TryGetValue(subId, out var list))
                                continue;

                            list.Add(new VaultItem
                            {
                                Name = name,
                                SubscriptionId = subId,
                                VaultUri = string.IsNullOrEmpty(uri)
                                    ? AzureSecretService.VaultUriFromName(name)
                                    : new Uri(uri),
                            });
                        }
                    }

                    skipToken = page.SkipToken;
                } while (!string.IsNullOrEmpty(skipToken));
            }

            foreach (var list in result.Values)
                list.Sort((a, b) => StringComparer.OrdinalIgnoreCase.Compare(a.Name, b.Name));

            return result;
        }
    }
}
