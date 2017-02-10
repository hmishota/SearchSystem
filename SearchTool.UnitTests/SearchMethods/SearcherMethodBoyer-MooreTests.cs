using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchTool.SearchMethods;

namespace SearchTool.UnitTests.SearchMethods
{
    [TestClass]
    public class SearcherMethodBoyer_MooreTests
    {
        [TestMethod]
        public void Search_WordInBeginSentenceBoyer_Moore()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_WordInBeginSentence();
            Assert.IsTrue(result, "Не находит слово в начале буфера. Метод Boyer_Moore");
        }

        [TestMethod]
        public void Search_WordInMediumSentenceBoyer_Moore()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_WordInMediumSentence();
            Assert.IsTrue(result, "Не находит слово в середине буфера. Метод Boyer_Moore");
        }

        [TestMethod]
        public void Search_SeveralWordInSentenceBoyer_Moore()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_SeveralWordInSentence();
            Assert.IsTrue(result, "Не находит несколько слов в буфере. Метод Boyer_Moore");
        }

        [TestMethod]
        public void Search_NoSerchWordInSentenceBoyer_Moore()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_NoSerchWordInSentence();
            Assert.AreEqual(0, result, "В буфере не должно быть совпадений. Метод Boyer_Moore");
        }

        [TestMethod]
        public void Search_WordInEndSentenceBoyer_Moore()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_WordInEndSentence();
            Assert.IsTrue(result, "Не находит несколько слов в буфере. Метод Boyer_Moore");
        }

        private SearcherMethodTests MakeSearcherMethodTests()
        {
            return new SearcherMethodTests(new SearcherMethodBoyer_Moore());
        }
    }
}
