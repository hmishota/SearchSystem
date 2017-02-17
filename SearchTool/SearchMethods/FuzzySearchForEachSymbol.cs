using System;
using System.Collections.Generic;
using System.Linq;
using SearchTool.Interfaces;
using SearchTool.Models;

namespace SearchTool.SearchMethods
{
    public class FuzzySearchForEachSymbol : FuzzySearch, ISearcherMethod
    {
        private int _limit;

        public FuzzySearchForEachSymbol(int limit):base(limit)
        {
            _limit = limit;
        }

        public List<SearchResult> Search(Data searchText, string source)
        {
            int n = searchText.Buffer.Length;
            int sourceLength = source.Length;
            List<SearchResult> listResults = new List<SearchResult>();

            string buffer = searchText.Buffer;
            for (int i = 0; i <= n - source.Length; i++) //проверить на концах
            {
                string str = buffer.Substring(i, sourceLength);
                var compareTwoWords = CompareTwoWords(str, source);

                if (compareTwoWords <= _limit)
                {
                    var searchResult = new SearchResult()
                    {
                        File = new File(searchText.Path),
                        Position = searchText.Position + i
                    };
                    listResults.Add(searchResult);
                    i += sourceLength-1;    // Возможно потом исравить. Лучше сохранять все рез-ты, а потом уже анализировать все
                }
            }
            return listResults;
        }
       
    }
}