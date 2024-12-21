namespace Tetris
{
    // Represents a position in the grid with row and column coordinates.
    public class Position
    {
        // The row index of the position.
        public int Row { get; set; }

        // The column index of the position.
        public int Column { get; set; }

        // Constructor to initialize the position with specific row and column values.
        public Position(int row, int column)
        {
            Row = row; // Set the row value.
            Column = column; // Set the column value.
        }
    }
}
