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
			bool restart = false;
			Position zeroPos = new Position(-1, -1);

			do
			{
				state = new List<int>();
				restart = false;
				Console.WriteLine("Generating initial state...");
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
				if (_isImpossible(state))
				{
					restart = true;
					Console.WriteLine("Generated puzzle was impossible, restarting...\n");
				}
			}
			while (restart);

			int[,] boardState = new int[3, 3];

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

		private static bool _isImpossible(List<int> state)
		{
			int inversions = 0;

			for (int i = 0; i < 9; i++)
			{
				if (state[i] == 0)
				{
					continue;
				}
				for (int j = i + 1; j < 9; j++)
				{
					if (state[j] != 0 && state[i] > state[j])
					{
						inversions++;
					}
				}
			}

			return inversions % 2 == 1;
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
	}
}
