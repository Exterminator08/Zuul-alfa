class Inventory
{
    //fields
    private int maxWeight;
    private Dictionary<string, Item> items;

    public int TotalWeight()
    {
        int total = 0;
        foreach (var item in items.Values)
        {
            total += item.Weight;
        }
        return total;
    }

    public int FreeWeight()
    {
        return maxWeight - TotalWeight();
    }
    //constructor
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        this.items = new Dictionary<string, Item>();
    }

    // methods
    public bool Put(string itemName, Item item)
    {
        if (item.Weight + TotalWeight() <= maxWeight)
        {
            items[itemName] = item; // Add or replace item
            return true;
        }
        return false;
    }

    public Item Get(string itemName)
    {
        if (items.TryGetValue(itemName, out Item item))
        {
            items.Remove(itemName); // Remove item if found
            return item;
        }
        return null;
    }

    //method
    public void Remove(string itemName)
    {
        items.Remove(itemName);
    }

    public string ShowInventory()
    {
        if (items.Count == 0)
        {
            return "";
        }
        return "" + string.Join(", ", items.Keys);
    }
}