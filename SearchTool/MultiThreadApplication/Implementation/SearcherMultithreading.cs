using Microsoft.Practices.Unity;
using SearchTool.Interfaces;
using SearchTool.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SearchTool
{
    public class SearcherMultithreading: IStartSearher
    {
        private IUnityContainer _unityContainer;
        private IFileManager _fileManager;
        private ISearcherMethod _searcherMethod;

        public static int SizeBufferReader = Convert.ToInt32(ConfigurationManager.AppSettings["ReaderBufferSizeReader"]),
            SizeBufferWritter = Convert.ToInt32(ConfigurationManager.AppSettings["ReaderBufferSizeWritter"]);

        public SearcherMultithreading(IFileManager fManager, ISearcherMethod searcherMethod, IUnityContainer unityContainer)
        {
            _fileManager = fManager;
            _searcherMethod = searcherMethod;
            _unityContainer = unityContainer;
        }

        public void Initialize(IUnityContainer unityContainer)
        {
            var configSetting = unityContainer.Resolve<ConfigSettings>();

            configSetting.SizeBufferReader = SizeBufferReader;
            configSetting.SizeBufferWritter = SizeBufferWritter;
        }

        public void DeterminationMinValue()
        {
            if (SizeBufferReader > SizeBufferWritter)
            {
                int buff = SizeBufferWritter;
                SizeBufferWritter = SizeBufferReader;

                SizeBufferReader = buff;
            }
        }

        public async Task Search(string path, bool nesting, string searchText)
        {
            var token = new CancellationTokenSource();
            var tasks = new List<Task>();

            var files = _fileManager.GetFiles(path, nesting);
            var reader = _unityContainer.Resolve<IReaderMulti>();
            reader.RegisterReadWithCounts(_unityContainer);

            var buffer = _unityContainer.Resolve<IBuffer>();
            buffer.RegisterInterceptor(new BufferInterceptor(searchText));

            var searcher = _unityContainer.Resolve<ISearcherMethodDecorator>();

            var totalRunTimeRead = Stopwatch.StartNew();

            foreach (var file in files)
            {
                // Запускает чтение файла в отдельном потоке
                tasks.Add(Task.Run(() => reader.ReadAsync(file, SizeBufferReader, SizeBufferWritter)));
            }

            var watchAndCount = _unityContainer.Resolve<WatchAndCount>();

            // Запускает поиск
            var searchTask = Task.Run(() => searcher.SearchAsync(searchText, token.Token));

            await Task.WhenAll(tasks);

            totalRunTimeRead.Stop();
            watchAndCount.TotalRunTimeRead = totalRunTimeRead.ElapsedMilliseconds;

            buffer.Stop();

            token.Cancel();

            var result = await searchTask;

            foreach (SearchResult res in result)
            {
                Log.Information($" Result: {res.Position} path: {res.File.Path}");
            }
        }
    }
}

