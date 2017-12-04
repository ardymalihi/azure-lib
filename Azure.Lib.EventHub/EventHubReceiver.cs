using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Lib.EventHub
{
    public class EventHubReceiver<T> : IEventHubReceiver<T>
    {
        private EventHubReceiverSettings _settings;

        private EventProcessorHost _eventProcessorHost;

        public event EventCloseHandler OnEventClose;

        public event EventOpenHandler OnEventOpen;

        public event EventErrorHandler OnEventError;

        public event ProcessEventsHandler<T> OnProcessEvents;

        public EventHubReceiver(EventHubReceiverSettings settings)
        {
            _settings = settings;
        }

        public async Task Start()
        {
            _eventProcessorHost = new EventProcessorHost(
                _settings.EntityPath,
                PartitionReceiver.DefaultConsumerGroupName,
                _settings.ConnectionString,
                string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", _settings.StorageAccountName, _settings.StorageAccountKey),
                _settings.StorageContainerName);

            EventProcessorFactory<T> eventProcessorFactory = new EventProcessorFactory<T>();

            await _eventProcessorHost.RegisterEventProcessorFactoryAsync(eventProcessorFactory);

            eventProcessorFactory.Processor.OnEventClose += (context, reason) => {
                OnEventClose?.Invoke(context, reason);
            };

            eventProcessorFactory.Processor.OnEventOpen += (context) => {
                OnEventOpen?.Invoke(context);
            };

            eventProcessorFactory.Processor.OnEventError += (context, error) => {
                OnEventError?.Invoke(context, error);
            };

            eventProcessorFactory.Processor.OnProcessEvents += (context, eventMessages) => {
                OnProcessEvents?.Invoke(context, eventMessages);
            };

        }

        public async Task Stop()
        {
            if (_eventProcessorHost != null)
            {
                await _eventProcessorHost.UnregisterEventProcessorAsync();
            }
        }
    }
}
