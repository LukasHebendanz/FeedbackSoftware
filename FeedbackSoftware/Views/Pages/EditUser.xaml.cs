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
        public SnackbarMessageQueue MessageQueue { get; }

        public EditUser()
        {
            InitializeComponent();
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
            Error.MessageQueue = MessageQueue;
        }

        private const string InitialPasswort = "teacher123";
        private const string BenutzerRolle = "Lehrer";

        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {

            if (usernameTextBox.Text != String.Empty && passwordBox.Password != String.Empty
                && confirmPasswordbox.Password != String.Empty && initialPasswordbox.Password != String.Empty)
            {

                // Hier muss dann geprüft werden, ob der User schon existiert
                if (!UserExists(usernameTextBox.Text.Trim()))
                {
                    if (passwordBox.Password == confirmPasswordbox.Password && initialPasswordbox.Password == InitialPasswort)
                    {
                        UserDto userDto = new UserDto()
                        {
                            Name = usernameTextBox.Text,
                            Passwort = passwordBox.Password,
                            Rolle = BenutzerRolle
                        };

                        db.InsertUser(userDto); 
                    }
                    else
                    {
                        Error.MessageQueue.Enqueue("Bitte Eingaben überprüfen!");
                    }
                }
                else
                {
                    Error.MessageQueue.Enqueue("Dieser Benutzername existiert bereits!");
                }
            }
            else
            {
                Error.MessageQueue.Enqueue("Bitte alle Felder ausfüllen!");
            }
        }

        private bool UserExists(string username)
        {
            List<UserDto> userInfos = db.SelectAllUsers();
            bool userExists = false;

            foreach (UserDto user in userInfos)
            {
                if (username == user.Name)
                {
                    userExists = true;
                }
            }

            return userExists;
        }
    }
}

