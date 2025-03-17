using FeedbackSoftware.Classes;
using FeedbackSoftware.Classes.Dtos;
using System.Collections.Generic;
using System.Linq;
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

            //später über konstruktorübergabe
            int schluessel = 72;

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
    }
}