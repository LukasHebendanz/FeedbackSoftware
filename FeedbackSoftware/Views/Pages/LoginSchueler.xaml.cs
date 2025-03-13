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
				if (fbDto.FeedbackArt == "Smiley")
				{
					SmileyBogen sb = new SmileyBogen();
					sb.ShowDialog();
				}
				if (fbDto.FeedbackArt == "Zielscheibe")
				{
					ZielscheibenFormular zf = new ZielscheibenFormular();
					zf.ShowDialog();
				}
				if (fbDto.FeedbackArt == "Fragebogen")
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
				fbDto = dbm.SelectKey(tbxSchlüssel.Password);
				if (fbDto.Schluessel.ToString() == tbxSchlüssel.Password)
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
