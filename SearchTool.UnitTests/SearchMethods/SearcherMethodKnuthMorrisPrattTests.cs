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
    public class SearcherMethodKnuthMorrisPrattTests
    {
        [TestMethod]
        public void Search_WordInBeginSentenceKnuthMorrisPratt()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_WordInBeginSentence();
            Assert.IsTrue(result, "Не находит слово в начале буфера. Метод KnuthMorrisPratt");
        }

        [TestMethod]
        public void Search_WordInMediumSentenceKnuthMorrisPratt()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_WordInMediumSentence();
            Assert.IsTrue(result, "Не находит слово в середине буфера. Метод KnuthMorrisPratt");
        }

        [TestMethod]
        public void Search_SeveralWordInSentenceKnuthMorrisPratt()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_SeveralWordInSentence();
            Assert.IsTrue(result, "Не находит несколько слов в буфере. Метод KnuthMorrisPratt");
        }

        [TestMethod]
        public void Search_NoSerchWordInSentenceKnuthMorrisPratt()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_NoSerchWordInSentence();
            Assert.AreEqual(0, result, "В буфере не должно быть совпадений. Метод KnuthMorrisPratt");
        }

        [TestMethod]
        public void Search_WordInEndSentenceKnuthMorrisPratt()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_WordInEndSentence();
            Assert.IsTrue(result, "Не находит несколько слов в буфере. Метод KnuthMorrisPratt");
        }

        private SearcherMethodTests MakeSearcherMethodTests()
        {
            return new SearcherMethodTests(new SearcherMethodKnuthMorrisPratt());
        }

    }
}
