namespace Tetris{
    public class GameState{
        private Block currentBlock;

        public Block currentBlock{
            get=> currentBlock;
            private set{
                currentBlock=value;
                currentBlock.Reset();
                for(int i = 0; i<2;i++){
                    currentBlock.Move(1,0);
                    if(!BlockFits()){
                        currentBlock.Move(-1,0);
                    }
                }
            }
        }
        public GameGrid GameGrid {get;}
        public BlockQueue BlockQueue{get; }
        public bool GameOver { get; private set;} 
        public int Score{ get; private set; }
        public Block Heldblock { get; private set; }
        public bool canHold {get; private set; }
        public GameState(){
            GameGrid= new GameGrid(22,10);
            BlockQueue= new BlockQueue();
            currentBlock=BlockQueue.GetAndUpdate();
            canHold = true;
        }

        private bool BlockFits(){
            foreach (Position p in currentBlock.TilePositions()){
                if (!GameGrid.IsEmpty(p.Row,p.Column)){
                    return false;
                }
                return true;
            }
        }
        public void HoldBlock(){
            if(!canHold){
                return;
            }
            if(Heldblock == null){
                Heldblock = currentBlock;
                currentBlock = BlockQueue.GetandUpdate();
            }
            else{
                Block now = currentBlock;
                currentBlock = Heldblock;
                Heldblock=now;
            }
            canHold = false;
        }
        public void RotateBlockCW(){
            currentBlock.RotateBlockCW();

            if (!BlockFits()){
                currentBlock.RotateCCW();
            }
        }

        public void RotateBlockCCW(){
            currentBlock.RotateBlockCCW();

            if (!BlockFits()){
                currentBlock.RotateCW();
            }
        }

        public void MoveBlockLeft(){
            currentBlock.Move(0,-1);

            if(!BlockFits()){
                currentBlock.Move(0,1);
            }
        }
        public void MoveBlockRight(){
            currentBlock.Move(0,1);

            if(!BlockFits()){
                currentBlock.Move(0,-1);
            }
        }
        private bool IsGameOver(){
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        private void PlaceBlock(){
            foreach(Position p in currentBlock.TilePositions()){
                GameGrid[p.Row, p.Column]=currentBlock.Id;
            }
            Score += GameGrid.ClearFullRows();
            GameGrid.ClearFullRows();

            if (IsGameOver()){
                GameOver=true;
            }
            else{
                currentBlock=BlockQueue.GetAndUpdate();
                canHold = true;
            }
        }

        public void MoveBlockDown(){
            currentBlock.Move(1,0);
            if(!BlockFits()){
                currentBlock.Move(-1,0);
                PlaceBlock();
            }
        }


    }
}