using FeedbackSoftware.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FeedbackSoftware
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Startseite festlegen
            MainFrame.NavigationService.Navigate(new RegisterAccountPage());
        }

        private void NavigateToRegisterAccountPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new RegisterAccountPage());
        }

        private void NavigateToLoginLehrer_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new LoginLehrer());
        }

        private void NavigateToLoginSchuelerPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new LoginSchueler());
        }
    }
}
