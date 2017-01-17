using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class FileManager : IFileManager
    {
        public IEnumerable<File> GetFiles(string path)
        {
            string[] arrayPath = Directory.GetFiles(path,"*.*",SearchOption.AllDirectories);

            List<File> list = new List<File>();
            foreach (var p in arrayPath)
            {
                list.Add(new File(p));
            }

            foreach (var p in list)
            {
                Console.WriteLine(p.Path);
            }

            return list;
        }
    }
}
