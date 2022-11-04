using System.Collections.Concurrent;

namespace MemoryStorage
{
    internal class InMemoryTable<T>
        where T : class, iEntry
    {
        private int _nextId = 1;

        public string Name { get; set; }

        public ConcurrentDictionary<int, T> Table { get; set; }

        public InMemoryTable(string name)
        {
            Name = name;
            Table = new ConcurrentDictionary<int, T>();
        }

        /* return the positive id if successful
         * return -1 if unsuccessfull 
         */
        public int Create(T entry)
        {
            Table.TryAdd(_nextId, entry);

            return _nextId++;
        }

        public T? Read(int id)
        {
            if (Table.ContainsKey(id))
            {
                return Table[id];
            }

            return null;
        }

        public T? Read(string id)
        {
            int readId = -1;
            
            if (int.TryParse(id, out readId) && Table.ContainsKey(readId))
            {
                return Table[readId];
            }

            return null;
        }

        public bool Update(T update)
        {
            if (Table.ContainsKey(update.Id))
            {
                Table[update.Id] = update;
                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            if (Table.ContainsKey(id))
            {
                T? value;

                return Table.TryRemove(id, out value);
            }

            return true;
        }
    }
}
