using SearchTool.Interfaces;
using SearchTool.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SearchTool
{
    public class Buffer : IBuffer
    {
        private ConcurrentQueue<Data> listData = new ConcurrentQueue<Data>();

        public IBufferInterceptor interceptor;

        private SemaphoreSlim ss = new SemaphoreSlim(1);

        public void Add(Data data)
        {
            if (interceptor != null)
                interceptor.Intercept(data);

            ss.Wait();

            listData.Enqueue(data);

            ss.Release();
        }

        public Data Get()
        {
            ss.Wait();
            Data data;
            listData.TryDequeue(out data);
            ss.Release();
                return data;
        }
    }


   
}
