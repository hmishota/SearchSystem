using Microsoft.Practices.Unity;
using SearchTool.Interfaces;
using SearchTool.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SearchTool
{
    public interface IReadCounter
    {
        WatchAndCount ReaderGetCount(string key, WatchAndCount watchAndCount);
    }

    //public class ThreadSafeBuffer : IBuffer
    //{
    //    IBuffer _buffer;

    //    public ThreadSafeBuffer(IBuffer buffer)
    //    {
    //        _buffer = buffer;
    //    }

    //    public void RegisterInterceptor(IBufferInterceptor interceptor)
    //    {
    //        _buffer.RegisterInterceptor(interceptor);
    //    }

    //    public void Add(Data data)
    //    {
    //        _buffer.Add(data);
    //    }

    //    public Data Get()
    //    {
    //        Data data = _buffer.Get();
    //        return data;
    //    }

    //    public void CompleteAdd()
    //    {
    //        _buffer.CompleteAdd();
    //    }
    //}

    //public interface IBufferCounter
    //{
    //    WatchAndCount GetCount(string key, WatchAndCount watchAndCount);
    //}

    public class WatchAndCount
    {
        public int GetExecutingNumberBuffer;
        public long GetExecuteTimeBuffer;

        public int GetExecutingNumberRead;
        public long GetExecuteTimeRead;

    }

    //public class BufferWithCounts : IBuffer, IBufferCounter
    //{
    //    private Buffer _buffer;
    //    private Stopwatch _getStopWatch;
    //    private int _getExecutingNumber = 0;

    //    public BufferWithCounts()
    //    {
    //        _buffer = new Buffer();
    //        _getStopWatch = new Stopwatch();
    //    }

    //    public void RegisterInterceptor(IBufferInterceptor interceptor)
    //    {
    //        _buffer.RegisterInterceptor(interceptor);
    //    }

    //    public void Add(Data data)
    //    {
    //        _buffer.Add(data);
    //    }

    //    public Data Get()
    //    {
    //        _getStopWatch.Start();
    //        Data res = _buffer.Get();
    //        _getStopWatch.Stop();

    //        Interlocked.Increment(ref _getExecutingNumber);

    //        //Interlocked.MemoryBarrier();//прочитать

    //        //Interlocked.MemoryBarrier();
    //        return res;
    //    }

    //    public WatchAndCount GetCount(string key, WatchAndCount watchAndCount)
    //    {
    //        WatchAndCount res = new WatchAndCount();
    //        if (key == "Buffer.Get")
    //        {
    //            res.GetExecuteTimeBuffer = _getStopWatch.ElapsedMilliseconds;
    //            res.GetExecutingNumberBuffer = _getExecutingNumber;
    //        }

    //        return res;
    //    }

    //    public void CompleteAdd()
    //    {
    //        _buffer.CompleteAdd();
    //    }
    //}


    public class Buffer : IBuffer
    {
        public BlockingCollection<Data> listData = new BlockingCollection<Data>();
        private SemaphoreSlim ss = new SemaphoreSlim(1);

        private IBufferInterceptor _interceptor;

        public void RegisterInterceptor(IBufferInterceptor interceptor)
        {
            _interceptor = interceptor;
        }

        public void Add(Data data)
        {
            //ss.Wait();

            if (_interceptor != null)
                _interceptor.Intercept(data);

            listData.Add(data);

            // ss.Release();
        }

        public Data Get()
        {
            Data data;
            //if (listData.Count != 0)
            //{
            //    ss.Wait();
            //    Data data = null;
            //    if (listData.Count != 0)
            //        data = listData.Dequeue();
            //    ss.Release();
            //    return data;
            //}
            try
            {
                data = listData.Take();
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("That's All!");
                data = null;
            }
            return data;
        }

        public IEnumerable<Data> GetEnumerable()
        {
            return listData.GetConsumingEnumerable();
        }

        public void CompleteAdd()
        {
            listData.CompleteAdding();
        }
    }

}
