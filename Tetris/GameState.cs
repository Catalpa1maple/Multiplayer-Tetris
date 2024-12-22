namespace Tetris
{
    public class GameState
    {
        // Represents the currently active block in the game.
        private Block currentBlock;

        // Public property for accessing and setting the current block. Resets and positions the block when set.
        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();
                for (int i = 0; i < 2; i++) // Attempt to move the block down two rows.
                {
                    currentBlock.Move(1, 0);
                    if (!BlockFits()) // If the block doesn't fit, move it back up.
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }

        // Represents the game grid.
        public Grid GameGrid { get; }

        // Represents the queue of upcoming blocks.
        public BlockQueue BlockQueue { get; }

        // Indicates if the game is over.
        public bool GameOver { get; private set; }

        // The player's score.
        public int Score { get; private set; }

        // Holds a block for later use.
        public Block HeldBlock { get; private set; }

        // Indicates if holding a block is allowed.
        public bool CanHold { get; private set; }

        // Constructor to initialize the game state.
        public GameState()
        {
            GameGrid = new Grid(22, 10); // Create a grid with 22 rows and 10 columns.
            BlockQueue = new BlockQueue(); // Initialize the block queue.
            currentBlock = BlockQueue.GetandUpdate(); // Set the first block.
            CanHold = true; // Allow holding a block initially.
        }

        // Checks if the current block fits in the grid.
        private bool BlockFits()
        {
            foreach (Position p in currentBlock.TilePositions())
            {
                // Allow tiles to spawn above the grid (row < 0).
                if (!GameGrid.isempty(p.Row, p.Column))
                {
                    return false;
                }
            }
            return true;
        }

        // Holds the current block, swapping it with the held block if necessary.
        public void HoldBlock()
        {
            if (!CanHold) return;

            if (HeldBlock == null) // If no block is held, hold the current block.
            {
                HeldBlock = currentBlock;
                currentBlock = BlockQueue.GetandUpdate();
            }
            else // Swap the held block with the current block.
            {
                Block now = currentBlock;
                currentBlock = HeldBlock;
                HeldBlock = now;
            }

            CanHold = false; // Disable holding until the block is placed.
        }

        // Rotates the current block clockwise. Reverts if it doesn't fit.
        public void RotateBlockCW()
        {
            currentBlock.RotateCW();
            if (!BlockFits())
            {
                currentBlock.RotateCCW();
            }
        }

        // Rotates the current block counterclockwise. Reverts if it doesn't fit.
        public void RotateBlockCCW()
        {
            currentBlock.RotateCCW();
            if (!BlockFits())
            {
                currentBlock.RotateCW();
            }
        }

        // Moves the current block to the left. Reverts if it doesn't fit.
        public void MoveBlockLeft()
        {
            currentBlock.Move(0, -1);
            if (!BlockFits())
            {
                currentBlock.Move(0, 1);
            }
        }

        // Moves the current block to the right. Reverts if it doesn't fit.
        public void MoveBlockRight()
        {
            currentBlock.Move(0, 1);
            if (!BlockFits())
            {
                currentBlock.Move(0, -1);
            }
        }

        // Checks if the game is over based on the top rows of the grid.
        private bool IsGameOver()
        {
            return !(GameGrid.isrowempty(0) && GameGrid.isrowempty(1));
        }

        // Places the current block in the grid, clears full rows, and spawns a new block.
        private void PlaceBlock()
        {
            foreach (Position p in currentBlock.TilePositions())
            {
                if (p.Row >= 0) // Only update the grid for rows within bounds.
                GameGrid[p.Row, p.Column] = currentBlock.Id; // Place the block tiles in the grid.
            }

            Score += GameGrid.ClearFullRow(); // Clear full rows and update the score.

            if (IsGameOver()) // Check if the game is over.
            {
                GameOver = true;
            }
            else
            {
                currentBlock = BlockQueue.GetandUpdate(); // Spawn a new block.
                CanHold = true; // Allow holding a block again.
            }
        }

        // Moves the current block down by one row. Places it if it doesn't fit.
        public void MoveBlockDown()
        {
            currentBlock.Move(1, 0);
            if (!BlockFits())
            {
                currentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }

        // Calculates how far a tile can drop before hitting another block or the grid bottom.
        private int TileDropDistance(Position p)
        {
            int drop = 0;
            while (GameGrid.isempty(p.Row + drop + 1, p.Column)) // Increment drop distance until a collision occurs.
            {
                drop++;
            }
            return drop;
        }

        // Calculates how far the entire block can drop.
        public int BlockDropDistance()
        {
            int drop = GameGrid.row;
            foreach (Position p in currentBlock.TilePositions())
            {
                drop = System.Math.Min(drop, TileDropDistance(p)); // Determine the minimum drop distance for all tiles.
            }
            return drop;
        }

        // Drops the current block to its lowest possible position and places it.
        public void DropBlock()
        {
            currentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }
    }
}
namespace Tetris
{
    public class GameState
    {
        private Block currentBlock;

        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();

                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);

                    if (!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }

        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; }
        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }

        public GameState()
        {
            GameGrid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
        }

        private bool BlockFits()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                if (!GameGrid.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }

            return true;
        }

        public void HoldBlock()
        {
            if (!CanHold)
            {
                return;
            }

            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate();
            }
            else
            {
                Block tmp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = tmp;
            }

            CanHold = false;
        }

        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCCW();
            }
        }

        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCW();
            }
        }

        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, 1);
            }
        }

        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);
            }
        }

        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }

            Score += GameGrid.ClearFullRows();

            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
                CanHold = true;
            }
        }

        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);

            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }

        private int TileDropDistance(Position p)
        {
            int drop = 0;

            while (GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }

            return drop;
        }

        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;

            foreach (Position p in CurrentBlock.TilePositions())
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }

            return drop;
        }

        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }
    }
}
