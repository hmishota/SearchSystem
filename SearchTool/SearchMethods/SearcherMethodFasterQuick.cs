using SearchTool.Interfaces;
using SearchTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool.SearchMethods
{
    public class SearcherMethodFasterQuick : ISearcherMethod
    {
        public List<SearchResult> Search(Data text, string searchText)
        {
            List<SearchResult> searchResult = new List<SearchResult>();
            searchResult = FasterQuick(text, searchText);
            foreach (var search in searchResult)
            {
                search.File = new File(text.Path);
                search.Position = text.Position + search.Position;
            }
            return searchResult;
        }


        public void PreQS(string P, int m, char[] uniqueSymbolsInSearchTexts, out Dictionary<char, int> qsBc)
        {
            int count = uniqueSymbolsInSearchTexts.Count();
            qsBc = new Dictionary<char, int>(count);
            int newM = m + 1;
            for (int i = 0; i < count; i++)
            {
                qsBc.Add(uniqueSymbolsInSearchTexts[i], newM);
            }

            for (int i = 0; i < m; i++)
            {
                qsBc[P[i]] = m - i;
            }
        }

        public int GetPos(string P,int m, char[] uniqueSymbolsInSearchTexts)
        {
            var ES = 0;
            var maxES = 0;
            var pos = 0;
            int count = uniqueSymbolsInSearchTexts.Count();
            Dictionary<char, int> PrePro = new Dictionary<char, int>(count);

            for (int i = 0; i < count; i++)
            {
                //PrePro.Add(P[i], -1);
                PrePro[uniqueSymbolsInSearchTexts[i]] = -1;
            }

            for (int j = 0; j < m; j++)
            {
                ES = ES + count - (j - PrePro[P[j]]);
                PrePro[P[j]] = j;
                if (ES >= maxES)
                {
                    maxES = ES;
                    pos = j;
                }
            }

            return pos;
        }

        public List<SearchResult> FasterQuick(Data data, string P)
        {
            List<SearchResult> foundResults = new List<SearchResult>();

            string T = data.Buffer;
            int m = P.Length;
            int n = T.Length;
            var uniqueSymbolsInSearchTexts = P.Distinct().ToArray();
            Dictionary<char, int> next, shift;
            var pos = GetPos(P, m, uniqueSymbolsInSearchTexts);
            PreQS(P, pos, uniqueSymbolsInSearchTexts, out next);
            PreQS(P, m, uniqueSymbolsInSearchTexts, out shift);
            int j = 0;
            while (j <= n - m)
            {
                while (P[pos] != T[j + pos])
                {
                    j = j + next[T[j + pos]];
                    if (j > (n - m))
                    {
                        return foundResults;
                    }
                }
                int compare = String.Compare(P,T.Substring(j,m));

                if(compare==0)
                {
                    foundResults.Add(new SearchResult { Position = j });
                }

                j = j + shift[T[j + m]];
            }
            return foundResults;
        }

    }
}
