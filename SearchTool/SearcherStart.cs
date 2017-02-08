using Microsoft.Practices.Unity;
using SearchTool.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using SearchTool.Models;

namespace SearchTool
{
    public class SearcherStart
    {
        private IStartSearher _iStartSearher;

        public void SearcherStartInit(IStartSearher IStartSearher)
        {
            _iStartSearher = IStartSearher;
        }

        public List<SearchResult> Search(string path, bool nesting, string searchText)
        {
            return _iStartSearher.Search(path, nesting, searchText).Result;
        }

        public void DeterminationMinValue()
        {
            _iStartSearher.DeterminationMinValue();
        }

        public void Initialize(IUnityContainer unityContainer)
        {
            _iStartSearher.Initialize(unityContainer);
        }
    }
}
