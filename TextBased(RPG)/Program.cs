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
        static char[,] map; // Map properties + Player & enemy
        static int mapHeight; 
        static int mapWidth;
        static int playerHealth = 20;
        static int enemyHealth = 10;
        static int goldScore = 0;
        static (int x, int y) playerPosition;
        static (int x, int y) enemyPosition;
        static Random random = new Random(); // For the enemy movement

        static void Main(string[] args) // Main 
        {
            LoadMap("mapArea.txt");
            InitializePlayer();
            InitializeEnemy();

            while (playerHealth > 0) // While the player is alive
            {
                DisplayMap();
                PlayerMovement();
                Console.Clear(); // Clear the old map update

                if (enemyHealth > 0) MoveEnemy(); // If enemy is alive, it can move

                Console.WriteLine($"Player Health: {playerHealth}"); // Simple health hud for now
                if (enemyHealth <= 0) Console.WriteLine("Enemy has been defeated!"); // Later make sure to remove the enemy when it dies
            }

            Console.WriteLine("Game Over"); // Player dies somehow
            Console.ReadKey();
        }

        static void LoadMap(string fileName) // Load the map text from the map file
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
                        Console.ForegroundColor = ConsoleColor.DarkBlue; // Player Color
                        Console.Write('█'); // Player icon
                    }
                    else if (i == enemyPosition.y && j == enemyPosition.x)
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Enemy Color
                        Console.Write('█'); // Enemy Icon
                    }
                    else
                    {
                        SetTextColor(map[i, j]);
                        Console.Write(map[i, j]);
                    }
                    Console.ResetColor();
                }

                Console.WriteLine("|"); // Right border
            }

            DrawBorder(); // Bottom border
        }

        static void DrawBorder() // Draw a border around the map so it looks nicer
        {
            Console.Write("+");
            for (int i = 0; i < mapWidth; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("+");
        }

        static bool WithinBounds(int x, int y)
        {
            return x >= 0 && x < mapWidth && y >= 0 && y < mapHeight;
        }

        static void InitializePlayer() // Starting spot of the player, maybe add random starting spot later?
        {
            playerPosition = (mapWidth - 78, mapHeight - 19); // Top left
        }

        static void InitializeEnemy() // Starting spot of the enemy
        {
            enemyPosition = (mapWidth - 50, mapHeight - 3); // Middle
        }

        static void PlayerMovement() // Controls for the player movement 
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.W:
                    MovePlayer(0, -1); // Up 1 unit
                    break;
                case ConsoleKey.S:
                    MovePlayer(0, 1); // Down 1 unit
                    break;
                case ConsoleKey.A:
                    MovePlayer(-1, 0); // Left 1 unit
                    break;
                case ConsoleKey.D:
                    MovePlayer(1, 0); // Right 1 unit
                    break;
            }
        }

        static void MovePlayer(int x, int y) // Handles where the player moves to on the map
        {
            int moveX = playerPosition.x + x;
            int moveY = playerPosition.y + y;

            if (WithinBounds(moveX, moveY))
            {
                if (map[moveY, moveX] == 'Θ') // Check if it's gold
                {
                    goldScore++; // Increase gold by 1
                    map[moveY, moveX] = '.'; // Readd background
                }
                else if (moveX == enemyPosition.x && moveY == enemyPosition.y)
                {
                    enemyHealth--; // Enemy takes 1 damage
                    Console.WriteLine("You attacked the enemy!");
                }
                else if (map[moveY, moveX] != '#' && map[moveY, moveX] != '|' && map[moveY, moveX] != '-') // Check if it's a wall
                {
                    playerPosition = (moveX, moveY); // Move the player
                }
            }
        }

        static void MoveEnemy() // Handles the random enemy movement
        {
            int direction = random.Next(4); // Make the enemy pick a random direction each time the player moves
            int x = 0, y = 0;
            switch (direction)
            {
                case 0: y = -1; break; // Move up
                case 1: y = 1; break; // Move Down
                case 2: x = -1; break; // Move left
                case 3: x = 1; break; // Move right
            }

            int moveX = enemyPosition.x + x;
            int moveY = enemyPosition.y + y;
            if (moveX == playerPosition.x && moveY == playerPosition.y)
            {
                playerHealth--; // Player takes 1 damage
                Console.WriteLine("The enemy attacked you!");
            }
            else if (moveX >= 0 && moveX < mapWidth && moveY >= 0 && moveY < mapHeight && (map[moveY, moveX] != '#' && map[moveY, moveX] != '|' && map[moveY, moveX] != '-')) // Check if it's a wall
            {
                enemyPosition = (moveX, moveY);
            }
        }

        static void SetTextColor(char textType) // Color for each text type
        {
            switch (textType)
            {
                case '.': // Grass
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case '~': // Acid
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case '#': // Walls
                case '|': 
                case '-': 
                    Console.ForegroundColor = ConsoleColor.DarkRed;
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