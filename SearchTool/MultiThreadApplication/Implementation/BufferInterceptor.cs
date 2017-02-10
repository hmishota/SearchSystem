using SearchTool.Interfaces;
using SearchTool.Models;
using System.Collections.Concurrent;

namespace SearchTool
{
    public class BufferInterceptor : IBufferInterceptor
    {
        ConcurrentDictionary<string, Data> storage;
        private int _searchTextLength;

        public BufferInterceptor(string source)
        {
            _searchTextLength = source.Length;
            storage = new ConcurrentDictionary<string, Data>();
        }

        // Добавление с предыдущего шага символов на n-1 длины искомого слова
        public void Intercept(Data data)
        {
            // Проверяется содержится ли в словаре такой путь
            if (storage.ContainsKey(data.Path))
            {
                var prevData = storage[data.Path];
                string str;

                // Если длина искомого слова меньше размера буфера
                if (_searchTextLength < prevData.Buffer.Length)
                {
                    str = prevData.Buffer.Substring(prevData.Buffer.Length - (_searchTextLength - 1));
                    data.Position = data.Position - (_searchTextLength - 1);
                }
                // Иначе просто присвоить весь буфер строке
                else
                {
                    str = prevData.Buffer;
                    data.Position = 0; 
                }

                data.Buffer = str + data.Buffer;
            }
            storage[data.Path] = data;
        }
    }
}
