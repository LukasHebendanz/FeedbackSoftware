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
					SmileyBogen sb = new SmileyBogen(tbxSchlüssel.Password);
					sb.ShowDialog();
				}
				if (fbDto.FormularArt == "Zielscheibe")
				{
					ZielscheibenFormular zf = new ZielscheibenFormular(tbxSchlüssel.Password);
					zf.ShowDialog();
				}
				if (fbDto.FormularArt == "Fragebogen")
				{
					FragebogenTabelle ft = new FragebogenTabelle(tbxSchlüssel.Password);
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
