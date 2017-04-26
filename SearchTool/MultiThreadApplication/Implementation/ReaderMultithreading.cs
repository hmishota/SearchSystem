using SearchTool.Interfaces;
using System.Threading.Tasks;
using SearchTool.Models;
using Microsoft.Practices.Unity;
using System.IO;

namespace SearchTool
{
    public class ReaderMultithreading : IReaderMulti
    {
        private IUnityContainer _unityContainer;
        private IFileOpen _fileOpen;

        public void RegisterReadWithCounts(IUnityContainer unityContainer, IFileOpen open)
        {
            _unityContainer = unityContainer;
            _fileOpen = open;
        }

        // Читать пока файл не пуст
        public async Task ReadAsync(Models.File file, int sizeBufferReader, int sizeBufferWritter)
        {
            Data data = new Data();
            Stream stream;
            var buffer = _unityContainer.Resolve<IBuffer>();
            using (var reader = _unityContainer.Resolve<IReader>())
            {
                stream = _fileOpen.Open(file);
                reader.InitVariables(stream, sizeBufferReader, sizeBufferWritter);
                while ((data = await reader.ReadAsync()) != null)
                {
                    data.Path = file.Path;
                    await Task.Run(() =>
                    {
                        while (!buffer.TryEnqueue(data)) ;

                    });
                }
            }
        }
    }
}
