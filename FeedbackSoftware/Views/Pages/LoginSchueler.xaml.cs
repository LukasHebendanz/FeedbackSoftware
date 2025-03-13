using FeedbackSoftware.Classes;
using FeedbackSoftware.Classes.Dtos;
using System.Data.Common;
using System.Windows;
using System.Windows.Controls;

namespace FeedbackSoftware.Views.Pages
{
    /// <summary>
    /// Interaction logic for LoginSchueler.xaml
    /// </summary>
    public partial class LoginSchueler : Page
    {
		DatabaseManager dbm = new DatabaseManager();
		public LoginSchueler()
        {
            InitializeComponent();
        }

		private void LoginS_Click(object sender, RoutedEventArgs e)
		{
			if (IsKeyValid())
			{
				//dann zu Formular weiterleiten
			}
		}

		private bool IsKeyValid()
		{
			UserDto userinfo = new UserDto();
			if (dbm != null)
			{
				userinfo = dbm.SelectKey();
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
