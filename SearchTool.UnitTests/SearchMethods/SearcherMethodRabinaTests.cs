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
    public class SearcherMethodRabinaTests
    {
        [TestMethod]
        public void Search_WordInBeginSentenceRabina()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_WordInBeginSentence();
            Assert.IsTrue(result,"Не находит слово в начале буфера. Метод Rabina");
        }

        [TestMethod]
        public void Search_WordInMediumSentenceRabina()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_WordInMediumSentence();
            Assert.IsTrue(result, "Не находит слово в середине буфера. Метод Rabina");
        }

        [TestMethod]
        public void Search_SeveralWordInSentenceRabina()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_SeveralWordInSentence();
            Assert.IsTrue(result, "Не находит несколько слов в буфере. Метод Rabina");
        }

        [TestMethod]
        public void Search_NoSerchWordInSentenceRabina()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_NoSerchWordInSentence();
            Assert.AreEqual(0, result, "В буфере не должно быть совпадений. Метод Rabina");
        }

        [TestMethod]
        public void Search_WordInEndSentenceRabina()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_WordInEndSentence();
            Assert.IsTrue(result, "Не находит несколько слов в буфере. Метод Rabina");
        }

        private SearcherMethodTests MakeSearcherMethodTests()
        {
            return new SearcherMethodTests(new SearcherMethodRabina());
        }


    }
}
