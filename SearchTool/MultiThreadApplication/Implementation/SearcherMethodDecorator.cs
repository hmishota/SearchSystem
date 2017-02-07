using SearchTool.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Practices.Unity;
using SearchTool.Models;
using System.Linq;
using System;
using System.Configuration;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace SearchTool
{
    public class SearcherMethodDecorator : ISearcherMethodDecorator
    {
        private IUnityContainer _container;
        private static int SearchThreadsNumber = Convert.ToInt32(ConfigurationManager.AppSettings["CountThreading"]);

        public SearcherMethodDecorator(IUnityContainer container)
        {
            _container = container;
        }
        
        public async Task<List<SearchResult>> SearchAsync(string source, CancellationToken cancel)
        {
            var totalRunTimeSearch = Stopwatch.StartNew();

            var tasks = new List<Task<List<SearchResult>>>();

            for (int i = 0; i < SearchThreadsNumber; i++)
            {
                // Запускает поиск в отдельном потоке
                tasks.Add(Task.Run(async () =>
                {
                    Stopwatch getStopWatch = new Stopwatch();
                    getStopWatch.Start();
                    var searcher = await SearchInternalAsync(source, cancel);
                    getStopWatch.Stop();
                    ResultTime.queryListSearch.Enqueue(getStopWatch.ElapsedMilliseconds);
                    return searcher;
                }));
            }

            var results = await Task.WhenAll(tasks);

            totalRunTimeSearch.Stop();

            var watchAndCount = _container.Resolve<WatchAndCount>();
            watchAndCount.TotalRunTimeSearch = totalRunTimeSearch.ElapsedMilliseconds;

            return results.SelectMany(x => x).ToList();
        }

        // Поиск source в data
        private async Task<List<SearchResult>> SearchInternalAsync(string source, CancellationToken cancel)
        {
            var buffer = _container.Resolve<IBuffer>();
            var method = _container.Resolve<ISearcherMethod>();
            
            return await Task.Run(() =>
            {
                List<SearchResult> result = new List<SearchResult>();
                Data data = new Data();
                while (data != null)
                {
                    data = buffer.Dequeue(); 
                    if (data != null)
                        result.AddRange(method.Search(data, source));
                }


                return result;
            });
        }

    }
}
