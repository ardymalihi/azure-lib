namespace Azure.Lib.QueueStorage
{
    public class QueueObject<T>
    {
        public string Message { get; set; }

        public T Data { get; set; }
    }
}
