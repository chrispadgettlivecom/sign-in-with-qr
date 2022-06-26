using WebApplication.Repositories.Stores;

namespace WebApplication.Repositories
{
    public abstract class AzureTableRepositoryBase
    {
        protected AzureTableRepositoryBase(IAzureTable table)
        {
            Table = table ?? throw new ArgumentNullException(nameof(table));
        }

        protected IAzureTable Table { get; }
    }
}
