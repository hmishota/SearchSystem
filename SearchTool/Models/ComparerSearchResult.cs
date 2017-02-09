using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
