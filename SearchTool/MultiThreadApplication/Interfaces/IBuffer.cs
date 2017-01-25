using Microsoft.Practices.Unity;
using SearchTool.Models;

namespace SearchTool.Interfaces
{
    public interface IBuffer
    {
        void RegisterInterceptor(IBufferInterceptor intrceptor);
        void Add(Data data);
        Data Get();
        //Task WaitAsync();   
    }
}
