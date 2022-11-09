namespace MemoryStorage
{
    internal interface IInMemoryTable
    {
        string Name { get; set; }

        bool Create(ref IEntry entry);

        IEntry? Read(int id);
        IEntry? Read(string id);
        IEntry? Copy(int id);


        public List<IEntry> Read(IEntry filter);

        // only updates the fields that are not null
        bool Update(IEntry update);

        bool Delete(int id);
    }
}
