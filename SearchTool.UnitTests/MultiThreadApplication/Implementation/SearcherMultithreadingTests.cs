using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SearchTool.Interfaces;
using Microsoft.Practices.Unity;
using SearchTool.Models;

namespace SearchTool.UnitTests.MultiThreadApplication.Implementation
{
    public class FakeSearcherMultithreading : IUnityContainer
    {
        private IReaderMulti _reader;
        public FakeSearcherMultithreading(IEnumerable<Data> datas)
        {
            var moskReader = new Mock<IReaderMulti>();
            var s = moskReader.Setup(x => x.ReadAsync(It.IsAny<File>(), It.IsAny<int>(), It.IsAny<int>()));
            var reader = new Reader();
            foreach (var data in datas)
            {
                s = s.Returns(new Task(f));
            }
            _reader = moskReader.Object;
        }

        public void f()
        {
            
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
            throw new NotImplementedException();
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
        [TestMethod]
        public void Search()
        {
           
            string searchText = "hello";

            var mockFileManager = new Mock<IFileManager>();
            List<File> files = new List<File>()
            {
                new File("233131"),
                new File("545454"),
                new File("7878778"),
                new File("910910910")
            };
            var mockSearherMethod = new Mock<ISearcherMethod>();
            FakeSearcherMultithreading container = new FakeSearcherMultithreading();
            mockFileManager.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<bool>())).Returns(files);
            SearcherMultithreading searcher = new SearcherMultithreading(mockFileManager.Object,
               mockSearherMethod.Object, container);
        }

        [TestMethod]
        public void DeterminationMinValue_SizeBufferReaderMoreSizeBufferWritter_Swap()
        {
            var mockFileManager = new Mock<IFileManager>();
            var mockSearherMethod = new Mock<ISearcherMethod>();
            var mockUnityContainer = new Mock<IUnityContainer>();

            SearcherMultithreading searcher = new SearcherMultithreading(mockFileManager.Object,
                mockSearherMethod.Object, mockUnityContainer.Object);
            int beginSizeBufferReader = 150;
            int beginSizeBufferWritter = 50;

            SearcherMultithreading.SizeBufferReader = beginSizeBufferReader;
            SearcherMultithreading.SizeBufferWritter = beginSizeBufferWritter;

            searcher.DeterminationMinValue();
            Assert.AreEqual(beginSizeBufferReader, SearcherMultithreading.SizeBufferWritter);
            Assert.AreEqual(beginSizeBufferWritter, SearcherMultithreading.SizeBufferReader);

        }
    }
}
