using SearchTool.Models;
using System.Collections.Generic;

namespace SearchTool.Interfaces
{
    public interface IBuffer
    {
        void CompleteAdd();
        void RegisterInterceptor(IBufferInterceptor intrceptor);
        void Add(Data data);
        IEnumerable<Data> GetEnumerable();
    }
}
