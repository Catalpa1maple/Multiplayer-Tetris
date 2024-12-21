using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
        private ImageSource[] blockImages = new ImageSource[]{
            new BitmapImage(new Uri("Images/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-Z.png", UriKind.Relative))
        };
        private Image[,] imageControls;
        private GameState gameState = new GameState();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void DrawGrid(Grid grid){
            for(int r= 0;r<grid.row;r++){
                for(int c=0;c<grid.column;c++){
                    int id = grid[r,c];
                    imageControls[r,c].Source = tileImages[id];
                }
            }
        }
        private void DrawBlock (Block block){
            foreach(Position p in block.TilePositions()){
                imageControls[p.Row,p.Column].Source = tileImages[block.Id];
            }
        }
        private void DrawNextBlock (Block blockQueue){
            Block next = blockQueue.NextBlock;
            NextImage.Source= blockImages[next.Id];
        }
        private void DrawHeldBlock(Block heldBlock){
            if(heldBlock == null){
                HoldImage.Source = blockImages[0];
            }
            else{
                HoldImage.Source = blockImages[heldBlock.Id];
            }
        }
        private void Draw(GameState gameState){
            DrawGrid(gameState.GameGrid);
            DrawBlock(gameState.currentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock(gameState.Heldblock);
            ScoreText.Text = $"You now have {gameState.Score} marks.";
        }
        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls=new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for(int c=0; c< grid.Rows; r++)
            {
                for(int c=0; c<grid.Columns;c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize,
                    };
                    //78:+10
                    Canvas.SetTop(imageControl, (r-2)* cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r,c]= imageControl;
                }
            }
            return imageControls;
        }
        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls=new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for(int c=0; c< grid.Rows; r++)
            {
                for(int c=0; c<grid.Columns;c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize,
                    };

                    Canvas.SetTop(imageControl, (r-2)* cellSize);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r,c]= imageControl;
                }
            }
            return imageControls;
        }
        
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(gameState.GameOver){
                return;
            }
            switch(e.Key){
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    gameState.RotateBlockCCW();
                    break;
                case Key.C:
                    gameState.HoldBlock();
                    break;
                default:
                    return;
            }
            Draw(gameState);
        }
        private async Task GameLoop(){
            Draw(gameState);
            while(!gameState.GameOver){
                await Task.Delay(250);
                gameState.MoveBlockDown();
                Draw(gameState);
            }
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"You gained {gameState.Score} marks.";
        }

        
        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e){
            await GameLoop();
        }
        private async void PlayAgain_Click(object sender, RoutedEventArgs e){
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }
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


        private void Join_Connect(object sender, RoutedEventArgs e)
        {
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
    }
}