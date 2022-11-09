using MemoryStorage;

// =============== CREATE TABLE ===============
IInMemoryTable table = new InMemoryTable<LCCommand>("LCCommandQueueItems");

// =============== CREATE MEMORY STORAGE; ADD TABLE TO MEMORY STORAGE ===============
InMemoryStorage memoryStorage = new InMemoryStorage();
memoryStorage.TryAdd("LCCommandQueueItems", table);


// =============== FILL TABLE ===============
for (int i = 0; i < 20; i++)
{
    IEntry entry = new LCCommand()
    {
        Command = $"cmd {i}",
        Status = (CommandStatus)(i%5),
        Observation = $"LCCommand observation {i}",
        ScannerName = "SCNR26739856",
        CreatedOn = DateTime.Now
    };

    if (table.Create(ref entry))
    {
        int id = entry.Id;
        Console.WriteLine($"Created LCCommand Id={id}");
    }
}


// =============== READ TABLE ===============
IInMemoryTable? iTbl;
if (memoryStorage.TryGet("LCCommandQueueItems", out iTbl) && iTbl != null)
{
    InMemoryTable<LCCommand>? tbl = (InMemoryTable<LCCommand>)iTbl;

    Console.WriteLine($"Read {tbl.Name} Table:");
    // when we don't know the IDs
    foreach (LCCommand cmd in tbl.Table.Values)
    {
        Console.WriteLine($"Entry:");
        cmd.Print();
    }

#if null
    // Read version #2: when we know the IDs
    for (int id = 1; id <= tbl.Table.Keys.Count; id++)
    {
        LCCommand? cmd = (LCCommand?)tbl.Read(id);
        cmd?.Print();
    }
    // Read version #3: when we know the IDs
    for (int id = 1; id <= tbl.Table.Keys.Count; id++)
    {
        LCCommand? cmd = (LCCommand?)tbl.Read(id.ToString());
        cmd?.Print();
    }
#endif

    // =============== READ TABLE with FILTER ===============
    Console.WriteLine("\n\nLCCommandQueueItems with Status = CommandStatus.Open:"); 
    
    LCCommand filter = new LCCommand() { Status = CommandStatus.Open };
    List<LCCommand> list = tbl.Read(filter).Cast<LCCommand>().ToList();

    foreach (LCCommand cmd in list)
    {
        Console.WriteLine($"Entry:");
        cmd.Print();
    }

    // =============== UPDATE ENTRY ===============
    // update each entry in the CommandStatus.Open list above and change it to CommandStatus.Running
    Console.WriteLine("\n\nUpdate list from Status = CommandStatus.Open to Status = CommandStatus.Running");
    foreach (LCCommand cmd in list)
    {
        LCCommand update = new LCCommand() { Id = cmd.Id, Status = CommandStatus.Running };

        cmd.Update(update);
    }

    // now filter & display again the CommandStatus.Open list
    Console.WriteLine("\n\nLCCommandQueueItems with Status = CommandStatus.Open:");
    list = tbl.Read(filter).Cast<LCCommand>().ToList();
    foreach (LCCommand cmd in list)
    {
        Console.WriteLine($"Entry:");
        cmd.Print();
    }

    // now filter & display again the CommandStatus.Running list
    Console.WriteLine("\n\nLCCommandQueueItems with Status = CommandStatus.Running:");
    filter = new LCCommand() { Status = CommandStatus.Running };
    list = tbl.Read(filter).Cast<LCCommand>().ToList();
    foreach (LCCommand cmd in list)
    {
        Console.WriteLine($"Entry:");
        cmd.Print();
    }

    // =============== DELETE ENTRY ===============
    Console.WriteLine("\n\nDelete entries with Status = CommandStatus.Running");
    foreach (LCCommand cmd in list)
    {
        tbl.Delete(cmd.Id);
    }

    // now display again table entries
    Console.WriteLine($"Read again {tbl.Name} Table:");
    foreach (LCCommand cmd in tbl.Table.Values)
    {
        Console.WriteLine($"Entry:");
        cmd.Print();
    }

}
else
{
    Console.WriteLine("LCCommandQueueItems table wasn't found in the DB.");
}






