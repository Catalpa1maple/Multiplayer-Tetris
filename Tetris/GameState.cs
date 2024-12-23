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
                InitializeBlockPostion();}
        }
        private void InitializeBlockPostion(){
            currentBlock.Reset();
            for(int i =0;i<2;i++){
                currentBlock.Move(1,0);
                if(!BlockFits()){
                    currentBlock.Move(-1,0);
                }
            }
        }
        public Tetris.GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; set; }
        public int Score { get; private set; }
        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }

        public int LinesToSend;


        public GameState()
        {
            LinesToSend = 0;
            GameGrid = new Tetris.GameGrid(22, 10);
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
            
                (HeldBlock,CurrentBlock) = (CurrentBlock, HeldBlock);
            CanHold = false;
        }

        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();
            if (BlockFits())
                return;
            else
                CurrentBlock.RotateCCW();
            
        }

        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();

            if (BlockFits())
                return;
            else
                CurrentBlock.RotateCW();
            
        }
        
        public void MoveBlockLeft()
        {
            int moverow = 0;
            int movecolumn= -1;
            CurrentBlock.Move(moverow, movecolumn);
            if (BlockFits())
                return;
            else{
                movecolumn =1;
                CurrentBlock.Move(moverow, movecolumn);}
            
        }

        public void MoveBlockRight()
        {
            int moverow = 0;
            int movecolumn= 1;
            CurrentBlock.Move(moverow, movecolumn);

            if (BlockFits())
                return;
            else{
                movecolumn = -1;
                CurrentBlock.Move(moverow, movecolumn);}
            
        }

        private bool IsGameOver()
        {
            int topRowsSum = GameGrid[0,0] + GameGrid[1,0];
            for(int i =0;i<GameGrid.Columns;i++){
                topRowsSum += GameGrid[0,i] + GameGrid[1,i];
            }
            if(topRowsSum >0)
                return true;
            else
                return false;
        }
        private int CalculateScoreIncrement(int cleared){
            int basepoint = 100;
            //int multiplier = cleared *cleared * basepoint;
            int multiplier = basepoint;
            int tmp=1;
            for(int i = 0; i < cleared; i++)
            {
                tmp = tmp * 2;
            }
            multiplier = basepoint * tmp;
            if(cleared == 0) return 0;
            return multiplier;
        }
        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }

            LinesToSend = GameGrid.ClearFullRows();
            int addition = CalculateScoreIncrement(LinesToSend);
            Score += addition;

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

        /*private void ReceiveAttack(int count)
        {
            GameGrid.BeingAttacked(count);

            if (IsGameOver())
            {
                GameOver = true;
            }
        }*/

        public void MoveBlockDown()
        {
            int moverow = 1;
            int movecolumn= 0;
            CurrentBlock.Move(moverow, movecolumn);

            if (!BlockFits())
            {
                moverow = -1;
                CurrentBlock.Move(moverow, movecolumn);
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
