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
            var tasks = new List<Task<List<SearchResult>>>();

           
                for (int i = 0; i < SearchThreadsNumber; i++)
                {
                    tasks.Add(Task.Run(async () => await SearchInternalAsync(source, cancel)));
                }
            
            var results = await Task.WhenAll(tasks);

            return results.SelectMany(x => x).ToList();
        }

        private Task<List<SearchResult>> SearchInternalAsync(string source, CancellationToken cancel)
        {
            var buffer = _container.Resolve<IBuffer>();
            var method = _container.Resolve<ISearcherMethod>();

            return Task.Run(() =>
            {
                List<SearchResult> result = new List<SearchResult>();

                foreach (var data in buffer.GetEnumerable())
                {
                    result.AddRange(method.Search(data, source));
                }

                return result;
            });

            //Task.WaitAll(t1, t2);


            //while (true)
            //{
            //    var data = buffer.Get();
            //    if (data == null)
            //    {
            //        if (cancel.IsCancellationRequested)
            //            return result;
            //        else
            //            await Task.Delay(10);
            //    }
            //    else
            //        result.AddRange(method.Search(data, source));
            //}
        }

    }
}
