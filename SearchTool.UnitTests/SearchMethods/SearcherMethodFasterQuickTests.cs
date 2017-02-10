using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchTool.SearchMethods;

namespace SearchTool.UnitTests.SearchMethods
{
    [TestClass]
    public class SearcherMethodFasterQuickTests
    {
        [TestMethod]
        public void Search_WordInBeginSentenceFasterQuick()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_WordInBeginSentence();
            Assert.IsTrue(result, "Не находит слово в начале буфера. Метод FasterQuick");
        }

        [TestMethod]
        public void Search_WordInMediumSentenceFasterQuick()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_WordInMediumSentence();
            Assert.IsTrue(result, "Не находит слово в середине буфера. Метод FasterQuick");
        }

        [TestMethod]
        public void Search_SeveralWordInSentenceFasterQuick()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_SeveralWordInSentence();
            Assert.IsTrue(result, "Не находит несколько слов в буфере. Метод FasterQuick");
        }

        [TestMethod]
        public void Search_NoSerchWordInSentenceFasterQuick()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_NoSerchWordInSentence();
            Assert.AreEqual(0, result, "В буфере не должно быть совпадений. Метод FasterQuick");
        }

        [TestMethod]
        public void Search_WordInEndSentenceFasterQuick()
        {
            var searcherMethod = MakeSearcherMethodTests();
            var result = searcherMethod.Search_WordInEndSentence();
            Assert.IsTrue(result, "Не находит несколько слов в буфере. Метод FasterQuick");
        }

        private SearcherMethodTests MakeSearcherMethodTests()
        {
            return new SearcherMethodTests(new SearcherMethodFasterQuick());
        }
    }
}
