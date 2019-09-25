using System;
using assignment1.eightpuzzle;
using assignment1.logging;

namespace assignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            AStar.Run();

            Logger.WriteLogFile();

            Console.WriteLine("\n\nPress enter to exit...");
            Console.ReadLine();
        }
    }
}
