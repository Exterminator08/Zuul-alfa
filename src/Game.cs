using System;
using System.Diagnostics;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;
	private Enemy enemy;

	private Stopwatch stopwatch;
	private Room chamber;
	// private Room currentRoom;

	// Constructor
	public Game()
	{
		enemy = new Enemy();
		parser = new Parser();
		player = new Player();
		CreateRooms();
		stopwatch = new Stopwatch();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		// Create the rooms
		Room startRoom = new Room("you landed right on the roof of the building and it collapsed and in order to unlock the system in order to return home, you need to collect $10,000.");
		Room corridor = new Room("There is a narrow corridor in front of you and a huge eye above, try not to look at it.");
		Room toilet = new Room("in a utility room. There are some tools laying around.");
		Room kitchen = new Room("Ugh! Mouse tails in a frying pan, it looks like Reuben's sewer. Don't you dare take the tail.");
		Room utilityRoom = new Room("in a storage room. There are some boxes and barrels laying around.");
		//!-------------------------------------- нижнюю часть надо сделать до стартовой комнаты
		Room stareWell1 = new Room("walking down the stairs but you are blocked by trash.");
		Room stareWell2 = new Room("walking up the stairs but you are blocked by trash.");
		Room overFlowChamber = new Room("in the overflow chamber. The water is rising and you are drowning.(You're taking 5 damage per second)");
		chamber = overFlowChamber;
		enemy.CurrentRoom = corridor;

		// Initialise room exits
		startRoom.AddExit("east", corridor);
		startRoom.AddExit("south", kitchen);
		startRoom.AddExit("west", toilet);
		//!-------------------------------------- нужно добавить и поправитт проходы этих комнат и переименовать комнаты и коменты к ним, затем изменить предметы на их ценности и что бы перс мог собраь юольше 10к тк у него только минимальный лимит 10к
		startRoom.AddExit("down", stareWell1);


		corridor.AddExit("west", startRoom);
		corridor.AddExit("east", overFlowChamber);
		overFlowChamber.AddExit("west", corridor);


		toilet.AddExit("east", startRoom);

		kitchen.AddExit("north", startRoom);
		kitchen.AddExit("east", utilityRoom);

		utilityRoom.AddExit("west", kitchen);
		utilityRoom.AddExit("up", stareWell2);


		stareWell1.AddExit("up", startRoom);
		stareWell2.AddExit("down", utilityRoom);


		// Create your Items here
		// ...
		// And add them to the Rooms
		// ...

		// startRoom game startRoom
		player.CurrentRoom = startRoom;
		Item axe = new Item(15, "You picked up an axe.");
		Item pistol = new Item(5, "You picked up a pistol.");
		Item sword = new Item(10, "You picked up a sword. ");


		kitchen.Chest.Put("axe", axe);
		utilityRoom.Chest.Put("pistol", pistol);
		toilet.Chest.Put("sword", sword);
	}

	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		bool finished = false;
		// execute them until the player wants to quit.
		while (!finished)
		{

			stopwatch.Start();

			Command command = parser.GetCommand();
			OverFlowChamber(command);


			finished = ProcessCommand(command);
			if (!player.IsAlive())
			//! if player is NOT alive (!) then finished is true
			{
				finished = true;
				Console.WriteLine("You died, noob!");
			}
			stopwatch.Reset();
		}
		Console.WriteLine("Thank you for playing.");
		Console.WriteLine("Press [Enter] to continue.");
		Console.ReadLine();
	}

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to 'R.E.P.O.' ");
		Console.WriteLine("You worked in a spaceship and were sent to earth to earn money.");
		Console.WriteLine("You flew alone without friends. The report says that this building is uninhabited. However, you hear very scary screams and sounds.");
		Console.WriteLine("Collect as many items with high monetary value as possible and return back to space!");
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
			case "use":
				PrintUse(command);
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
		Console.WriteLine("You wander around in the sewers besides the stinky water.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}

	private void PrintStatus()
	{
		Console.WriteLine("Your Health is: " + player.Health);
		Console.WriteLine("Your suitcase contains: " + player.Backpack.ShowInventory());
		Console.WriteLine("You are carrying: " + player.Backpack.TotalWeight() + "kg. You have " + player.Backpack.FreeWeight() + "kg free space.");
	}

	private void PrintUse(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Use what?");
			return;
		}
		string itemName = command.SecondWord;
		// Item item = player.backpack.Get(itemName);


		if (!player.Use(itemName, enemy))
		{
			Console.WriteLine($"You don't have a {itemName} to use.");
		}
	}

	private void PrintLook()
	{
		Console.WriteLine("Items in the room: " + player.CurrentRoom.Chest.ShowInventory());
		if (enemy != null && enemy.CurrentRoom == player.CurrentRoom)
 		{
 			Console.WriteLine($"There is an {enemy} standing in front of you. It has {enemy.Health} health! Use your weapon to kill it");
 		}
 		else
 		{
 			Console.WriteLine("Enemies in the room: None");
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

		player.Damage(5);

		player.CurrentRoom = nextRoom;
		Console.WriteLine(player.CurrentRoom.GetLongDescription());

	}

	//methods
	private void Take(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Take what?");
			return;
		}

		string itemName = command.SecondWord;

		Item item = player.CurrentRoom.Chest.Get(itemName);

		if (item == null)
		{
			Console.WriteLine("There is no " + itemName + " in this room.");
			return;
		}

		switch (itemName)
		{
			case "sword":
				Console.WriteLine("You picked up a sword.");
				break;
			case "pistol":
				Console.WriteLine("You picked up a pistol.");
				break;
			case "axe":
				Console.WriteLine("You picked up the " + itemName + ".");
				break;
		}

		player.Backpack.Put(itemName, item);

	}

	private void Drop(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Drop what?");
			return;
		}

		string itemName = command.SecondWord;

		Item item = player.Backpack.Get(itemName);
		if (item != null)
		{
			player.CurrentRoom.Chest.Put(itemName, item);
			Console.WriteLine($"You dropped the {itemName}.");
		}
		else
		{
			Console.WriteLine($"You don't have a {itemName} to drop.");
		}
	}

	private async void OverFlowChamber(Command command)
{
    if (player.CurrentRoom == chamber) 
    {
        stopwatch.Stop();
        int s = stopwatch.Elapsed.Seconds;

        Console.WriteLine("You're struggling in the flooded chamber!");

        for (int i = 0; i < s; i++)
        {
            player.Damage(5);
            Console.WriteLine("-5hp");
            await Task.Delay(1000); // Задержка в 1 секунду
        }

        if (!player.IsAlive())
        {
            Console.WriteLine("You drowned in the overflow chamber!");
        }
    }
}
}