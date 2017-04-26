using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Moq;
using SearchTool.Interfaces;
using SearchTool.Models;

namespace SearchTool.UnitTests.MultiThreadApplication.Implementation
{
    public class FakeSearcherUnityContainer : IUnityContainer
    {
        public ISearcherMethod _searcherNeMethod;
        public IBuffer Buffer;

        public FakeSearcherUnityContainer(List<Data> data, string source)
        {
            var mockBuffer = new Mock<IBuffer>();
            var s = mockBuffer.SetupSequence(x => x.Dequeue());

            foreach (var item in data)
            {
                s = s.Returns(item);
            }

            Buffer = mockBuffer.Object;

            var mochMethod = new Mock<ISearcherMethod>();
            List<SearchResult> result = new List<SearchResult>();
            result.Add(new SearchResult {Position = 4});
            List<SearchResult> notfoundResults = new List<SearchResult>();

            //mochMethod.Setup(x => x.Search(It.IsAny<Data>(), It.IsAny<string>())).Returns(result);

            mochMethod.Setup(x => x.Search(data[0], source)).Returns(notfoundResults);
            mochMethod.Setup(x => x.Search(data[1], source)).Returns(notfoundResults);
            mochMethod.Setup(x => x.Search(data[2], source)).Returns(notfoundResults);
            mochMethod.Setup(x => x.Search(data[3], source)).Returns(result);
            mochMethod.Setup(x => x.Search(data[4], source)).Returns(notfoundResults);
            mochMethod.Setup(x => x.Search(data[5], source)).Returns(notfoundResults);
            mochMethod.Setup(x => x.Search(data[6], source)).Returns(notfoundResults);
            mochMethod.Setup(x => x.Search(data[7], source)).Returns(notfoundResults);

            _searcherNeMethod = mochMethod.Object;
            //_searcherNeMethod = new SearcherMethodRabina();
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

        public IUnityContainer RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            if (t.Name == typeof(ISearcherMethod).Name)
                return _searcherNeMethod;

            if (t.Name == typeof(IBuffer).Name)
                return Buffer;

            return null;
        }

        public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            throw new NotImplementedException();
        }

        public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            throw new NotImplementedException();
        }

        public void Teardown(object o)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            throw new NotImplementedException();
        }

        public object Configure(Type configurationInterface)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RemoveAllExtensions()
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
    }
    // переписать этот метод
    //[TestClass]
    //public class SearcherMethodDecoratorTests
    //{
    //    [TestMethod]
    //    public void SearchInternalAsync_()
    //    {
    //        //var mockUnityContainer = new Mock<IUnityContainer>();
    //        string source = "hello";
    //        var expected = new List<Data>()
    //        {
    //            new Data {Buffer = "111"},
    //            new Data {Buffer = "222"},
    //            new Data {Buffer = "333"},
    //            new Data {Buffer = "hello"},
    //            new Data {Buffer = "555"},
    //            new Data {Buffer = "666"},
    //            new Data {Buffer = "777"},
    //            new Data {Buffer = "888"}
    //        };

    //        FakeSearcherUnityContainer container = new FakeSearcherUnityContainer(expected, source);
    //        SearcherMethodDecorator searcher = new SearcherMethodDecorator(container, 4);
    //        var result = searcher.SearchInternalAsync(source);
    //        result.Wait();
    //        Assert.AreEqual(4,result.Result[0].Position);
    //    }
        
    //}
}
