using System;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;
	// private Room currentRoom;

	// Constructor
	public Game()
	{
		parser = new Parser();
		player = new Player();
		CreateRooms();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		// Create the rooms
		Room outside = new Room("outside the main entrance of the university");
		Room theatre = new Room("in a lecture theatre");
		Room pub = new Room("in the campus pub");
		Room lab = new Room("in a computing lab");
		Room office = new Room("in the computing admin office");
		Room basement = new Room("in the basement");
		Room attic = new Room("in the attic");


		// Initialise room exits
		outside.AddExit("east", theatre);
		outside.AddExit("south", lab);
		outside.AddExit("west", pub);
		outside.AddExit("down", basement);
		outside.AddExit("up", attic);

		theatre.AddExit("west", outside);

		pub.AddExit("east", outside);

		lab.AddExit("north", outside);
		lab.AddExit("east", office);

		office.AddExit("west", lab);

		attic.AddExit("down", outside);
		basement.AddExit("up", outside);

		// Create your Items here
		// ...
		// And add them to the Rooms
		// ...

		// Start game outside
		player.CurrentRoom = outside;
		Item mousetail = new Item(3, "Why would you even want to pick up a mousetail?");


		// outside.AddItem(mousetail);
		outside.Chest.Put("mousetail", mousetail);

	}

	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		// execute them until the player wants to quit.
		bool finished = false;
		while (!finished)
		{
			Command command = parser.GetCommand();
			finished = ProcessCommand(command);
		}
		Console.WriteLine("Thank you for playing.");
		Console.WriteLine("Press [Enter] to continue.");
		Console.ReadLine();
	}

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to Zuul!");
		Console.WriteLine("Zuul is a new, incredibly boring adventure game.");
		Console.WriteLine("Type 'help' if you need help.");
		Console.WriteLine();
		Console.WriteLine(player.CurrentRoom.GetLongDescription());

	}

	// Given a command, process (that is: execute) the command.
	// If this command ends the game, it returns true.
	// Otherwise false is returned.
	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;

		if (command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...");
			return wantToQuit; // false
		}

		switch (command.CommandWord)
		{
			case "help":
				PrintHelp();
				break;
			case "go":
				GoRoom(command);
				break;
			case "quit":
				wantToQuit = true;
				break;
			case "look":
				PrintLook();
				break;
			case "status":
				PrintStatus();
				break;
			case "take":
				Take(command);
				break;
			case "drop":
				Drop(command);
				break;

		}

		return wantToQuit;
	}

	// ######################################
	// implementations of user commands:
	// ######################################

	// Print out some help information.
	// Here we print the mission and a list of the command words.
	private void PrintHelp()
	{
		Console.WriteLine("You are lost. You are alone.");
		Console.WriteLine("You wander around at the university.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}

	private void Take(Command command)
{
    if (!command.HasSecondWord())
    {
        Console.WriteLine("Take what?");
        return;
    }

    string itemName = command.SecondWord;

    Item item = player.CurrentRoom.items.Find(i => i.Name.ToLower() == itemName.ToLower());

    if (item != null)
    {
        if (player.backpack.Put(itemName, item))
        {
            player.CurrentRoom.items.Remove(item);
            Console.WriteLine($"You picked up the {itemName}.");
        }
        else
        {
            Console.WriteLine("Your inventory is too full to take this item!");
        }
    }
    else
    {
        Console.WriteLine($"There is no {itemName} here.");
    }
}

	private void PrintStatus()
	{
		Console.WriteLine("Your health is: " + player.health);
		Console.WriteLine("Your backpack contains: " + player.backpack.ShowInventory());
	}

	private void PrintLook()
	{
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
		Console.WriteLine("Items in the room: " + player.CurrentRoom.Chest.ShowInventory());
	}

	private void Drop(Command command)
{
    if (!command.HasSecondWord())
    {
        Console.WriteLine("Drop what?");
        return;
    }

    string itemName = command.SecondWord;

    Item item = player.backpack.Get(itemName);
    if (item != null)
    {
        player.CurrentRoom.AddItem(item);
        Console.WriteLine($"You dropped the {itemName}.");
    }
    else
    {
        Console.WriteLine($"You don't have a {itemName} to drop.");
    }
}

	// Try to go to one direction. If there is an exit, enter the new
	// room, otherwise print an error message.
	private void GoRoom(Command command)
	{
		if (!command.HasSecondWord())
		{


			// if there is no second word, we don't know where to go...
			Console.WriteLine("Go where?");
			return;
		}

		string direction = command.SecondWord;

		// Try to go to the next room.
		Room nextRoom = player.CurrentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to " + direction + "!");
			return;
		}

		player.Damage(10);



		player.CurrentRoom = nextRoom;
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}
}

