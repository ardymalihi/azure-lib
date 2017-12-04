using System;

namespace Azure.Lib.TableStorage
{
    public class TableStorageSettings
    {
        public string StorageAccount { get; }

        public string StorageKey { get; }

        public string TableName { get; }

        public TableStorageSettings(string storageAccount, string storageKey, string tableName)
        {
            if (string.IsNullOrWhiteSpace(storageAccount)) throw new ArgumentNullException("StorageAccount");

            if (string.IsNullOrWhiteSpace(storageKey)) throw new ArgumentNullException("StorageKey");

            StorageAccount = storageAccount;
            StorageKey = storageKey;
            TableName = tableName;
        }
    }
}
