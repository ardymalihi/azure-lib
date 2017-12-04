using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Lib.EventHub
{
    public class EventHubReceiverSettings
    {
        public string ConnectionString { get; set; }

        public string EntityPath { get; set; }

        public string StorageContainerName { get; set; }

        public string StorageAccountName { get; set; }

        public string StorageAccountKey { get; set; }

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
