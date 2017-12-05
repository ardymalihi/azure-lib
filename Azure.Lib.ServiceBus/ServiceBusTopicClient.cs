using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Lib.ServiceBus
{
    public class ServiceBusTopicClient : IDisposable
    {
        private ServiceBusTopicSettings _settings;

        private TopicClient _topicClient;

        public ServiceBusTopicClient(ServiceBusTopicSettings settings)
        {
            _settings = settings;
            _topicClient = new TopicClient(_settings.ConnectionString, _settings.TopicName);
        }

        public void Dispose()
        {
            _topicClient.CloseAsync();
        }

        public async Task SendMessageToTopic(string message)
        {
            var topicMessage = new Message(Encoding.UTF8.GetBytes(message));
            await _topicClient.SendAsync(topicMessage);
        }
    }
}
