using System.Collections.Generic;

namespace SearchTool.Interfaces
{
    public interface IFileManager
    {
        IEnumerable<File> GetFiles(string path, bool nesting);
    }
    
}
