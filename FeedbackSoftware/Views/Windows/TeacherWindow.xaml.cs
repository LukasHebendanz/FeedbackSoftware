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
        public TeacherWindow()
        {
            InitializeComponent();
            // Startseite festlegen
            MainFrame.NavigationService.Navigate(new FeedbackKeyWindow());
        }

        private void NavigateToFeedbackPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new FeedbackKeyWindow());
        }
        private void NavigateToKlasse_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new SchoolClassPage());
        }


        //private static readonly Random random = new Random();
        //private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        //public static string GenerateKey(int length = 10)
        //{
        //    return new string(Enumerable.Repeat(chars, length)
        //      .Select(s => s[random.Next(s.Length)]).ToArray());
        //}

        // Code-Behind für den Speicherbutton-Click-Event
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseManager dbManager = new DatabaseManager();

            // 1. Schlüssel generieren

            // 2. FormularDto erstellen und Daten zuweisen
            FeedbackDto feedback = new FeedbackDto
            {
                Key = "sf",
                KlasseId = 1,
                Name = TitelTextBox.Text,
                FormularArt = BeschreibungTextBox.Text
                // Weitere Felder hier
            };

            // 3. In die Datenbank einfügen
            dbManager.InsertFormular(feedback);

            // 4. Bestätigung anzeigen
            // MessageBox.Show($"Formular mit Schlüssel {key} gespeichert!");
        }
    }   
}
