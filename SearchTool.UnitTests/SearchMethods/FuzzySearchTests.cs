using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SearchTool.UnitTests.SearchMethods
{
    [TestClass]
    public class FuzzySearchTests
    {
        private int _limit = 3;

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
    "Если dataBufferPart1 равно null в методе LevenshteinDistance, то тогда ожидается ошибка")]
        public void LevenshteinDistance_dataBufferPart1_ArgumentNullException()
        {
            string dataBufferPart1 = null;
            string searchText = "Text";
            var fuzzySearcher = MakeFuzzySearchTests();
            var Result = fuzzySearcher.LevenshteinDistance(dataBufferPart1, searchText);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
    "Если dataBufferPart2 равно null в методе LevenshteinDistance, то тогда ожидается ошибка")]
        public void LevenshteinDistanceReverse_dataBufferPart2_ArgumentNullException()
        {
            string dataBufferPart1 = "Text";
            string searchText = null;
            var fuzzySearcher = MakeFuzzySearchTests();
            var Result = fuzzySearcher.LevenshteinDistanceReverse(dataBufferPart1, searchText);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
    "Если dataBufferPart1 равно null в методе LevenshteinDistanceReverse, то тогда ожидается ошибка")]
        public void LevenshteinDistanceReverse_dataBufferPart1_ArgumentNullException()
        {
            string dataBufferPart1 = null;
            string searchText = "Text";
            var fuzzySearcher = MakeFuzzySearchTests();
            var Result = fuzzySearcher.LevenshteinDistanceReverse(dataBufferPart1, searchText);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
    "Если dataBufferPart2 равно null в методе LevenshteinDistanceReverse, то тогда ожидается ошибка")]
        public void LevenshteinDistance_dataBufferPart2_ArgumentNullException()
        {
            string dataBufferPart1 = "Text";
            string searchText = null;
            var fuzzySearcher = MakeFuzzySearchTests();
            var Result = fuzzySearcher.LevenshteinDistance(dataBufferPart1, searchText);
        }

        [TestMethod]
        public void LevenshteinDistance_InputCorrectedData_ReturnArrayDifference()
        {
            string dataBufferPart1 = "ACGTAC";
            string searchText = "AGTACCTACCGT";
            int[] expected = {6, 5, 4, 3, 2, 1, 2, 3, 4, 4, 5, 6, 7}; 

            var fuzzySearcher = MakeFuzzySearchTests();
            int[] result = fuzzySearcher.LevenshteinDistance(dataBufferPart1, searchText);
            var compare = expected.SequenceEqual(result);
            Assert.IsTrue(compare, "Неправильно подсчитывает сколько изменений необходимо для того чтобы два слова стали равны. Считая сверху вниз.");
        }

        [TestMethod]
        public void LevenshteinDistanceReverse_InputCorrectedData_ReturnArrayDifference()
        {
            string dataBufferPart1 = "CGTACGT";
            string searchText = "AGTACCTACCGT";
            int[] expected = { 6, 6, 5, 4, 3, 2, 2, 3, 4, 3, 4, 5, 6 };

            var fuzzySearcher = MakeFuzzySearchTests();
            int[] result = fuzzySearcher.LevenshteinDistanceReverse(dataBufferPart1, searchText);
            var compare = expected.SequenceEqual(result);
            Assert.IsTrue(compare, "Неправильно подсчитывает сколько изменений необходимо для того чтобы два слова стали равны. Считая снизу в верх.");
        }

        [TestMethod]
        public void MinCountChangesTests()
        {
            int[] distanceSearchTextPart1 = { 6, 5, 4, 3, 2, 1, 2, 3, 4, 4, 5, 6, 7 };
            int[] distanceSearchTextPart2 = { 6, 6, 5, 4, 3, 2, 2, 3, 4, 3, 4, 5, 6 };
            int expected = 3;

            var fuzzySearcher = MakeFuzzySearchTests();
            var result = fuzzySearcher.MinCountChanges(distanceSearchTextPart1, distanceSearchTextPart2);

            Assert.AreEqual(expected, result, "Неправильно определяет минимальное число изменений, которые необходимо сделать для того чтобы слова стали равны.");

        }

        [TestMethod]
        public void CompareTwoWords_InputDataBufferEquallyOne_ReturnSearchTextLength()
        {
            var dataBuffer = "1";
            var searchText = "SearchText";
            int expected = searchText.Length;

            var fuzzySearcher = MakeFuzzySearchTests();
            var result = fuzzySearcher.CompareTwoWords(dataBuffer,searchText);

            Assert.AreEqual(expected, result, "Если dataBuffer равен 1, вернуть необходимо длину поискового слова.");

        }

        [TestMethod]
        public void CompareTwoWords_InputDataBufferEquallyNull_ReturnSearchTextLength()
        {
            var dataBuffer = "";
            var searchText = "SearchText";
            int expected = searchText.Length;

            var fuzzySearcher = MakeFuzzySearchTests();
            var result = fuzzySearcher.CompareTwoWords(dataBuffer, searchText);

            Assert.AreEqual(expected, result, "Если dataBuffer равен 0, вернуть необходимо длину поискового слова.");
        }

        [TestMethod]
        public void CompareTwoWords()
        {
            var dataBuffer = "ACGTACGTACGT";
            var searchText = "AGTACCTACCGT";
            int expected = 3;

            var fuzzySearcher = MakeFuzzySearchTests();
            var result = fuzzySearcher.CompareTwoWords(dataBuffer, searchText);

            Assert.AreEqual(expected, result, "Неправильно находит минимальное число изменений в методе CompareTwoWords");
        }

        [TestMethod]
        public void Search_WordInEndSentenceFuzzy()
        {
            _limit = 1;
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.FuzzySearch_LimitOneSymbol();
            Assert.IsTrue(result, "Не находит несколько слов в буфере. Метод Fuzzy");
        }

        private SearcherMethodTests MakeSearcherMethodTests()
        {
            return new SearcherMethodTests(MakeFuzzySearchTests());
        }

        private FuzzySearch MakeFuzzySearchTests()
        {
            return new FuzzySearch(_limit);
        }
    }
}
