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
        public SnackbarMessageQueue MessageQueue { get; }
        public SmileyBogen()
        {
            InitializeComponent();
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
            Error.MessageQueue = MessageQueue;

            //Später im unteren Konstruktor durch Übergabe, vorläufiger Test
            this.Schluessel = 72;
            this.FeedbackVorgangName = "Salamig";
        }

        public SmileyBogen(int schluessel, string feedbackVorgangName)
        {
            InitializeComponent();

            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
            Error.MessageQueue = MessageQueue;

            this.Schluessel = schluessel;
            this.FeedbackVorgangName = feedbackVorgangName;
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
                Error.MessageQueue.Enqueue("Formular erfolgreich eingereicht");
            }
            catch (Exception)
            {
                Error.MessageQueue.Enqueue("Dieser Schlüssel existiert nicht!");
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
            int formularCount = dbm.SelectAllFormularsByKey(this.Schluessel).Count != null ? dbm.SelectAllFormularsByKey(this.Schluessel).Count : 0;
            string formularNumber = Convert.ToString(formularCount+1);

            return $"{this.FeedbackVorgangName}_{formularNumber}";
        }
    }
}
