using SearchTool.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool
{
    public class FileManager : IFileManager
    {
        public IEnumerable<File> GetFiles(string path, bool nesting)
        {
            string[] arrayPath;

            if (nesting == true)
                arrayPath = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            else
                arrayPath = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);

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
