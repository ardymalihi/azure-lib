using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Azure.Lib.EventHub
{
    public class EventMessage<T>
    {
        private EventData _eventData;

        public EventMessage(EventData eventData)
        {
            _eventData = eventData;
        }

        public string Message
        {
            get
            {
                return Encoding.UTF8.GetString(_eventData.Body.Array, _eventData.Body.Offset, _eventData.Body.Count);
            }
        }

        public T Data
        {
            get
            {
                return JsonConvert.DeserializeObject<T>(Message);
            }
        }
    }
}
