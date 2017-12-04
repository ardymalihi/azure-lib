namespace Azure.Lib.EventHub
{
    public class EventHubObject<T>
    {
        public string Message { get; set; }

        public T Data { get; set; }
    }
}
