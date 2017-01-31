using SearchTool.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchTool.Interfaces
{
    public interface IBuffer
    {
        void RegisterInterceptor(IBufferInterceptor intrceptor);

        void Stop();
        Data Dequeue();
        bool TryEnqueue(Data item);
    }
}
