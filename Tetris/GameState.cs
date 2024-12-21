namespace Tetris{
    public class GameState{
        private Block currentBlock;

        public Block currentBlock{
            get=> currentBlock;
            private set{
                currentBlock=value;
                currentBlock.Rest();
            }
        }
        public GameGrid GameGrid {get;}
        public BlockQueue BlockQueue{get; }
        public bool GameOver { get; private set;} 

        public GameState(){
            GameGrid= new GameGrid(22,10);
            BlockQueue= new BlockQueue();
            currentBlock=BlockQueue.GetAndUpdate();
        }

        private bool BlockFits(){
            foreach (Position p in currentBlock.TilePositions()){
                if (!GameGrid.IsEmpty(p.Row,p.Column)){
                    return false;
                }
                return true;
            }
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

        private bool IsGameOver(){
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        private void PlaceBlock(){
            foreach(Position p in currentBlock.TilePositions()){
                GameGrid[p.Row, p.Column]=currentBlock.Id;
            }

            GameGrid.ClearFullRows();

            if (IsGameOver()){
                GameOver=true;
            }
            else{
                currentBlock=BlockQueue.GetAndUpdate();
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