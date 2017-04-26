using System.Collections.Generic;

namespace SearchTool.Models
{
    public class ComparerSearchResult : IComparer<SearchResult>
    {
        public int Compare(SearchResult x, SearchResult y)
        {
            return x.Position.CompareTo(y.Position);
        }
    }
}
