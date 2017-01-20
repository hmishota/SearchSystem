using Microsoft.Practices.Unity;
using SearchTool.Interfaces;
using SearchTool.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool
{
    public class SearcherMultithreading
    {
        private IUnityContainer _unityContainer;
        public SearcherMultithreading(IFileManager fManager, ISearcherMethod searcherMethod, IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public async Task Search(string path, bool nesting, string searchText)
        {

            var fileManager = _unityContainer.Resolve<IFileManager>();
            var reader = _unityContainer.Resolve<IReader>();
            var searcher = _unityContainer.Resolve<ISearcherMethodDecorator>();
            _unityContainer.RegisterType<IBufferInterceptor, BufferInterceptor>(new InjectionConstructor(path));


            var files = fileManager.GetFiles(path, nesting);

            var tasks = new List<Task>();

            foreach (var file in files)
            {
                //tasks.Add(reader.Read(file));
            }

          //  var searchTask = searcher.Search();
            await Task.WhenAll(tasks);
            


        }

    }
}

