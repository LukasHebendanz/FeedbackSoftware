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
                    this.selectedUser.Passwort = passwordBox.Password;
                    //this.selectedUser.Rolle = Benutzer.Rolle;
                    db.UpdateUser(this.selectedUser);
                    MessageBox.Show("User erfolgreich bearbeitet");
				}
                else
                {
                    MessageBox.Show("Passwörter stimmen nicht überein!");
                }
            
            
        }
    }
}

