using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryStorage
{
    public interface IEntry
    {
        int Id { get; set; }

        /* Only update the entry fields that are not null */
        bool Update(IEntry entry, bool ignoreId = false);

        bool Match(in IEntry filter);

        IEntry Copy();
    }
}
