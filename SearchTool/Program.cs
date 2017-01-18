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
        public static int SizeBufferReader = Convert.ToInt16(ConfigurationManager.AppSettings["ReaderBufferSizeReader"]),
            SizeBufferWritter = Convert.ToInt16(ConfigurationManager.AppSettings["ReaderBufferSizeWritter"]);

        static void Main(string[] args)
        {
            var path = string.Empty;
            bool nesting = false;
            string searchText= string.Empty;

            InitializationAdditions.Serilog();
            InitializationAdditions.InputVariables(args, ref path, ref nesting, ref searchText);
            InitializationAdditions.DeterminationMinValue(ref SizeBufferReader, ref SizeBufferWritter);
            var unityContainer = InitializationAdditions.UnityContainer();

            var searcher = unityContainer.Resolve<Searcher>();
            searcher.Initialize(path, nesting, searchText, SizeBufferReader, SizeBufferWritter);
            searcher.Search();

            Console.Read();

        }
    }
}
