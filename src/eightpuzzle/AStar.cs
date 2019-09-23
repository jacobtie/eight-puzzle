using System;
using System.Collections.Generic;
using System.Linq;
using assignment1.structures;

namespace assignment1.eightpuzzle
{
	public static class AStar
	{
		public static void Run(bool useMisplacedHeuristic)
		{
			int[,] testState = { { 4, 1, 3 }, { 0, 2, 6 }, { 7, 5, 8 } };
			var testBoard = new BoardState { State = testState, ZeroPosition = new Position(1, 0) };
			// var initialState = new BoardNode(BoardState.BuildRandomBoard(), 0, useMisplacedHeuristic, null);
			var initialState = new BoardNode(testBoard, 0, useMisplacedHeuristic, null);
			var frontier = new MinHeap<BoardNode>();
			frontier.Add(initialState);
			var visited = new List<BoardState>();
			BoardNode current = null;

			while (!frontier.IsEmpty())
			{
				current = frontier.RemoveMin();
				current.State.PrintBoard();
				Console.WriteLine($"Misplaced Heuristic: {current.HValMisplaced}");
				Console.WriteLine($"Manhatten Heuristic: {current.HValManhat}");
				Console.WriteLine($"G Value: {current.GVal}");
				Console.WriteLine();
				if (current.GoalNode)
				{
					break;
				}
				visited.Add(current.State);
				var successors = current.GenerateSuccessors().Where(successor => !visited.Contains(successor.State));
				foreach (var successor in successors)
				{
					frontier.Add(successor);
				}
			}

			var path = new Stack<BoardNode>();

			while (current != null)
			{
				path.Push(current);
				current = current.Parent;
			}

			foreach (var item in path)
			{
				item.State.PrintBoard();
				Console.WriteLine();
			}
		}
	}
}
