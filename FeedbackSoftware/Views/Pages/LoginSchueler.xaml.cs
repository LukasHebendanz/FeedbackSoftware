using FeedbackSoftware.Classes;
using FeedbackSoftware.Classes.Dtos;
using MaterialDesignThemes.Wpf;
using System;
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
		FeedbackDto fbDto = new FeedbackDto();

		public SnackbarMessageQueue MessageQueue { get; }

		public LoginSchueler()
        {
            InitializeComponent();
			MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
			Error.MessageQueue = MessageQueue;
		}

		private void LoginS_Click(object sender, RoutedEventArgs e)
		{
			if (IsKeyValid())
			{
				if (fbDto.FormularArt == "Smiley")
				{
					SmileyBogen sb = new SmileyBogen();
					sb.ShowDialog();
				}
				if (fbDto.FormularArt == "Zielscheibe")
				{
					ZielscheibenFormular zf = new ZielscheibenFormular();
					zf.ShowDialog();
				}
				if (fbDto.FormularArt == "Fragebogen")
				{
					FragebogenTabelle ft = new FragebogenTabelle();
					ft.ShowDialog();
				}
			}
		}

		private bool IsKeyValid()
		{
			if (dbm != null)
			{
				fbDto = dbm.GetAllFromFeedbackVorgang(tbxSchlüssel.Password);
				if (fbDto.Schluessel != 0)
				{
					return true;
				}
				else
				{
					Error.MessageQueue.Enqueue("Dieser Schlüssel existiert nicht!");
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
