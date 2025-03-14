﻿using FeedbackSoftware.Views.Pages;
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
            //this.Rolle = rolle;
            InitializeComponent();
            LoadFormData();
            DataContext = this;
        }

        public IList<ComboBoxItem> KlassenListe
        {
            get
            {
                IList<ComboBoxItem> list = new List<ComboBoxItem>();
                list.Add(new ComboBoxItem() { Content = "Bitte auswählen!", Visibility = Visibility.Collapsed });
                List<string> klassen = new DatabaseManager().GetKlassenNames();
                foreach(string k in klassen)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = k;
                    list.Add(item);
                }
                return list;
            }
        }

        private void LoadFormData()
        {
            DatabaseManager dbManager = new DatabaseManager();

            // Vorgangsnamen laden
            //List<string> vorgangNamen = dbManager.GetVorgangName();
            //formularComboBox.ItemsSource = vorgangNamen;

            // KlassenIds laden
            //List<string> klassenNames = dbManager.GetKlassenNames();
            //klassenNames.Insert(0, "Bitte Klasse Wählen");
            //classComboBox.ItemsSource = klassenNames;

            // Feedbackarten laden
            List<string> feedbackArten = dbManager.GetFeedbackArt();
            einsehenComboBox.ItemsSource = feedbackArten;
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
            if (classComboBox.SelectedValue != String.Empty
                && formularComboBox.SelectedIndex > 0
                && nameTextBox.Text != String.Empty)
            {
                // 1. Daten aus den ComboBoxen und dem Textfeld abrufen
                string selectedClass = classComboBox.SelectedValue.ToString();
                string selectedFormularArt = formularComboBox.SelectedValue.ToString().Split(' ').Skip(1).FirstOrDefault() ?? "Kein zweites Wort gefunden";
                string name = nameTextBox.Text;

                DatabaseManager dbManager = new DatabaseManager();

                // 2. Neues Formular-Objekt erstellen
                FeedbackDto newFeedback = new FeedbackDto
                {
                    KlasseId = dbManager.GetKlassenIdByName(selectedClass),
                    FeedbackArt = selectedFormularArt,
                    Name = name
                };

                // 3. In die Datenbank speichern
                try
                {
                    dbManager.InsertFeedback(newFeedback);

                    MessageBox.Show("Formular erfolgreich gespeichert!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Speichern des Formulars: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Bitte füllen Sie alle erforderlichen Felder aus.");
            }
        }
    }
}
