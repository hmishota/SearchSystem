using SearchTool.Interfaces;
using SearchTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool
{
    public class BufferInterceptor : IBufferInterceptor
    {
        Dictionary<string, Data> storage;
        private int _searchTextLength;

        public BufferInterceptor(string source)
        {
            _searchTextLength = source.Length;
        }

        public void Intercept(Data data)
        {
            if (storage.ContainsKey(data.Path))
            {
                var prevData = storage[data.Path];
                string str = prevData.Buffer.Substring(prevData.Buffer.Length - (_searchTextLength- 1));
                data.Buffer = str + data.Buffer;
            }

            storage[data.Path] = data;
        }
    }
}
