using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace amk_dyno
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool toggle = false;

        public MainWindow()
        {
            InitializeComponent();
            startstop.Content = "test12345";
            startstop.Height = 50;
            startstop.Width = 600;
            startstop.ToolTip = "start-stop button";
            Canvas.SetLeft(startstop, canvas1.ActualWidth / 2 - startstop.ActualWidth / 2);
            Canvas.SetTop(startstop, canvas1.ActualHeight / 2 - startstop.ActualHeight / 2);
            startstop.Content = String.Format("cw: {0} ch: {1} bw: {2} bh: {3}", canvas1.ActualWidth, canvas1.ActualHeight, startstop.ActualWidth, startstop.ActualHeight);
        }

        private void StartStop(object sender, RoutedEventArgs e)
        {
            //start stop button press
            toggle = !toggle;

            if (toggle)
            {
                //turn on
                startstop.Background = new SolidColorBrush(Colors.Green);
            }
            else
            {
                //turn off
                startstop.Background = new SolidColorBrush(Colors.Red);
            }

        }
    }
}
