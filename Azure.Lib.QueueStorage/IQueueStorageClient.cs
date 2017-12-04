using System;
using System.Threading.Tasks;

namespace Azure.Lib.QueueStorage
{
    public interface IQueueStorageClient<T>
    {
        Task Push(QueueObject<T> queueObject);

        Task<QueueObject<T>> Pop(TimeSpan? invisibleTimeSpan, Action<QueueObject<T>> action);
    }
}
