using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool
{
    public interface IFileManager
    {
        IEnumerable<File> GetFiles(string path, bool nesting);
    }
    
}
