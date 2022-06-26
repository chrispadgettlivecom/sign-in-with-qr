using Microsoft.Azure.Cosmos.Table;
using WebApplication.Entities;
using WebApplication.Providers;
using WebApplication.Repositories.Stores;

namespace WebApplication.Repositories
{
    public class AzureTableDeviceAuthRepository : AzureTableWithHashKeyRepositoryBase, IDeviceAuthRepository
    {
        public AzureTableDeviceAuthRepository(IAzureTable table, IHashProvider hashProvider)
            : base(table, hashProvider)
        {
        }

        public async Task DeleteDeviceAuth(string deviceCode)
        {
            var entity = await GetDeviceAuth(deviceCode);

            if (entity != null)
            {
                var tableOperation = TableOperation.Delete(entity);
                await Table.ExecuteAsync(tableOperation);
            }
        }

        public async Task<DeviceUserAuthEntity> GetDeviceAuth(string deviceCode)
        {
            var (partitionKey, rowKey) = await GenerateKeys(deviceCode);
            var tableOperation = TableOperation.Retrieve<DeviceUserAuthEntity>(partitionKey, rowKey);
            var tableResult = await Table.ExecuteAsync(tableOperation);
            var entity = (DeviceUserAuthEntity)tableResult.Result;
            return entity;
        }

        public async Task SaveDeviceAuth(string deviceCode, DeviceUserAuthEntity entity)
        {
            var (partitionKey, rowKey) = await GenerateKeys(deviceCode);
            entity.PartitionKey = partitionKey;
            entity.RowKey = rowKey;
            var tableOperation = TableOperation.InsertOrReplace(entity);
            await Table.ExecuteAsync(tableOperation);
        }
    }
}
