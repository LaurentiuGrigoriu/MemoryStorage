using MemoryStorage;

// =============== CREATE TABLE ===============
iInMemoryTable table = new InMemoryTable<LCCommand>("LCCommandQueueItems");

// =============== CREATE MEMORY STORAGE; ADD TABLE TO MEMORY STORAGE ===============
InMemoryStorage memoryStorage = new InMemoryStorage();
memoryStorage.TryAdd("LCCommandQueueItems", table);


// =============== FILL TABLE ===============
for (int i = 0; i < 20; i++)
{
    iEntry entry = new LCCommand()
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
iInMemoryTable? iTbl;
if (memoryStorage.TryGet("LCCommandQueueItems", out iTbl))
{
    InMemoryTable<LCCommand> tbl = (InMemoryTable<LCCommand>)iTbl;

    Console.WriteLine($"Read {tbl.Name} Table:");
    foreach (LCCommand cmd in tbl.Table.Values)
    {
        Console.WriteLine($"Entry:");
        cmd.Print();
    }

    // =============== READ TABLE with FILTER ===============
    Console.WriteLine("\n\nLCCommandQueueItems with Status = CommandStatus.Open."); 
    
    LCCommand filter = new LCCommand() { Status = CommandStatus.Open };
    List<LCCommand> list = tbl.Read(filter).Cast<LCCommand>().ToList();

    foreach (LCCommand cmd in list)
    {
        Console.WriteLine($"Entry:");
        cmd.Print();
    }

}
else
{
    Console.WriteLine("LCCommandQueueItems table wasn't found in the DB.");
}



