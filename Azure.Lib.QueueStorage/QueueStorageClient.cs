using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Lib.QueueStorage
{
    public class QueueStorageClient<T> : IQueueStorageClient<T>
    {
        private QueueStorageSettings _settings;

        public QueueStorageClient(QueueStorageSettings settings)
        {
            _settings = settings;
        }

        private async Task<CloudQueue> GetQueueAsync()
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(_settings.StorageAccount, _settings.StorageKey), false);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(_settings.QueueName);
            await queue.CreateIfNotExistsAsync();
            return queue;
        }

        public async Task Push(QueueObject<T> queueObject)
        {
            CloudQueue queue = await GetQueueAsync();
            var text = JsonConvert.SerializeObject(queueObject);
            CloudQueueMessage message = new CloudQueueMessage(text);
            await queue.AddMessageAsync(message);

        }

        public async Task<QueueObject<T>> Pop(TimeSpan? invisibleTimeSpan = null, Action < QueueObject<T>> action = null)
        {
            CloudQueue queue = await GetQueueAsync();
            CloudQueueMessage peekedMessage = await queue.GetMessageAsync(invisibleTimeSpan, new QueueRequestOptions(), new OperationContext());
            if (peekedMessage != null)
            {
                QueueObject<T> queueObject = JsonConvert.DeserializeObject<QueueObject<T>>(peekedMessage.AsString);
                if (action != null)
                {
                    action(queueObject);
                    await queue.DeleteMessageAsync(peekedMessage);
                }

                return queueObject;
            }
            else
            {
                return null;
            }
        }
        
    }
}
