namespace Tetris{
    // Represents the "J" shaped block in Tetris.
    public class JBlock : Block{
        // Defines the tile positions for the four rotation states of the "J" block.
        // Each array represents a rotation state, and the positions are relative to the block's offset.
        private readonly Position[][] tiles = new Position[][]{
            new Position[] {new(0,0), new(1,0), new(1,1), new(1,2) },
            new Position[] {new(0,1), new(0,2), new(1,1), new(2,1) },
            new Position[] {new(1,0), new(1,1), new(1,2), new(2,2) },
            new Position[] {new(0,1), new(1,1), new(2,1), new(2,0) }
        };

        // Unique identifier for the "J" block type.
        public override int Id=>2;

        // Defines the starting offset for the "J" block when it spawns on the grid.
        //protected override Position StartOffset=> new Position(0,3);
        protected override Position StartOffset=> new (0,3);
        // Provides access to the rotation states of the "J" block.
        //protected override Position[][] Tiles => tiles;
    }
}