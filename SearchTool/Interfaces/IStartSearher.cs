using Microsoft.Practices.Unity;
using SearchTool.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchTool.Interfaces
{
    public interface IStartSearher
    {
        Task<List<SearchResult>> Search(string path, bool nesting, string searchText);
        //void Initialize(IUnityContainer unityContainer);
        void DeterminationMinValue();
    }
}
