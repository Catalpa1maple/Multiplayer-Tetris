namespace Tetris
{
    // Represents the "I" shaped block in Tetris.
    public class IBlock : Block
    {
        // Defines the tile positions for the four rotation states of the "I" block.
        // Each array represents a rotation state, and the positions are relative to the block's offset.
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(1, 0), new(1, 1), new(1, 2), new(1, 3) }, // Horizontal
            new Position[] { new(0, 2), new(1, 2), new(2, 2), new(3, 2) }, // Vertical
            new Position[] { new(2, 0), new(2, 1), new(2, 2), new(2, 3) }, // Horizontal (flipped)
            new Position[] { new(0, 1), new(1, 1), new(2, 1), new(3, 1) }  // Vertical (flipped)
        };

        // Unique identifier for the "I" block type.
        public override int Id => 1;

        // Defines the starting offset for the "I" block when it spawns on the grid.
        protected override Position StartOffset => new Position(-1, 3);

        // Provides access to the rotation states of the "I" block.
        protected override Position[][] Tiles => tiles;
    }
}
