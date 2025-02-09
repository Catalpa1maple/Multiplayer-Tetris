﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Documents;
using TCP;
using System;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Array of images for grid tiles representing different block types.
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileRed.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileGray.png", UriKind.Relative))
        };

        // Array of images for displaying the block queue and held block.
        private ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-Z.png", UriKind.Relative))
        };

        // 2D array to hold the Image controls for the game grid.
        private Image[,] imageControls;
        private Image[,] imageControlsPlayer2;
        // Timing and game delay settings.
        private int maxDelay = 1000;
        private int mindelay = 100;
        private int delayDecrease = 5;
        private bool isMultiplayer = false;
        private bool isPlayer2 = false;
        private int isWin;
        private static TCPSocket tcp = new TCPSocket();
        
        private Multiplayer multiplayer = new Multiplayer();

        // The game's state.
        private GameState gameState;
        //gameStateplayer2
        private GameState gameStatePlayer2;

        public MainWindow()
        {
            InitializeComponent();
            gameState = new GameState();
            gameStatePlayer2 = new GameState();
            imageControls = SetupGameCanvas(gameState.GameGrid, 1);
            imageControlsPlayer2 = SetupGameCanvas(gameStatePlayer2.GameGrid,2);
        }
        

        // Sets up the game canvas with a grid of Image controls.
        private Image[,] SetupGameCanvas(GameGrid grid, int player)
        {
            int cellSize = 25;
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            if(player==2){
                for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas2.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }return imageControls;
            }
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }

            return imageControls;
        }

        private void DrawGrid(GameGrid grid,int player)
        {
            if(player ==2){
                for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    imageControlsPlayer2[r,c].Opacity =1;
                    imageControlsPlayer2[r, c].Source = tileImages[grid[r,c]];
                }
            } return;
            }

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    imageControls[r,c].Opacity =1;
                    imageControls[r, c].Source = tileImages[grid[r,c]];
                }
            }
        }

        // Draws the currently active block.
       private void DrawBlock(Block block, int player)
        {
            if(player==2){
                foreach (Position p in block.TilePositions())
            {
                if (p.Row >= 0) // Only draw tiles within the visible grid.
                {
                    imageControlsPlayer2[p.Row,p.Column].Opacity =1;
                    imageControlsPlayer2[p.Row, p.Column].Source = tileImages[block.Id];
                } 
            }
                return;
            }
            foreach (Position p in block.TilePositions())
            {
                if (p.Row >= 0) // Only draw tiles within the visible grid.
                {
                    imageControls[p.Row,p.Column].Opacity =1;
                    imageControls[p.Row, p.Column].Source = tileImages[block.Id];
                }
            }
        }


        // Draws the next block in the block queue.
        private void DrawNextBlock(BlockQueue blockQueue, int player)
        {
            Block next = blockQueue.NextBlock;
            if( player ==2){
                NextImage2.Source = blockImages[next.Id];
                return;
            }
            NextImage.Source = blockImages[next.Id];
            
        }

        // Draws the held block.
        private void DrawHeldBlock(Block heldBlock, int player)
        {
            if(player==2){
                HoldImage2.Source = heldBlock == null ? blockImages[0] : blockImages[heldBlock.Id];
                return;}
            HoldImage.Source = heldBlock == null ? blockImages[0] : blockImages[heldBlock.Id];
        }
        // Draws the ghost block indicating where the block would land.
        private void DrawGhostBlock(Block block, int player)
        {
            if(player ==2){
                int dropDistance2 = gameStatePlayer2.BlockDropDistance();

            foreach (Position p in block.TilePositions())
            {
                imageControlsPlayer2[p.Row + dropDistance2, p.Column].Opacity = 0.25;
                imageControlsPlayer2[p.Row + dropDistance2, p.Column].Source = tileImages[block.Id];
            } return;
            }
            int dropDistance = gameState.BlockDropDistance();

            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }
        // Combines all drawing operations.
        private void Draw(GameState gameState,int player)
        {
            DrawGrid(gameState.GameGrid,player);
            DrawGhostBlock(gameState.CurrentBlock,player);
            DrawBlock(gameState.CurrentBlock,player);
            DrawNextBlock(gameState.BlockQueue,player);
            DrawHeldBlock(gameState.HeldBlock,player);
            if (player == 1)
            {
                ScoreText.Text = $"You now have {gameState.Score} marks.";
            }
            if (player == 2)
            {
                ScoreText2.Text = $"You now have {gameState.Score} marks.";
            }
        }
        private int CalculateDelay(int score)
        {
            int delay = maxDelay;
            int scoreAdjustment = 0;
            int scoped = score/100;
            for(int i=0;i<scoped;i++)
              scoreAdjustment += delayDecrease;
            delay -= scoreAdjustment;

            return Math.Max(mindelay, delay);
        }

        private async Task GameLoopLocal()
        {
            Draw(gameState,1);
            Draw(gameStatePlayer2,2);
            while(!(gameState.GameOver||gameStatePlayer2.GameOver))
            {
                int delay = CalculateDelay(Math.Max(gameState.Score,gameStatePlayer2.Score));
                gameState.MoveBlockDown();
                gameStatePlayer2.MoveBlockDown();
                //do some multiplayer update
                isWin = multiplayer.MultiPlayerLocalUpdate(gameState,gameStatePlayer2);
                await Task.Delay(delay); //delay constant time/ do not depend on score
                Draw(gameState,1);
                Draw(gameStatePlayer2,2);
                
            }
            GameOverMenu.Visibility = Visibility.Visible;
            switch (isWin)
            {
                case 0:
                    FinalScoreText.Text = $"You Win! You gained {gameState.Score} marks. " +
                        $"You Will Go back to the StartPage Automatically";
                    break;
                case -1:
                    FinalScoreText.Text = $"You Lose! You gained {gameState.Score} marks. " +
                        $"You Will Go back to the StartPage Automatically";
                    break;
                case 2:
                    FinalScoreText.Text = $"Draw! You gained {gameState.Score} marks. " +
                        $"You Will Go back to the StartPage Automatically";
                    break;
            }
            PlagAgain.Visibility = Visibility.Hidden;
            await Task.Delay(5000);
            Quit();
        }

        // Main game loop.
        private async Task GameLoop()
        {
            Draw(gameState,1);
            while (!gameState.GameOver)
            {
                int delay = CalculateDelay(gameState.Score);
                gameState.MoveBlockDown();
                if (isMultiplayer)
                {
                    try
                    {
                        isWin = multiplayer.MultiplayerUpdate(gameState, tcp);
                        if (isWin != 1) continue;
                        DrawRivals(multiplayer);
                    }
                    catch (ConnectionClosedException)
                    {
                        MessageBox.Show("Rival quit the Game, You Win");
                        Quit();
                    }
                    catch
                    {
                        MessageBox.Show("Connection error, You Lose");
                        Quit();
                    }
                }
                await Task.Delay(delay);
                Draw(gameState,1);
            }

            if (isMultiplayer)
            {
                GameOverMenu.Visibility = Visibility.Visible;
                switch (isWin)
                {
                    case 0:
                        FinalScoreText.Text = $"You Win! You gained {gameState.Score} marks. " +
                            $"You Will Go back to the StartPage Automatically";
                        break;
                    case -1:
                        FinalScoreText.Text = $"You Lose! You gained {gameState.Score} marks. " +
                            $"You Will Go back to the StartPage Automatically";
                        break;
                    case 2:
                        FinalScoreText.Text = $"Draw! You gained {gameState.Score} marks. " +
                            $"You Will Go back to the StartPage Automatically";
                        break;
                }
                PlagAgain.Visibility = Visibility.Hidden;
                await Task.Delay(5000);
                Quit();
            }
            else
            {
                GameOverMenu.Visibility = Visibility.Visible;
                FinalScoreText.Text = $"You gained {gameState.Score} marks.";
            }
        }
        

        // Handles user input via keyboard.
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (isPlayer2){
                if(gameState.GameOver||gameStatePlayer2.GameOver) return;
                switch (e.Key){
                    case Key.A: gameState.MoveBlockLeft(); Draw(gameState,1);break;
                    case Key.D: gameState.MoveBlockRight(); Draw(gameState,1);break;
                    case Key.S: gameState.MoveBlockDown(); Draw(gameState,1);break;
                    case Key.Q: gameState.RotateBlockCW(); Draw(gameState,1);break;
                    case Key.E: gameState.RotateBlockCCW(); Draw(gameState,1);break;
                    case Key.W: gameState.HoldBlock(); Draw(gameState,1);break;
                    case Key.Space: gameState.DropBlock(); Draw(gameState,1);break;
                    case Key.Left: gameStatePlayer2.MoveBlockLeft(); Draw(gameStatePlayer2,2);break;
                    case Key.Right: gameStatePlayer2.MoveBlockRight(); Draw(gameStatePlayer2,2);break;
                    case Key.Down: gameStatePlayer2.MoveBlockDown(); Draw(gameStatePlayer2,2);break;
                    case Key.P: gameStatePlayer2.RotateBlockCW(); Draw(gameStatePlayer2,2);break;
                    case Key.I: gameStatePlayer2.RotateBlockCCW(); Draw(gameStatePlayer2,2);break;
                    case Key.K: gameStatePlayer2.HoldBlock(); Draw(gameStatePlayer2,2);break;
                    case Key.M: gameStatePlayer2.DropBlock(); Draw(gameStatePlayer2,2);break;
                    default: return;
                }
                
                return;
            }
            if (gameState.GameOver) return;

            switch (e.Key)
            {
                case Key.Left: gameState.MoveBlockLeft(); break;
                case Key.Right: gameState.MoveBlockRight(); break;
                case Key.Down: gameState.MoveBlockDown(); break;
                case Key.C: gameState.RotateBlockCW(); break;
                case Key.Z: gameState.RotateBlockCCW(); break;
                case Key.X: gameState.HoldBlock(); break;
                case Key.Space: gameState.DropBlock(); break;
                default: return;
            }

            Draw(gameState,1);
        }

       
        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e){}
        // Resets the game and starts a new session when "Play Again" is clicked.
        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            if (isMultiplayer)
            {
                if (tcp.TCPConnected())
                {
                    gameState = new GameState();
                    StartPage.Visibility = Visibility.Hidden;
                    GameOverMenu.Visibility = Visibility.Hidden;
                    
                    await GameLoop();
                }
                else
                {
                    MessageBox.Show("You don't have multiplayer connection now (Switch to single player)");
                    isMultiplayer = false;
                    tcp.TCPClose();
                }
            }
            else if (isPlayer2)
            {
                gameState = new GameState();
                gameStatePlayer2 = new GameState();
                StartPage.Visibility = Visibility.Hidden;
                GameOverMenu.Visibility = Visibility.Hidden;
                await GameLoopLocal();
            }
            else 
            {
                gameState = new GameState();
                StartPage.Visibility = Visibility.Hidden;
                GameOverMenu.Visibility = Visibility.Hidden;
                await GameLoop();
            }
            
        }
 
        // Sets up a host connection for multiplayer.
        private void Host_Connect(object sender, RoutedEventArgs e)
        {
            try
            {
                tcp.TCPlisten();
                MessageBox.Show("Hosted");
                Connect.IsEnabled = false;
                Accept.IsEnabled = false;
                isMultiplayer = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to host");
            }
        }

        // Connects to a host for multiplayer.
        private void Join_Connect(object sender, RoutedEventArgs e)
        {
            if(IPAddressTextBox.Text.Length == 0)
            {
                MessageBox.Show("Please enter an IP address");
                return;
            }
            try
            {
                string IP = IPAddressTextBox.Text;
                tcp.TCPconnect(IP);
                MessageBox.Show("Joined");
                Connect.IsEnabled = false;
                Accept.IsEnabled = false;
                isMultiplayer = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to join");
            }
        }

        private void IPAddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Connect != null)
            {
                if (IPAddressTextBox.Text.Length > 0)
                {

                    Connect.IsEnabled = true;
                    Connect.Cursor = Cursors.Hand;

                }
                else
                {
                    Connect.IsEnabled = false;
                    Connect.Cursor = Cursors.No;

                }
            }
        }

        private void JoinPlayer(object sender, RoutedEventArgs e)
        {
            if (!isPlayer2)
            {
                isPlayer2 = true;
                GameCanvas2.Visibility = Visibility.Visible;
                HoldImage2.Visibility = Visibility.Visible;
                NextImage2.Visibility = Visibility.Visible;
                ScoreText2.Visibility = Visibility.Visible;
                MessageBox.Show("Player 2 Join");
                return;
            }
            else
            {
                isPlayer2 = false;
                GameCanvas2.Visibility = Visibility.Collapsed;
                HoldImage2.Visibility = Visibility.Collapsed;
                NextImage2.Visibility = Visibility.Collapsed;
                ScoreText2.Visibility = Visibility.Collapsed;
                MessageBox.Show("Player 2 disabled");
                return;
            }
        }

        private void Instruct(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("When single and remote mode:\n left_arrow, right_arrow, down_arrow, C=Clockwise, Z=CounterClockwise,  X=Hold,Space=Drop\n\n" +
                            "When 2  players mode(loacl):\n P1: A:Left,  D:Right,  S:Down,  Q:Clockwise,  E:CounterClocker,  W:Hold,  Space:Drop\n"
                            + "P2: Left:Left,  Right:Right,  Down:Down,  P:Clockwise,  I:CounterClocker,  K:Hold M:Drop");
        }

        private void DrawRivals(Multiplayer multiplayer) {
            //if (!isMultiplayer) {
            //    Rival.Visibility = Visibility.Hidden;
            //    return;
            //}
            Rival.Visibility = Visibility.Visible;
            RivalScore.Text = $"Rival's score: {multiplayer.rivalMessage.score}";
            RivalLines.Text = $"Lines For You: {multiplayer.rivalMessage.lineToSend}";
        }

        private void QuitForBtn(object sender, RoutedEventArgs e)
        {
            StartPage.Visibility = Visibility.Visible;
            gameState.GameOver = true;
            multiplayer.MultiplayerUpdate(gameState, tcp);
            Thread.Sleep(1000);
            if (isMultiplayer)
            {
                tcp.TCPClose();
                Connect.IsEnabled = true;
                Accept.IsEnabled = true;
            }
        }

        public void Quit() 
        {
            StartPage.Visibility = Visibility.Visible;
            Thread.Sleep(1000);
            if (isMultiplayer)
            {
                tcp.TCPClose();
                Connect.IsEnabled = true;
                Accept.IsEnabled = true;
            }
        }

    }
}
