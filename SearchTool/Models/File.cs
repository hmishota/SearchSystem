namespace SearchTool.Models
{
    public class File
    {
        public string Path;
                        
        public File(string path)
        {
            this.Path = path;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            File file = obj as File;
            if ((object)file == null)
            {
                return false;
            }

            return (this.Path == file.Path);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
