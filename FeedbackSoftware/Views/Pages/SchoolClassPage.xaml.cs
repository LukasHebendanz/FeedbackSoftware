using FeedbackSoftware.Classes;
using FeedbackSoftware.Classes.Dtos;
using MaterialDesignThemes.Wpf;
using Mysqlx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FeedbackSoftware.Views.Pages
{
    /// <summary>
    /// Interaction logic for SchoolClassPage.xaml
    /// </summary>
    public partial class SchoolClassPage : Page
    {
        DatabaseManager dbm = new DatabaseManager();
		public SnackbarMessageQueue MessageQueue { get; }
		public SchoolClassPage()
        {
            InitializeComponent();
			MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
			Error.MessageQueue = MessageQueue;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (tbxName.Text != string.Empty && tbxJahrgang.Text != string.Empty && tbxJahr.Text != string.Empty
				&& tbxAbteilung.Text != string.Empty && tbxFach.Text != string.Empty)
			{
				if (!UserExists(tbxName.Text.Trim()))
				{
					KlasseDto kDto = new KlasseDto()
					{
						Name = tbxName.Text,
						Jahrgangsstufe = tbxJahrgang.Text,
						Schuljahr = tbxJahr.Text,
						Abteilung = tbxAbteilung.Text,
						Fach = tbxFach.Text,
					};

					dbm.CreateKlasse(kDto);
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

		private bool UserExists(string name)
		{
			List<string> userInfos = dbm.GetKlassenNames();
			bool userExists = false;

			foreach (string user in userInfos)
			{
				if (name == user)
				{
					userExists = true;
				}
			}

			return userExists;
		}
	}
}
