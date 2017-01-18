using SearchTool.Interfaces;
using SearchTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool
{
    public class Searcher
    {
        private IFileManager _fileManager;
        private ISearcherMethod _searcherMethod;
        private string _path;
        private bool _nesting;
        private string _searchText;
        private int _sizeBufferReader;
        private int _sizeBufferWritter;


        public Searcher(IFileManager fManager, ISearcherMethod searcherMethod, string path, bool nesting, string searchText, int sizeBufferReader, int sizeBufferWritter)
        {
            this._fileManager = fManager;
            this._searcherMethod = searcherMethod;
            this._path = path;
            this._nesting = nesting;
            this._searchText = searchText;
            this._sizeBufferReader = sizeBufferReader;
            this._sizeBufferWritter = sizeBufferWritter;
        }

        public void Search()
        {
            var files = _fileManager.GetFiles(_path, _nesting);
            List<SearchResult> result = new List<SearchResult>();

            foreach (var file in files)
            {
                Data data;

                Data prevData = null;
                using (var reader = new Reader(_sizeBufferReader, _sizeBufferWritter, file))
                {
                    while (reader.Read(out data))
                    {
                        if (prevData != null)
                        {
                            // Добавление конца текста из предыдущего чтения
                            data.Buffer = prevData.Buffer.Substring(prevData.Buffer.Length - (_searchText.Length - 1)) + data.Buffer;

                            // Изменение позиции с учетом того что добавлен текст в начало файла
                            data.Position = data.Position - (_searchText.Length - 1);
                        }

                        // Поиск подстроки в строке
                        result.AddRange(_searcherMethod.Search(data, _searchText));
                        if (prevData == null)
                            prevData = new Data();

                        prevData.Buffer = data.Buffer;
                    }
                }
            }

            foreach (SearchResult res in result)
            {
                Console.WriteLine($" Result: {res.Position} path: {res.File.Path}");
            }

            Console.Read();

        }

    }
}

