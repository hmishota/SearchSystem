using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool
{
    interface IBuffer
    {
        void Add(string data);
        string Get();
    }
}
