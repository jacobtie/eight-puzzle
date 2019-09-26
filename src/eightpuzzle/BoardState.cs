using System;
using System.Collections.Generic;
using assignment1.logging;

namespace assignment1.eightpuzzle
{
    public class BoardState : IEquatable<BoardState>
    {
        // Creation of the basic getters and setters
        public int[,] State { get; set; }
        public Position ZeroPosition { get; set; }

        // Method to build a random board 
        public static BoardState BuildRandomBoard()
        {
            // Create variable to create random board
            List<int> state;
            var rand = new Random();
            Position zeroPos = new Position(-1, -1);
            int[,] boardState = new int[3, 3];
            BoardState random;

            // While the random starting state cannot reach the goal state
            do
            {
                // Reset the list
                state = new List<int>();

                Logger.WriteLine("Generating initial state...");
                Logger.WriteLine();

                // Fill the list with random values from zero to 8 that are unique
                for (int i = 0; i < 9; i++)
                {
                    // Reset the random number
                    int randomNum = -1;

                    // While the number is not unique
                    do
                    {
                        // Create a random number from 0 to 8
                        randomNum = rand.Next(0, 9);
                    }
                    while (state.Contains(randomNum));

                    // Add the random number to the list
                    state.Add(randomNum);

                    // If the random number is 0
                    if (randomNum == 0)
                    {
                        // Convert the index of the empty tile to 2D
                        int actualI = i / 3;
                        int actualJ = i % 3;

                        // Store the 2D coordinates in the field
                        zeroPos.Row = actualI;
                        zeroPos.Col = actualJ;
                    }
                }

                // Convert the list into a 2D array
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        boardState[i, j] = state[(i * 3) + j];
                    }
                }

                // Set random equal to a new board with the random configuration
                random = new BoardState { State = boardState, ZeroPosition = zeroPos };
            }
            while (!AStar.goalState.checkPolarity(random));

            // Return the random board
            return random;
        }

        // Method to create a board with a given configuration
        public static BoardState SetBoard(string[] nums)
        {
            // Create the variables to set the configuration of the board
            int[,] boardState = new int[3, 3];
            List<int> state = new List<int>();
            Position zeroPos = new Position();

            // If the number of elements is not equal to nine
            if (nums.Length != 9)
            {
                Console.WriteLine("The given configuration has " + nums.Length + " values. It must have nine. ");
                Console.WriteLine();
                return null;
            }

            // Check and store each value in the given array
            for (int i = 0; i < 9; i++)
            {
                var parsedNum = -1;

                if (!Int32.TryParse(nums[i], out parsedNum))
                {
                    return null;
                }

                // If the current value is not between zero and eight
                if (parsedNum > 8 || parsedNum < 0)
                {
                    Console.WriteLine("All numbers must be between 0 and 8 inclusive. ");
                    Console.WriteLine();
                    return null;
                }

                // If the current value is stored in the list multiple times
                if (state.Contains(parsedNum))
                {
                    Console.WriteLine("All numbers must be unique from one another. ");
                    Console.WriteLine();
                    return null;
                }

                // If the current value is equal to zero
                if (parsedNum == 0)
                {
                    // Set the position of the empty tile
                    zeroPos = new Position(i / 3, i % 3);
                }

                // Add the current value to the list
                state.Add(parsedNum);
            }

            // Convert the list to a 2D array
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    boardState[i, j] = state[(i * 3) + j];
                }
            }

            // Return the new board state with the given configuration
            return new BoardState { State = boardState, ZeroPosition = zeroPos };
        }

        // Method to copy the contents of this object to a new one
        public static BoardState BuildFromBoard(BoardState state)
        {
            // Create 2D array to store the copied board
            int[,] samedState = new int[3, 3];

            // Populate the board
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    samedState[i, j] = state[i, j];
                }
            }

            // Return the new board state with the same values
            return new BoardState { State = samedState, ZeroPosition = new Position(state.ZeroPosition.Row, state.ZeroPosition.Col) };
        }

        // Method to print the board to the console
        public void PrintBoard()
        {
            // Print the board and store the output
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Logger.Write(State[i, j] + " ");
                }
                Logger.WriteLine();
            }
            Logger.WriteLine();
        }

        // Method to swap an adjacent tile with the empty tile
        public void ShwapTiles(Position otherPos)
        {
            // Set the empty tile's value equal to the adjacent tile's value
            State[ZeroPosition.Row, ZeroPosition.Col] = State[otherPos.Row, otherPos.Col];

            // Set the adjacent tile's value equal to zero
            State[otherPos.Row, otherPos.Col] = 0;

            // Update the new empty position for the board
            ZeroPosition = new Position(otherPos.Row, otherPos.Col);
        }

        // Method to simply allow direct reference to the 2D array from BoardNode
        public int this[int i, int j]
        {
            get => State[i, j];
            set => State[i, j] = value;
        }

        // Method that is used when checking if the state is in the closed list
        public bool Equals(BoardState other)
        {
            // Return false if any value of the configuration is different
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (this[i, j] != other[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // Method to check if the polarity of each board is equal to prove if it is impossible
        public bool checkPolarity(BoardState other)
        {
            // Create variables to get the number of inversions from each
            List<int> state1 = new List<int>();
            List<int> state2 = new List<int>();
            int inversions1 = 0;
            int inversions2 = 0;

            // Convert each 2D array to a 1D list
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    state1.Add(State[i, j]);
                    state2.Add(other.State[i, j]);
                }
            }

            // Find the number of inversions for each board
            for (int i = 0; i < 9; i++)
            {
                // If the current value in state1 is not equal to zero
                if (state1[i] != 0)
                {
                    // Compare the current value to each value after it in state1
                    for (int j = i + 1; j < 9; j++)
                    {
                        // If the latter value is greater than the current, increment the inversions
                        if (state1[j] != 0 && state1[i] > state1[j])
                        {
                            inversions1++;
                        }
                    }
                }

                // If the current value in state2 is not equal to zero
                if (state2[i] != 0)
                {
                    // Compare the current value to each value after it in state1
                    for (int j = i + 1; j < 9; j++)
                    {
                        // If the latter value is greater than the current, increment the inversions
                        if (state2[j] != 0 && state2[i] > state2[j])
                        {
                            inversions2++;
                        }
                    }
                }
            }

            // If the polarity of each board is not the same
            if (inversions1 % 2 != inversions2 % 2)
            {
                Logger.WriteLine("The intial state cannot reach the goal state. ");
                Logger.WriteLine();
            }

            // Return if the polarities of both boards are equal
            return inversions1 % 2 == inversions2 % 2;
        }

        // Method to convert the 2D array to a list
        public List<int> toList()
        {
            // Create a list variable to store the converted 2D array
            List<int> stateList = new List<int>();

            // Add each tile to the list
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    stateList.Add(State[i, j]);
                }
            }

            // Return the populated list
            return stateList;
        }

    }
}
