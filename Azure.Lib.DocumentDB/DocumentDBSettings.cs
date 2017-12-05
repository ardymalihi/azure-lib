using System;

namespace Azure.Lib.DocumentDB
{
    public class DocumentDBSettings
    {
        public string EndpointUri { get; }
        public string PrimaryKey { get; }
        public string DatabaseName { get; }

        public DocumentDBSettings(string endpointUri, string primaryKey, string databaseName)
        {
            if (string.IsNullOrWhiteSpace(endpointUri)) throw new ArgumentNullException("EndpointUri");

            if (string.IsNullOrWhiteSpace(primaryKey)) throw new ArgumentNullException("PrimaryKey");

            if (string.IsNullOrWhiteSpace(databaseName)) throw new ArgumentNullException("DatabaseName");

            EndpointUri = endpointUri;
            PrimaryKey = primaryKey;
            DatabaseName = databaseName;
        }
    }
}
