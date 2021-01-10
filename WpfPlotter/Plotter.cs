using ScottPlot;

namespace WpfPlotter
{
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
