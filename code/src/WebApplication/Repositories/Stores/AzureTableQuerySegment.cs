using Microsoft.Azure.Cosmos.Table;

namespace WebApplication.Repositories.Stores
{
    public class AzureTableQuerySegment<TElement> : IAzureTableQuerySegment<TElement>
        where TElement : ITableEntity, new()
    {
        private readonly TableQuerySegment<TElement> _innerTableQuerySegment;

        public AzureTableQuerySegment(TableQuerySegment<TElement> innerTableQuerySegment)
        {
            _innerTableQuerySegment = innerTableQuerySegment ?? throw new ArgumentNullException(nameof(innerTableQuerySegment));
        }

        public TableContinuationToken ContinuationToken => _innerTableQuerySegment.ContinuationToken;

        public List<TElement> Results => _innerTableQuerySegment.Results;
    }
}
