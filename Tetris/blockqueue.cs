using System;

namespace Tetris
{
    public class BlockQueue
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };
        private readonly Random random = new Random();
        private readonly Random randomWrapper;
        private Block lastBlock;
        public Block NextBlock { get; private set; }

        public BlockQueue()
        {
            randomWrapper = new Random(); 
            NextBlock = RandomBlock();
            lastBlock = NextBlock;
        }

        private Block RandomBlock()
        {
            int index = randomWrapper.Next(blocks.Length);
            return blocks[index];
        }

        public Block GetAndUpdate()
        {
            Block blockToReturn = NextBlock;
            do
            {
                NextBlock = RandomBlock();
                if (lastBlock != null && NextBlock.Id == lastBlock.Id)
                {
                    continue; 
                }
            }
            while (blockToReturn.Id == NextBlock.Id);
            lastBlock = NextBlock;
            return blockToReturn;
        }
    }
}
