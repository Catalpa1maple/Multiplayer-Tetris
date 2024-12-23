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

        public int MultiPlayerLocalUpdate(GameState player1, GameState player2)
        {
            int lineToSendPlayer1 = player1.LinesToSend;
            int lineToSendPlayer2 = player2.LinesToSend;
            if (player1.GameOver && player2.GameOver) return 2;
            else if (player1.GameOver) return 0;
            else if (player2.GameOver) return -1;
            if (lineToSendPlayer1 > lineToSendPlayer2)
            {
                player1.GameGrid.BeingAttacked(0);
                player2.GameGrid.BeingAttacked(lineToSendPlayer1 - lineToSendPlayer2);
                return 1;
            }
            else
            {
                player2.GameGrid.BeingAttacked(0);
                player1.GameGrid.BeingAttacked(lineToSendPlayer2 - lineToSendPlayer1);
                return 1;
            }
        }


        public int MultiplayerUpdate(GameState gameSate, TCPSocket tcp)
        {

            message.score = gameSate.Score;
            message.lineToSend = gameSate.LinesToSend;
            if (gameSate.GameOver) message.lineToSend = -1;
            gameSate.LinesToSend = 0;
            
            
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

            if (gameSate.GameOver && rivalMessage.lineToSend == -1)
            {
                gameSate.GameOver = true;
                return 2; //Game Draw
            }
            if (gameSate.GameOver) 
            { 
                gameSate.GameOver = true;
                return -1; //You Lose
            }
            if (rivalMessage.lineToSend == -1)
            {
                gameSate.GameOver = true;
                return 0; //You Win
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




