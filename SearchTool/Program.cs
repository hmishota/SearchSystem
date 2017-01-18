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
        int x = 0;
        public static int SizeBufferReader = Convert.ToInt16(ConfigurationManager.AppSettings["ReaderBufferSizeReader"]),
            SizeBufferWritter = Convert.ToInt16(ConfigurationManager.AppSettings["ReaderBufferSizeWritter"]);

        static void Main(string[] args)
        {
            var path = string.Empty;
            bool nesting = false;
            string searchText= string.Empty;

            if (SizeBufferReader > SizeBufferWritter)
            {
                int buff = SizeBufferWritter;
                SizeBufferWritter = SizeBufferReader;

                SizeBufferReader = buff;
            }


            var parser = new OptionSet(){
                {
                    "path:","the path to the file.", p => path = p
                },
                {
                    "nesting:","the path to the file.", n => nesting = Convert.ToBoolean(n)
                },
                {
                    "text:","the path to the file.", text => searchText = text
                }
                };

            try
            {
                parser.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("Exseption: ");
                Console.WriteLine(e.Message);
                return;
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("Enter the path to the file");
                        path = Console.ReadLine();
            }

            //if (string.IsNullOrWhiteSpace(path))
            //{
            //    Console.WriteLine("Enter the path to the file");
            //    path = Console.ReadLine();
            //}

            if (string.IsNullOrWhiteSpace(searchText))
            {
                Console.WriteLine("Enter search text");

                searchText = Console.ReadLine();
            }

            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<IFileManager, FileManager>();
            unityContainer.RegisterType<ISearcherMethod, SeatherMethod1>();

            var searcher = unityContainer.Resolve<Searcher>(new ResolverOverride[]
                                   {
                                       new ParameterOverride("path", path), new ParameterOverride("nesting", nesting),
                                       new ParameterOverride("searchText", searchText),new ParameterOverride("sizeBufferReader", SizeBufferReader),
                                       new ParameterOverride("sizeBufferWritter", SizeBufferWritter)
                                   });
            searcher.Search();
        }
    }
}
