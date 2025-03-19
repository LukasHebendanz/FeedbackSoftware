using FeedbackSoftware.Classes;
using FeedbackSoftware.Classes.Dtos;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FeedbackSoftware.Views.Pages
{
    /// <summary>
    /// Interaction logic for LoginLehrer.xaml
    /// </summary>
    public partial class LoginLehrer : Page
	{
		public SnackbarMessageQueue MessageQueue { get; }

		public LoginLehrer()
        {
			InitializeComponent();
			MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
			Error.MessageQueue = MessageQueue;
		}

        DatabaseManager dbm = new DatabaseManager();
        UserDto userDto = new UserDto();
        private string Rolle { get; set; }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (IsLoginValid())
            {
                TeacherWindow tw = new TeacherWindow(this.Rolle);
                tw.ShowDialog();
            }
        }

        private bool IsLoginValid()
		{
            if (dbm != null)
            {
				userDto = dbm.SelectUserByPasswortAndUsername(tbxPassword.Password, tbxUsername.Text);

                //Speichern der nötigen Information für Konstruktorübergabe
                this.Rolle = userDto.Rolle;

                if (userDto.Passwort == tbxPassword.Password && userDto.Name == tbxUsername.Text)
                {
					return true;
				}
                else
                {
                    Error.MessageQueue.Enqueue("Bitte Eingaben überprüfen!");
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