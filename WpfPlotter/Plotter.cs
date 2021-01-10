using ScottPlot;

namespace WpfPlotter
{
    /// <summary>
    /// Windows plotter. Uses ScottPlot. To use, make your project target net5.0-windows,
    /// and add the STAThread attribute to your program.
    /// </summary>
    public class Plotter
    {
        public Plot Plt => _window.OnlyOne.plt;

        private readonly MainWindow _window;

        public Plotter()
        {
            _window = new MainWindow();
        }

        public void Show()
        {
            var app = new System.Windows.Application();
            app.Run(_window);
        }
    }
}
