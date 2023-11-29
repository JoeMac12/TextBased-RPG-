﻿using System;
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

                if (playerPosition == enemyPosition) // Both take damage if they are in the same spot with eachother, maybe add support to check who move into each other first?
                {
                    playerHealth--;
                    enemyHealth--;
                    Console.WriteLine("Encountered a enemy! You both took 1 damage");
                }

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

        static void DisplayMap() // Display the map text
        {
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    if (i == playerPosition.y && j == playerPosition.x)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow; // Player Color
                        Console.Write('0'); // Player icon
                    }
                    else if (i == enemyPosition.y && j == enemyPosition.x)
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Enemy Color
                        Console.Write('X'); // Enemy Icon
                    }
                    else
                    {
                        SetTextColor(map[i, j]);
                        Console.Write(map[i, j]);
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        static void InitializePlayer() // Starting spot of the player, maybe add random starting spot later?
        {
            playerPosition = (mapWidth - 40, mapHeight - 20); // Top middle
        }

        static void InitializeEnemy() // Starting spot of the enemy
        {
            enemyPosition = (mapWidth - 40, mapHeight - 10); // Middle
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
            if (moveX >= 0 && moveX < mapWidth && moveY >= 0 && moveY < mapHeight && map[moveY, moveX] != '#') // Check if it's a wall
            {
                playerPosition = (moveX, moveY); // Move the player to the new position
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
            if (moveX >= 0 && moveX < mapWidth && moveY >= 0 && moveY < mapHeight && map[moveY, moveX] != '#') // Check if it's a wall
            {
                enemyPosition = (moveX, moveY);
            }
        }

        static void SetTextColor(char textType) // Color for each text type
        {
            switch (textType)
            {
                case '`': // Grass
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case '#': // Walls
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ResetColor();
                    break;
            }
        }
    }
}