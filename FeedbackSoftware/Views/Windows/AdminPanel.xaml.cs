using FeedbackSoftware.Classes;
using FeedbackSoftware.Classes.Dtos;
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

        DatabaseManager db = new DatabaseManager();
        //public List<string> TeacherList { get; set; }
        public List<UserDto> TeacherList { get; set; }
        public AdminPanel()
        {
            InitializeComponent();
            TeacherList = db.SelectAllUsers();
            DataContext = this;
            //foreach (var element in TeacherList)
            //{
            //    UserList.Add(element.Name);
            //}
        }

        private void NavigateToCreateTeacherPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new RegisterAccountPage());
        }
        private void NavigateToKlasse_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new SchoolClassPage());
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is UserDto selectedUser)
            {
                // navigate to edit page and call the edit http request in there
                Console.WriteLine($"Editing: {selectedUser.Name}");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is UserDto selectedUser)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {selectedUser.Name}?",
                                                          "Confirm Delete",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    TeacherList.Remove(selectedUser);
                    // Put Lukas Method to delete the value in here. INstead of the line above
                    TeacherListView.Items.Refresh();
                }
            }
        }
    }
}
