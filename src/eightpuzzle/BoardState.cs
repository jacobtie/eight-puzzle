using System;
using System.Collections.Generic;
using System.Linq;

namespace assignment1.eightpuzzle
{
    public class BoardState : IEquatable<BoardState>
    {
        public int[,] State { get; set; }
        public Position ZeroPosition { get; set; }

        public static BoardState BuildRandomBoard()
        {
            List<int> state;
            var rand = new Random();
            Position zeroPos = new Position(-1, -1);
            int[,] boardState = new int[3, 3];
            BoardState random;

            do
            {
                state = new List<int>();
                Console.WriteLine("Generating initial state...");
                Console.WriteLine();

                for (int i = 0; i < 9; i++)
                {
                    int randomNum = -1;
                    do
                    {
                        randomNum = rand.Next(0, 9);
                    }
                    while (state.Contains(randomNum));

                    state.Add(randomNum);

                    if (randomNum == 0)
                    {
                        int actualI = i / 3;
                        int actualJ = i % 3;
                        zeroPos.Row = actualI;
                        zeroPos.Col = actualJ;
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        boardState[i, j] = state[(i * 3) + j];
                    }
                }

                random = new BoardState { State = boardState, ZeroPosition = zeroPos };
            }
            while (!AStar.goalState.checkParity(random));

            return random;
        }

        public static BoardState SetBoard(string[] nums)
        {
            int[,] boardState = new int[3, 3];
            List<int> state = new List<int>();
            Position zeroPos = new Position();

            if (nums.Length != 9)
            {
                return null;
            }

            for (int i = 0; i < 9; i++)
            {
                if (Int32.Parse(nums[i]) > 8 || Int32.Parse(nums[i]) < 0)
                {
                    return null;
                }

                if (state.Contains(Int32.Parse(nums[i])))
                {
                    return null;
                }

                if (Int32.Parse(nums[i]) == 0)
                {
                    zeroPos = new Position(i / 3, i % 3);
                }

                state.Add(Int32.Parse(nums[i]));
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    boardState[i, j] = state[(i * 3) + j];
                }
            }

            return new BoardState { State = boardState, ZeroPosition = zeroPos };
        }

        public static BoardState BuildFromBoard(BoardState state)
        {
            int[,] samedState = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    samedState[i, j] = state[i, j];
                }
            }
            return new BoardState { State = samedState, ZeroPosition = new Position(state.ZeroPosition.Row, state.ZeroPosition.Col) };
        }

        public void PrintBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(State[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void ShwapTiles(Position otherPos)
        {
            State[ZeroPosition.Row, ZeroPosition.Col] = State[otherPos.Row, otherPos.Col];
            State[otherPos.Row, otherPos.Col] = 0;
            ZeroPosition = new Position(otherPos.Row, otherPos.Col);
        }

        public int this[int i, int j]
        {
            get => State[i, j];
            set => State[i, j] = value;
        }

        public bool Equals(BoardState other)
        {
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

        public bool checkParity(BoardState other)
        {
            List<int> state1 = new List<int>();
            List<int> state2 = new List<int>();
            int inversions1 = 0;
            int inversions2 = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    state1.Add(State[i,j]);
                    state2.Add(other.State[i,j]);
                }
            }

            for (int i = 0; i < 9; i++)
            {
                if (state1[i] != 0)
                {
                    for (int j = i + 1; j < 9; j++)
                    {
                        if (state1[j] != 0 && state1[i] > state1[j])
                        {
                            inversions1++;
                        }
                    }
                }
                
                if (state2[i] != 0)
                {
                    for (int j = i + 1; j < 9; j++)
                    {
                        if (state2[j] != 0 && state2[i] > state2[j])
                        {
                            inversions2++;
                        }
                    }
                }
            }

            if (inversions1 % 2 != inversions2 % 2)
            {
                Console.WriteLine("The intial state cannot reach the goal state. ");
                Console.WriteLine();
            }

            return inversions1 % 2 == inversions2 % 2;
        }

        public List<int> toList()
        {
            List<int> stateList = new List<int>();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    stateList.Add(State[i,j]);
                }
            }

            return stateList;
        }

    }
}
