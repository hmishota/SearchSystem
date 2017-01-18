using Microsoft.Practices.Unity;
using NDesk.Options;
using SearchTool.Interfaces;
using SearchTool.SearchMethods;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool
{
    public static class InitializationAdditions
    {

        public static void Serilog()
        {
            var log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.LiterateConsole(LogEventLevel.Information)
                .WriteTo.RollingFile("log1-{Date}.txt", LogEventLevel.Information)
                .CreateLogger();
            Log.Logger = log;
        }

        public static void InputVariables(string[] args, ref string pathOut,ref bool nestingOut,ref string searchTextOut)
        {
            var path = string.Empty;
            bool nesting = false;
            string searchText = string.Empty;

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

            pathOut = path;
            nestingOut = nesting;
            searchTextOut = searchText;
        }

        public static void DeterminationMinValue(ref int _sizeBufferReader, ref int _sizeBufferWritter)
        {
            if (_sizeBufferReader > _sizeBufferWritter)
            {
                int buff = _sizeBufferWritter;
                _sizeBufferWritter = _sizeBufferReader;

                _sizeBufferReader = buff;
            }
        }

        public static IUnityContainer UnityContainer()
        {
            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<IFileManager, FileManager>();
            unityContainer.RegisterType<ISearcherMethod, SearcherMethodRabina>();

            return unityContainer;
        }

    }
}
