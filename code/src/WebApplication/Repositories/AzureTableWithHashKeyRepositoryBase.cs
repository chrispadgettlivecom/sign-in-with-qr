using System.Security.Cryptography;
using WebApplication.Providers;
using WebApplication.Repositories.Stores;

namespace WebApplication.Repositories
{
    public abstract class AzureTableWithHashKeyRepositoryBase : AzureTableRepositoryBase
    {
        protected AzureTableWithHashKeyRepositoryBase(IAzureTable table, IHashProvider hashProvider)
            : base(table)
        {
            HashProvider = hashProvider ?? throw new ArgumentNullException(nameof(hashProvider));
        }

        protected IHashProvider HashProvider { get; }

        protected async Task<(string, string)> GenerateKeys(string key)
        {
            var keyHash = await HashProvider.GenerateHash<SHA1CryptoServiceProvider>(key.ToLowerInvariant());
            var partitionKey = keyHash.Substring(0, 5);
            var rowKey = keyHash;
            return (partitionKey, rowKey);
        }
    }
}
