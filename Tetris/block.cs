using System.Collections.Generic;
namespace Tetris
{
    public abstract class Block
    {
        // Represents the different rotation states of the block as an array of tile positions.
        protected abstract Position[][] Tiles { get; }

        // The starting offset of the block on the grid.
        protected abstract Position StartOffset { get; }

        // Unique identifier for the type of block (e.g., for different Tetris shapes).
        public abstract int Id { get; }

        // Tracks the current rotation state of the block.
        private int rotationState;

        // Tracks the current position offset of the block on the grid.
        private Position offset;

        // Constructor to initialize the block's starting position and rotation state.
        public Block()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        // Enumerates the current positions of all the tiles of the block based on its rotation state and offset.
        public IEnumerable<Position> TilePositions()
        {
            foreach (Position p in Tiles[rotationState]) // Iterate through all tiles in the current rotation state.
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        // Rotates the block clockwise by changing its rotation state.
        public void RotateCW()
        {
            rotationState = (rotationState + 1) % Tiles.Length; // Increment rotation state and wrap around if necessary.
        }

        // Rotates the block counterclockwise by changing its rotation state.
        public void RotateCCW()
        {
            if (rotationState == 0) // If at the first rotation state, wrap around to the last state.
            {
                rotationState = Tiles.Length - 1;
            }
            else // Otherwise, decrement the rotation state.
            {
                rotationState--;
            }
        }

        // Moves the block by a specified number of rows and columns.
        public void Move(int rows, int columns)
        {
            offset.Row += rows; // Adjust the row offset.
            offset.Column += columns; // Adjust the column offset.
        }

        // Resets the block to its initial position and rotation state.
        public void Reset()
        {
            rotationState = 0; // Reset rotation state to the default.
            offset.Row = StartOffset.Row; // Reset row offset to the starting position.
            offset.Column = StartOffset.Column; // Reset column offset to the starting position.
        }
    }
}
