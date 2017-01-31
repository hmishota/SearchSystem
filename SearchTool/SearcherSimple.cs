using Microsoft.Practices.Unity;
using SearchTool.Interfaces;
using SearchTool.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SearchTool
{
    public class SearcherSimple: IStartSearher
    {
        private IFileManager _fileManager;
        private ISearcherMethod _searcherMethod;
        private IUnityContainer _unityContainer;
       
        public static int SizeBufferReader = Convert.ToInt32(ConfigurationManager.AppSettings["ReaderBufferSizeReader"]),
            SizeBufferWritter = Convert.ToInt32(ConfigurationManager.AppSettings["ReaderBufferSizeWritter"]);

        public SearcherSimple(IFileManager fManager, ISearcherMethod searcherMethod, IUnityContainer unityContainer)
        {
            _fileManager = fManager;
            _searcherMethod = searcherMethod;
            _unityContainer = unityContainer;
        }

        public void DeterminationMinValue()
        {
            if (SizeBufferReader > SizeBufferWritter)
            {
                int buff = SizeBufferWritter;
                SizeBufferWritter = SizeBufferReader;

                SizeBufferReader = buff;
            }
        }

        public void Initialize(IUnityContainer unityContainer)
        {
            var configSetting = unityContainer.Resolve<ConfigSettings>();

            configSetting.SizeBufferReader = SizeBufferReader;
            configSetting.SizeBufferWritter = SizeBufferWritter;
        }

        public async Task Search(string path, bool nesting, string searchText)
        {
         Stopwatch getStopWatch = new Stopwatch();

        var files = _fileManager.GetFiles(path, nesting);
            List<SearchResult> result = new List<SearchResult>();

            foreach (var file in files)
            {
                Data data;

                Data prevData = null;
                using (var reader =  _unityContainer.Resolve<IReader>())
                {
                    reader.InitVariables(SizeBufferReader, SizeBufferWritter, file);
                    while ((data = await reader.ReadAsync()) != null)
                    {
                        if (prevData != null)
                        {
                            // Добавление конца текста из предыдущего чтения
                            data.Buffer = prevData.Buffer.Substring(prevData.Buffer.Length - (searchText.Length - 1)) + data.Buffer;

                            // Изменение позиции с учетом того что добавлен текст в начало файла
                            data.Position = data.Position - (searchText.Length - 1);
                        }

                        // Поиск подстроки в строке
                        getStopWatch.Start();
                        result.AddRange(_searcherMethod.Search(data, searchText));
                        getStopWatch.Stop();

                        if (prevData == null)
                            prevData = new Data();

                        prevData.Buffer = data.Buffer;
                    }
                }
            }
            ResultTime.queryListSearch.Enqueue(getStopWatch.ElapsedMilliseconds);

            foreach (SearchResult res in result)
            {
                Log.Information($" Result: {res.Position} path: {res.File.Path}");
                //Console.WriteLine($" Result: {res.Position} path: {res.File.Path}");
            }

        }

    }
}

