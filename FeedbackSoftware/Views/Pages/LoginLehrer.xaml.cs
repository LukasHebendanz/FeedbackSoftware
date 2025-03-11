using FeedbackSoftware.Classes;
using FeedbackSoftware.Classes.Dtos;
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
        public LoginLehrer()
        {
			InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (IsLoginValid())
            {
                TeacherWindow tw = new TeacherWindow(userDto.Rolle);
                tw.ShowDialog();
            }
        }

        private bool IsLoginValid()
		{
            if (dbm != null)
            {
				userDto = dbm.SelectUserByPasswortAndUsername(tbxPassword.Password, tbxUsername.Text);
				return true;
			}
            else
            {
                return false;
            }
        }
    }
}