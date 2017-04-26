using System;
using System.IO;
using System.Threading.Tasks;

namespace SearchTool.Interfaces
{
    public interface IReader : IDisposable
    {
        Task<Models.Data> ReadAsync();
        long InitVariables(Stream stream, int sizeBufferReader, int sizeBufferWritter);
    }
}
