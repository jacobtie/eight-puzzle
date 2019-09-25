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
        // Reference variable to store the custom goal state
        public static BoardState goalState;

        // Method to run the overall A* algorithm and print the results to console
        public static void Run()
        {
            // Create the open and closed lists
            var frontier = new MinHeap<BoardNode>();
            var visited = new List<BoardState>();

            // Variables to show the amount of nodes generated and the cost
            int numExpanded = 0;
            int numGenerated = 0;
            int pathCost = 0;

            // Create node to interate through open list
            BoardNode current = null;

            // Create Node for the starting state
            BoardNode initialState;

            // Ask for user starting and goal states until configuration is possible
            do
            {
                // Ask user for goal state
                goalState = getGoal();

                // Ask user for starting state
                initialState = getInitial();
            }
            while (!goalState.checkPolarity(initialState.State));

            // Print starting and goal nodes
            Logger.WriteLine("Initial State: ");
            initialState.State.PrintBoard();

            Logger.WriteLine("Goal State: ");
            goalState.PrintBoard();

            // Add starting state to the open list
            frontier.Add(initialState);

            // While the open list is not empty
            while (!frontier.IsEmpty())
            {
                // Store the best move from the open list
                current = frontier.RemoveMin();
                numExpanded++;

                // If the current node is the goal
                if (current.GoalNode)
                {
                    // Set the path cost and break from the loop
                    pathCost = current.GVal;
                    break;
                }

                // Add the current state to the closed list
                visited.Add(current.State);

                // Retrieve the nodes around the current node
                var successors = current.GenerateSuccessors().Where(successor => !visited.Contains(successor.State));
                numGenerated += successors.ToList().Count;

                // Add each successor to the open list
                foreach (var successor in successors)
                {
                    frontier.Add(successor);
                }
            }

            // Create a stack to store the path
            var path = new Stack<BoardNode>();

            // Starting from the goal, push each parent to the stack
            while (current != null)
            {
                path.Push(current);
                current = current.Parent;
            }

            Logger.WriteLine("Path found. Showing path... ");
            Logger.WriteLine();

            // For each node in the path
            foreach (var item in path)
            {
                // Print the board
                item.State.PrintBoard();

                // If the user chose the misplaced tile heuristic
                if (initialState.UseMisplacedHeuristic)
                {
                    // Print the misplaced tile heuristic
                    Logger.WriteLine($"Misplaced Heuristic: {item.HValMisplaced}");
                }
                else
                {
                    // Print the manhattan distance heuristic
                    Logger.WriteLine($"Manhattan Heuristic: {item.HValManhat}");
                }

                // Print the G and F values
                Logger.WriteLine($"G Value: {item.GVal}");
                Logger.WriteLine($"F Value: {item.FVal}");
                Logger.WriteLine();
            }

            // Print the number of nodes generated, expanded, and the path cost
            Logger.WriteLine("Number of Nodes Generated: " + numGenerated);
            Logger.WriteLine("Number of Nodes Expanded: " + numExpanded);
            Logger.WriteLine("Total Path Cost: " + pathCost);
        }

        // Method to allow user input of the starting state and heuristic
        private static BoardNode getInitial()
        {
            // Create variables to get user input for starting state
            BoardNode init;
            string[] start;
            char input1;
            char input2;

            // While the input is invalid 
            do
            {
                // Ask user if they would like to create a custom starting state
                Console.WriteLine("Would you like to create an intial state? (Y/N) ");
                input1 = Console.ReadLine().ToCharArray()[0];
                Console.WriteLine();
            }
            while (char.ToUpper(input1) != 'Y' && char.ToUpper(input1) != 'N');

            // If the user wants to create an initial state
            if (char.ToUpper(input1) == 'Y')
            {
                // While the input is invalid
                do
                {
                    // Ask user to input starting state
                    Console.WriteLine("Please enter the starting state(1, 2, ... 8, 0): ");
                    start = Regex.Split(Console.ReadLine(), @"\D+");
                    Console.WriteLine();
                }
                while (BoardState.SetBoard(start) == null);

                // While the input is invalid
                do
                {
                    // Ask user if they would like to use the misplaced tile heuristic
                    Console.WriteLine("Would you like to use the Misplaced Tile Heuristic? (Y/N) ");
                    input2 = Console.ReadLine().ToCharArray()[0];
                    Console.WriteLine();
                }
                while (char.ToUpper(input2) != 'Y' && char.ToUpper(input2) != 'N');

                // If the user wants to use the misplaced tile heuristic
                if (char.ToUpper(input2) == 'Y')
                {
                    // Create node with the custom starting board and misplaced tile heuristic
                    init = new BoardNode(BoardState.SetBoard(start), 0, true, null);
                }
                else
                {
                    // Create node with the custom starting board and the manhattan distance heuristic
                    init = new BoardNode(BoardState.SetBoard(start), 0, false, null);
                }
            }
            else
            {
                // While the input is invalid
                do
                {
                    // Ask the user if they would like to use the misplaced tile heuristic
                    Console.WriteLine("Would you like to use the Misplaced Tile Heuristic? (Y/N) ");
                    input2 = Console.ReadLine().ToCharArray()[0];
                    Console.WriteLine();
                }
                while (char.ToUpper(input2) != 'Y' && char.ToUpper(input2) != 'N');

                // If the user wants to use the misplaced tile heuristic
                if (char.ToUpper(input2) == 'Y')
                {
                    // Create node using random board and misplaced tile heuristic
                    init = new BoardNode(BoardState.BuildRandomBoard(), 0, true, null);
                }
                else
                {
                    // Create node using random board and manhattan distance heuristic
                    init = new BoardNode(BoardState.BuildRandomBoard(), 0, false, null);
                }
            }

            // Return the created node as the starting node
            return init;
        }

        // Method to allow user input of the goal state
        private static BoardState getGoal()
        {
            // Create variables to get user input for the goal
            BoardState final;
            string[] goal;
            char input1;

            // While the input is invalid
            do
            {
                // Ask the user if they would like to create a custom goal state
                Console.WriteLine("Would you like to create a goal state? (Y/N) ");
                input1 = Console.ReadLine().ToCharArray()[0];
                Console.WriteLine();
            }
            while (char.ToUpper(input1) != 'Y' && char.ToUpper(input1) != 'N');

            // If the user wants to create a custom goal state
            if (char.ToUpper(input1) == 'Y')
            {
                // Ask the user to enter the goal state
                do
                {
                    Console.WriteLine("Please enter the goal state(1, 2, ... 8, 0): ");
                    goal = Regex.Split(Console.ReadLine(), @"\D+");
                    Console.WriteLine();
                }
                while (BoardState.SetBoard(goal) == null);

                // Set final equal to the board state created by the user
                final = BoardState.SetBoard(goal);
            }
            else
            {
                // Set the goal state to the default
                goal = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "0" };
                final = BoardState.SetBoard(goal);
            }

            // Return the final as the goal state
            return final;
        }
    }
}