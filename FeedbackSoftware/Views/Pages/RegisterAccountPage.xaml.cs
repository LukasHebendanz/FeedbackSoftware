using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FeedbackSoftware.Classes;
using FeedbackSoftware.Classes.Dtos;
using FeedbackSoftware.Classes.Helpers;
using MaterialDesignThemes.Wpf;

namespace FeedbackSoftware.Views
{
    /// <summary>
    /// Interaction logic for RegisterAccountPage.xaml
    /// </summary>
    public partial class RegisterAccountPage : Page
    {
        DatabaseManager db = new DatabaseManager();
        Helper helper = new Helper();

        public RegisterAccountPage()
        {
            InitializeComponent();
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

                        userDto.Passwort = helper.Encrypt(userDto.Passwort);
						db.InsertUser(userDto);
						MessageBox.Show("Registrierung war erfolgreich");
					}
					else
                    {
                        MessageBox.Show("Bitte Eingaben überprüfen!");
                    }
                }
                else
                {
                    MessageBox.Show("Dieser Benutzername existiert bereits!");
                }
            }
            else
            {
                MessageBox.Show("Bitte alle Felder ausfüllen!");
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
