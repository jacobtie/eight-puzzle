using System;
using System.Collections.Generic;

namespace assignment1.eightpuzzle
{
    public class BoardNode : IComparable<BoardNode>
    {
        public BoardState State { get; set; }
        public int HValMisplaced { get; set; }
        public int HValManhat { get; set; }
        public int GVal { get; set; }
        public int FVal { get => UseMisplacedHeuristic ? HValMisplaced + GVal : HValManhat + GVal; }
        public bool GoalNode { get => HValMisplaced == 0; }
        public bool UseMisplacedHeuristic { get; set; }
        public BoardNode Parent { get; set; }

        public BoardNode(BoardState state, int distance, bool useMisplacedHeuristic, BoardNode parent)
        {
            State = state;
            GVal = distance;
            UseMisplacedHeuristic = useMisplacedHeuristic;
            Parent = parent;
            _calculateHVals();
        }

        public List<BoardNode> GenerateSuccessors()
        {
            var successors = new List<BoardNode>();

            if (State.ZeroPosition.Row > 0)
            {
                var above = BoardState.BuildFromBoard(State);
                above.ShwapTiles(new Position(above.ZeroPosition.Row - 1, above.ZeroPosition.Col));
                var nextState = new BoardNode(above, GVal + 1, UseMisplacedHeuristic, this);
                successors.Add(nextState);
            }
            if (State.ZeroPosition.Row < 2)
            {
                var below = BoardState.BuildFromBoard(State);
                below.ShwapTiles(new Position(below.ZeroPosition.Row + 1, below.ZeroPosition.Col));
                var nextState = new BoardNode(below, GVal + 1, UseMisplacedHeuristic, this);
                successors.Add(nextState);
            }
            if (State.ZeroPosition.Col > 0)
            {
                var left = BoardState.BuildFromBoard(State);
                left.ShwapTiles(new Position(left.ZeroPosition.Row, left.ZeroPosition.Col - 1));
                var nextState = new BoardNode(left, GVal + 1, UseMisplacedHeuristic, this);
                successors.Add(nextState);
            }
            if (State.ZeroPosition.Col < 2)
            {
                var right = BoardState.BuildFromBoard(State);
                right.ShwapTiles(new Position(right.ZeroPosition.Row, right.ZeroPosition.Col + 1));
                var nextState = new BoardNode(right, GVal + 1, UseMisplacedHeuristic, this);
                successors.Add(nextState);
            }

            return successors;
        }

        private void _calculateHVals()
        {
            // Calculate misplaced heuristic
            int misplacedValue = 0;
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

            HValMisplaced = misplacedValue;

            // Calculate manhattan distance heuristic
            int manhatHeuristic = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (State[i, j] == 0)
                    {
                        continue;
                    }

                    int actualI = AStar.goalState.toList().IndexOf(State[i, j]) / 3;
                    int actualJ = AStar.goalState.toList().IndexOf(State[i, j]) % 3;
                    manhatHeuristic += Math.Abs(i - actualI) + Math.Abs(j - actualJ);
                }
            }

            HValManhat = manhatHeuristic;
        }

        public int CompareTo(BoardNode other)
        {
            return this.FVal == other.FVal ? 0 : this.FVal < other.FVal ? -1 : 1;
        }
    }
}
