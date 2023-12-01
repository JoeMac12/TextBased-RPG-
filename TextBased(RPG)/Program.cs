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

        static bool Win()
        {
            return goldScore >= 10; // Check if player has 10 coins
        }

        static string actionMessage = ""; // Action text

        static void Main(string[] args) // Main 
        {
            LoadMap("mapArea.txt");
            InitializePlayer();
            InitializeEnemy();

            while (playerHealth > 0) // While the player is alive
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

                if (Win())
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Congratulations! You have collected all 10 gold coins!");
                    Console.WriteLine();
                    Console.WriteLine("You win!");
                    Console.ResetColor();
                    Console.ReadKey();
                    return; // Close game
                }
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You have Died"); // Player dies somehow
            Console.ResetColor();
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
                        Console.ForegroundColor = ConsoleColor.Green; // Player Color
                        Console.Write('█'); // Player icon
                    }
                    else if (enemyHealth > 0 && i == enemyPosition.y && j == enemyPosition.x)
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

            if (enemyHealth <= 0)
            {
                enemyPosition = (-1, -1); // Remove the enemy of the map when killed
                actionMessage = "The enemy has been killed!";
            }
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

        static void MovePlayer(int x, int y) // Handles where the player moves to on the map
        {
            int moveX = playerPosition.x + x;
            int moveY = playerPosition.y + y;

            if (WithinBounds(moveX, moveY) && map[moveY, moveX] != '#' && map[moveY, moveX] != '|' && map[moveY, moveX] != '-') // Check if it's a wall
            {
                if (moveX == enemyPosition.x && moveY == enemyPosition.y)
                {
                    PlayerAttack(); // Attack the enemy but does not cause a move
                }
                else
                {
                    if (map[moveY, moveX] == 'Θ') // Check if it's gold
                    {
                        goldScore++;
                        map[moveY, moveX] = '.';
                        actionMessage = "You collected a gold coin!";
                    }

                    playerPosition = (moveX, moveY); // Move the player if not attacking

                    if (map[moveY, moveX] == '~')
                    {
                        AcidDamage();
                    }
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
                EnemyAttack(); // Attack the player but does not cause a move
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
                case '.': // Floor
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