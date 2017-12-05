using System;

namespace Azure.Lib.ServiceBus
{
    public class ServiceBusTopicSettings
    {
        public string ConnectionString { get; }

        public string TopicName { get; }

        public ServiceBusTopicSettings(string connectionString, string topicName)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException("ConnectionString");

            if (string.IsNullOrWhiteSpace(topicName)) throw new ArgumentNullException("TopicName");

            ConnectionString = connectionString;
            TopicName = topicName;
        }
    }
}
