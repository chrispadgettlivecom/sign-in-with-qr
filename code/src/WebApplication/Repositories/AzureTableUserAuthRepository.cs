using Microsoft.Azure.Cosmos.Table;
using WebApplication.Entities;
using WebApplication.Providers;
using WebApplication.Repositories.Stores;

namespace WebApplication.Repositories
{
    public class AzureTableUserAuthRepository : AzureTableWithHashKeyRepositoryBase, IUserAuthRepository
    {
        public AzureTableUserAuthRepository(IAzureTable table, IHashProvider hashProvider)
            : base(table, hashProvider)
        {
        }

        public async Task DeleteUserAuth(string userCode)
        {
            var entity = await GetUserAuth(userCode);

            if (entity != null)
            {
                var tableOperation = TableOperation.Delete(entity);
                await Table.ExecuteAsync(tableOperation);
            }
        }

        public async Task<DeviceUserAuthEntity> GetUserAuth(string userCode)
        {
            var (partitionKey, rowKey) = await GenerateKeys(userCode);
            var tableOperation = TableOperation.Retrieve<DeviceUserAuthEntity>(partitionKey, rowKey);
            var tableResult = await Table.ExecuteAsync(tableOperation);
            var entity = (DeviceUserAuthEntity)tableResult.Result;
            return entity;
        }

        public async Task SaveUserAuth(string userCode, DeviceUserAuthEntity entity)
        {
            var (partitionKey, rowKey) = await GenerateKeys(userCode);
            entity.PartitionKey = partitionKey;
            entity.RowKey = rowKey;
            var tableOperation = TableOperation.InsertOrReplace(entity);
            await Table.ExecuteAsync(tableOperation);
        }
    }
}
