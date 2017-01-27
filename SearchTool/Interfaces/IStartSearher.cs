using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool.Interfaces
{
    public interface IStartSearher
    {
        Task Search(string path, bool nesting, string searchText);
        void Initialize(IUnityContainer unityContainer);
        void DeterminationMinValue();
        
    }
}
