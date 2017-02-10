using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SearchTool.Interfaces;
using Microsoft.Practices.Unity;
using SearchTool.Models;
using SearchTool.SearchMethods;

namespace SearchTool.UnitTests.MultiThreadApplication.Implementation
{
    public class FakeSearcherMultithreading : IUnityContainer
    {
        private IReaderMulti _reader;
        private IBuffer _buffer;
        private ISearcherMethod _searcher;
        private ISearcherMethodDecorator _SearcherMethodDecorator;
        private WatchAndCount watch = new WatchAndCount();

        public FakeSearcherMultithreading(List<Data> datas)
        {
            var endData = new List<Data>();

            var mockBuffer = new Mock<IBuffer>();
            _buffer = mockBuffer.Object;
            var resultDeque = mockBuffer.SetupSequence(x => x.Dequeue());

            // Буффер при каждом вызове метода buffer.SetupSequence разное значение
            foreach (var data in datas)
            {
                resultDeque = resultDeque.Returns(data);
            }
            
            _searcher = new SearcherMethodRabina();

            var mockReader = new Mock<IReaderMulti>();
            _reader = mockReader.Object;

            _SearcherMethodDecorator = new SearcherMethodDecorator(this, 4);
        }

        public IUnityContainer Parent
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<ContainerRegistration> Registrations
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            throw new NotImplementedException();
        }

        public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            throw new NotImplementedException();
        }

        public object Configure(Type configurationInterface)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer CreateChildContainer()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RemoveAllExtensions()
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            if (t.Name == (typeof(IReaderMulti).Name))
                return _reader;
            if (t.Name == (typeof(ISearcherMethodDecorator).Name))
                return _SearcherMethodDecorator;
            if (t.Name == (typeof(IBuffer).Name))
                return _buffer;
            if (t.Name == (typeof(ISearcherMethod).Name))
                return _searcher;
            return t.Name == (typeof(WatchAndCount).Name) ? watch : null;
        }

        public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            throw new NotImplementedException();
        }

        public void Teardown(object o)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class SearcherMultithreadingTests
    {
        [Ignore]
        [TestMethod]
        public void Search()
        {
            var searchText = "hello";
            var beginData = new List<Data>()
            {
                new Data {Buffer = "111", Position = 8, Path = "C:\\MyProject\\SearchSystem\\1232131"},
                new Data {Buffer = "222", Position = 18, Path = "C:\\MyProject\\SearchSystem\\1232131"},
                new Data {Buffer = "333", Position = 28, Path = "C:\\MyProject\\SearchSystem\\55555"},
                new Data {Buffer = "444", Position = 38, Path = "C:\\MyProject\\SearchSystem\\1232131"},
                new Data {Buffer = "hello", Position = 48, Path = "C:\\MyProject\\SearchSystem\\5556"},
                new Data {Buffer = "666", Position = 58, Path = "C:\\MyProject\\SearchSystem\\1232131"},
                new Data {Buffer = "777", Position = 68, Path = "C:\\MyProject\\SearchSystem\\1232131"},
                new Data {Buffer = "hello", Position = 4, Path = "C:\\MyProject\\SearchSystem\\1232131"}
            };

            var expected = new List<SearchResult>()
            {
                new SearchResult {Position = 48, File = new File("C:\\MyProject\\SearchSystem\\5556")},
                new SearchResult {Position = 4, File = new File("C:\\MyProject\\SearchSystem\\1232131")}
            };

            var mockFileManager = new Mock<IFileManager>();
            FakeSearcherMultithreading container = new FakeSearcherMultithreading(beginData);
            List<File> files = new List<File>();
            mockFileManager.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<bool>())).Returns(files);
            int sizeBufferReader = 10, sizeBufferWritter = 50;
            SearcherMultithreading searcher = new SearcherMultithreading(mockFileManager.Object,
                container, sizeBufferReader, sizeBufferWritter);
            var result = searcher.Search(It.IsAny<string>(), It.IsAny<bool>(), searchText);
            result.Wait();

           
            var comparer = new ComparerSearchResult();
            expected.Sort(comparer);
            result.Result.Sort(comparer);
            var compareBeginAndEnd = expected.SequenceEqual(result.Result);


            Assert.IsTrue(compareBeginAndEnd, "Не корректно находит слово. "+ result.Result.Count);

        }

        [TestMethod]
        public void DeterminationMinValue_SizeBufferReaderMoreSizeBufferWritter_Swap()
        {
            var mockFileManager = new Mock<IFileManager>();
            var mockUnityContainer = new Mock<IUnityContainer>();

            int beginSizeBufferReader = 150, endSizeBufferReader=50;
            int beginSizeBufferWritter = 50, endSizeBufferWritter = 150;


            SearcherMultithreading searcher = new SearcherMultithreading(mockFileManager.Object,
                mockUnityContainer.Object,beginSizeBufferReader,beginSizeBufferWritter);

            searcher.DeterminationMinValue();
            Assert.AreEqual(endSizeBufferReader, searcher._sizeBufferReader);
            Assert.AreEqual(endSizeBufferWritter, searcher._sizeBufferWritter);

        }
    }
}
