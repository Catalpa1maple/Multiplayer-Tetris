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
            get
            {
                tempValue = grid[r, c];
                return tempValue;
            }
            set
            {
                tempValue = value;
                grid[r, c] = tempValue;
            }
        }

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < columns; c++)
                    grid[r, c] = 0;
        }

        public bool IsEmpty(int r, int c)
        {
            bool inside;
            if (r >= 0 && r < Rows && c >= 0 && c < Columns)
                inside = true;
            else
                inside = false;
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
                rowStatus[c] = IsEmpty(r, c);
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

        public void BeingAttacked(int count)
        {
            Random random = new Random();
            int hole;
            for(int i = 0; i < count; i++)
            {
                hole = random.Next(0,10);
                for(int r = 1; r < Rows; r++)
                {
                    for(int c = 0 ; c < Columns; c++)
                        grid[r - 1, c] = grid[r,c];
                }
                for (int c = 0; c < Columns; c++ ){
                    if (c != hole)
                        grid[Rows - 1,c] = 8;
                    else
                        grid[Rows-1,c] = 0;
                }
            }       
        }

        public int ClearFullRows()
        {
            int cleared = 0;
            int[] rowsToClear = new int[Rows];
            for (int r = Rows - 1; r >= 0; r--)
            {
                bool rowFull = IsRowFull(r);
                if (rowFull)
                {
                    rowsToClear[cleared++] = r;
                }
            }
            for (int i = 0; i < cleared; i++)
            {
                ClearRow(rowsToClear[i]); // Clear each row
            }
            for (int i = cleared - 1; i >= 0; i--)
            {
                for (int r = rowsToClear[i] - 1; r >= 0; r--)
                {
                    MoveRowDown(r, 1); // Move rows above down one by one
                }
            }
            return cleared;
        }
    }
}
