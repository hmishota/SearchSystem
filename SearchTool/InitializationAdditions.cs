using Microsoft.Practices.Unity;
using NDesk.Options;
using SearchTool.Interfaces;
using SearchTool.SearchMethods;
using Serilog;
using Serilog.Events;
using System;
using System.Configuration;

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

        public static void InputVariables(string[] args, ref string pathOut, ref bool nestingOut, ref string searchTextOut)
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

        public static IUnityContainer UnityContainer()
        {
            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<IFileManager, FileManager>();

            unityContainer.RegisterType<IReader, ReadWithCounts>("ReadWithCounts");
            unityContainer.RegisterType<IReader, ThreadSafeReader>(new InjectionConstructor(new ResolvedParameter(typeof(IReader), "ReadWithCounts")));

            unityContainer.RegisterInstance(new ConfigSettings());

            unityContainer.RegisterInstance(new SearcherStart());
            unityContainer.RegisterInstance(new WatchAndCount());
            int thread = Convert.ToInt32(ConfigurationManager.AppSettings["SearcherThreading"]);
            switch (thread)
            {
                case 0:
                    Console.WriteLine("Single Threading");
                    unityContainer.RegisterType<IStartSearher, SearcherSimple>();

                    break;

                case 1:
                    Console.WriteLine("Multi Threading");
                    unityContainer.RegisterType<IStartSearher, SearcherMultithreading>();
                    UnityContainerMulti(unityContainer);
                    break;

                default:
                    break;
            }

            int method = Convert.ToInt32(ConfigurationManager.AppSettings["SearcherMethod"]);
            switch (method)
            {
                case 0:
                    Console.WriteLine("MethodRabina");
                    unityContainer.RegisterType<ISearcherMethod, SearcherMethodRabina>();
                    break;

                case 1:
                    Console.WriteLine("MethodBoyer_Moore");
                    unityContainer.RegisterType<ISearcherMethod, SearcherMethodBoyer_Moore>();
                    break;

                default:
                    break;
            }


            return unityContainer;

        }
        
        public static IUnityContainer UnityContainerMulti(IUnityContainer unityContainer) // СПРОСИТЬ
        {
            //unityContainer.RegisterType<IReaderMulti, ReaderMultithreading>();.
            //var read = new ReadWithCounts();
            unityContainer.RegisterType<IReaderMulti, ReaderMultithreading>(new ContainerControlledLifetimeManager());
            //unityContainer.RegisterInstance<IReadCounter>(read);
            //var buffer = new BufferWithCounts();
            unityContainer.RegisterType<IBuffer, Buffer>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ISearcherMethodDecorator, SearcherMethodDecorator>();

            return unityContainer;
        }

    }
}