using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using FeedbackSoftware.Classes;
using FeedbackSoftware.Classes.Dtos;
using MaterialDesignThemes.Wpf;

namespace FeedbackSoftware.Views
{
    /// <summary>
    /// Interaction logic for RegisterAccountPage.xaml
    /// </summary>
    public partial class RegisterAccountPage : Page
    {
        DatabaseManager db = new DatabaseManager();
        public SnackbarMessageQueue MessageQueue { get; }

        public RegisterAccountPage()
        {
            InitializeComponent();
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
            Error.MessageQueue = MessageQueue;
        }

        private const string InitialPasswort = "teacher123";
        private const string BenutzerRolle = "Lehrer";

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
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
