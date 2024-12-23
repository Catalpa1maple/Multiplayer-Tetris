using TCP;

namespace Tetris
{
    class Multiplayer
    {
        private Message message = new Message();
        private Message rivalMessage = new Message();
        private int LinesToAdd;

        private Multiplayer()
        {
            LinesToAdd = 0;
            message.score = 0;
            message.lineToSend = 0;
        }

        public int MultiplayerUpdate(GameState gameSate, TCPSocket tcp)
        {
            if (gameSate.LinesToSend != 0)
            {
                message.score = gameSate.Score;
                message.lineToSend = gameSate.LinesToSend;
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
                }

                if (message.lineToSend > rivalMessage.lineToSend)return 0;
                else return LinesToAdd = rivalMessage.lineToSend - message.lineToSend;


            }
            else return 0;
        }

    }
}
