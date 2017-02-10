using SearchTool.Models;

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
