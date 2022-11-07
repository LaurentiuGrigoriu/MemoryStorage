using System.Collections.Concurrent;

namespace MemoryStorage
{
    internal class InMemoryTable<T> : iInMemoryTable
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

        private bool Validate(iEntry entry)
        {
            if (entry.GetType() != typeof(T))
            {
                return false;
            }

            return true;
        }

        /* return true if successful, false otherwise
         * Id field is automatically generated, it doesn't take the parameter Id value
         */
        public bool Create(ref iEntry entry)
        {
            if(!Validate(entry))
            {
                throw new ArgumentException($"Wrong argument in Create function for {typeof(T)} type.");
            }

            T e = (T)entry;
            
            e.Id = _nextId;
            if (Table.TryAdd(_nextId, e))
            {
                _nextId++;
                return true;
            }

            return false;
        }

        public iEntry? Read(int id)
        {
            if (Table.ContainsKey(id))
            {
                return Table[id];
            }

            return null;
        }

        public iEntry? Read(string id)
        {
            int readId = -1;
            
            if (int.TryParse(id, out readId) && Table.ContainsKey(readId))
            {
                return Table[readId];
            }

            return null;
        }

        // return list fulfilling the filtering condition
        public List<iEntry> Read(iEntry filter)
        {
            List<T> list = new List<T>();

            foreach (int id in Table.Keys)
            {
                if (Table[id].Match(filter))
                {
                    list.Add(Table[id]);
                }
            }

            return list.Cast<iEntry>().ToList();
        }

        // only updates the fields that are not null
        public bool Update(iEntry update)
        {
            if (!Validate(update))
            {
                throw new ArgumentException($"Wrong argument in Uodate function for {typeof(T)} type.");
            }

            T u = (T)update;
            
            if (Table.ContainsKey(u.Id))
            {
                return Table[u.Id].Update(update);
            }

            return false;
        }

        public bool Delete(int id)
        {
            if (Table.ContainsKey(id))
            {
                return Table.TryRemove(id, out _);
            }

            return true;
        }
    }
}
