namespace Tetris
{
    public class Position
    {
        private int rowValue;
        private int columnValue;
        public int Row { 
            get{
                int temp = rowValue;
                if (temp < 0)
                {
                    temp = 0; 
                }
                return temp;
            }
            set{
                if (value < 0)
                {
                    rowValue = 0;
                }
                else
                {
                    rowValue = value;
                }
            }
        }
        public int Column { 
            get
            {
                int temp = columnValue;
                if (temp < 0)
                {
                    temp = 0;
                }
                return temp;
            }
        
         set
         {
               
                if (value < 0)
                {
                    columnValue = 0;
                }
                else
                {
                    columnValue = value;
                }
            }
            }

        public Position(int row, int column)
        {
            rowValue = row;
            columnValue = column;
            ValidatePosition();
        }
        private void ValidatePosition()
        {
            if (rowValue < 0)
            {
                rowValue = 0;
            }

            if (columnValue < 0)
            {
                columnValue = 0;
            }
        }
    }
}
