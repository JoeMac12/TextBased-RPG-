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
        static (int x, int y) playerPosition;

        static void Main(string[] args) // Main 
        {
            LoadMap("mapArea.txt");
            InitializePlayer();

            while (true) // Creats a new map every input. I'll try and fix that later to make it just the one map
            {
                DisplayMap();
                PlayerMovement();
            }

            //Console.WriteLine("Press any key to exit");
            //Console.ReadKey(true);
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
                        Console.Write('0'); // player icon
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
            playerPosition = (1, 1);
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