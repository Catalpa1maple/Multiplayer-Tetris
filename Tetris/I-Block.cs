namespace Tetris
{
    public class IBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(1,0), new(1,1), new(1,2), new(1,3) },
            new Position[] { new(0,2), new(1,2), new(2,2), new(3,2) },
            new Position[] { new(2,0), new(2,1), new(2,2), new(2,3) },
            new Position[] { new(0,1), new(1,1), new(2,1), new(3,1) }
        };
        private Position[][] tempTiles;
        private Position tempStartOffset;
        public override int Id => 1;
        protected override Position StartOffset {
            get
            {
                tempStartOffset = new Position(-1, 3);
                return tempStartOffset;
            }
        }
        protected override Position[][] Tiles {
            get
            {
                tempTiles = new Position[tiles.Length][];
                for (int i = 0; i < tiles.Length; i++)
                {
                    tempTiles[i] = new Position[tiles[i].Length];
                    for (int j = 0; j < tiles[i].Length; j++)
                    {
                        tempTiles[i][j] = tiles[i][j];
                    }
                }
                return tempTiles;
            }
        }
        public IBlock() 
        {
            tempTiles = new Position[tiles.Length][];
            for (int i = 0; i < tiles.Length; i++)
            {
                tempTiles[i] = new Position[tiles[i].Length];
                for (int j = 0; j < tiles[i].Length; j++)
                {
                    tempTiles[i][j] = tiles[i][j];
                }
            }

            tempStartOffset = new Position(-1, 3);
        }
    }
}
