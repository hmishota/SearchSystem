using Fclp;
using Microsoft.Practices.Unity;
using NDesk.Options;
using SearchTool.Interfaces;
using SearchTool.Models;
using SearchTool.SearchMethods;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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

            var searcherStart = unityContainer.Resolve<SearcherStart>();
            searcherStart.SearcherStartInit(unityContainer.Resolve<IStartSearher>());
            searcherStart.Initialize(unityContainer);
            searcherStart.DeterminationMinValue();

            var stop = Stopwatch.StartNew();

            searcherStart.Search(path, nesting, searchText).Wait();

            stop.Stop();

            Console.WriteLine("Total Run Time: {0}",stop.ElapsedMilliseconds);

//            var buffWatch = unityContainer.Resolve<IBufferCounter>();  
            var watchAndCount = unityContainer.Resolve<WatchAndCount>();
            //var res = buffWatch.GetCount("Buffer.Get", watchAndCount);

            //var readWatch = unityContainer.Resolve<IReadCounter>();
            //res = readWatch.ReaderGetCount("Buffer.Get", res);

            //Console.WriteLine($"TimeBuffer = {res.GetExecuteTimeBuffer}; CountBuffer = {res.GetExecutingNumberBuffer}; TimeRead = {res.GetExecuteTimeRead}; CountRead = {res.GetExecutingNumberRead};");
             

            Console.WriteLine($"Total Time Read = {watchAndCount.TotalRunTimeRead}; Total Time Search = {watchAndCount.TotalRunTimeSearch}; Sum read of each thread = {ResultTime.queryListRead.Sum()} Sum search of each thread = {ResultTime.queryListSearch.Sum()}");

            Console.Read();

        }
    }
}
