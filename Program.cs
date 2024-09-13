//Snake - hoarseProgramming

//Todo
//Fancier start screen
//Save highscore in file inbetween sessions   

int highscore = 0;
while (true)
{
    ConsoleKeyInfo cki;
    Console.WriteLine("Play Snake!");
    Console.WriteLine("Use ARROWS to steer");
    Console.WriteLine("Press SPACE to start");
    Console.WriteLine("Press ESC to quit");
    Console.WriteLine($"Highscore {highscore}");
    cki = Console.ReadKey(true);
    if (cki.Key == ConsoleKey.Spacebar)
    {
        highscore = playGame(highscore);
    }
    else if (cki.Key == ConsoleKey.Escape)
    {
        break;
    }
    Console.Clear();
}
Console.Clear();
Console.WriteLine("See ya!");

//Functions
static int playGame(int highscore)
{
    //Setup variables
    ConsoleKey lastPressedKey = ConsoleKey.None;
    Random r = new Random();
    int score = 0;

    //Playingfield
    int playingFieldWidth = 15;
    int playingFieldHeight = 10;
    char[,] playingField = new char[playingFieldWidth, playingFieldHeight];

    //Player
    bool playerIsAlive = true;
    string[] playerPositionXY = new string[playingFieldHeight * playingFieldWidth - (playingFieldHeight - 2) * 2 - playingFieldWidth * 2];
    int playerPositionX = r.Next(1, playingFieldWidth - 1);
    int playerPositionY = r.Next(1, playingFieldHeight - 1);
    playerPositionXY[0] = $"{playerPositionX} {playerPositionY}";
    int[] playerVelocityXY = new int[2];
    int playerLength = 1;
    int playerSpeed = 1;

    //Food
    string foodPositionXY = String.Empty;
    foodPositionXY = MakeFood(foodPositionXY, playerPositionXY, playerLength, playingFieldWidth, playingFieldHeight);
    bool needFood = false;

    //Game Loop
    while (playerIsAlive)
    {
        //Movement input
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo cki;
            cki = Console.ReadKey(true);
            lastPressedKey = cki.Key;
            playerVelocityXY = MovePlayer(lastPressedKey, playerVelocityXY[0], playerVelocityXY[1]);
        }
        playerPositionX += playerVelocityXY[0];
        playerPositionY += playerVelocityXY[1];
        needFood = FoundFood(foodPositionXY, playerPositionXY);
        if (needFood)
        {
            foodPositionXY = MakeFood(foodPositionXY, playerPositionXY, playerLength, playingFieldWidth, playingFieldHeight);
            needFood = false;
            playerLength++;
            score++;
            Console.Beep(440, 50);
            if ((playerLength - 1) % 5 == 0)
            {
                playerSpeed++;
            }
        }
        playerPositionXY = GetPlayerPosition(playerPositionXY, playerLength, playerPositionX, playerPositionY);
        playerIsAlive = Crashed(playerPositionXY, playerLength, playingFieldWidth, playingFieldHeight);

        Console.Clear();
        Console.WriteLine($"Score {score} Speed {playerSpeed}");
        PrintPlayingField(GetPlayingField(playingFieldWidth, playingFieldHeight, playerPositionXY, playerLength, foodPositionXY), foodPositionXY);
        Thread.Sleep(400 - playerSpeed * 40);
    }

    //Death screen
    Console.Clear();
    Console.WriteLine($"Score {score} Speed {playerSpeed}");
    PrintPlayingField(GetPlayingField(playingFieldWidth, playingFieldHeight, playerPositionXY, playerLength, foodPositionXY), foodPositionXY);
    Console.WriteLine("You crashed!");
    Thread.Sleep(3500);
    Console.Clear();
    if (score > highscore)
    {
        highscore = score;
    }
    return highscore;

}
static int[] MovePlayer(ConsoleKey direction, int velocityX, int velocityY)
{
    if (direction == ConsoleKey.UpArrow)
    {
        if (!(velocityY == 1))
        {
            velocityY = -1;
            velocityX = 0;
        }
    }
    else if (direction == ConsoleKey.DownArrow)
    {
        if (!(velocityY == -1))
        {
            velocityY = 1;
            velocityX = 0;
        }
    }
    else if (direction == ConsoleKey.RightArrow)
    {
        if (!(velocityX == -1))
        {
            velocityX = 1;
            velocityY = 0;
        }

    }
    else if (direction == ConsoleKey.LeftArrow)
    {
        if (!(velocityX == 1))
        {
            velocityX = -1;
            velocityY = 0;
        }
    }
    int[] xYVelocity = new int[] { velocityX, velocityY };
    return xYVelocity;
}
static bool FoundFood(string foodPositionXY, string[] playerArrayXY)
{
    bool foundFood = false;

    if (playerArrayXY[0] == foodPositionXY)
    {
        foundFood = true;
    }
    return foundFood;

}
static string MakeFood(string foodPosition, string[] playerArray, int playerLength, int playingFieldWidth, int playingFieldHeight)
{
    Random r = new Random();
    bool foundSpotForFood = false;

    while (!foundSpotForFood)
    {
        foodPosition = $"{r.Next(1, playingFieldWidth - 1)} {r.Next(1, playingFieldHeight - 1)}";

        for (int i = 0; i < playerLength; i++)
        {
            if (!(foodPosition == playerArray[i]))
            {
                foundSpotForFood = true;
            }
            else
            {
                foundSpotForFood = false;
                break;
            }
        }

    }

    return foodPosition;
}
static string[] GetPlayerPosition(string[] playerArray, int playerLength, int playerX, int playerY)
{
    for (int i = playerLength - 1; i >= 0; i--)
    {
        if (i > 0)
        {
            playerArray[i] = playerArray[i - 1];
        }
        else
        {
            playerArray[i] = $"{playerX} {playerY}";
        }
    }
    return playerArray;
}
static bool Crashed(string[] playerArrayXY, int playerLength, int playingFieldWidth, int playingFieldHeight)
{
    //Check wall collision
    string[] playerHeadXY = playerArrayXY[0].Split(" ");
    if (Int32.Parse(playerHeadXY[0]) == 0 || Int32.Parse(playerHeadXY[0]) == playingFieldWidth - 1 || Int32.Parse(playerHeadXY[1]) == 0 || Int32.Parse(playerHeadXY[1]) == playingFieldHeight - 1)
    {
        Console.Beep(200, 150);
        Console.Beep(200, 150);
        Console.Beep(200, 150);
        Console.Beep(140, 200);
        return false;
    }
    //Check player collision
    for (int i = 1; i < playerLength; i++)
    {
        if (playerArrayXY[0] == playerArrayXY[i])
        {
            Console.Beep(200, 150);
            Console.Beep(200, 150);
            Console.Beep(200, 150);
            Console.Beep(140, 200);
            return false;
        }
    }
    return true;
}
static char[,] GetPlayingField(int width, int height, string[] playerPositionXY, int playerLength, string foodPositionXY)
{
    Random r = new Random();
    char[,] playingField = new char[width, height];

    //Add background    
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (y == 0 || y == height - 1 || x == 0 || x == width - 1)
            {
                playingField[x, y] = '#';
            }
            else
            {
                playingField[x, y] = ' ';
            }
        }
    }
    //Add player
    for (int i = playerLength - 1; i >= 0; i--)
    {
        string[] currentSnakePartXY = playerPositionXY[i].Split(' ');
        if (i == 0)
        {
            playingField[Int32.Parse(currentSnakePartXY[0]), Int32.Parse(currentSnakePartXY[1])] = '@';
        }
        else
        {
            playingField[Int32.Parse(currentSnakePartXY[0]), Int32.Parse(currentSnakePartXY[1])] = 'o';
        }
    }
    //Add Food
    string[] currentFoodPositionXY = foodPositionXY.Split(' ');
    if (playerPositionXY[0] == foodPositionXY)
    {
        playingField[Int32.Parse(currentFoodPositionXY[0]), Int32.Parse(currentFoodPositionXY[1])] = '@';
    }
    else
    {
        playingField[Int32.Parse(currentFoodPositionXY[0]), Int32.Parse(currentFoodPositionXY[1])] = '*';
    }



    return playingField;
}
static void PrintPlayingField(char[,] playingField, string foodPositionXY)
{
    string[] foodXY = foodPositionXY.Split(" ");
    for (int y = 0; y < playingField.GetLength(1); y++)
    {
        for (int x = 0; x < playingField.GetLength(0); x++)
        {
            if (x == Int32.Parse(foodXY[0]) && y == Int32.Parse(foodXY[1]))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(playingField[x, y]);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(playingField[x, y]);
            }
        }
        Console.WriteLine();
    }
}