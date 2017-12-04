using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Lib.TableStorage
{
    public class TableStorageClient<T> : ITableStorageClient<T> where T : ITableEntity, new()
    {
        private TableStorageSettings _settings;

        public TableStorageClient(TableStorageSettings settings)
        {
            _settings = settings;
        }

        private async Task<CloudTable> GetTableAsync()
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(_settings.StorageAccount, _settings.StorageKey), false);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(_settings.TableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }

        public async Task<List<T>> GetList()
        {
            CloudTable table = await GetTableAsync();
            TableQuery<T> query = new TableQuery<T>();

            List<T> results = new List<T>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<T> queryResults = await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);

            } while (continuationToken != null);

            return results;
        }

        public async Task<List<T>> GetList(string partitionKey)
        {
            CloudTable table = await GetTableAsync();
            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            List<T> results = new List<T>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<T> queryResults = await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);
            } while (continuationToken != null);

            return results;
        }

        public async Task<T> GetItem(string partitionKey, string rowKey)
        {
            CloudTable table = await GetTableAsync();
            TableOperation operation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            TableResult result = await table.ExecuteAsync(operation);

            return (T)result.Result;
        }

        public async Task Insert(T item)
        {
            CloudTable table = await GetTableAsync();
            TableOperation operation = TableOperation.Insert(item);
            await table.ExecuteAsync(operation);
        }

        public async Task Update(T item)
        {
            CloudTable table = await GetTableAsync();
            TableOperation operation = TableOperation.InsertOrReplace(item);
            await table.ExecuteAsync(operation);
        }

        public async Task Delete(string partitionKey, string rowKey)
        {
            T item = await GetItem(partitionKey, rowKey);
            CloudTable table = await GetTableAsync();
            TableOperation operation = TableOperation.Delete(item);
            await table.ExecuteAsync(operation);
        }
    }
}
