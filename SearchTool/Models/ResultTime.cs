using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
