using System;

namespace Azure.Lib.ServiceBus
{
    public class ServiceBusSubscriptionSettings
    {
        public string ConnectionString { get; }

        public string TopicName { get; }

        public string SubscriptionName { get; }
        
        public ServiceBusSubscriptionSettings(string connectionString, string topicName, string subscriptionName)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException("ConnectionString");

            if (string.IsNullOrWhiteSpace(topicName)) throw new ArgumentNullException("TopicName");

            if (string.IsNullOrWhiteSpace(subscriptionName)) throw new ArgumentNullException("SubscriptionName");

            ConnectionString = connectionString;
            TopicName = topicName;
            SubscriptionName = subscriptionName;
        }
    }
}
