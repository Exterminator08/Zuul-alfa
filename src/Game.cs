using System.Diagnostics;
using System.Runtime.CompilerServices;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;
	private Enemy enemy;

	private Stopwatch stopwatch;

	private Room startRoom;

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
		startRoom = new Room("You landed right on the roof of the building and it collapsed and in order to unlock the system in order to return home, you need to collect $10,000. If you have collected all the money, you can use it here");
		Room corridor = new Room("There is a narrow corridor in front of you and a huge eye above, try not to look at it.");
		Room toilet = new Room("In a utility room. There are some tools laying around.");
		Room kitchen = new Room("Ugh! Mouse tails in a frying pan, it looks like Reuben's sewer. Don't you dare take the tail.");
		Room utilityRoom = new Room("In a storage room. There are some boxes and barrels laying around.");
		Room tower = new Room("An infinitely high tower and stairs, but I see something, maybe You can climb the stairs?");
		Room stairs = new Room("So tired. Just a little bit more.");
		Room stairs1 = new Room("So tired. Just a little bit more.");
		Room stairs2 = new Room("So tired.. Just a little bit more.");
		Room stairs3 = new Room("So tired... Just a little bit more.");
		Room stairs4 = new Room("So tired.... Just a little bit more.");
		Room stairs5 = new Room("So tired..... Just a little bit more.");
		Room topOfTheTower = new Room("Finally you reached the top. WAIT... WHAT??? Look at this! God and teleport?!");
		Room sauna = new Room("The human skeleton here is disgusting. Wait, what is he holding in his hands?");

		// enemy.CurrentRoom = corridor;

		startRoom.AddExit("east", corridor);
		startRoom.AddExit("south", kitchen);
		startRoom.AddExit("west", toilet);
		startRoom.AddExit("north", tower);

		corridor.AddExit("west", startRoom);
		corridor.AddExit("east", sauna);
		sauna.AddExit("west", corridor);


		toilet.AddExit("east", startRoom);

		kitchen.AddExit("north", startRoom);
		kitchen.AddExit("east", utilityRoom);

		utilityRoom.AddExit("west", kitchen);


		tower.AddExit("south", startRoom);
		tower.AddExit("up", stairs1);
		stairs1.AddExit("up", stairs2);
		stairs2.AddExit("up", stairs3);
		stairs3.AddExit("up", stairs4);
		stairs4.AddExit("up", stairs5);
		stairs5.AddExit("up", stairs);
		stairs.AddExit("up", topOfTheTower);
		topOfTheTower.AddExit("teleport", tower);

		topOfTheTower.AddExit("down", stairs);
		stairs.AddExit("down", stairs5);
		stairs5.AddExit("down", stairs4);
		stairs4.AddExit("down", stairs3);
		stairs3.AddExit("down", stairs2);
		stairs2.AddExit("down", stairs1);
		stairs1.AddExit("down", tower);
		
		player.CurrentRoom = startRoom;
		Item axe = new Item(500, "You picked up an axe.");
		Item pistol = new Item(1200, "You picked up a pistol.");
		Item minigun = new Item(2400, "You picked up a minigun.");
		Item excalibur = new Item(4800, "You picked up an excalibur. ");
		Item bible = new Item(9999, "You picked up a Bible. aura +999999999999 ");
		Item koran = new Item(9999, "You picked up a Koran. aura +999999999999");
		Item book = new Item(100, "You picked up a book. May be God can help you? Check the top of the tower.");

		kitchen.Chest.Put("axe", axe);
		utilityRoom.Chest.Put("pistol", pistol);
		toilet.Chest.Put("excalibur", excalibur);
		topOfTheTower.Chest.Put("bible", bible);
		topOfTheTower.Chest.Put("koran", koran);
		corridor.Chest.Put("minigun", minigun);
		sauna.Chest.Put("book", book);
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

	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;

		if (command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...");
			return wantToQuit;
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

	private void PrintHelp()
	{
		Console.WriteLine("You are lost. You are alone.");
		Console.WriteLine("You are alone here, no one will help you...");
		Console.WriteLine("But here are the commands you can use.");
		Console.WriteLine();
		parser.PrintValidCommands();
	}

	private void PrintStatus()
{
    Console.WriteLine("Your Health is: " + player.Health);
    
    Console.WriteLine("Your suitcase contains: " + player.Backpack.ShowInventory());

    Console.WriteLine("You have collected: $" + player.Backpack.GetCurrentAmount());

    // Достиг ли игрок цели по сумме
    if (player.Backpack.IsTargetReached())
    {
        Console.WriteLine("You have already reached your target of $" + player.Backpack.TargetAmount + " or more.");
    }
    else
    {
        // Сколько еще нужно собрать
        Console.WriteLine("You still need $" + (player.Backpack.TargetAmount - player.Backpack.GetCurrentAmount()) + " to reach your target.");
    }
}


	private void PrintUse(Command command)
{
    if (!command.HasSecondWord())
    {
        Console.WriteLine("Use what?");
        return;
    }

    string itemName = command.SecondWord;

    if (itemName == "money")
    {
        if (player.CurrentRoom == startRoom && player.Backpack.IsTargetReached())  
        {
            Console.WriteLine("Congratulations! You successfully delivered the money and completed the mission!");
            Console.WriteLine("Press [Enter] to finish the game.");
            Console.ReadLine();  // Ожидание перед завершением
            Environment.Exit(0);  // Завершаем программу
        }
        else
        {
            Console.WriteLine("You need to be in the start room to use the money.");
        }
    }
    else
    {
        if (!player.Use(itemName, enemy))
        {
            Console.WriteLine($"You don't have a {itemName} to use.");
        }
    }
}


	private void PrintLook()
	{
		Console.WriteLine("Items in the room: " + player.CurrentRoom.Chest.ShowInventory());

		//? in this game we don't need to see the enemy, but i have a code for it if you need it
		// if (enemy != null && enemy.CurrentRoom == player.CurrentRoom)
 		// {
 		// 	Console.WriteLine($"There is an {enemy} standing in front of you. It has {enemy.Health} health! Use your weapon to kill it");
 		// }
 		// else
 		// {
 		// 	Console.WriteLine("Enemies in the room: None");
 		// }
	}

	private void GoRoom(Command command)
	{


		if (!command.HasSecondWord())
		{
			Console.WriteLine("Go where?");
			return;
		}

		string direction = command.SecondWord;

		Room nextRoom = player.CurrentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to " + direction + "!");
			return;
		}

		player.Damage(5); // niet nodig voor de game
		player.CurrentRoom = nextRoom;
		Console.WriteLine(player.CurrentRoom.GetLongDescription());

	}

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

    // Проверка на достижение цели
    if (!player.Backpack.CollectItem(itemName, item))
    {
        Console.WriteLine("You can't collect this item as you've already reached your target amount.");
        return;
    }

    Console.WriteLine($"You picked up {itemName}. Your current total is ${player.Backpack.GetCurrentAmount()}.");
    if (player.Backpack.GetCurrentAmount() >= player.Backpack.TargetAmount)
    {
        Console.WriteLine("You did it!");
    }
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

	
public void Play()
{
    PrintWelcome();

    bool finished = false;
    bool targetReached = false;  // Достиг ли игрок нужной суммы

    while (!finished)
    {
        stopwatch.Start();

        Command command = parser.GetCommand();

        finished = ProcessCommand(command);

        if (!player.IsAlive())
        {
            finished = true;
            Console.WriteLine("You died, noob!");
        }

        // Если игрок собрал нужную сумму, уведомляем его, но не завершаем игру
        if (player.Backpack.IsTargetReached() && !targetReached)
        {
            Console.WriteLine("You have collected enough money! Return to the start room and use 'use money' to finish the game.");
            targetReached = true;
        }

        stopwatch.Reset();
    }

    Console.WriteLine("Thank you for playing.");
    Console.WriteLine("Press [Enter] to exit.");
    Console.ReadLine();
}

}