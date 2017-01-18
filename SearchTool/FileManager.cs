using SearchTool.Interfaces;
using SearchTool.Models;
using System;
using System.Collections.Generic;
using IO = System.IO;

namespace SearchTool
{
    public class FileManager : IFileManager
    {
        public IEnumerable<File> GetFiles(string path, bool nesting)
        {
            string[] arrayPath;

            if (nesting == true)
                arrayPath = IO.Directory.GetFiles(path, "*.*", IO.SearchOption.AllDirectories);
            else
                arrayPath = IO.Directory.GetFiles(path, "*.*", IO.SearchOption.TopDirectoryOnly);

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
