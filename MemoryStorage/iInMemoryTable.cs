using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryStorage
{
    internal interface iInMemoryTable
    {
        string Name { get; set; }

        bool Create(ref iEntry entry);
        iEntry? Read(int id);
        iEntry? Read(string id);
        bool Update(iEntry update);
        bool Update(iEntry update, iEntry filter);
        bool Delete(int id);
    }
}
