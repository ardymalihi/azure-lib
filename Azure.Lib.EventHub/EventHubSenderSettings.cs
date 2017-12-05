using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Lib.EventHub
{
    public class EventHubSenderSettings
    {
        public string ConnectionString { get; }

        public string EntityPath { get; }

        public EventHubSenderSettings(string connectionString, string entityPath)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException("ConnectionString");

            if (string.IsNullOrWhiteSpace(entityPath)) throw new ArgumentNullException("EntityPath");

            ConnectionString = connectionString;
            EntityPath = entityPath;
        }
    }
}
