using System;

namespace Azure.Lib.BlobStorage
{
    public class BlobStorageSettings
    {
        public string StorageAccount { get; }

        public string StorageKey { get; }

        public string ContainerName { get; }

        public BlobStorageSettings(string storageAccount, string storageKey, string containerName)
        {
            if (string.IsNullOrWhiteSpace(storageAccount)) throw new ArgumentNullException("StorageAccount");

            if (string.IsNullOrWhiteSpace(storageKey)) throw new ArgumentNullException("StorageKey");

            if (string.IsNullOrWhiteSpace(containerName)) throw new ArgumentNullException("containerName");

            StorageAccount = storageAccount;
            StorageKey = storageKey;
            ContainerName = containerName;
        }
    }
}
