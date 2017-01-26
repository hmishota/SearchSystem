using SearchTool.Interfaces;
using System.Threading.Tasks;
using SearchTool.Models;
using Microsoft.Practices.Unity;
using System.Threading;
using System.Diagnostics;
using System;
using System.Collections.Concurrent;

namespace SearchTool
{
    public class ThreadSafeReader : IReaderMulti
    {
        IReaderMulti _readerMulti;

        public ThreadSafeReader(IReaderMulti readerMulti)
        {
            _readerMulti = readerMulti;
        }

        public Task ReadAsync(File file, int sizeBufferReader, int sizeBufferWritter)
        {
            return _readerMulti.ReadAsync(file, sizeBufferReader, sizeBufferWritter);
        }

        public void RegisterReadWithCounts(IUnityContainer unityContainer)
        {
            _readerMulti.RegisterReadWithCounts(unityContainer);
        }
    }

    public class ReadWithCounts : IReaderMulti, IReadCounter
    {
        IReaderMulti _readerMultithreading;
        private Stopwatch _getStopWatch;
        private int _getExecutingNumber = 0;

        public ReadWithCounts()
        {
            _readerMultithreading = new ReaderMultithreading();
            _getStopWatch = new Stopwatch();
        }

        public void RegisterReadWithCounts(IUnityContainer unityContainer)
        {
            _readerMultithreading.RegisterReadWithCounts(unityContainer);
        }

        public WatchAndCount ReaderGetCount(string key, WatchAndCount watchAndCount)
        {
            if (key == "Buffer.Get")
            {
                watchAndCount.GetExecuteTimeRead = _getStopWatch.ElapsedMilliseconds;
                watchAndCount.GetExecutingNumberRead = _getExecutingNumber;
            }
            return watchAndCount;
        }

        public Task ReadAsync(File file, int sizeBufferReader, int sizeBufferWritter)
        {
            _getStopWatch.Start();
            var taskRead = _readerMultithreading.ReadAsync(file, sizeBufferReader, sizeBufferWritter);
            _getStopWatch.Stop();

            Interlocked.Increment(ref _getExecutingNumber);

            return taskRead;
        }
    }

    public class ReaderMultithreading : IReaderMulti
    {
        private IUnityContainer _unityContainer;

        public void RegisterReadWithCounts(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public async Task ReadAsync(Models.File file, int sizeBufferReader, int sizeBufferWritter)
        {
            //BlockingCollection<Data> bc = new BlockingCollection<Data>();
            Data data = new Data();
            var buffer = _unityContainer.Resolve<IBuffer>();
            using (var reader = _unityContainer.Resolve<IReader>())
            {
                reader.InitVariables(sizeBufferReader, sizeBufferWritter, file);
                while ((data = await reader.ReadAsync()) != null)
                {
                    buffer.Add(data);
                }
                

            }
        }
    }
}
