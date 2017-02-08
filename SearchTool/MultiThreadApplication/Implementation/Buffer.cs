using SearchTool.Interfaces;
using SearchTool.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SearchTool
{

    public class Buffer : IBuffer
    {
        private IBufferInterceptor _interceptor;

        private readonly Queue<Data> _queue = new Queue<Data>();
        private bool _stopped;
        private int _limit = 100;
        private int _count = 0;

        public void RegisterInterceptor(IBufferInterceptor interceptor)
        {
            _interceptor = interceptor;
        }

        // Добавить элемент в очередь
        public bool TryEnqueue(Data item)
        {
            // Если добавление в очередь больше не будет и кол-во ==_limit
            if (_stopped || _count == _limit)
                return false;

            lock (_queue)
            {
                if (_stopped || _count == _limit)
                    return false;
                // Если не null, добавить к буфферу часть предыдущего слова
                _interceptor?.Intercept(item);
                _queue.Enqueue(item);
                _count++;
                Monitor.Pulse(_queue);
            }
            return true;
        }

        // Извлечь элемент из очереди
        public Data Dequeue()
        {
            // Если добавление в очередь больше не будет и кол-во == 0 
            if (_stopped && _count == 0)
                return default(Data);
            lock (_queue)
            {
                if (_stopped && _count == 0)
                    return default(Data);
                // Подождать пока кол-во элементов станет !=0
                while (_count == 0)
                {
                    Monitor.Wait(_queue);
                    if (_stopped)
                        return default(Data);
                }
                _count--;
                return _queue.Dequeue();
            }
        }

        // Сообщить, что больше не будет добавления в очередь
        public void Stop()
        {
            if (_stopped)
                return;
            lock (_queue)
            {
                if (_stopped)
                    return;
                _stopped = true;
                Monitor.PulseAll(_queue);
            }
        }

    }

}
