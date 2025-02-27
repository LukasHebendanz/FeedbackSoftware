using FeedbackSoftware.Classes;
using System.Collections.Generic;
using System.Windows;

namespace FeedbackSoftware.Views
{
    /// <summary>
    /// Interaction logic for FormularListWindow.xaml
    /// </summary>
    public partial class FormularListWindow : Window
    {
        public List<Formular> Formularliste { get; set; }

        public FormularListWindow()
        {
            InitializeComponent();

            //Test zum Befüllen der Liste, später werden hier aus der Datenbank alle Formularnamen des ausgewählten Schlüssels geladen
            Formularliste = new List<Formular>()
            {
                new Formular { FormularName = "Formular1_Schlüssel" },
                new Formular { FormularName = "Formular2_Schlüssel" },
                new Formular { FormularName = "Formular3_Schlüssel" }
            };

            DataContext = this;
        }
    }
}