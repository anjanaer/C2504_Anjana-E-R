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

namespace _30_Brushes_AndPanels
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSolidColor_Click(object sender, RoutedEventArgs e)
        {
            FormConfig.FrmSolid.Show();
            
        }

        private void btnLinearGradient_Click(object sender, RoutedEventArgs e)
        {
            FormConfig.FrmLinear.Show();
        }

        private void btnRadialGradient_Click(object sender, RoutedEventArgs e)
        {
            FormConfig.FrmRadient.Show();
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            FormConfig.FrmImage.Show();
        }

        private void btnVisual_Click(object sender, RoutedEventArgs e)
        {
            FormConfig.FrmVisual.Show();
        }

        private void btnStack_Click(object sender, RoutedEventArgs e)
        {
            FormConfig.FrmStack.Show();
        }

        private void btnGrid_Click(object sender, RoutedEventArgs e)
        {
            FormConfig.FrmGrid.Show();
        }

        private void btnDock_Click(object sender, RoutedEventArgs e)
        {
            FormConfig.FrmDock.Show();
        }

        private void btnCanvas_Click(object sender, RoutedEventArgs e)
        {
            FormConfig.FrmCanvas.Show();
        }

        private void btnWrap_Click(object sender, RoutedEventArgs e)
        {
            FormConfig.FrmWrap.Show();
        }
    }
}
