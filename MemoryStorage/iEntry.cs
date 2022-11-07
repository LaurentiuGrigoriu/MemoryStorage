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

        /* Only update the entry fields that are not null */
        bool Update(iEntry entry, bool ignoreId = true);

        bool Match(iEntry filter);
    }
}
