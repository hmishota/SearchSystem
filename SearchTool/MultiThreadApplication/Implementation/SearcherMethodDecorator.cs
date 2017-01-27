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
    public interface ISearchCounter
    {
        void SeacherAddTime();
    }

    //public class SeachWithCounts : ISearcherMethodDecorator, ISearchCounter
    //{
    //    private SearcherMethodDecorator _search;

    //    public SeachWithCounts(IUnityContainer container)
    //    {
    //        _search = new SearcherMethodDecorator(container);
    //        _getStopWatch = new Stopwatch();
    //    }

    //    //public void SearcherMethodInit(IUnityContainer container)
    //    //{
    //    //    _search.SearcherMethodInit(container);
    //    //}

    //    //public void InitTimer()
    //    //{
    //    //    _getStopWatch = new Stopwatch();
    //    //}

    //    public Task<List<SearchResult>> SearchAsync(string source, CancellationToken cancel)
    //    {
    //        return _search.SearchAsync(source, cancel);
    //    }

    //    public void SeacherAddTime()
    //    {
    //        ResultTime.queryListSearch.Enqueue(_getStopWatch.ElapsedMilliseconds);
    //    }

    //    public async Task<List<SearchResult>> SearchInternalAsync(string source, CancellationToken cancel)
    //    {
    //        _getStopWatch.Start();
    //        var searcher = await _search.SearchInternalAsync(source, cancel);
    //        _getStopWatch.Stop();
    //        SeacherAddTime();
    //        return searcher;
    //    }
    //}

    public class SearcherMethodDecorator : ISearcherMethodDecorator
    {
        private IUnityContainer _container;
        private static int SearchThreadsNumber = Convert.ToInt32(ConfigurationManager.AppSettings["CountThreading"]);


        public SearcherMethodDecorator(IUnityContainer container)
        {
            _container = container;
        }

        //public void SearcherMethodInit(IUnityContainer container)
        //{
        //    _container = container;
        //}
        public async Task<List<SearchResult>> SearchAsync(string source, CancellationToken cancel)
        {

            var totalRunTimeSearch = Stopwatch.StartNew();

            var tasks = new List<Task<List<SearchResult>>>();

            for (int i = 0; i < SearchThreadsNumber; i++)
            {

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

        public async Task<List<SearchResult>> SearchInternalAsync(string source, CancellationToken cancel)
        {
            var buffer = _container.Resolve<IBuffer>();
            var method = _container.Resolve<ISearcherMethod>();

            return await Task.Run(() =>
            {
                List<SearchResult> result = new List<SearchResult>();

                foreach (var data in buffer.GetEnumerable())
                {
                    result.AddRange(method.Search(data, source));
                }

                return result;
            });

        }

    }
}
