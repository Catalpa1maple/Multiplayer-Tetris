using System;
namespace Tetris
{
    public class BlockQueue{
        private Block[] blocks = new Block[]{
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock(),
        };
        private Random random = new Random();
        public Block NextBlock{get;private set;}
        private Block RandomBlock(){
            return blocks[random.Next(blocks.Length)];
        }
        public BlockQueue(){
            NextBlock = RandomBlock();
        }
        public Block GetandUpdate(){
            Block block = NextBlock;
            while(NextBlock.Id == block.Id)
                NextBlock = RandomBlock();
            return block;
        }
    }
}