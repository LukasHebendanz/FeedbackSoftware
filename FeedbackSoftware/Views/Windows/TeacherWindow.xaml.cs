using FeedbackSoftware.Views.Pages;
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
using System.Windows.Shapes;
using FeedbackSoftware.Views;
using MySql.Data.MySqlClient;
using FeedbackSoftware.Classes.Dtos;
using FeedbackSoftware.Classes;
using static Mysqlx.Expect.Open.Types.Condition.Types;

namespace FeedbackSoftware.Views
{
    /// <summary>
    /// Interaction logic for TeacherWindow.xaml
    /// </summary>
    public partial class TeacherWindow : Window
    {
        private List<string> klassenNames = new List<string>();
        public TeacherWindow()
        {
            //this.Rolle = rolle;
            InitializeComponent();
            LoadFormData();
            // Startseite festlegen
            //MainFrame.NavigationService.Navigate(new FeedbackKeyWindow());
        }
        //public string Rolle { get; set; }

   //     public void OnStart()
   //     {
   //         UserDto udto = new UserDto();
   //         if (Rolle == "Admin")
   //         {
   //             btnAdminWindow.Visibility = Visibility.Visible;
			//}
   //         else
   //         {
			//	btnAdminWindow.Visibility = Visibility.Hidden;
			//}
   //     }

        //private void NavigateToFeedbackPage_Click(object sender, RoutedEventArgs e)
        //{
        //    MainFrame.NavigationService.Navigate(new FeedbackKeyWindow());
        //}
        //private void NavigateToKlasse_Click(object sender, RoutedEventArgs e)
        //{
        //    MainFrame.NavigationService.Navigate(new SchoolClassPage());
        //}


        //private static readonly Random random = new Random();
        //private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        //public static string GenerateKey(int length = 10)
        //{
        //    return new string(Enumerable.Repeat(chars, length)
        //      .Select(s => s[random.Next(s.Length)]).ToArray());
        //}

        // Code-Behind für den Speicherbutton-Click-Event

        private void LoadFormData()
        {
            DatabaseManager dbManager = new DatabaseManager();

            // Vorgangsnamen laden
            //List<string> vorgangNamen = dbManager.GetVorgangName();
            //formularComboBox.ItemsSource = vorgangNamen;

            // KlassenIds laden
            List<KlasseDto> klassen = dbManager.GetKlassenNames();
            //List<string> klassenNames = [];
            foreach (KlasseDto klasse in klassen)
            {
                this.klassenNames.Add(klasse.Name);
            }
            classComboBox.ItemsSource = klassenNames;

            // Feedbackarten laden
            List<string> feedbackArten = dbManager.GetFeedbackArt();
            einsehenComboBox.ItemsSource = feedbackArten;
        }



        public FeedbackDto GetFeedbackByKey(string schluessel)
        {
            FeedbackDto feedback = new FeedbackDto();

            using (MySqlConnection con = DatabaseManager.GetConnection())
            {
                con.Open();

                string sql = "SELECT Schluessel, KlasseId, Name, FormularArt FROM FeedbackTabelle WHERE Schluessel = @Schluessel";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Schluessel", schluessel);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            feedback = new FeedbackDto
                            {
                                Schluessel = (int)reader["Schluessel"],
                                KlasseId = (int)reader["KlasseId"],
                                Name = reader["Name"].ToString(),
                                FeedbackArt = reader["FormularArt"].ToString()
                            };
                        }
                    }
                }
            }

            return feedback;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseManager dbManager = new DatabaseManager();

            // 1. Schlüssel generieren

            // 2. FormularDto erstellen und Daten zuweisen

            //FeedbackDto feedback = new FeedbackDto
            //{
            //    Schluessel =,
            //    KlasseId =,
            //    Name =,
            //    FormularArt =
            //    // Weitere Felder hier
            //};

            // 3. In die Datenbank einfügen
            //dbManager.InsertFeedback(GetFeedbackByKey("sdfzugr"));

            // 4. Bestätigung anzeigen
            // MessageBox.Show($"Formular mit Schlüssel {key} gespeichert!");
        }


        private void TemplateButton_Click(object sender, RoutedEventArgs e)
        {
            // Überprüfen, ob eine Auswahl getroffen wurde
            if (formularComboBox.SelectedItem == null)
            {
                MessageBox.Show("Bitte wählen Sie eine Formularart aus.");
                return;
            }

            // Ausgewähltes Formular abrufen
            string selectedFormular = ((ComboBoxItem)formularComboBox.SelectedItem).Content.ToString();

            Window formWindow = new Window();

            // Fenster basierend auf der Auswahl öffnen
            switch (selectedFormular)
            {
                case "Smiley":
                    formWindow = new SmileyBogen(); // Fenster für Smiley
                    break;
                case "Zielscheibe":
                    formWindow = new ZielscheibenFormular(); // Fenster für Zielscheibe
                    break;
                case "Fragebogen":
                    formWindow = new FragebogenTabelle(); // Fenster für Fragebogen
                    break;
                default:
                    MessageBox.Show("Unbekannte Formularart.");
                    return;
            }

            // Fenster im Read-Only-Modus öffnen
            if (formWindow != null)
            {
                formWindow.ShowDialog(); // Zeigt das Fenster modal an
            }
        }

        private void SaveFeedbackVorgang_Click(object sender, RoutedEventArgs e)
        {

        }
    }   
}
