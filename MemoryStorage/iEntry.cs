using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryStorage
{
    internal interface iEntry
    {
        int Id { get; set; }

        bool UpdatePartial(iEntry entry);

        bool Match(iEntry filter);
    }
}
