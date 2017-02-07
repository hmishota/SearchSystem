using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool
{
    public interface IFileOpen
    {
        Stream Open(Models.File f);
    }

    public class FileOpen : IFileOpen
    {
        public Stream Open(Models.File f)
        {
            return new FileStream(f.Path, FileMode.Open);
        }
    }
}
