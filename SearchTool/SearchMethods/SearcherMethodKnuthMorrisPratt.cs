using SearchTool.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchTool.Models;

namespace SearchTool.SearchMethods
{
    public class SearcherMethodKnuthMorrisPratt : ISearcherMethod
    {
        int length = 0;
        public List<SearchResult> Search(Data text, string searchText)
        {
            List<SearchResult> searchResult = new List<SearchResult>();
            length = searchText.Length;
            searchResult = KMP(text, searchText);
            foreach (var search in searchResult)
            {
                search.File = new File(text.Path);
                search.Position = text.Position + search.Position;
            }
            return searchResult;
        }


        //Префикс-функция для КМП
        public int[] PrefFunc(string x)
        {
            //Инициализация массива-результата длиной X
            int[] res = new int[x.Length];
            int i = 0;
            int j = -1;
            res[0] = -1;
            //Вычисление префикс-функции
            while (i < x.Length - 1)
            {
                while ((j >= 0) && (x[j] != x[i]))
                    j = res[j];
                i++;
                j++;
                if (x[i] == x[j])
                    res[i] = res[j];
                else
                    res[i] = j;
            }
            return res;//Возвращение префикс-функции
        }

        //Функция поиска алгоритмом КМП
        public List<SearchResult> KMP(Data data, string x)
        {
            List<SearchResult> foundResults = new List<SearchResult>();

            string s = data.Buffer;
            if (x.Length > s.Length) return foundResults; //Возвращает 0 поиск если образец больше исходной строки
            //Вызов префикс-функции
            int[] d = PrefFunc(x);
            int i = 0, j;
            while (i < s.Length)
            {
                for (j = 0; (i < s.Length) && (j < x.Length); i++, j++)
                    while ((j >= 0) && (x[j] != s[i]))
                        j = d[j];
                if (j == x.Length)
                    foundResults.Add(new SearchResult { Position = (i - length) });
            }

            return foundResults; //Возвращение результата поиска
        }

    }
}
