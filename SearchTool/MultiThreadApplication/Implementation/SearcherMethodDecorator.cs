using SearchTool.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Practices.Unity;
using SearchTool.Models;
using System.Linq;
using System;
using System.Configuration;

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

        public async Task<List<SearchResult>> Search(string source, CancellationToken cancel)
        {
            var tasks = new List<Task<List<SearchResult>>>();

            for (int i = 0; i < SearchThreadsNumber; i++)
            {
                tasks.Add(Task.Run( async () => await SearchInternal(source, cancel)));
            }

            var results = await Task.WhenAll(tasks);

            return results.SelectMany(x => x).ToList();
        }

        private async Task<List<SearchResult>> SearchInternal(string source, CancellationToken cancel)
        {
            List<SearchResult> result = new List<SearchResult>();
            var buffer = _container.Resolve<IBuffer>();
            var method = _container.Resolve<ISearcherMethod>();

            while (true)
            {
                var data = buffer.Get();
                if (data == null)
                {
                    if (cancel.IsCancellationRequested)
                        return result;
                    else
                        await Task.Delay(10);
                }
                else
                    result.AddRange(method.Search(data, source));
            }
        }

    }
}
