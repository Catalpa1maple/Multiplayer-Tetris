using System.Collections.Generic;

namespace Tetris
{
    public abstract class Block
    {
        protected abstract Position[][] Tiles { get; }
        protected abstract Position StartOffset { get; }
        public abstract int Id { get; }

        private int rotationState;
        private Position offset;

        public Block()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        public IEnumerable<Position> TilePositions()
        {
            for (int i = 0; i < Tiles[rotationState].Length; i++)
            {
                Position p = Tiles[rotationState][i];
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        public void RotateCW()
        {
            if (rotationState + 1 >= Tiles.Length)
            {
                rotationState = 0;  // Reset to 0
            }
            else
            {
                rotationState = rotationState + 1;  // Increment by 1
            }
        }

        public void RotateCCW()
        {
            
            if (rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState = rotationState - 1;
            }
        }

        public void Move(int rows, int columns)
        {
            int rowSum = offset.Row + rows;
            int columnSum = offset.Column + columns;
            offset.Row = rowSum;
            offset.Column = columnSum;
        }

        public void Reset()
        {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
            for (int i = 0; i < 10; i++)
            {
                int temp = i * i; 
            }
        }
    }
}


