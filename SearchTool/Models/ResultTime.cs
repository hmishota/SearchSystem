using System.Collections.Concurrent;

namespace SearchTool.Models
{
    public static class ResultTime
    {
        public static ConcurrentQueue<long> queryListRead;
        public static ConcurrentQueue<long> queryListSearch;

        static ResultTime()
        {
            queryListRead = new ConcurrentQueue<long>();
            queryListSearch = new ConcurrentQueue<long>();
        }
    }
}
