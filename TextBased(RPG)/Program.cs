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

        static bool playerMoved = false;

        static string actionMessage = ""; // Action text

        static void Main(string[] args) // Main 
        {
            LoadMap("mapArea.txt");
            InitializePlayer();
            InitializeEnemy();

            while (playerHealth >= 0) // While the player is alive
            {
                Console.Clear(); // Clear the old map update
                DisplayMap();
                DisplayHUD();

                playerMoved = false;

                PlayerMovement();

                if (playerMoved && enemyHealth > 0) // If enemy is alive and player has moved, it can move
                {
                    MoveEnemy();
                }

                if (enemyHealth <= 0) actionMessage = "The enemy has been killed!";
            }

            Console.WriteLine("You have Died"); // Player dies somehow
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

        static void DisplayHUD()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Player Health: {playerHealth}");
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

        static void PlayerAttack() // Player attacking
        {
            enemyHealth--; // Enemy takes 1 damage
            actionMessage = "You attacked the enemy for 1 damage!";
        }

        static void EnemyAttack() // Enemy attacking
        {
            playerHealth--; // Player takes 1 damage
            actionMessage = "The enemy attacked you for 1 damage!";
        }

        static void AcidDamage() // Acid damage
        {
            playerHealth--; // Player takes 1 damage
            actionMessage = "You have stepped in acid and took 1 damage!";
        }

        static void PlayerMovement() // Controls for the player movement 
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.W:
                    MovePlayer(0, -1); // Up 1 unit
                    playerMoved = true;
                    break;
                case ConsoleKey.S:
                    MovePlayer(0, 1); // Down 1 unit
                    playerMoved = true;
                    break;
                case ConsoleKey.A:
                    MovePlayer(-1, 0); // Left 1 unit
                    playerMoved = true;
                    break;
                case ConsoleKey.D:
                    MovePlayer(1, 0); // Right 1 unit
                    playerMoved = true;
                    break;
            }
        }

        static void MovePlayer(int x, int y) // Handles where the player moves to on the map
        {
            int moveX = playerPosition.x + x;
            int moveY = playerPosition.y + y;

            if (WithinBounds(moveX, moveY) && map[moveY, moveX] != '#' && map[moveY, moveX] != '|' && map[moveY, moveX] != '-') // Check if it's a wall
            {
                if (map[moveY, moveX] == 'Θ') // Check if it's gold
                {
                    goldScore++; // Increase gold by 1
                    map[moveY, moveX] = '.'; // Readd background
                    actionMessage = "You collected a gold coin!";
                }
                else if (moveX == enemyPosition.x && moveY == enemyPosition.y)
                {
                    PlayerAttack();
                }

                playerPosition = (moveX, moveY); // Move the player

                if (map[moveY, moveX] == '~') // Acid
                {
                    AcidDamage();
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
                case 1: y = 1; break; // Move down
                case 2: x = -1; break; // Move left
                case 3: x = 1; break; // Move right
            }

            int moveX = enemyPosition.x + x;
            int moveY = enemyPosition.y + y;

            if (moveX == playerPosition.x && moveY == playerPosition.y)
            {
                EnemyAttack();
            }
            else if (WithinBounds(moveX, moveY) && (map[moveY, moveX] != '#' && map[moveY, moveX] != '|' && map[moveY, moveX] != '-')) // Check if it's a wall
            {
                enemyPosition = (moveX, moveY); // Move enemy if not attacking
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
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
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