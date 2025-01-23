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