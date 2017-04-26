using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SearchTool.Interfaces;
using SearchTool.Models;

namespace SearchTool.UnitTests
{
    [TestClass]
    public class ReaderTests
    {
        [TestMethod]
        public void ReadAsync_IfBuffStreamPositionEqualsLength_returnNull()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));
            var sizeBufferReader = 1;
            var sizeBufferWritter = 2;

            var reader = new Reader();
            reader.InitVariables(stream, sizeBufferReader, sizeBufferWritter);
            var dataResult = reader.ReadAsync();

            Assert.AreEqual(null, dataResult.Result, "Не вернуло null");
        }

        [TestMethod]
        public void ReadAsync_ReadTheFirstTime_ReturnString()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("1234567890"));
            var sizeBufferReader = 1;
            var sizeBufferWritter = 2;

            var reader = new Reader();
            reader.InitVariables(stream, sizeBufferReader, sizeBufferWritter);
            var dataResult = reader.ReadAsync();

            Assert.AreEqual("12", dataResult.Result.Buffer, "Не верно прочитано при первом проходе");

        }

        [TestMethod]
        public void ReadAsync_ReadTheSecondTime_ReturnString()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("1234567890"));
            var sizeBufferReader = 1;
            var sizeBufferWritter = 2;

            var reader = new Reader();
            reader.InitVariables(stream, sizeBufferReader, sizeBufferWritter);
            reader.ReadAsync();
            var dataResult = reader.ReadAsync();

            Assert.AreEqual("34", dataResult.Result.Buffer, "Не верно прочитано при втором проходе");

        }

    }
}
