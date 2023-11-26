using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBased_RPG_
{
    internal class Program
    {
        static void Main(string[] args) // Main stuff
        {

            Console.WriteLine("Text Based RPG Map");
            Console.WriteLine();
            Console.WriteLine("------------------");
            Console.WriteLine();

            DisplayLegend(); // Show info

            Console.WriteLine();
            Console.WriteLine("------------------");
            Console.WriteLine();

            Console.WriteLine("[Unscaled map]");
            Console.WriteLine();

            DisplayMap(); // Normal size map

            Console.WriteLine();
            Console.WriteLine("[Double size map]" + " " + "(X2)");
            Console.WriteLine();

            DisplayMap(2); // Double size map

            Console.WriteLine();
            Console.WriteLine("[Triple size map]" + " " + "(X3)");
            Console.WriteLine();

            DisplayMap(3); // Triple size map

            Console.WriteLine();

            Console.WriteLine("Press any key to exit the program");
            Console.ReadKey(true);

        }

        static char[,] map = new char[,] // The map itelf
        {
            {'^','^','^','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
            {'^','^','`','`','`','`','*','*','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','~','~','~','`','`','`'},
            {'^','^','`','`','`','*','*','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','~','~','~','`','`','`','`','`'},
            {'^','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
            {'`','`','`','`','~','~','~','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
            {'`','`','`','`','~','~','~','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
            {'`','`','`','~','~','~','~','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','^','^','`','`','`','`','`','`'},
            {'`','`','`','`','`','~','~','~','`','`','`','`','`','`','`','`','`','`','`','`','`','^','^','^','^','`','`','`','`','`'},
            {'`','`','`','`','`','~','~','~','~','`','`','`','`','`','`','`','`','`','`','`','`','`','`','^','^','^','^','`','`','`'},
            {'`','`','`','`','`','`','`','~','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
            {'`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
            {'`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
        };

        // The size of the unscaled map

        static int MapHeight = map.GetLength(0);
        static int MapWidth = map.GetLength(1);

        static void DisplayMap() // Display the map chart
        {
            DisplayBorder(MapWidth);
            for (int i = 0; i < MapHeight; i++)
            {
                Console.Write("|"); // Simple border maker
                for (int k = 0; k < MapWidth; k++)
                {
                    SetTextColor(map[i, k]);
                    Console.Write(map[i, k]);
                    Console.ResetColor();
                }
                Console.WriteLine("|"); // Other side of border
            }
            DisplayBorder(MapWidth);
        }

        static void DisplayMap(int scale) // Draw the map but to a scale amount instead
        {
            DisplayBorder(MapWidth * scale);
            for (int i = 0; i < MapHeight; i++)
            {
                for (int x = 0; x < scale; x++)
                {
                    Console.Write("|");
                    for (int v = 0; v < MapWidth; v++)
                    {
                        SetTextColor(map[i, v]);
                        for (int k = 0; k < scale; k++)
                        {
                            Console.Write(map[i, v]);
                        }
                        Console.ResetColor();
                    }
                    Console.WriteLine("|");
                }
            }
            DisplayBorder(MapWidth * scale);
        }

        static void SetTextColor(char TextType) // Should Make different text types like grass and mountains have different colors
        {
            switch (TextType) // Apply colors 
            {
                case '^':
                    Console.ForegroundColor = ConsoleColor.Gray; // Mountains
                    break;
                case '`':
                    Console.ForegroundColor = ConsoleColor.Green; // Grass
                    break;
                case '~':
                    Console.ForegroundColor = ConsoleColor.Blue; // Water
                    break;
                case '*':
                    Console.ForegroundColor = ConsoleColor.Yellow; // Trees
                    break;
                default:
                    Console.ResetColor(); // Base text color 
                    break;

            }
        }

        static void DisplayBorder(int size) // Draws the border on top and bottem of map
        {
            Console.Write("+");
            for (int i = 0; i < size; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("+");
        }

        static void DisplayLegend() // Tell the user what each text is for the map
        {
            Console.WriteLine("Map Legend:");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray; // Mountains
            Console.WriteLine("^ = Mountains");
            Console.ForegroundColor = ConsoleColor.Green; // Grass
            Console.WriteLine("` = Grass");
            Console.ForegroundColor = ConsoleColor.Blue; // Water
            Console.WriteLine("~ = Water");
            Console.ForegroundColor = ConsoleColor.Yellow; // Trees
            Console.WriteLine("* = Trees");
            Console.ResetColor();
        }
    }
}
