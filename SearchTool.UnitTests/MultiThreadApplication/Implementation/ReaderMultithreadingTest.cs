using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SearchTool.Models;
using Microsoft.Practices.Unity;
using SearchTool.Interfaces;

namespace SearchTool.UnitTests.MultiThreadApplication.Implementation
{

    public class FakeUnityContainer : IUnityContainer
    {
        private IReader _reader;
        public IBuffer Buffer;


        public FakeUnityContainer(IEnumerable<Data> datas)
        {
            var moqReader = new Mock<IReader>();
            var s = moqReader.SetupSequence(x => x.ReadAsync());

            foreach (var data in datas)
            {
                s = s.Returns(Task.FromResult(data));
            }
            _reader = moqReader.Object;

            Buffer = new Buffer();
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
            if (t.Name == typeof (IReader).Name)
            {
                return _reader;
            }
            if (t.Name == typeof (IBuffer).Name)
                return Buffer;
            return null;
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
    public class ReaderMultithreadingTest
    {
        [TestMethod]
        public void ReadAsync_ReadOneFile()
        {
            Models.File file = new File("C:\\MyProject\\SearchSystem\\1232131");
            var sizeBufferReader = 1;
            var sizeBufferWritter = 2;


            ReaderMultithreading reader = new ReaderMultithreading();

            var expected = new List<Data>()
            {
                new Data {Buffer = "111"},
                new Data{Buffer = "222"},
                new Data{Buffer = "333"},
                new Data{Buffer = "444"},
                new Data{Buffer = "555"},
                new Data{Buffer = "666"},
                new Data{Buffer = "777"},
                new Data{Buffer = "888"}
            };

            var container = new FakeUnityContainer(expected);

            var moqFileOpen = new Mock<IFileOpen>();

            reader.RegisterReadWithCounts(container, moqFileOpen.Object);
            reader.ReadAsync(file, sizeBufferReader, sizeBufferWritter).Wait();


            var actualResult = new List<Data>();
            container.Buffer.Stop();
            Data dataDequueue = container.Buffer.Dequeue();
            bool variableEquals = true;
            int k = 0;
            while (dataDequueue != null)
            {
                actualResult.Add(dataDequueue);
                dataDequueue = container.Buffer.Dequeue();

            }

            foreach (var item in actualResult)
            {
                for (int i = 0; i < expected.Count; i++)
                {
                    if (expected[i].Buffer == item.Buffer && expected[i].Path == item.Path &&
                        expected[i].Position == item.Position)
                    {
                        expected.RemoveAt(i);
                        break;
                    }
                    if (i == (expected.Count - 1))
                    {
                        variableEquals = false;
                        break;
                    }
                }
                if (variableEquals == false)
                    break;
            }

            Assert.IsTrue(variableEquals);

        }
    }
}
