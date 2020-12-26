using System.Windows;

namespace random_walk.Playground.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            onlyOne.plt.PlotScatter(dataX, dataY);
            onlyOne.Render();
        }
    }
}
