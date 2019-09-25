using System;
using System.Collections.Generic;

namespace assignment1.eightpuzzle
{
    public class BoardNode : IComparable<BoardNode>
    {
        // Creation of the basic getter and setter methods
        public BoardState State { get; set; }
        public int HValMisplaced { get; set; }
        public int HValManhat { get; set; }
        public int GVal { get; set; }
        public bool UseMisplacedHeuristic { get; set; }
        public BoardNode Parent { get; set; }

        // Getter methods for the F values and the check for the goal node
        public int FVal { get => UseMisplacedHeuristic ? HValMisplaced + GVal : HValManhat + GVal; }
        public bool GoalNode { get => HValMisplaced == 0; }

        // Overloaded Constructor to create the board with the inital values
        public BoardNode(BoardState state, int distance, bool useMisplacedHeuristic, BoardNode parent)
        {
            State = state;
            GVal = distance;
            UseMisplacedHeuristic = useMisplacedHeuristic;
            Parent = parent;

            // Calculated the heuristic based on the goal node
            _calculateHVals();
        }

        // Method to store the surrounding nodes of this node
        public List<BoardNode> GenerateSuccessors()
        {
            // Create a new list to store the surrounding nodes
            var successors = new List<BoardNode>();

            // If the position of the empty tile is not along the top row
            if (State.ZeroPosition.Row > 0)
            {
                // Store the board state from the above node
                var above = BoardState.BuildFromBoard(State);

                // Swap the above tile with the empty tile
                above.ShwapTiles(new Position(above.ZeroPosition.Row - 1, above.ZeroPosition.Col));
                
                // Create a new node with this board state
                var nextState = new BoardNode(above, GVal + 1, UseMisplacedHeuristic, this);
                
                // Add the new node to the list of successors
                successors.Add(nextState);
            }


            if (State.ZeroPosition.Row < 2)
            {
                // Store the board state from the below node
                var below = BoardState.BuildFromBoard(State);

                // Swap the below tile with the empty tile
                below.ShwapTiles(new Position(below.ZeroPosition.Row + 1, below.ZeroPosition.Col));
                
                 // Create a new node with this board state               
                var nextState = new BoardNode(below, GVal + 1, UseMisplacedHeuristic, this);
                
                // Add the new node to the list of successors                
                successors.Add(nextState);
            }
            if (State.ZeroPosition.Col > 0)
            {
                // Store the board state from the left node
                var left = BoardState.BuildFromBoard(State);
                
                // Swap the left tile with the empty tile
                left.ShwapTiles(new Position(left.ZeroPosition.Row, left.ZeroPosition.Col - 1));
                
                 // Create a new node with this board state               
                var nextState = new BoardNode(left, GVal + 1, UseMisplacedHeuristic, this);
                
                // Add the new node to the list of successors
                successors.Add(nextState);
            }
            if (State.ZeroPosition.Col < 2)
            {
                // Store the board state from the right node
                var right = BoardState.BuildFromBoard(State);

                // Swap the right tile with the empty tile
                right.ShwapTiles(new Position(right.ZeroPosition.Row, right.ZeroPosition.Col + 1));
                
                 // Create a new node with this board state               
                var nextState = new BoardNode(right, GVal + 1, UseMisplacedHeuristic, this);
                
                // Add the new node to the list of successors
                successors.Add(nextState);
            }

            // Return the list of successors
            return successors;
        }

        // Method to calculate the misplaced tile heuristic and manhattan distance heuristic
        private void _calculateHVals()
        {
            // Create variable to store the number of misplaced tiles
            int misplacedValue = 0;

            // Add each tile that is not the same as the goal state, except the empty tile
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (State[i, j] == 0)
                    {
                        continue;
                    }
                    if (State[i, j] != AStar.goalState[i, j])
                    {
                        misplacedValue++;
                    }
                }
            }

            // Set the field equal to the value
            HValMisplaced = misplacedValue;

            // Create variable to store the total distance that all tiles would have to move
            int manhatHeuristic = 0;

            // Add the each distance that each tile is from the goal configuration
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (State[i, j] == 0)
                    {
                        continue;
                    }

                    // Equations to find and calculate distances
                    int actualI = AStar.goalState.toList().IndexOf(State[i, j]) / 3;
                    int actualJ = AStar.goalState.toList().IndexOf(State[i, j]) % 3;
                    manhatHeuristic += Math.Abs(i - actualI) + Math.Abs(j - actualJ);
                }
            }

            // Set field equal to the value
            HValManhat = manhatHeuristic;
        }

        // Method to compare nodes to one another in the heap
        public int CompareTo(BoardNode other)
        {
            // Return an integer based on the F value of each node
            return this.FVal == other.FVal ? 0 : this.FVal < other.FVal ? -1 : 1;
        }
    }
}
