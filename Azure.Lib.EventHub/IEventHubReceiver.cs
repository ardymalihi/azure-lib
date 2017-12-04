using System.Threading.Tasks;

namespace Azure.Lib.EventHub
{
    public interface IEventHubReceiver<T>
    {
        event EventCloseHandler OnEventClose;

        event EventOpenHandler OnEventOpen;

        event EventErrorHandler OnEventError;

        event ProcessEventsHandler<T> OnProcessEvents;

        Task Start();

        Task Stop();
    }
}
