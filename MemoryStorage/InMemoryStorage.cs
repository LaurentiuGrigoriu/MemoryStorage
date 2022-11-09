using System.Collections.Concurrent;

namespace MemoryStorage
{
    internal class InMemoryStorage
    {
        public ConcurrentDictionary<string, IInMemoryTable> MemoryDB { get; set; }

        public InMemoryStorage()
        {
            MemoryDB = new ConcurrentDictionary<string, IInMemoryTable>();
        }

        public bool TryAdd(string name, IInMemoryTable table)
        {
            return MemoryDB.TryAdd(name, table);
        }

        public bool TryGet(string name, out IInMemoryTable table)
        {
            return MemoryDB.TryGetValue(name, out table);
        }
    }
}
