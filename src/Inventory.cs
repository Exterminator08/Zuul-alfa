class Inventory
{
    private int maxWeight;
    private Dictionary<string, Item> items;

    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        this.items = new Dictionary<string, Item>();
    }

    public bool Put(string itemName, Item item)
    {
        if (item.Weight + TotalWeight() <= maxWeight)
        {
            items[itemName] = item; // Add or replace item
            return true;
        }
        // TODO implement:
        // Check the Weight of the Item and check
        //  for enough space in the Inventory
        // Does the Item fit?// Put Item in the items Dictionary
        // Return true/false for success/failure
        return false;
    }

    public Item Get(string itemName)
    {
        if (items.TryGetValue(itemName, out Item item))
        {
            items.Remove(itemName); // Remove item if found
            return item;
        }
        // TODO implement:
        // Find Item in items Dictionary
        // remove Item from items Dictionary if found
        // return Item or null
        return null;
    }

    public int TotalWeight()
    {
        int total = 0;
        foreach (var item in items.Values)
        {
            total += item.Weight;
        }
        // TODO implement:
        // loop through the items, and add all the weights
        return total;
    }

    public int FreeWeight()
    {
        // TODO implement:
        // compare MaxWeight and TotalWeight()
        return maxWeight - TotalWeight();
    }
}