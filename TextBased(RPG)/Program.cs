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
        static char[,] map; // Map properties
        static int mapHeight; 
        static int mapWidth;

        static void Main(string[] args) // Main 
        {
            LoadMap("mapArea.txt");
            DisplayMap();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey(true);
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
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}