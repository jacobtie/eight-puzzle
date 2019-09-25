using System;
using System.Collections.Generic;
using System.Linq;
using assignment1.structures;
using System.Text.RegularExpressions;
using assignment1.logging;

namespace assignment1.eightpuzzle
{
    public static class AStar
    {
        public static BoardState goalState;

        public static void Run()
        {
            var frontier = new MinHeap<BoardNode>();
            var visited = new List<BoardState>();
            int numExpanded = 0;
            int numGenerated = 0;
            int pathCost = 0;
            BoardNode current = null;
            BoardNode initialState;

            do
            {
                goalState = getGoal();
                initialState = getInitial();
            }
            while (!goalState.checkParity(initialState.State));

            Logger.WriteLine("Initial State: ");
            initialState.State.PrintBoard();

            Logger.WriteLine("Goal State: ");
            goalState.PrintBoard();

            frontier.Add(initialState);

            while (!frontier.IsEmpty())
            {
                current = frontier.RemoveMin();
                numExpanded++;

                if (current.GoalNode)
                {
                    pathCost = current.GVal;
                    break;
                }
                visited.Add(current.State);
                var successors = current.GenerateSuccessors().Where(successor => !visited.Contains(successor.State));
                numGenerated += successors.ToList().Count;
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

            Logger.WriteLine("Path found. Showing path... ");
            Logger.WriteLine();

            foreach (var item in path)
            {
                item.State.PrintBoard();

                if (initialState.UseMisplacedHeuristic)
                {
                    Logger.WriteLine($"Misplaced Heuristic: {item.HValMisplaced}");
                }
                else
                {
                    Logger.WriteLine($"Manhatten Heuristic: {item.HValManhat}");
                }

                Logger.WriteLine($"G Value: {item.GVal}");
                Logger.WriteLine($"F Value: {item.FVal}");
                Logger.WriteLine();
            }

            Logger.WriteLine("Number of Nodes Generated: " + numGenerated);
            Logger.WriteLine("Number of Nodes Expanded: " + numExpanded);
            Logger.WriteLine("Total Path Cost: " + pathCost);
        }

        private static BoardNode getInitial()
        {
            BoardNode init;
            string[] start;
            char input1;
            char input2;

            do
            {
                Console.WriteLine("Would you like to create an intial state? (Y/N) ");
                input1 = Console.ReadLine().ToCharArray()[0];
                Console.WriteLine();
            }
            while (char.ToUpper(input1) != 'Y' && char.ToUpper(input1) != 'N');

            if (char.ToUpper(input1) == 'Y')
            {
                do
                {
                    Console.WriteLine("Please enter the starting state(1, 2, ... 8, 0): ");
                    start = Regex.Split(Console.ReadLine(), @"\D+");
                    Console.WriteLine();
                }
                while (BoardState.SetBoard(start) == null);

                do
                {
                    Console.WriteLine("Would you like to use the Misplaced Tile Heuristic? (Y/N) ");
                    input2 = Console.ReadLine().ToCharArray()[0];
                    Console.WriteLine();
                }
                while (char.ToUpper(input2) != 'Y' && char.ToUpper(input2) != 'N');

                if (char.ToUpper(input2) == 'Y')
                {
                    init = new BoardNode(BoardState.SetBoard(start), 0, true, null);
                }
                else
                {
                    init = new BoardNode(BoardState.SetBoard(start), 0, false, null);
                }
            }
            else
            {
                do
                {
                    Console.WriteLine("Would you like to use the Misplaced Tile Heuristic? (Y/N) ");
                    input2 = Console.ReadLine().ToCharArray()[0];
                    Console.WriteLine();
                }
                while (char.ToUpper(input2) != 'Y' && char.ToUpper(input2) != 'N');

                if (char.ToUpper(input2) == 'Y')
                {
                    init = new BoardNode(BoardState.BuildRandomBoard(), 0, true, null);
                }
                else
                {
                    init = new BoardNode(BoardState.BuildRandomBoard(), 0, false, null);
                }
            }

            return init;
        }

        private static BoardState getGoal()
        {
            BoardState final;
            string[] goal;
            char input1;

            do
            {
                Console.WriteLine("Would you like to create a goal state? (Y/N) ");
                input1 = Console.ReadLine().ToCharArray()[0];
                Console.WriteLine();
            }
            while (char.ToUpper(input1) != 'Y' && char.ToUpper(input1) != 'N');

            if (char.ToUpper(input1) == 'Y')
            {
                do
                {
                    Console.WriteLine("Please enter the goal state(1, 2, ... 8, 0): ");
                    goal = Regex.Split(Console.ReadLine(), @"\D+");
                    Console.WriteLine();
                }
                while (BoardState.SetBoard(goal) == null);

                final = BoardState.SetBoard(goal);
            }
            else
            {
                goal = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "0" };
                final = BoardState.SetBoard(goal);
            }

            return final;
        }
    }
}
