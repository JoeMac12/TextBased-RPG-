using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBased_RPG_
{
    internal class Program
    {
        static char[,] map; // All main variables for the game
        static int mapHeight; 
        static int mapWidth;
        static int playerHealth = 20;
        static int enemyHealth = 10;
        static int goldScore = 0;
        static (int x, int y) playerPosition;
        static (int x, int y) enemyPosition;
        static Random random = new Random(); // Used for the enemy movement

        static bool playerMoved = false; // Used for checking if the user has pressed a key that moves the player

        static bool Win()
        {
            return goldScore >= 10; // Checks if the player has collected 10 gold coins
        }

        static string actionMessage = ""; // Used for displaying game info to the player

        static void Main(string[] args) // Main call
        {
            LoadMap("mapArea.txt");
            InitializePlayer();
            InitializeEnemy();

            while (playerHealth > 0) // Game runs if the player has health
            {
                Console.Clear(); // Used for clearing the old map to keep the console clean every input
                DisplayMap();
                DisplayHUD();

                playerMoved = false; // Set to false on first loadup 

                PlayerMovement();

                if (playerMoved && enemyHealth > 0) // Checks if the player has moved and the enemy has health
                {
                    MoveEnemy();
                }

                if (Win()) // Called when gold score has reached 10 / 10
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Congratulations! You have collected all 10 gold coins!");
                    Console.WriteLine();
                    Console.WriteLine("You win!");
                    Console.ResetColor();
                    Console.ReadKey();
                    return; // Close the game
                }
            }

            Console.Clear(); // Everything else is called if the player dies to the enemy or acid 
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You have Died");
            Console.ResetColor();
            Console.ReadKey();
        }

        static void LoadMap(string fileName) // Loads the map text from the map file
        {
            string[] lines = File.ReadAllLines(fileName);
            mapHeight = lines.Length;
            mapWidth = lines[0].Length;
            map = new char[mapHeight, mapWidth];

            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    map[i, j] = lines[i][j];
                }
            }
        }

        static void DisplayHUD() // Displays the stats of everything
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Player Health: {playerHealth}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Enemy Health: {enemyHealth}");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Gold: {goldScore} / 10");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Action: {actionMessage}");
            Console.ResetColor();
        }

        static void DisplayMap() // Display the map text and border
        {
            DrawBorder(); // Top border

            for (int i = 0; i < mapHeight; i++)
            {
                Console.Write("|"); // Left border

                for (int j = 0; j < mapWidth; j++)
                {
                    if (i == playerPosition.y && j == playerPosition.x)
                    {
                        Console.ForegroundColor = ConsoleColor.Green; // Player color
                        Console.Write('█'); // Player icon
                    }
                    else if (enemyHealth > 0 && i == enemyPosition.y && j == enemyPosition.x)
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Enemy color
                        Console.Write('█'); // Enemy icon
                    }
                    else
                    {
                        SetTextColor(map[i, j]); // Used for setting the color for each map text type
                        Console.Write(map[i, j]);
                    }
                    Console.ResetColor();
                }

                Console.WriteLine("|"); // Right border
            }

            DrawBorder(); // Bottom border
        }

        static void DrawBorder() // Draws a simple border for the top and bottem of the map
        {
            Console.Write("+");
            for (int i = 0; i < mapWidth; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("+");
        }

        static bool WithinBounds(int x, int y) // Used for checking if the player or enemy is within the map border so that they can't leave
        {
            return x >= 0 && x < mapWidth && y >= 0 && y < mapHeight;
        }

        static void InitializePlayer() // Starting spot of the player
        {
            playerPosition = (mapWidth - 78, mapHeight - 19); // Top left of map
        }

        static void InitializeEnemy() // Starting spot of the enemy
        {
            enemyPosition = (mapWidth - 50, mapHeight - 3); // Middle bottem of map
        }

        static void PlayerAttack() // Player attacking enemy
        {
            enemyHealth--; // Enemy takes 1 damage
            actionMessage = "You attacked the enemy for 1 damage!";

            if (enemyHealth <= 0) // Move the enemy outside the map when it dies
            {
                enemyPosition = (-1, -1); 
                actionMessage = "The enemy has been killed!";
            }
        }

        static void EnemyAttack() // Enemy attacking player
        {
            playerHealth--; // Player takes 1 damage
            actionMessage = "The enemy attacked you for 1 damage!";
        }

        static void AcidDamage() // Acid damage for puddle
        {
            playerHealth--; // Player takes 1 damage
            actionMessage = "You stepped in acid and took 1 damage!";
        }

        static void PlayerMovement() // Controls for the player movement 
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    MovePlayer(0, -1); // Up 1 unit
                    playerMoved = true;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    MovePlayer(0, 1); // Down 1 unit
                    playerMoved = true;
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    MovePlayer(-1, 0); // Left 1 unit
                    playerMoved = true;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    MovePlayer(1, 0); // Right 1 unit
                    playerMoved = true;
                    break;
            }
        }

        static void MovePlayer(int x, int y) // Main handler for the player
        {
            int moveX = playerPosition.x + x;
            int moveY = playerPosition.y + y;

            if (WithinBounds(moveX, moveY) && map[moveY, moveX] != '#' && map[moveY, moveX] != '|' && map[moveY, moveX] != '-') // If not a wall, the player can move
            {
                if (moveX == enemyPosition.x && moveY == enemyPosition.y)
                {
                    PlayerAttack(); // Player attacks the enemy but does not move in the process
                }
                else
                {
                    if (map[moveY, moveX] == 'Θ') // Check if it's gold and updates gold score
                    {
                        goldScore++;
                        map[moveY, moveX] = '.'; // Replace the gold text with the background text
                        actionMessage = "You collected a gold coin!";
                    }

                    playerPosition = (moveX, moveY); // Move the player as normal if not attacking

                    if (map[moveY, moveX] == '~')
                    {
                        AcidDamage(); // Player takes damage if they are standing on acid
                    }
                }
            }
        }

        static void MoveEnemy() // Handles the enemy movement
        {
            int direction = random.Next(4); // Makes the enemy pick a random direction each time the player moves
            int x = 0, y = 0;

            switch (direction)
            {
                case 0: y = -1; break; // Move up
                case 1: y = 1; break; // Move down
                case 2: x = -1; break; // Move left
                case 3: x = 1; break; // Move right
            }

            int moveX = enemyPosition.x + x;
            int moveY = enemyPosition.y + y;

            if (moveX == playerPosition.x && moveY == playerPosition.y)
            {
                EnemyAttack(); // Enemy attacks the player but does not move in the process
            }
            else if (WithinBounds(moveX, moveY) && (map[moveY, moveX] != '#' && map[moveY, moveX] != '|' && map[moveY, moveX] != '-')) // If not a wall, the enemy can move
            {
                enemyPosition = (moveX, moveY); // Move the enemy as normal if not attacking
            }
        }

        static void SetTextColor(char textType) // Used for setting the color for each text type
        {
            switch (textType)
            {
                case '.': // Floor / Background
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case '~': // Acid
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case '#': // Walls
                case '|': 
                case '-': 
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case 'Θ': // Gold
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                default:
                    Console.ResetColor();
                    break;
            }
        }
    }
}