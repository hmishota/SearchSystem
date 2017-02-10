using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool.Models
{
    public static class PrintFindFile
    {
        public static void Print(List<File> list)
        {
            foreach (var p in list)
            {
                Console.WriteLine(p.Path);
            }
        }
    }
}
