using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchTool.Models;
using Moq;
using SearchTool.Interfaces;

namespace SearchTool.UnitTests.MultiThreadApplication.Implementation
{
    [TestClass]
    public class BufferTests
    {

        [TestMethod]
        public void TryEnqueue_IfStoppedTrue_ReturnFalse()
        {
            var buffer = new Buffer();
            buffer.Stop();
            var result =  buffer.TryEnqueue(new Data());
            Assert.IsFalse(result,"переменная stopped!=true");
        }

        [TestMethod]
        public void TryEnqueue_IfInterceptorNotNull_CallIntercept()
        {
            var data = new Data {Buffer = "123698745", Path = "5", Position = 10};
            var mockBufferInterceptor = new Mock<IBufferInterceptor>();
            mockBufferInterceptor.Setup(x => x.Intercept(data)).Verifiable("dfsdf");
            var buffer = new Buffer();
            buffer.RegisterInterceptor(mockBufferInterceptor.Object);
            var result = buffer.TryEnqueue(data);

            var data1 = buffer.Dequeue();

            Assert.AreEqual(data, data1);

            mockBufferInterceptor.Verify();
        }

        [TestMethod]
        public void TryEnqueue_ThenDequeue()
        {
            var data = new Data { Buffer = "123698745", Path = "5", Position = 10 };
            var buffer = new Buffer();
            var result = buffer.TryEnqueue(data);
            var data1 = buffer.Dequeue();
            Assert.AreEqual(data, data1);
        }

        [TestMethod]
        public async Task TryEnqueueAndDequeueInMultithreading()
        {
           var data = new List<Data>()
            {
                new Data { Buffer = "123698745", Path = "5", Position = 10 },
                new Data { Buffer = "111111111", Path = "6", Position = 9 },
                new Data { Buffer = "222222222", Path = "7", Position = 8 },
                new Data { Buffer = "333333333", Path = "8", Position = 7 },
                new Data { Buffer = "444444444", Path = "9", Position = 6 },
                new Data { Buffer = "555555555", Path = "10", Position = 5 },
                new Data { Buffer = "666666666", Path = "11", Position = 4 },
                new Data { Buffer = "777777777", Path = "12", Position = 3 },
                new Data { Buffer = "888888888", Path = "13", Position = 2 },
                new Data { Buffer = "999999999", Path = "14", Position = 1 }
            };

            var buffer = new Buffer();
            var tasksTryEnqueue = new List<Task>();
            var tasksDequeue = new List<Task<Data>>();


            foreach (var d in data)
            {
                tasksTryEnqueue.Add(Task.Run(() => buffer.TryEnqueue(d)));
            }

            for (int i = 0; i < data.Count; i++)
            {
                tasksDequeue.Add(Task.Run(() => buffer.Dequeue()));
            }
            await Task.WhenAll(tasksTryEnqueue);
            var resultDequeue = await Task.WhenAll(tasksDequeue);
            bool logic = true;
            foreach (var item in tasksDequeue)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].Buffer == item.Result.Buffer && data[i].Path == item.Result.Path &&
                        data[i].Position == item.Result.Position)
                    {
                        data.RemoveAt(i);
                        break;
                    }
                    if (i == (data.Count - 1))
                    {
                        logic = false;
                        break;
                    }
                }
                if(logic==false)
                    break;
            }
            
            Assert.IsTrue(logic);
        }



    }
}
