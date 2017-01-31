using SearchTool.Interfaces;
using System.Threading.Tasks;
using SearchTool.Models;
using Microsoft.Practices.Unity;

namespace SearchTool
{
    public class ReaderMultithreading : IReaderMulti
    {
        private IUnityContainer _unityContainer;

        public void RegisterReadWithCounts(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        // Читать пока файл не пуст
        public async Task ReadAsync(Models.File file, int sizeBufferReader, int sizeBufferWritter)
        {
            Data data = new Data();
            var buffer = _unityContainer.Resolve<IBuffer>();
            using (var reader = _unityContainer.Resolve<IReader>())
            {
                reader.InitVariables(sizeBufferReader, sizeBufferWritter, file);
                while ((data = await reader.ReadAsync()) != null)
                {
                    await Task.Run(() =>
                    {
                        while (!buffer.TryEnqueue(data)) ;
                        
                    });
                }
            }
        }
    }
}
