using Microsoft.Practices.Unity;
using SearchTool.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SearchTool.Interfaces
{
    public interface IBuffer
    {
        void CompleteAdd();
        void RegisterInterceptor(IBufferInterceptor intrceptor);
        void Add(Data data);
        Data Get();
        IEnumerable<Data> GetEnumerable();

        //Task WaitAsync();   
    }
}
