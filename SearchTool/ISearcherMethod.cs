using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool
{
    public interface ISearcherMethod
    {
        List<SearchResult> Search(Data searchText, string source);
    }
}
