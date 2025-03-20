using FeedbackSoftware.Classes;
using FeedbackSoftware.Classes.Dtos;
using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace FeedbackSoftware.Views
{
    /// <summary>
    /// Interaction logic for FormularListWindow.xaml
    /// </summary>
    public partial class FormularListWindow : Window
    {
        public List<Formular> Formularliste { get; set; }

        public FormularListWindow() { InitializeComponent(); }

        //Konstruktor zum Formular einsehen in FormularListWindow
        public FormularListWindow(int schluessel)
        {
            InitializeComponent();

            DatabaseManager dbm = new DatabaseManager();
            List<FormularDto> formulardtos = dbm.SelectAllFormularsByKey(schluessel).ToList();
            this.Formularliste = ConvertToFormular(formulardtos);

            DataContext = this;
        }

        private List<Formular> ConvertToFormular(List<FormularDto> formulardtos)
        {
            List<Formular> formulare = new List<Formular>();

            foreach (FormularDto dto in formulardtos)
            {
                Formular formular = new Formular()
                {
                    FormularId = dto.FormularId,
                    Schluessel = dto.Schluessel,
                    Data = dto.Data,
                    FormularName = dto.Name,
                };

                formulare.Add(formular);
            }

            return formulare;
        }

        private void btnFormularOeffnen_Click(object sender, RoutedEventArgs e)
        {
            DatabaseManager dbm = new DatabaseManager();

            if (sender is Button button && button.CommandParameter is Formular rowData)
            {
                //Entschlüssle die base64 data
                byte[] decodedBytes = Convert.FromBase64String(rowData.Data);
                string decodedString = Encoding.UTF8.GetString(decodedBytes);

                string name = rowData.FormularName;
                int schluessel = rowData.Schluessel;
                string formularArt = dbm.GetFormularArtByKey(Convert.ToString(schluessel));

                Window formWindow = new Window();
                // Fenster basierend auf der Auswahl öffnen
                switch (formularArt)
                {
                    case "Smiley":
                        formWindow = new SmileyBogen(decodedString, name); // Fenster für Smiley
                        break;
                    case "Zielscheibe":
                        formWindow = new ZielscheibenFormular(decodedString, name); // Fenster für Zielscheibe
                        break;
                    case "Fragebogen":
                        formWindow = new FragebogenTabelle(decodedString, name); // Fenster für Fragebogen
                        break;
                    default:
                        MessageBox.Show("Unbekannte Formularart.");
                        return;
                }
                formWindow.ShowDialog();
            }
        }
    }
}