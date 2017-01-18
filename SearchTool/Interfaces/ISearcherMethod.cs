using System.Collections.Generic;

namespace SearchTool.Interfaces
{
    public interface ISearcherMethod
    {
        List<SearchResult> Search(Data searchText, string source);
    }
}
