using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool
{
    interface IReader : IDisposable
    {
        bool Read(out Data data);
    }
}
