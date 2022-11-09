using System.Collections.Concurrent;

namespace MemoryStorage
{
    public class InMemoryTable<T> : IInMemoryTable
        where T : class, IEntry
    {
        private int _nextId = 1;

        public string Name { get; set; }

        public ConcurrentDictionary<int, T> Table { get; set; }

        public InMemoryTable(string name)
        {
            Name = name;
            Table = new ConcurrentDictionary<int, T>();
        }

        private bool Validate(IEntry entry)
        {
            if (entry.GetType() != typeof(T))
            {
                return false;
            }

            return true;
        }

        /* return true if successful, false otherwise
         * Id field is automatically generated, entry.Id value doesn't count
         */
        public bool Create(ref IEntry entry)
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

        // returns the actual element in the table, rather than a copy of it
        public IEntry? Read(int id)
        {
            if (Table.ContainsKey(id))
            {
                return Table[id];
            }

            return null;
        }

        public IEntry? Copy(int id)
        {
            return Table[id]?.Copy();
        }

        // returns the actual element in the table, rather than a copy of it
        public IEntry? Read(string id)
        {
            int readId = -1;
            
            if (int.TryParse(id, out readId) && Table.ContainsKey(readId))
            {
                return Table[readId];
            }

            return null;
        }

        // return list fulfilling the filtering condition
        public List<IEntry> Read(IEntry filter)
        {
            List<T> list = new List<T>();

            foreach (int id in Table.Keys)
            {
                if (Table[id].Match(filter))
                {
                    list.Add(Table[id]);
                }
            }

            return list.Cast<IEntry>().ToList();
        }

        // only updates the fields that are not null;
        // the Id must be valid (Id > 0)
        // return false If update.Id not found
        public bool Update(IEntry update)
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

        // idempotent: return true is Id entry was deleted, even if it was already deleted/unexistent
        public bool Delete(int id)
        {
            if (Table.ContainsKey(id))
            {
                return Table.TryRemove(id, out _);
            }

            return true;
        }

        public void Clear()
        {
            Table.Clear();
        }
    }
}
