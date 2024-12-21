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
        private void Draw(GameState gameState){
            DrawGrid(gameState.GameGrid);
            DrawBlock(gameState.currentBlock);
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(gameState.GameOver){
                return;
            }
            switch(e.Key){
                case Key.Left:
            }
        }

        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            Draw(gameState);
        }

        private void Host_Connect(object sender, RoutedEventArgs e)
        {
            TCPSocket tcp = new TCPSocket();
            tcp.TCPlisten();
        }

        private void Join_Connect(object sender, RoutedEventArgs e)
        {
            string IP = IPAddressTextBox.Text;
            TCPSocket tcp = new TCPSocket();
            tcp.TCPconnect(IP);
        }
    }
}