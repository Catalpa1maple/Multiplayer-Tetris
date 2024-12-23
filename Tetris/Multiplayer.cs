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

        public void MultiplayerUpdate(GameState gameSate, TCPSocket tcp)
        {
            if (rivalMessage.lineToSend == -1)
            {
                return;
            }
            message.score = gameSate.Score;
            message.lineToSend = gameSate.LinesToSend;
            gameSate.LinesToSend = 0;

            if (gameSate.GameOver)
            {
                gameSate.LinesToSend = -1;
                rivalMessage = tcp.TCPupdate(message);
                return;
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
                return;
            }

            if (message.lineToSend > rivalMessage.lineToSend)
            {
                gameSate.GameGrid.BeingAttacked(0);
                return;
            }
            else
            {
                gameSate.GameGrid.BeingAttacked(rivalMessage.lineToSend - message.lineToSend);
                return;
            }


        }

    }
}




