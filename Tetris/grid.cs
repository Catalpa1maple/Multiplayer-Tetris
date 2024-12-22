namespace Tetris
{
    public class Grid
    {
        // 2D array representing the grid of the Tetris game.
        private int[,] grid;

        // Number of rows in the grid.
        public int row { get; }

        // Number of columns in the grid.
        public int column { get; }

        // Indexer to get or set the value at a specific row and column in the grid.
        public int this[int r, int c]
        {
            get { return grid[r, c]; }
            set { grid[r, c] = value; }
        }

        // Constructor to initialize the grid with a specific number of rows and columns.
        public Grid(int r, int c)
        {
            row = r;
            column = c;
            grid = new int[row, column];
        }

        // Checks if a specific cell in the grid is empty (contains 0).
        public bool isempty(int r, int c)
        {
            if (r >= 0 && r < row && c >= 0 && c < column) // Ensure indices are within bounds.
                if (grid[r, c] == 0)
                    return true;
            return false;
        }

        // Checks if an entire row is empty (all cells in the row contain 0).
        public bool isrowempty(int r)
        {
            for (int c = 0; c < column; c++)
                if (!isempty(r, c)) // If any cell in the row is not empty, return false.
                    return false;
            return true;
        }

        // Checks if an entire row is full (no cells in the row contain 0).
        public bool isfull(int r)
        {
            for (int c = 0; c < column; c++)
            {
                if (isempty(r, c)) // If any cell in the row is empty, return false.
                    return false;
            }
            return true;
        }

        // Clears all cells in a specific row by setting them to 0.
        private void Clear(int r)
        {
            for (int c=0; c < column; c++) // Loops through each column in the row.
                grid[r, c] = 0;
        }

        // Moves the contents of a row down by a specified number of rows.
        private void MoveDown(int r, int rowcount)
        {
            for (int c=0; c < column; c++) // Loops through each column in the row.
            {
                grid[r + rowcount, c] = grid[r, c];
            }
        }

        // Clears all full rows in the grid, moves rows above down as needed,
        // and returns the number of rows cleared.
        public int ClearFullRow()
        {
            int clearcount = 0; // Counter for the number of cleared rows.
            for (int r = row - 1; r >= 0; r--) // Start from the bottom row and move upward.
            {
                if (isfull(r)) // Check if the row is full.
                {
                    clearcount++;
                    Clear(r); // Clear the full row.
                }
                else if (clearcount > 0) // If rows have been cleared above, move current row down.
                {
                    MoveDown(r, clearcount);
                    Clear(r); // Clear the moved-down row.
                }
            }
            return clearcount;
        }
    }
}
