using System;

namespace SearchTool.Interfaces
{
    interface IReader : IDisposable
    {
        bool Read(out Data data);
    }
}
