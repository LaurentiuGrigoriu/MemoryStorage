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
        public List<iEntry> Read(iEntry filter);

        // only updates the fields that are not null
        bool Update(iEntry update);

        bool Delete(int id);
    }
}
