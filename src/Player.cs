class Player
{
    // auto property
    public Room CurrentRoom { get; set; }

    public int health { get; set; }
    public Inventory Inventory { get; private set; }
    
    // constructor
    public Player()
    {
       CurrentRoom = null;
       this.health = 100;
       Inventory = new Inventory(10);
    }

    public int Damage(int amount)
    {
        this.health -= amount;
        if (this.health < 0) this.health = 0;
        return this.health;
    }
    public int Heal(int amount)
    {
        this.health += amount;
        if (this.health > 100) this.health = 100;
        return this.health;

    } // player's health restores
    public bool IsAlive()
    {
        return this.health > 0;
    } // checks whether the player is alive or not

	public string GetStatus()
    {
        return $"You have {this.health} health.\nInventory weight: {Inventory.TotalWeight()}/{Inventory.TotalWeight}";
    }
    
}