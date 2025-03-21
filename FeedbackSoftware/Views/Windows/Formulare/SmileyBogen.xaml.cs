using FeedbackSoftware.Classes;
using FeedbackSoftware.Classes.Dtos;
using MaterialDesignThemes.Wpf;
using Mysqlx;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Xml.Linq;
using System.Xml.XPath;

namespace FeedbackSoftware
{
    /// <summary>
    /// Interaction logic for SmileyBogen.xaml
    /// </summary>
    public partial class SmileyBogen : Window
    {
        // Konstruktor beim Öffnen des Templates
        public SmileyBogen()
        {
            InitializeComponent();

            DisableTextboxes();
        }

        //Konstruktor beim Ausfüllen eines Formulars
        public SmileyBogen(string schluessel)
        {
            InitializeComponent();

            DatabaseManager dbm = new DatabaseManager();
            this.FeedbackVorgangName = dbm.GetNameBySchluessel(schluessel);
            this.Schluessel = Convert.ToInt32(schluessel);
        }

        //Konstruktor zum Auslesen der Data
        public SmileyBogen(string data, string formularName)
        {
            InitializeComponent();

            labelFormularName.Content = formularName;
            btnSubmit.Visibility = Visibility.Collapsed;

            ReadData(data);

            DisableTextboxes();
        }

        private int Schluessel { get; set; }
        private string FeedbackVorgangName { get; set; }

        private string pfadPositiv = ConfigurationManager.AppSettings["Positiv"];
        private string pfadNeutral = ConfigurationManager.AppSettings["Neutral"];
        private string pfadNegativ = ConfigurationManager.AppSettings["Negativ"];

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string goodInput = GoodTextBox.Text;
            string sadInput = SadTextBox.Text;
            string midInput = midTextBox.Text;

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = System.IO.Path.Combine(basePath, "Data", "SmileyBogenFormular.xml");

            XDocument xDoc = XDocument.Load(filePath);

            if (goodInput != String.Empty && midInput != String.Empty && sadInput != String.Empty)
            {
                XElement xPositiv = xDoc.XPathSelectElement(pfadPositiv);
                xPositiv.Value = goodInput;

                XElement xNeutral = xDoc.XPathSelectElement(pfadNeutral);
                xNeutral.Value = midInput;

                XElement xNegativ = xDoc.XPathSelectElement(pfadNegativ);
                xNegativ.Value = sadInput;
            }

            FormularDto formularDto = new FormularDto()
            {
                Schluessel = Convert.ToInt32(this.Schluessel),
                Data = GetDataAsBase64(xDoc),
                Name = GetFormularName()
            };

            DatabaseManager dbm = new DatabaseManager();
            try
            {
                dbm.InsertFormular(formularDto);
				MessageBox.Show("Formular erfolgreich eingereicht");
			}
            catch (Exception)
            {
				MessageBox.Show("Dieser Schlüssel existiert nicht!");
			}
        }

        private string GetDataAsBase64(XDocument xdoc)
        {
            string xmlString = xdoc.ToString();
            byte[] xmlBytes = Encoding.UTF8.GetBytes(xmlString);
            string xmlBase64 = Convert.ToBase64String(xmlBytes);

            return xmlBase64;
        }

        private string GetFormularName()
        {
            DatabaseManager dbm = new DatabaseManager();
            int formularCount = dbm.SelectAllFormularsByKey(this.Schluessel).Count;
            string formularNumber = Convert.ToString(formularCount + 1);

            return $"{this.FeedbackVorgangName}_{formularNumber}";
        }

        private void ReadData(string data)
        {
            XDocument xDoc = XDocument.Parse(data);

            try
            {
                GoodTextBox.Text = xDoc.XPathSelectElement(ConfigurationManager.AppSettings["Positiv"]).Value;
                midTextBox.Text = xDoc.XPathSelectElement(ConfigurationManager.AppSettings["Neutral"]).Value;
                SadTextBox.Text = xDoc.XPathSelectElement(ConfigurationManager.AppSettings["Negativ"]).Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DisableTextboxes()
        {
            btnSubmit.Visibility = Visibility.Collapsed;

            foreach (var child in questionGrid.Children)
            {
                if (child is Border border)
                {
                    if (border.Child is TextBox textbox)
                    {
                        textbox.IsReadOnly = true;
                    }
                }
            }
        }
    }
}
