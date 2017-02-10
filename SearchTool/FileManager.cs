using SearchTool.Interfaces;
using SearchTool.Models;
using System;
using System.Collections.Generic;
using IO = System.IO;

namespace SearchTool
{
    public class FileManager : IFileManager
    {
        public List<File> GetFiles(string path, bool nesting)
        {
            var arrayPath = IO.Directory.GetFiles(path, "*.*",
                nesting == true ? IO.SearchOption.AllDirectories : IO.SearchOption.TopDirectoryOnly);

            List<File> list = new List<File>();
            foreach (var p in arrayPath)
            {
                list.Add(new File(p));
            }

            return list;
        }
    }
}
