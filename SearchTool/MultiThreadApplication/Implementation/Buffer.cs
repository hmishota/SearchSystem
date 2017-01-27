using SearchTool.Interfaces;
using SearchTool.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SearchTool
{

    public class Buffer : IBuffer
    {
        public BlockingCollection<Data> listData = new BlockingCollection<Data>();
        private IBufferInterceptor _interceptor;

        public void RegisterInterceptor(IBufferInterceptor interceptor)
        {
            _interceptor = interceptor;
        }

        // Добавление прочитанных данных в буфер 
        public void Add(Data data)
        {
            // Добавление с предыдущего шага символов на n-1 длины искомого слова
            if (_interceptor != null)
                _interceptor.Intercept(data);

            listData.Add(data);
        }

        // Получение коллекции данных
        public IEnumerable<Data> GetEnumerable()
        {
            return listData.GetConsumingEnumerable();
        }

        // Говорит о том, что больше не будет добавлений
        public void CompleteAdd()
        {
            listData.CompleteAdding();
        }
    }

}
