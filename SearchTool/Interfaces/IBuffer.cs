﻿using SearchTool.Models;

namespace SearchTool.Interfaces
{
    interface IBuffer
    {
        void Add(Data data);
        Data Get();
    }
}
