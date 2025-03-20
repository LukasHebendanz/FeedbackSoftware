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
using System.Windows.Navigation;
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
        public List<KlasseDto> ClassList { get; set; }
        public AdminPanel()
        {
            InitializeComponent();
            TeacherList = db.SelectAllUsers();
            ClassList = db.GetKlassenNames();
            DataContext = this;            
        }

        private void NavigateToCreateTeacherPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new RegisterAccountPage());
            this.Content = new RegisterAccountPage();
        }
        private void NavigateToKlasse_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.NavigationService.Navigate(new SchoolClassPage());
            this.Content = new SchoolClassPage();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is UserDto selectedUser)
            {
                // navigate to edit page and call the edit http request in there
                //MainFrame.NavigationService.Navigate(new EditUser(selectedUser));
                this.Content = new EditUser(selectedUser);
                Console.WriteLine($"Editing: {selectedUser.Name}");
            }
        }

        private void EditClassButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is KlasseDto selectedClass)
            {
                // navigate to edit page and call the edit http request in there
                //MainFrame.NavigationService.Navigate(new EditClass(selectedClass));

                this.Content = new EditClass(selectedClass);
                Console.WriteLine($"Editing: {selectedClass.Name}");
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
                    db.DeleteUser(selectedUser.UserID);
                    MessageBox.Show("User erfolgreich gelöscht");
                    TeacherListView.Items.Refresh();
                }
            }
        }

        private void DeleteClassButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is KlasseDto selectedClass)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {selectedClass.Name}?",
                                                          "Confirm Delete",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {

                    db.DeleteKlasse(selectedClass.KlasseId);
					MessageBox.Show("Klasse erfolgreich gelöscht");
					TeacherListView.Items.Refresh();
                }
            }
        }
    }
}
