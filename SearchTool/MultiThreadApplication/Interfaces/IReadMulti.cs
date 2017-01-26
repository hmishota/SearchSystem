using Microsoft.Practices.Unity;
using SearchTool.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SearchTool.Interfaces
{
    public interface IReaderMulti
    {
        Task ReadAsync(File file, int sizeBufferReader, int sizeBufferWritter);
        void RegisterReadWithCounts(IUnityContainer unityContainer);
        //WatchAndCount ReaderGetCount(string key, WatchAndCount watchAndCount);
    }
}
