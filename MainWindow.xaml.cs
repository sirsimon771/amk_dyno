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
        public MainWindow()
        {
            InitializeComponent();
            button1.Content = "test12345";
            button1.Height = 50;
            button1.Width = 600;
            Canvas.SetLeft(button1, canvas1.ActualWidth / 2 - button1.ActualWidth / 2);
            Canvas.SetTop(button1, canvas1.ActualHeight / 2 - button1.ActualHeight / 2);
            button1.Content = String.Format("cw: {0} ch: {1} bw: {2} bh: {3}", canvas1.ActualWidth, canvas1.ActualHeight, button1.ActualWidth, button1.ActualHeight);
        }
    }
}
