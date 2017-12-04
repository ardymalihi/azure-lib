using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Lib.EventHub
{
    public delegate void EventCloseHandler(PartitionContext context, CloseReason reason);

    public delegate void EventOpenHandler(PartitionContext context);

    public delegate void EventErrorHandler(PartitionContext context, Exception error);

    public delegate void ProcessEventsHandler<T>(PartitionContext context, IEnumerable<EventMessage<T>> eventMessages);

    public class EventProcessorFactory<T> : IEventProcessorFactory
    {
        private EventProcessor<T> _eventProcessor;

        public EventProcessor<T> Processor
        {
            get
            {
                return _eventProcessor;
            }
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            _eventProcessor = new EventProcessor<T>();
            return _eventProcessor;
        }
    }

    public class EventProcessor<T> : IEventProcessor
    {
        public event EventCloseHandler OnEventClose;

        public event EventOpenHandler OnEventOpen;

        public event EventErrorHandler OnEventError;

        public event ProcessEventsHandler<T> OnProcessEvents;

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            OnEventClose?.Invoke(context, reason);
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            OnEventOpen?.Invoke(context);
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            OnEventError?.Invoke(context, error);
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            List<EventMessage<T>> eventMessages = new List<EventMessage<T>>();

            foreach (var message in messages)
            {
                eventMessages.Add(new EventMessage<T>(message));
            }

            OnProcessEvents?.Invoke(context, eventMessages);

            return context.CheckpointAsync();
        }
    }
}
