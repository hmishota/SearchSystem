using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchTool.Models;

namespace SearchTool.UnitTests.MultiThreadApplication.Implementation
{
    [TestClass]
    public class BufferInterceptorTests
    {

        [TestMethod]
        public void Intercept_searchTextLengthMorePrevDataBufferLength_AddPrevDataBuffer()
        {
            string source = "999888777444555666";
            Data data1 = new Data {Buffer = "1236", Position = 2, Path = "222"};
            Data data2 = new Data {Buffer = "789", Position = 20, Path = "222"};

            BufferInterceptor bufferInterceptor = new BufferInterceptor(source);
            bufferInterceptor.Intercept(data1);
            bufferInterceptor.Intercept(data2);

            Assert.AreEqual("1236789", data2.Buffer,"Если буфер меньше чем поисковое слово, то этот буфер должен добавиться целиком");
            Assert.AreEqual(0, data2.Position,"Позиция должна быть равной нулю");

        }

        [TestMethod]
        public void Intercept_IfDataNotContainInConcurrentDictionaryStorage_ReturnWithoutChanges()
        {
            string source = "999888777444555666";
            Data data = new Data { Buffer = "1236", Position = 2, Path = "222" };

            BufferInterceptor bufferInterceptor = new BufferInterceptor(source);
            bufferInterceptor.Intercept(data);

            Assert.AreEqual("1236", data.Buffer,"Изменился буфер, при первом вызове он не должны измениться.");
            Assert.AreEqual(2, data.Position, "Изменилась позиция, при первом вызове она не должны измениться.");


        }

        [TestMethod]
        public void Intercept_searchTextLengthLessPrevDataBufferLength_AddPrevDataBuffer()
        {
            string source = "12345";
            Data data1 = new Data { Buffer = "PrevData", Position = 20, Path = "222" };
            Data data2 = new Data { Buffer = "StringBuffer", Position = 28, Path = "222" };

            BufferInterceptor bufferInterceptor = new BufferInterceptor(source);
            bufferInterceptor.Intercept(data1);
            bufferInterceptor.Intercept(data2);

            Assert.AreEqual("DataStringBuffer", data2.Buffer,"Ошибка при склеивании части данных и dataBuffer");
            Assert.AreEqual(24, data2.Position, "Ошибка при определении позиции при склеивании данных");

        }
    }
}
