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

        //public BlockingCollection<Data> listData = new BlockingCollection<Data>();
        private IBufferInterceptor _interceptor;

        private readonly Queue<Data> _queue = new Queue<Data>();
        private bool _stopped;
        private int _limit = 100;
        private int _count = 0;

        public void RegisterInterceptor(IBufferInterceptor interceptor)
        {
            _interceptor = interceptor;
        }

        public bool TryEnqueue(Data item)
        {
            if (_stopped || _count == _limit)
                return false;

            lock (_queue)
            {
                if (_stopped || _count == _limit)
                    return false;
                _interceptor?.Intercept(item);
                _queue.Enqueue(item);
                _count++;
                Monitor.Pulse(_queue);
            }
            return true;
        }

        public Data Dequeue()
        {
            if (_stopped && _count == 0)
                return default(Data);
            lock (_queue)
            {
                if (_stopped && _count == 0)
                    return default(Data);
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
