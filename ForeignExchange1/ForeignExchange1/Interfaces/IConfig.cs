using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using SQLite.Net.Interop;

namespace ForeignExchange1.Interfaces
{
    public interface IConfig
    {
        string DirectoryDB { get; }
        ISQLitePlatform Platfrom { get; }
        
    }
}
