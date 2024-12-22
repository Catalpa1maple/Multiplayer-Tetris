using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;

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
            new BitmapImage(new Uri("Images/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileRed.png", UriKind.Relative))
        };

        // Array of images for displaying the block queue and held block.
        private ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Images/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-Z.png", UriKind.Relative))
        };

        // 2D array to hold the Image controls for the game grid.
        private Image[,] imageControls;

        // Timing and game delay settings.
        private int maxDelay = 750;
        private int mindelay = 50;
        private int delayDecrease = 70;

        // The game's state.
        private GameState gameState = new GameState();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Draws the current game grid.
        private void DrawGrid(Grid grid)
        {
            for (int r = 0; r < grid.row; r++)
            {
                for (int c = 0; c < grid.column; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        // Draws the currently active block.
        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
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

        // Sets up the game canvas with a grid of Image controls.
        private Image[,] SetupGameCanvas(Grid grid)
        {
            Image[,] imageControls = new Image[grid.row, grid.column];
            int cellSize = 25;

            for (int r = 0; r < grid.row; r++)
            {
                for (int c = 0; c < grid.column; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize,
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }

            return imageControls;
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

        // Main game loop.
        private async Task GameLoop()
        {
            Draw(gameState);
            while (!gameState.GameOver)
            {
                int newspeed = maxDelay - gameState.Score * delayDecrease;
                int delay = Math.Max(mindelay, newspeed);
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                Draw(gameState);
            }
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"You gained {gameState.Score} marks.";
        }

        // Starts the game loop when the canvas is loaded.
        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        // Resets the game and starts a new session when "Play Again" is clicked.
        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }

        // Sets up a host connection for multiplayer.
        private void Host_Connect(object sender, RoutedEventArgs e)
        {
            try
            {
                TCPSocket tcp = new TCPSocket();
                tcp.TCPlisten();
                MessageBox.Show("Hosted");
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
                TCPSocket tcp = new TCPSocket();
                tcp.TCPconnect(IP);
                MessageBox.Show("Joined");
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to join");
            }
        }

        private int IPAddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IPAddressTextBox.Text.Length > 0)
            {
                Connect.IsEnabled = true;
                return 1;
            }
            else
            {
                Accept.IsEnabled = false;
                return 0;
            }
        }
    }
}
