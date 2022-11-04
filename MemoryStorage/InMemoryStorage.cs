using System.Collections.Concurrent;

namespace MemoryStorage
{
    internal class InMemoryStorage
    {
        public ConcurrentDictionary<string, iInMemoryTable> MemoryDB { get; set; }

        public InMemoryStorage()
        {
            MemoryDB = new ConcurrentDictionary<string, iInMemoryTable>();
        }

        public bool TryAdd(string name, iInMemoryTable table)
        {
            return MemoryDB.TryAdd(name, table);
        }

        public bool TryGet(string name, out iInMemoryTable table)
        {
            return MemoryDB.TryGetValue(name, out table);
        }
    }
}
