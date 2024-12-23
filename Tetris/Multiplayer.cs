using TCP;

namespace Tetris
{
    public class Multiplayer
    {
        public Message message = new Message();
        public Message rivalMessage = new Message();
        public int LinesToAdd;

        public Multiplayer()
        {
            LinesToAdd = 0;
            message.score = 0;
            message.lineToSend = 0;
        }

        public int MultiplayerUpdate(GameState gameSate, TCPSocket tcp)
        {

            message.score = gameSate.Score;
            message.lineToSend = gameSate.LinesToSend;
            gameSate.LinesToSend = 0;

            if (gameSate.GameOver)
            {
                message.lineToSend = -1;
                rivalMessage = tcp.TCPupdate(message);
                return -1;
            }

            try
            {
                rivalMessage = tcp.TCPupdate(message);
                Console.WriteLine("Rival Score: " + rivalMessage.score);
                Console.WriteLine("Rival Lines: " + rivalMessage.lineToSend);
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot Send to rival");
                tcp.TCPClose();
                throw;
            }

            if (rivalMessage.lineToSend == -1)
            {
                gameSate.GameOver = true;
                return 0;
            }

            if (message.lineToSend > rivalMessage.lineToSend)
            {
                gameSate.GameGrid.BeingAttacked(0);
                return 1;
            }
            else
            {
                gameSate.GameGrid.BeingAttacked(rivalMessage.lineToSend - message.lineToSend);
                return 1;
            }


        }

    }
}




