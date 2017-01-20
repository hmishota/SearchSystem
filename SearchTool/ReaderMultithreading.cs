using SearchTool.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchTool.Models;
using System.Threading;

namespace SearchTool
{
    public class ReaderMultithreading : IReader
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void InitVariables(int sizeBufferReader, int sizeBufferWritter, File f)
        {
            throw new NotImplementedException();
        }

        public async Task<Data> Read()
        {
            return new Data();
}

        public async Task<Data> Read(File file, CancellationToken cancel)
        {
            //if(дочитала до конца)
            //    {
            //    cancel.ThrowIfCancellationRequested();
            //}


            ////читаю как-то и получаю дата
            Data data = new Data();




            IBuffer buffer = new Buffer();
            buffer.Add(data);
            return data;
        }
    }
}
