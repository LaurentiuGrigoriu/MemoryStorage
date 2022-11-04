internal interface iRow
{
    int Id { get; set; }

    bool Update(iRow entry);
}

internal class Row : iRow
{
    private int _id;

    public int Data { get; set; }
    public string Name { get; set; }

    public int Id
    {
        get => _id;
        set { _id = value; }
    }

    public bool Update(iRow entry)
    {
        if (entry.GetType() == typeof(Row))
        {

        }

        throw new NotImplementedException();
    }
}
