using Microsoft.Practices.Unity;
using SearchTool.Interfaces;
using SearchTool.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SearchTool
{
    public class SearcherMultithreading: IStartSearher
    {
        private IUnityContainer _unityContainer;
        private IFileManager _fileManager;

        public int _sizeBufferReader, _sizeBufferWritter;

        public SearcherMultithreading(IFileManager fManager, IUnityContainer unityContainer, int sizeBufferReader, int sizeBufferWritter)
        {
            _fileManager = fManager;
            _unityContainer = unityContainer;
            _sizeBufferReader = sizeBufferReader;
            _sizeBufferWritter = sizeBufferWritter;
        }
        
        public void DeterminationMinValue()
        {
            if (_sizeBufferReader > _sizeBufferWritter)
            {
                int buff = _sizeBufferWritter;
                _sizeBufferWritter = _sizeBufferReader;

                _sizeBufferReader = buff;
            }
        }

        public async Task<List<SearchResult>> Search(string path, bool nesting, string searchText)
        {
            var token = new CancellationTokenSource();
            var tasks = new List<Task>();

            var files = _fileManager.GetFiles(path, nesting);
            PrintFindFile.Print(files);
            var reader = _unityContainer.Resolve<IReaderMulti>();
            reader.RegisterReadWithCounts(_unityContainer, new FileOpen());

            var buffer = _unityContainer.Resolve<IBuffer>();
            buffer.RegisterInterceptor(new BufferInterceptor(searchText));

            var searcher = _unityContainer.Resolve<ISearcherMethodDecorator>();

            var totalRunTimeRead = Stopwatch.StartNew();

            foreach (var file in files)
            {
                // Запускает чтение файла в отдельном потоке
                tasks.Add(Task.Run(() => reader.ReadAsync(file, _sizeBufferReader, _sizeBufferWritter)));
            }

            var watchAndCount = _unityContainer.Resolve<WatchAndCount>();

            // Запускает поиск
            var searchTask = Task.Run(() => searcher.SearchAsync(searchText));

            await Task.WhenAll(tasks);

            totalRunTimeRead.Stop();
            watchAndCount.TotalRunTimeRead = totalRunTimeRead.ElapsedMilliseconds;

            buffer.Stop();

            token.Cancel();

            var result = await searchTask;

            return result;
        }
    }
}

