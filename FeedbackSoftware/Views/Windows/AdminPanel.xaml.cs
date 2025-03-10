using FeedbackSoftware.Views.Pages;
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
using System.Windows.Shapes;

namespace FeedbackSoftware.Views.Windows
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();
            MainFrame.NavigationService.Navigate(new FeedbackKeyWindow());
        }

        private void NavigateToFeedbackPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new FeedbackKeyWindow());
        }
        private void NavigateToKlasse_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new SchoolClassPage());
        }
    }
}
