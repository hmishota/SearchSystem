using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchTool.SearchMethods;

namespace SearchTool.UnitTests.SearchMethods
{
    [TestClass]
    public class FuzzySearchForEachSymbolTests
    {
        private int _limit;

        [TestMethod]
        public void Search_WordInEndSentenceFuzzySearchForEachSymbol_limitOne()
        {
            _limit = 1;
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.FuzzySearchForEachSymbol_LimitOneSymbol();
            Assert.IsTrue(result, "Не находит несколько слов в буфере. Метод FuzzySearchForEachSymbol");
        }

        private SearcherMethodTests MakeSearcherMethodTests()
        {
            return new SearcherMethodTests(MakeFuzzySearchForEachSymbolTests());
        }

        private FuzzySearchForEachSymbol MakeFuzzySearchForEachSymbolTests()
        {
            return new FuzzySearchForEachSymbol(_limit);
        }
    }
}
