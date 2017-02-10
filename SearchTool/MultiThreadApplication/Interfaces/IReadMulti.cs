using Microsoft.Practices.Unity;
using SearchTool.Models;
using System.Threading.Tasks;

namespace SearchTool.Interfaces
{
    public interface IReaderMulti
    {
        Task ReadAsync(File file, int sizeBufferReader, int sizeBufferWritter);
        void RegisterReadWithCounts(IUnityContainer unityContainer, IFileOpen open);
    }
}
