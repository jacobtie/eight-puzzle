using System;
using assignment1.eightpuzzle;

namespace assignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            // var board = BoardState.BuildRandomBoard();

            // board.PrintBoard();

            // var boardNode = new BoardNode(board, 0, true, null);

            // Console.WriteLine($"Misplaced Heuristic: {boardNode.HValMisplaced}");
            // Console.WriteLine($"Manhatten Heuristic: {boardNode.HValManhat}");
            // Console.WriteLine($"G Value: {boardNode.GVal}");

            // Console.WriteLine("Successors: ");
            // foreach (var successor in boardNode.GenerateSuccessors())
            // {
            // 	Console.WriteLine();
            // 	successor.State.PrintBoard();
            // 	Console.WriteLine($"Misplaced Heuristic: {successor.HValMisplaced}");
            // 	Console.WriteLine($"Manhatten Heuristic: {successor.HValManhat}");
            // 	Console.WriteLine($"G Value: {successor.GVal}");
            // }

            AStar.Run(false);

            Console.WriteLine("\n\nPress enter to exit...");
            Console.ReadLine();
        }
    }
}
