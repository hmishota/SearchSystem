using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    interface IBuffer
    {
        void Add(string data);
        string Get();
    }
}
