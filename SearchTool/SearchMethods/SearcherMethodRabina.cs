using SearchTool.Interfaces;
using SearchTool.Models;
using System;
using System.Collections.Generic;

namespace SearchTool.SearchMethods
{
    public class SearcherMethodRabina : ISearcherMethod
    {
        public List<SearchResult> Search(Data text, string searchText)
        {
            List<SearchResult> searchResult = new List<SearchResult>();
            searchResult = Rabina(text, searchText);
            foreach (var search in searchResult)
            {
                search.File = new File(text.Path);
                search.Position = text.Position + search.Position;
            }
            return searchResult;
        }

        // Хеш-функция для алгоритма Рабина-Карпа
        public int Hash(string x)
        {
            int p = 31;
            int rez = 0;
            for (int i = 0; i < x.Length; i++)
            {
                rez += (int)Math.Pow(p, x.Length - 1 - i) * (int)(x[i]);
            }
            return rez;
        }
        // Функция поиска алгоритмом Рабина-Карпа
        public List<SearchResult> Rabina(Data data, string searchText)
        {
            List<SearchResult> foundResults = new List<SearchResult>();
            string buffer = data.Buffer;
            if (searchText.Length > buffer.Length) return foundResults;

            // Вычисление хэшкода искомого текста
            int xhash = Hash(searchText);

            // Вычисление хэшкода у буфера длинной равной размеру искомого текста
            int shash = Hash(buffer.Substring(0, searchText.Length));
            bool flag;
            int j;
            var count = buffer.Length - searchText.Length;
            for (int i = 0; i <= count; i++)
            {
                // Сравнение хешкодов
                if (xhash == shash)
                {
                    flag = true;
                    j = 0;
                    while ((flag == true) && (j < searchText.Length))
                    {
                        // Усли хешкоды равны, проверяем посимвольно
                        if (searchText[j] != buffer[i + j]) flag = false;
                        j++;
                    }
                    if (flag == true)
                    {
                        foundResults.Add(new SearchResult { Position = i });
                    }
                }
                // Вычисление нового хешкода, вычитая хешкод первого символа в старом хешкоде и добавляя хешкод след. символа
                if (i != count)
                    shash = (shash - (int)Math.Pow(31, searchText.Length - 1) * (int)(buffer[i])) * 31 + (int)(buffer[i + searchText.Length]);
            }

            return foundResults;
        }
    }
}
