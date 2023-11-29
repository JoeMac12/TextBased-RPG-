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

            while (true) // Basic support for updating the map for game inputs
            {
                DisplayMap();
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