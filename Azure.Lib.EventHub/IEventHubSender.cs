using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Lib.EventHub
{
    public interface IEventHubSender<T>
    {
        Task SendEvent(EventHubObject<T> eventHubObject);

        Task SendEvents(List<EventHubObject<T>> eventHubObjects);
    }
}
