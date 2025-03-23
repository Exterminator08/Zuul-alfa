class Item
{
    public int Price { get; }
    public string Description { get; }

    public Item(int price, string description)
    {
        Price = price;
        Description = description;
    }
}