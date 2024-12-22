namespace Tetris
{
    public class OBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(0, 0), new(0, 1), new(1, 0), new(1, 1) }
        };

        public override int Id => 4; // Assign a unique ID for the O block.

        protected override Position StartOffset => new Position(0, 4); // Starting offset for the O block.

        protected override Position[][] Tiles => tiles; // Tile definitions for the O block.
    }
}
