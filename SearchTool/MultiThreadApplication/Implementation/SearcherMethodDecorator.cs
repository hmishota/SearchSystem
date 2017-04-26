using System;
using SearchTool.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using SearchTool.Models;
using System.Linq;
using System.Diagnostics;

namespace SearchTool
{
    public class SearcherMethodDecorator : ISearcherMethodDecorator
    {
        private IUnityContainer _container;
        private int _searchThreadsNumber;

        public SearcherMethodDecorator(IUnityContainer container,int searchThreadsNumber)
        {
            _container = container;
            //_searchThreadsNumber = searchThreadsNumber;
            _searchThreadsNumber = Environment.ProcessorCount;
        }
        
        public async Task<List<SearchResult>> SearchAsync(string source)
        {
            var totalRunTimeSearch = Stopwatch.StartNew();

            var tasks = new List<Task<List<SearchResult>>>();

            for (int i = 0; i < _searchThreadsNumber; i++)
            {
                // Запускает поиск в отдельном потоке
                tasks.Add(Task.Run(() =>
                {
                    Stopwatch getStopWatch = new Stopwatch();
                    getStopWatch.Start();
                    var searcher = SearchInternalAsync(source);
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
        private List<SearchResult> SearchInternalAsync(string source)
        {
            var buffer = _container.Resolve<IBuffer>();
            var method = _container.Resolve<ISearcherMethod>();
          
                List<SearchResult> result = new List<SearchResult>();
                Data data = new Data();
                while (data != null)
                {
                        data = buffer.Dequeue();
                    if (data != null)
                        result.AddRange(method.Search(data, source));
                }


                return result;
        }

    }
}
