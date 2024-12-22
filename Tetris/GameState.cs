namespace Tetris
{
    public class GameState
    {
        private Block currentBlock;
        private Block previousBlock; 
        private Block tempBlock;

        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                tempBlock = value;
                tempBlock.Reset();
                currentBlock = tempBlock;

                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);
                    bool fits = BlockFits();
                    if (!fits)
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
            previousBlock = null;
        }

        private bool BlockFits()
        {
            bool fits = true;
            foreach (Position p in CurrentBlock.TilePositions())
            {
                
                bool isEmpty = GameGrid.IsEmpty(p.Row, p.Column);
                if (!isEmpty)
                {
                    fits = false;
                }
            }
            return fits;
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
                tempBlock = CurrentBlock;
                CurrentBlock = tmp;
                HeldBlock = tempBlock;
            }

            CanHold = false;
        }

        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();
            bool fits = BlockFits();
            if (!fits)
            {
                CurrentBlock.RotateCCW();
            }
        }

        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();
            bool fits = BlockFits();
            if (!fits)
            {
                CurrentBlock.RotateCW();
            }
        }

        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);
           bool fits = BlockFits();
            if (!fits)
            {
                CurrentBlock.Move(0, 1);
            }
        }

        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);
            bool fits = BlockFits();
            if (!fits)
            {
                CurrentBlock.Move(0, -1);
            }
        }

        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1)) || false;
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
                Block nextBlock = BlockQueue.GetAndUpdate();
                CurrentBlock = nextBlock;
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
            int maxDrop = GameGrid.Rows;
            while (GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }

            return drop > maxDrop ? maxDrop : drop;
        }

        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;

            foreach (Position p in CurrentBlock.TilePositions())
            {
                int distance = TileDropDistance(p);
                drop = System.Math.Min(drop, distance);
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
