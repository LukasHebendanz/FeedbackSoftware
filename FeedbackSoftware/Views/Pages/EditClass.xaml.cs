using FeedbackSoftware.Classes.Dtos;
using FeedbackSoftware.Classes;
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
using static MaterialDesignThemes.Wpf.Theme;

namespace FeedbackSoftware.Views.Pages
{
    /// <summary>
    /// Interaction logic for EditClass.xaml
    /// </summary>
    public partial class EditClass : Page
    {
        DatabaseManager db = new DatabaseManager();
        public SnackbarMessageQueue MessageQueue { get; }
        KlasseDto selectedClass = new KlasseDto();

        public EditClass(KlasseDto selectedClass)
        {
            InitializeComponent();
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
            Error.MessageQueue = MessageQueue;
            this.selectedClass = selectedClass;
            tbxName.Text = selectedClass.Name;
        }

        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            this.selectedClass.Name = tbxJahr.Text;
            this.selectedClass.Jahrgangsstufe = tbxJahrgang.Text;
            this.selectedClass.Name = tbxName.Text;
            this.selectedClass.Fach = tbxFach.Text;
            this.selectedClass.Abteilung = tbxAbteilung.Text;
            //this.selectedUser.Rolle = Benutzer.Rolle;
            db.UpdateKlasse(this.selectedClass);
        }
    }
}
