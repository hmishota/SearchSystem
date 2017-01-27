using SearchTool.Models;
using System;
using System.Threading.Tasks;

namespace SearchTool.Interfaces
{
    public interface IReader : IDisposable
    {
        Task<Models.Data> ReadAsync();
        void InitVariables(int sizeBufferReader, int sizeBufferWritter, Models.File f);
    }
}
