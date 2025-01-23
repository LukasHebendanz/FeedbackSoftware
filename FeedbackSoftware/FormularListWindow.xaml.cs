using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using FeedbackSoftware.Classes;

namespace FeedbackSoftware
{
    /// <summary>
    /// Interaction logic for FormularListWindow.xaml
    /// </summary>
    public partial class FormularListWindow : Window
    {
        public List<Formular> Formularliste {  get; set; }

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
