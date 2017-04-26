using SearchTool.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool
{
    public static class TheValuesFound
    {
        public static ConcurrentQueue<SearchResult> listData = new ConcurrentQueue<SearchResult>();
    }
}
