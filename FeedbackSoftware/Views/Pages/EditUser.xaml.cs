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
using FeedbackSoftware.Classes;
using FeedbackSoftware.Classes.Dtos;
using FeedbackSoftware.Classes.Helpers;
using FeedbackSoftware.Views.Windows;
using MaterialDesignThemes.Wpf;
using Mysqlx;

namespace FeedbackSoftware.Views.Pages
{
    /// <summary>
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser : Page
    {
        DatabaseManager db = new DatabaseManager();
        UserDto selectedUser = new UserDto();
        Helper helper = new Helper();

        public EditUser(UserDto selectedUser)
        {
            InitializeComponent();
            this.selectedUser = selectedUser;
            usernameTextBox.Text = selectedUser.Name;
        }

        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
                if (passwordBox.Password == confirmPasswordbox.Password)
                {
                    this.selectedUser.Name = usernameTextBox.Text;
                    //this.selectedUser.Rolle = Benutzer.Rolle;
                    var encPW = helper.Encrypt(passwordBox.Password);
                    this.selectedUser.Passwort = encPW;
                    db.UpdateUser(this.selectedUser);
                    MessageBox.Show("User erfolgreich bearbeitet");
				}
                else
                {
                    MessageBox.Show("Passwörter stimmen nicht überein!");
                }
            
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();

            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();

        }
    }
}

