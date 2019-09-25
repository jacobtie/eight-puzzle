using System;
using assignment1.eightpuzzle;
using assignment1.logging;

namespace assignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Run A* algorithm
            AStar.Run();

            // Write data to /logs/.keep
            Logger.WriteLogFile();

            Console.WriteLine("\n\nPress enter to exit...");
            Console.ReadLine();
        }
    }
}
