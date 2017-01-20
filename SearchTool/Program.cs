using Fclp;
using Microsoft.Practices.Unity;
using NDesk.Options;
using SearchTool.Interfaces;
using SearchTool.SearchMethods;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool
{

    class Program
    {
        

        static void Main(string[] args)
        {
            var path = string.Empty;
            bool nesting = false;
            string searchText= string.Empty;

            InitializationAdditions.Serilog();
            InitializationAdditions.InputVariables(args, ref path, ref nesting, ref searchText);
            var unityContainer = InitializationAdditions.UnityContainer();
            var searcher = unityContainer.Resolve<SearcherSimple>();
           /* searcher.Initialize(unityContainer);
            searcher.DeterminationMinValue();
            searcher.Search(path, nesting, searchText).Wait();*/


            var sear = unityContainer.Resolve<SearcherMultithreading>();
            sear.Search(path, nesting, searchText);








            Console.Read();

        }
    }
}
