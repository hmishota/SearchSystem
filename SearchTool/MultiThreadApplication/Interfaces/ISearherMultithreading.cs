using SearchTool.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchTool.Interfaces
{
    public interface ISearcherMethodDecorator
    {
        Task<List<SearchResult>> SearchAsync(string source);
    }
}
