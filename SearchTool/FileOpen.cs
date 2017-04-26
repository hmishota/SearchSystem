using System.IO;

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
