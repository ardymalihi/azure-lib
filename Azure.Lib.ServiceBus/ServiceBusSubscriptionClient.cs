using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Lib.ServiceBus
{
    public delegate void ProcessMessageHandler(string message);

    public class ServiceBusSubscriptionClient
    {
        public event ProcessMessageHandler OnProcessMessage;

        private ServiceBusSubscriptionSettings _settings;

        private SubscriptionClient _subscriptionClient;

        public ServiceBusSubscriptionClient(ServiceBusSubscriptionSettings settings)
        {
            _settings = settings;
            _subscriptionClient = new SubscriptionClient(_settings.ConnectionString, _settings.TopicName,_settings.SubscriptionName);
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _subscriptionClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            return Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(Message message, CancellationToken token)
        {
            OnProcessMessage?.Invoke(Encoding.UTF8.GetString(message.Body));
            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        public void Dispose()
        {
            _subscriptionClient.CloseAsync();
        }


    }
}
