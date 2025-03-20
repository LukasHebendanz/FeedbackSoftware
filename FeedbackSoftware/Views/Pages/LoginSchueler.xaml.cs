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

		public LoginSchueler()
        {
            InitializeComponent();
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
				fbDto = dbm.SelectKey(tbxSchlüssel.Password);
				if (fbDto.Schluessel.ToString() == tbxSchlüssel.Password)
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
