using System.Collections.Generic;

class Room
{
	// Private fields

	public string Description { get; private set; }
	private string description;
	public Enemy Enemy { get; private set; }
	private Dictionary<string, Room> exits;
	private Inventory chest;

	public Room(string desc)
	{
		description = desc;
		exits = new Dictionary<string, Room>();
		chest = new Inventory(10000000);

	}

	public void AddExit(string direction, Room neighbor)
	{
		exits.Add(direction, neighbor);
	}

	public string GetShortDescription()
	{
		return description;
	}

	public string GetLongDescription()
	{
		string str = "You are ";
		str += description;
		str += ".\n";
		str += GetExitString();
		return str;
	}

	public Room GetExit(string direction)
	{
		if (exits.ContainsKey(direction))
		{
			return exits[direction];
		}
		return null;
	}

	private string GetExitString()
	{
		string str = "Exits: ";
		str += String.Join(", ", exits.Keys);

		return str;
	}

	public Inventory Chest
	{
		get { return chest; }
	}

	public void RemoveEnemy()
	{
		Enemy = null; // Remove the enemy by setting the Enemy property to null
	}
}