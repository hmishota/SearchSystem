using Fclp;
using Microsoft.Practices.Unity;
using NDesk.Options;
using SearchTool.Interfaces;
using SearchTool.Models;
using SearchTool.SearchMethods;
using Serilog;
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

            //string T = "he7472llo112277472";
            //string P = "7472";
            //Temp search = new Temp();
            //List<SearchResult> listresult = search.FasterQuick(new Data { Buffer = T, Position = 0 }, P);


            //string T = "GCATCGCAGTC32helGACAGTATACAGTAChelGAC";
            //string P = "helGAC";
            //Temp search = new Temp();
            //List<SearchResult> listresult = search.FasterQuick(new Data { Buffer = T, Position = 0 }, P);


            var path = string.Empty;
            bool nesting = false;
            string searchText = string.Empty;

            InitializationAdditions.Serilog();
            InitializationAdditions.InputVariables(args, ref path, ref nesting, ref searchText);

            var unityContainer = InitializationAdditions.UnityContainer();

            var searcherStart = unityContainer.Resolve<SearcherStart>();
            searcherStart.SearcherStartInit(unityContainer.Resolve<IStartSearher>());
            searcherStart.Initialize(unityContainer);
            searcherStart.DeterminationMinValue();

            var stop = Stopwatch.StartNew();

            var result = searcherStart.Search(path, nesting, searchText);

            stop.Stop();

            foreach (SearchResult res in result)
            {
                Log.Information($" Result: {res.Position} path: {res.File.Path}");
                //Console.WriteLine($" Result: {res.Position} path: {res.File.Path}");
            }

            Console.WriteLine("Total Run Time: {0}", stop.ElapsedMilliseconds);

            var watchAndCount = unityContainer.Resolve<WatchAndCount>();
            Console.WriteLine($"Total Time Read = {watchAndCount.TotalRunTimeRead}; Total Time Search = {watchAndCount.TotalRunTimeSearch}; Sum read of each thread = {ResultTime.queryListRead.Sum()} Sum search of each thread = {ResultTime.queryListSearch.Sum()}");

            Console.Read();

        }
    }
}
