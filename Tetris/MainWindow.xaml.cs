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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
        
        }

        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}