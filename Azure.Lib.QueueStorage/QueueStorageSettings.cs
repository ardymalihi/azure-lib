using System;

namespace Azure.Lib.QueueStorage
{
    public class QueueStorageSettings
    {
        public string StorageAccount { get; }

        public string StorageKey { get; }

        public string QueueName { get; }

        public QueueStorageSettings(string storageAccount, string storageKey, string queueName)
        {
            if (string.IsNullOrWhiteSpace(storageAccount)) throw new ArgumentNullException("StorageAccount");

            if (string.IsNullOrWhiteSpace(storageKey)) throw new ArgumentNullException("StorageKey");

            if (string.IsNullOrWhiteSpace(queueName)) throw new ArgumentNullException("QueueName");

            StorageAccount = storageAccount;
            StorageKey = storageKey;
            QueueName = queueName;
        }
    }
}
