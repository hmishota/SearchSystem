namespace SearchTool.Models
{
    public class SearchResult
    {
        public long Position;
        public File File;
        
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            SearchResult searchResult = obj as SearchResult;
            if ((object) searchResult == null)
            {
                return false;
            }

            return (this.Position == searchResult.Position) && this.File.Equals(searchResult.File);

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        
    }
}
