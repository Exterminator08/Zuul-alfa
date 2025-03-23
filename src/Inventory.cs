class Inventory
{
    public int TargetAmount { get; } 
    private int currentAmount; 
    private Dictionary<string, Item> collectedItems;

    public Inventory(int targetAmount)
    {
        this.TargetAmount = targetAmount;
        this.currentAmount = 0;
        this.collectedItems = new Dictionary<string, Item>();
    }

    // Метод для добавления предмета
    public bool CollectItem(string itemName, Item item)
    {
        if (currentAmount >= TargetAmount)
        {
            return false;
        }
        
        collectedItems[itemName] = item;
        currentAmount += item.Price;  // Добавляем цену предмета
        return true;
    }

    public bool Put(string itemName, Item item)
    {
        collectedItems[itemName] = item;
        currentAmount += item.Price;
        return true;
    }

    public Item Get(string itemName)
    {
        if (collectedItems.TryGetValue(itemName, out Item item))
        {
            collectedItems.Remove(itemName);
            currentAmount -= item.Price;
            return item;
        }
        return null;
    }

    public void Remove(string itemName)
    {
        if (collectedItems.TryGetValue(itemName, out Item item))
        {
            collectedItems.Remove(itemName);
            currentAmount -= item.Price;
        }
    }

    public string ShowInventory()
    {
        if (collectedItems.Count == 0)
        {
            return "No items collected";
        }
        return string.Join(", ", collectedItems.Keys);
    }

    public int GetCurrentAmount()
    {
        return currentAmount;
    }

    // Метод, который проверяет, достиг ли игрок цели
    public bool IsTargetReached()
    {
        return currentAmount >= TargetAmount;
    }
}
