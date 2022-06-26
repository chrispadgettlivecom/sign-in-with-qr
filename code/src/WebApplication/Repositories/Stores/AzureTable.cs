using Microsoft.Azure.Cosmos.Table;

namespace WebApplication.Repositories.Stores
{
    public class AzureTable : IAzureTable
    {
        private readonly CloudTable _innerTable;

        public AzureTable(CloudTable innerTable)
        {
            _innerTable = innerTable ?? throw new ArgumentNullException(nameof(innerTable));
        }

        public Task<TableResult> ExecuteAsync(TableOperation operation)
        {
            return _innerTable.ExecuteAsync(operation);
        }

        public async Task<IAzureTableQuerySegment<TElement>> ExecuteQuerySegmentedAsync<TElement>(TableQuery<TElement> query, TableContinuationToken continuationToken)
            where TElement : ITableEntity, new()
        {
            var innerTableQuerySegment = await _innerTable.ExecuteQuerySegmentedAsync(query, continuationToken);
            return new AzureTableQuerySegment<TElement>(innerTableQuerySegment);
        }
    }
}
