using Microsoft.Azure.Cosmos.Table;

namespace WebApplication.Repositories.Stores
{
    public interface IAzureTableQuerySegment<TElement>
        where TElement : ITableEntity, new()
    {
        TableContinuationToken ContinuationToken { get; }

        List<TElement> Results { get; }
    }
}
