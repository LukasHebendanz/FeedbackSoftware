﻿using FeedbackSoftware.Classes.Dtos;
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
using FeedbackSoftware.Views.Windows;

namespace FeedbackSoftware.Views.Pages
{
    /// <summary>
    /// Interaction logic for EditClass.xaml
    /// </summary>
    public partial class EditClass : Page
    {
        DatabaseManager db = new DatabaseManager();
        KlasseDto selectedClass = new KlasseDto();

        public EditClass(KlasseDto selectedClass)
        {
            InitializeComponent();
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
            db.UpdateKlasse(this.selectedClass);
            MessageBox.Show("Klasse erfolgreich bearbeitet");
		}

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();

            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }
    }
}
