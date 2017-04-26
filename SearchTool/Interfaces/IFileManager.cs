using SearchTool.Models;
using System.Collections.Generic;

namespace SearchTool.Interfaces
{
    public interface IFileManager
    {
        List<File> GetFiles(string path, bool nesting);
    }
    
}
