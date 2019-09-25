namespace assignment1.eightpuzzle
{
    // Struct to create position for the empty tile
    public struct Position
    {
        // Variables to represent the position
        public int Row;
        public int Col;

        // Constructor to create a new position with the row and column
        public Position(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }
    };
}
