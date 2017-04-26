using SearchTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool
{
    public class Tokenizer
    {
        public Dictionary<string, List<SearchResult>> Parse(Data text)
        {
            string str = text.Buffer;
            var stringSeparators = new char[] { ' ', '-', '.', '?', '!', ')', '(', ',', ':', '\n', '\r', '—','/','\\' };
            var results = str.Split(stringSeparators, StringSplitOptions.None);
            long k = text.Position;
            var tokens = new Dictionary<string, List<SearchResult>>();
            List<SearchResult> list;
            SearchResult searchResult;
            foreach (var res in results)
            {
                if (res != "")
                {
                    if (tokens.TryGetValue(res, out list))
                    {
                        searchResult = new SearchResult() {File = new File(text.Path), Position = k };
                        list.Add(searchResult);
                        tokens[res] = list;
                    }
                    else
                    {
                        list = new List<SearchResult>();
                        searchResult = new SearchResult() { File = new File(text.Path), Position = k };
                        list.Add(searchResult);
                        tokens.Add(res, list);

                    }
                    k = k + res.Length + 1;
                }
                else
                {
                    k = k + 1;
                }
            }
            return tokens;
        }
    }
}
