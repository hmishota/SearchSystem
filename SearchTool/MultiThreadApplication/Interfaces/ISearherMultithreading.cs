using SearchTool.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SearchTool.Interfaces
{
    public interface ISearcherMethodDecorator
    {
        Task<List<SearchResult>> SearchAsync(string source, CancellationToken cancel);
        Task<List<SearchResult>> SearchInternalAsync(string source, CancellationToken cancel);
    }
}
