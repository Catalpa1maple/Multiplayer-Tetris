using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using System.Windows.Documents;

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
            new BitmapImage(new Uri("assets/TileRed.png", UriKind.Relative))
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

        // Timing and game delay settings.
        private int maxDelay = 1500;
        private int mindelay = 100;
        private int delayDecrease = 70;
        private bool isMultiplayer = false;
        private static TCPSocket tcp = new TCPSocket();

        // The game's state.
        private GameState gameState = new GameState();

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
        }
        

        // Sets up the game canvas with a grid of Image controls.
        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

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

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r,c].Opacity =1;
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        // Draws the currently active block.
       private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                //if (p.Row >= 0) // Only draw tiles within the visible grid.
                //{
                    imageControls[p.Row,p.Column].Opacity =1;
                    imageControls[p.Row, p.Column].Source = tileImages[block.Id];
                //}
            }
        }


        // Draws the next block in the block queue.
        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        // Draws the held block.
        private void DrawHeldBlock(Block heldBlock)
        {
            HoldImage.Source = heldBlock == null ? blockImages[0] : blockImages[heldBlock.Id];
        }
        // Draws the ghost block indicating where the block would land.
        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();

            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }
        // Combines all drawing operations.
        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock(gameState.HeldBlock);
            ScoreText.Text = $"You now have {gameState.Score} marks.";
        }
         // Main game loop.
        private async Task GameLoop()
        {
            Draw(gameState);
            while (!gameState.GameOver)
            {
                int delay = Math.Max(mindelay, maxDelay - (gameState.Score * delayDecrease));
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                Draw(gameState);
            }
            /*while (!gameState.GameOver)
            {
                int delay = maxDelay;
                int buffer = 0;
                for(int i = 0; i<gameState.Score;i++)
                    buffer += delayDecrease; 
                delay -= buffer;
                if(delay<mindelay) delay=mindelay;
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                Draw(gameState);
            }*/
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"You gained {gameState.Score} marks.";
        }
        
        // Handles user input via keyboard.
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver) return;

            switch (e.Key)
            {
                case Key.Left: gameState.MoveBlockLeft(); break;
                case Key.Right: gameState.MoveBlockRight(); break;
                case Key.Down: gameState.MoveBlockDown(); break;
                case Key.Up: gameState.RotateBlockCW(); break;
                case Key.Z: gameState.RotateBlockCCW(); break;
                case Key.C: gameState.HoldBlock(); break;
                case Key.Space: gameState.DropBlock(); break;
                default: return;
            }

            Draw(gameState);
        }

       

        // Starts the game loop when the canvas is loaded.
        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            
            await GameLoop();
        }

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
                }
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

        private void Quit(object sender, RoutedEventArgs e)
        {
            StartPage.Visibility = Visibility.Visible;
        }

    }
}
