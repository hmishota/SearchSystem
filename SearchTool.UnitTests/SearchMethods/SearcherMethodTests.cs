using System.Collections.Generic;
using System.Linq;
using SearchTool.Interfaces;
using SearchTool.Models;

namespace SearchTool.UnitTests.SearchMethods
{
    public class SearcherMethodTests
    {
        private ISearcherMethod _searcher;

        public SearcherMethodTests(ISearcherMethod searcher)
        {
            _searcher = searcher;
        }

        public bool Search_WordInBeginSentence()
        {
            Data begindata = new Data
            {
                Buffer = "to hello world. SeekOrigin is used the helloby the Seek methodshello of Stream, BufferedStream, FileStream, MemoryStream, BinaryWriter, and other classes.Hello. The Seek methods take an offset parameter that is relative to the position specified by SeekOrigin.",
                Path = "C:\\MyProject\\SearchSystem\\1232131",
                Position = 0
            };
            List<SearchResult> expectedData = new List<SearchResult>
            {
                new SearchResult {Position = 3, File = new File("C:\\MyProject\\SearchSystem\\1232131")},
                new SearchResult {Position = 39, File = new File("C:\\MyProject\\SearchSystem\\1232131")},
                new SearchResult {Position = 63, File = new File("C:\\MyProject\\SearchSystem\\1232131")}

            };

            var source = "hello";
            var searchResult = _searcher.Search(begindata, source);
            return expectedData.SequenceEqual(searchResult);
        }

        public bool Search_WordInMediumSentence()
        {
            Data begindata = new Data
            {
                Buffer = "3sdsfhellosdfef",
                Path = "C:\\MyProject\\SearchSystem\\1232131",
                Position = 0
            };
            List<SearchResult> expectedData = new List<SearchResult>
            {
                new SearchResult {Position = 5, File = new File("C:\\MyProject\\SearchSystem\\1232131")}
            };

            var source = "hello";
            var result = _searcher.Search(begindata, source);
            return expectedData.SequenceEqual(result);
        }

        public bool Search_SeveralWordInSentence()
        {
            Data begindata = new Data
            {
                Buffer = "3sdhellosfhellosdfef",
                Path = "C:\\MyProject\\SearchSystem\\1232131",
                Position = 0
            };
            List<SearchResult> expectedData = new List<SearchResult>
            {
                new SearchResult {Position = 3, File = new File("C:\\MyProject\\SearchSystem\\1232131")},
                new SearchResult {Position = 10, File = new File("C:\\MyProject\\SearchSystem\\1232131")}

            };

            var source = "hello";
            var result = _searcher.Search(begindata, source);
            return expectedData.SequenceEqual(result);
        }

        public bool Search_WordInEndSentence()
        {
            Data begindata = new Data
            {
                Buffer = "3sdsfhosdfefhello",
                Path = "C:\\MyProject\\SearchSystem\\1232131",
                Position = 0
            };
            List<SearchResult> expectedData = new List<SearchResult>
            {
                new SearchResult {Position = 12, File = new File("C:\\MyProject\\SearchSystem\\1232131")}
            };

            var source = "hello";
            var result = _searcher.Search(begindata, source);
            return expectedData.SequenceEqual(result);
        }

        public int Search_NoSerchWordInSentence()
        {
            Data begindata = new Data
            {
                Buffer = "3sdsfhosdfefhelo",
                Path = "C:\\MyProject\\SearchSystem\\1232131",
                Position = 0
            };
           
            var source = "hello";
            var result = _searcher.Search(begindata, source);
            return result.Count();
        }
    }
}
