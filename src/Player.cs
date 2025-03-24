class Player
{
	public int Health { get; private set; }
	public Room CurrentRoom { get; set; }
	public Inventory Backpack { get; private set; }

	public Player()
	{
		CurrentRoom = null;
		Health = 100;
		Backpack = new Inventory(10000); // сколько денег нужно собрать
	}

	public bool Use(string itemName, Enemy enemy)
	{
		Item item = Backpack.Get(itemName);

		if (item == null)
		{
            Console.WriteLine($"You don't have a {itemName} to use.");
			return false;
		}

		switch (itemName)
		{
			case "sword":
				Console.WriteLine("You used the sword.");
				this.Damage(10);
				Backpack.Remove(itemName);
				break;
			case "pistol":
				Console.WriteLine("You used the pistol.");
				this.Damage(15);
				break;
			case "axe":
			if (enemy != null && enemy.CurrentRoom == this.CurrentRoom)
 				{
 					Console.WriteLine("You used the axe on the enemy!");
 					enemy.Damage(5); // Apply 5 damage to the enemy
 					if (!enemy.IsAlive())
 					{
 						Console.WriteLine("You defeated the enemy!");
 					}
 				}
 				else
 				{
 					Console.WriteLine("There is no enemy here to use the axe on.");
 				}	
				break;
				// default:
				// return item.Use(); // Call the Use method on the Item instance


				
		default:
 				Console.WriteLine($"You can't use the {itemName}.");
 				break;
		}

		return true;
	}

	public int Damage(int amount)
	{
		this.Health -= amount;
		if (this.Health < 0)
		{
			this.Health = 0;
		}
		return this.Health;
	} // player loses some Health

	public int Heal(int amount)
	{

		this.Health += amount;
		if (this.Health > 100)
		{
			this.Health = 100;
		}
		return this.Health;


	} // player gains some Health

	public bool IsAlive()
	{
		if (this.Health == 0)
		{
			// Console.WriteLine("You died, noob! Write 'quit' to exit the game");	
			return false;
		}
		return true;
	} // returns true if player is alive
	
}
