using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        int x = 0;
        public static int sizeBufferReader = Convert.ToInt16(ConfigurationManager.AppSettings["ReaderBufferSizeReader"]),
            sizeBufferWritter = Convert.ToInt16(ConfigurationManager.AppSettings["ReaderBufferSizeWritter"]);

        static void Main(string[] args)
        {
            string path = "C:\\MyProject\\SearchSystem\\TestFile";
            bool nesting = true;
            string searchText = "hello";

            if (sizeBufferReader>sizeBufferWritter)
            {
                int buff = sizeBufferWritter;
                sizeBufferWritter = sizeBufferReader;
                sizeBufferReader = buff;
            }

            switch (args.Length)
            {
                case 0:
                    Console.WriteLine("Enter search text");
                    searchText = Console.ReadLine();
                    Console.WriteLine("Enter the path to the file");
                    path = Console.ReadLine();
                    Console.WriteLine("It is to look in subfolders? Enter true or false");
                    nesting = Convert.ToBoolean(Console.ReadLine());
                    break;
                case 1:
                    
                    Console.WriteLine("Enter search text");
                    searchText = Console.ReadLine();
                    Console.WriteLine("Enter the path to the file");
                    path = Console.ReadLine();
                    break;
                case 2:
                    Console.WriteLine("Enter search text");
                    searchText = Console.ReadLine();
                    break;

                case 3:
                    path = args[0];
                    nesting = Convert.ToBoolean(args[1]);
                    searchText = args[2];
                    break;

                default:
                    Console.WriteLine("Error input");
                    Console.Read();
                    Environment.Exit(0);
                    break;
            }

           
            IFileManager fileManager = new FileManager();
            var files = fileManager.GetFiles(path);
            List<SearchResult> result = new List<SearchResult>();
            ISearcher searcher = new SeatherMethod1();

            foreach (var file in files)
            {
                Data data;

                Data prevData = null;
                using (var reader = new Reader(sizeBufferReader, sizeBufferWritter, file))
                {
                    while (reader.Read(out data)) 
                    {
                        if (prevData != null)
                        {
                            data.Buffer = prevData.Buffer.Substring(prevData.Buffer.Length - (searchText.Length - 1)) + data.Buffer;
                            data.Position = data.Position - (searchText.Length - 1);
                        }

                        result.AddRange(searcher.Search(data, searchText)); 
                        if (prevData == null)
                            prevData = new Data();

                        prevData.Buffer = data.Buffer;
                    }
                }
                
                //using (FileStream fin = new FileStream(file.path, FileMode.Open))
                //{
                //    IReader reader = new Reader(sizeBufferReader, sizeBufferWritter, fin);
                //    do
                //    {
                //        do
                //        {
                //            data = reader.Read();
                //            data.path = file.path;
                //        }
                //        while (!data.recognition);
                //        ISearcher searcher = new SeatherMethod1();
                //        SearchResult res = searcher.Search(data, searchText);
                //        if (res.searchResult != "")
                //            result.Add(res);
                //    }
                //    while (!data.theEndFile);   
                //}
            }

            foreach(SearchResult res in result)
            {
                Console.WriteLine($" Result: {res.Position} path: {res.File.Path}");
            }


            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //foreach (var file in files)
            //{
            //    Data data;

            //        do
            //        {
            //        using (FileStream fin = new FileStream(file.path, FileMode.Open))
            //        {
            //            IReader reader = new Reader(sizeBufferReader, sizeBufferWritter, fin);
            //                data = reader.Read();
            //                data.path = file.path;
            //            ISearcher searcher = new SeatherMethod1();
            //            var res = searcher.Search(data, searchText);
            //            Console.WriteLine(res.searchResult);
            //        }
            //    }
            //        while (data.doing == true);


            //}

            Console.Read();
        }
    }
}
