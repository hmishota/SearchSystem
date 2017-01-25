using Microsoft.Practices.Unity;
using SearchTool.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool
{
    public class SearcherStart
    {
        private IStartSearher _iStartSearher;


        public void SearcherStartInit(IStartSearher IStartSearher)
        {
            _iStartSearher = IStartSearher;
        }

        public Task Search(string path, bool nesting, string searchText)
        {
            return _iStartSearher.Search(path, nesting, searchText);
        }

        public void DeterminationMinValue()
        {
            _iStartSearher.DeterminationMinValue();
        }

        public void Initialize(IUnityContainer unityContainer)
        {
            _iStartSearher.Initialize(unityContainer);
        }

       /* public void UnityContainer()
        {
            _iStartSearher.UnityContainer();
        }*/

    }
}
