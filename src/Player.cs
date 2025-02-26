class Player
{
    // auto property
    public Room CurrentRoom { get; set; }

    public int health { get; set; }
    
    // constructor
    public Player()
    {
       CurrentRoom = null;
       health = 100;
    }

    public int Damage(int amount)
    {
        return this.health -= amount;
    } // player loses some health
    public int Heal(int amount)
    {
        return this.health += amount;

    } // player's health restores
    public bool IsAlive()
    {
        return this.health > 0;
    } // checks whether the player is alive or not

	public string GetStatus()
    {
        string str = "You have ";
		str += this.health;
		str += ".\n";
		return str;
    }
    
}