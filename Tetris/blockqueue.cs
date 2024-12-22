using System;
namespace Tetris
{
    public class BlockQueue
    {
        // Array containing all possible block types.
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock(),
        };

        // Random number generator for selecting blocks randomly.
        private readonly Random random = new Random();

        // Holds the next block to be used in the game.
        public Block NextBlock { get; private set; }
        // Constructor to initialize the queue with a random block as the next block.
        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }
        // Selects a random block from the available block types.
        private Block RandomBlock()
        {
            return blocks[random.Next(blocks.Length)];
        }


        // Returns the current next block and updates to a new random block.
        public Block GetandUpdate()
        {
            Block block = NextBlock; // Store the current next block.
            //Ensure the new block is different from the previous one.
            do
            {
                NextBlock = RandomBlock();
            }while (block.Id == NextBlock.Id);
            return block;
        }
        /*private Block previousBlock;

        public Block GetandUpdate()
        {
            Block block = NextBlock; // Store the current block.
            NextBlock = RandomBlock(); // Pick a new block.

            // Avoid consecutive repetition only.
            while (previousBlock != null && NextBlock.Id == previousBlock.Id)
            {
                NextBlock = RandomBlock();
            }

            previousBlock = block; // Update the previous block.
            return block;
        }*/

    }
}
