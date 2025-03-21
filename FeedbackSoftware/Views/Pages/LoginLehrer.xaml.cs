using FeedbackSoftware.Classes;
using FeedbackSoftware.Classes.Dtos;
using FeedbackSoftware.Classes.Helpers;
using FeedbackSoftware.Views.Windows;
using MaterialDesignThemes.Wpf;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace FeedbackSoftware.Views.Pages
{
    /// <summary>
    /// Interaction logic for LoginLehrer.xaml
    /// </summary>
    public partial class LoginLehrer : Page
	{
		DatabaseManager dbm = new DatabaseManager();
		UserDto userDto = new UserDto();
        Helper helper = new Helper();
        string encryptedpassword = "";

		public LoginLehrer()
        {
			InitializeComponent();
		}

        private void Login_Click(object sender, RoutedEventArgs e)
        {
			var userInfo = dbm.SelectUserInfoByUsername(tbxUsername.Text);
			userDto.Rolle = userInfo.Rolle;
			encryptedpassword = helper.Encrypt(tbxPassword.Password);
            if (IsLoginValid())
            {
                Window parentWindow = Window.GetWindow(this);

                // Hide the login window
                parentWindow?.Hide();

                // Open the new window
                TeacherWindow tw = new TeacherWindow(userInfo.Rolle);
                tw.ShowDialog(); // Show the new window modally

                // Now close the login window after TeacherWindow is closed
                parentWindow?.Close();
            }
        }

        private bool IsLoginValid()
		{
            if (dbm != null)
            {
                bool passwordokay = true;
                var userInfo = dbm.SelectUserInfoByUsername(tbxUsername.Text);
				
                string savedPasswordHash = userInfo.Passwort;
				byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
				byte[] salt = new byte[16];
				Array.Copy(hashBytes, 0, salt, 0, 16);
				var pbkdf2 = new Rfc2898DeriveBytes(tbxPassword.Password, salt, 100000);
				byte[] hash = pbkdf2.GetBytes(20);
				for (int i = 0; i < 20; i++)
					if (hashBytes[i + 16] != hash[i])
						passwordokay = false;
				if (passwordokay == true && userInfo.Name == tbxUsername.Text)
                {
					return true;
				}
                else
                {
                    MessageBox.Show("Bitte Eingaben überprüfen!");
                    return false;
                }
			}
            else
            {
                return false;
            }
        }
	}
}