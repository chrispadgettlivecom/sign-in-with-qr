using Microsoft.Azure.Cosmos.Table;

namespace WebApplication.Repositories.Stores
{
    public interface IAzureTable
    {
        Task<TableResult> ExecuteAsync(TableOperation operation);

        Task<IAzureTableQuerySegment<TElement>> ExecuteQuerySegmentedAsync<TElement>(TableQuery<TElement> query, TableContinuationToken continuationToken)
            where TElement : ITableEntity, new();
    }
}
