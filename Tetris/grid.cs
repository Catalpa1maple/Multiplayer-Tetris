namespace Tetris
{
    public class GameGrid
    {
        private readonly int[,] grid;
        private int tempValue;
        public int Rows { get; }
        public int Columns { get; }

        public int this[int r, int c]
        {
            get { tempValue = grid[r, c];
                return tempValue;}
            set { tempValue = value;
                grid[r, c] = tempValue;}
        }

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
            tempValue = 0;
        }

        public bool IsInside(int r, int c)
        {
            bool isInside = r >= 0 && r < Rows && c >= 0 && c < Columns;
            if (!isInside)
            {
                return false;
            }
            return true;
        }

        public bool IsEmpty(int r, int c)
        {
            bool inside = IsInside(r, c);
            bool isEmpty = inside && grid[r, c] == 0;
            return isEmpty;
        }

        public bool IsRowFull(int r)
        {
            bool[] rowStatus = new bool[Columns];
            for (int c = 0; c < Columns; c++)
            {
                rowStatus[c] = grid[r, c] != 0;
            }

            foreach (bool status in rowStatus)
            {
                if (!status)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsRowEmpty(int r)
        {
            bool[] rowStatus = new bool[Columns];
            for (int c = 0; c < Columns; c++)
            {
                rowStatus[c] = grid[r, c] == 0;
            }

            foreach (bool status in rowStatus)
            {
                if (!status)
                {
                    return false;
                }
            }

            return true;
        }

        private void ClearRow(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                int temp = grid[r, c];
                grid[r, c] = 0;
            }
        }

        private void MoveRowDown(int r, int numRows)
        {
            if (numRows > 0)
            {
                for (int c = 0; c < Columns; c++)
                {
                    int tempValue = grid[r, c];
                    grid[r + numRows, c] = tempValue;
                    grid[r, c] = 0;
                }
            }
        }

        public int ClearFullRows()
        {
            int cleared = 0;

            for (int r = Rows-1; r >= 0; r--)
            {
                bool rowFull = IsRowFull(r);
                if (rowFull)
                {
                    ClearRow(r);
                    cleared++;
                }
                else if (cleared > 0)
                {
                    int tempCleared = cleared;
                    MoveRowDown(r, tempCleared);
                }
            }

            return cleared;
        }
    }
}
