using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Lib.EventHub
{
    public class EventHubSender<T> : IEventHubSender<T>
    {
        private EventHubSenderSettings _settings;

        public EventHubSender(EventHubSenderSettings settings)
        {
            _settings = settings;
        }

        private EventHubClient GetEventClient()
        {
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(_settings.ConnectionString)
            {
                EntityPath = _settings.EntityPath
            };

            return EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
        }

        public async Task SendEvent(EventHubObject<T> eventHubObject)
        {
            EventHubClient eventHubClient = GetEventClient();

            var text = JsonConvert.SerializeObject(eventHubObject);

            await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(text)));

            await eventHubClient.CloseAsync();
        }

        public async Task SendEvents(List<EventHubObject<T>> eventHubObjects)
        {
            EventHubClient eventHubClient = GetEventClient();

            foreach (var eventHubObject in eventHubObjects)
            {
                var text = JsonConvert.SerializeObject(eventHubObject);

                await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(text)));
            }

            await eventHubClient.CloseAsync();
        }
    }
}
