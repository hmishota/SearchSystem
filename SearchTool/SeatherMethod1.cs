using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class SeatherMethod1:ISearcher
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

        //Хеш-функция для алгоритма Рабина-Карпа
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
        //Функция поиска алгоритмом Рабина-Карпа
        public List<SearchResult> Rabina(Data searchText, string x)
        {
            List<SearchResult> foundResults = new List<SearchResult>();
            string s = searchText.Buffer;
            if (x.Length > s.Length) return foundResults; 
            int xhash = Hash(x);
            int shash = Hash(s.Substring(0, x.Length)); 
            bool flag;
            int j;
            var count = s.Length - x.Length;
            for (int i = 0; i <= count; i++)
            {
                if (xhash == shash)
                {
                    flag = true;
                    j = 0;
                    while ((flag == true) && (j < x.Length))
                    {
                        if (x[j] != s[i + j]) flag = false;
                        j++;
                    }
                    if (flag == true)
                    {
                        foundResults.Add(new SearchResult { Position = i});
                    }
                }
                if (i != count )
                    shash = (shash - (int)Math.Pow(31, x.Length - 1) * (int)(s[i])) * 31 + (int)(s[i + x.Length]);
            }
            
            return foundResults;
        }
    }
}
