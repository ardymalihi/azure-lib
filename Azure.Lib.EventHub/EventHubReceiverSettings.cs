using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Lib.EventHub
{
    public class EventHubReceiverSettings
    {
        public string ConnectionString { get; }

        public string EntityPath { get; }

        public string StorageContainerName { get; }

        public string StorageAccountName { get; }

        public string StorageAccountKey { get; }

        public EventHubReceiverSettings(string connectionString, string entityPath, string storageContainerName, string storageAccountName, string storageAccountKey)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException("ConnectionString");

            if (string.IsNullOrWhiteSpace(entityPath)) throw new ArgumentNullException("EntityPath");

            if (string.IsNullOrWhiteSpace(storageContainerName)) throw new ArgumentNullException("StorageContainerName");

            if (string.IsNullOrWhiteSpace(storageAccountName)) throw new ArgumentNullException("StorageAccountName");

            if (string.IsNullOrWhiteSpace(storageAccountKey)) throw new ArgumentNullException("StorageAccountKey");

            ConnectionString = connectionString;
            EntityPath = entityPath;
            StorageContainerName = storageContainerName;
            StorageAccountName = storageAccountName;
            StorageAccountKey = storageAccountKey;
        }
    }
}
