using SearchTool.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SearchTool.Interfaces
{
    public interface ISearcherMethodDecorator
    {
        Task<List<SearchResult>> Search(string source, CancellationToken cancel);
    }
}
