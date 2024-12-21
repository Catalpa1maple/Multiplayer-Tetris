namespace Tetris
{
    public class Grid{
        private int[,] grid;
        public int row {get;}
        public int column {get;}
        public int this[int r, int c]{
            get {return grid[r, c]; } 
            set {grid[r,c] = value;}
        }
        public Grid(int r, int c){
            row =r;
            column = c;
            grid = new int[row,column];
        }
        public bool isempty(int r, int c){
            if (r>=0 && r < row && c>=0 && c < column)
                if (grid[r,c] == 0)
                    return true;
            return false;
        }
        public bool isrowempty(int r){
            for(int c=0;c<column;c++)
                if(!isempty(r,c))
                    return false;
            return true;
        }
        public bool isfull(int r){
            for(int c=0;c<column;c++){
                if(isempty(r,c))
                    return false;
            }
            return true;
        }
        private void Clear(int r){
            for(int c;c<column;c++)
                grid[r,c]=0;
        }
        private void MoveDown(int r, int rowcount){
            for(int c;c<column;c++){
                grid[r+rowcount,c]= grid[r,c];
                }
        }
        public int ClearFullRow(){
            int clearcount = 0;
            for(int r = row-1;r>=0;r--){
                if(isfull(r)){
                    clearcount++;
                    Clear(r);
                }
                else if(clearcount>0){
                    MoveDown(r,clearcount);
                    Clear(r);
                }
            }
            return clearcount;
        }

    }
}