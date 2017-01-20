using SearchTool.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Practices.Unity;
using SearchTool.SearchMethods;
using SearchTool.Models;

namespace SearchTool
{
    public class SearcherMethodDecorator : ISearcherMethodDecorator
    {

        private IUnityContainer _container;

        public SearcherMethodDecorator(IUnityContainer container)
        {
            _container = container;
        }

        public async Task Search(string source, CancellationToken cancel)
        {
            while(true)
            {
          
                SearcherMethodRabina method = new SearcherMethodRabina(); // уберу

                IBuffer buffer = new Buffer();
                var res = method.Search(buffer.Get(), source);
                if (cancel.IsCancellationRequested)
                    return;

            }
        }
    }
}
