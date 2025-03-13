using FeedbackSoftware.Classes;
using FeedbackSoftware.Classes.Dtos;
using MaterialDesignThemes.Wpf;
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
using System.Xml.Linq;
using System.Xml.XPath;

namespace FeedbackSoftware
{
    /// <summary>
    /// Interaction logic for FragebogenTabelle.xaml
    /// </summary>
    public partial class FragebogenTabelle : Window
    {
        public SnackbarMessageQueue MessageQueue { get; }

        public FragebogenTabelle()
        {
            InitializeComponent();

            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
            Error.MessageQueue = MessageQueue;
        }
        public FragebogenTabelle(int schluessel)
        {
            InitializeComponent();

            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
            Error.MessageQueue = MessageQueue;

            this.Schluessel = schluessel;
        }

        private int Schluessel { get; set; }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Aktuelle Checkbox holen
            var currentCheckBox = sender as CheckBox;

            // Reihe der Aktuellen Checkbox holen
            int currentRow = Grid.GetRow(currentCheckBox);

            //Grid ist parent und hier holt man es sich
            var parent = VisualTreeHelper.GetParent(currentCheckBox);

            while (parent != null && parent.GetType() != typeof(Grid))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent is Grid grid)
            {

                foreach (UIElement child in grid.Children)
                {
                    if (child is CheckBox checkBox && checkBox != currentCheckBox)
                    {
                        // alle checkboxen in ind der gleichen Grid.Row ausschalten
                        if (Grid.GetRow(checkBox) == currentRow)
                        {
                            checkBox.IsChecked = false;
                        }
                    }
                }
            }
        }
        private string GetData()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = System.IO.Path.Combine(basePath, "Data", "FragebogenFormular.xml");

            XDocument xdoc = XDocument.Load(filePath);
            XElement currentElement = xdoc.Root?.Elements().FirstOrDefault();


            foreach (UIElement child in questionGrid.Children)
            {
                if (child is CheckBox checkBox)
                {
                    string checkBoxName = checkBox.Name;
                    bool? isChecked = checkBox.IsChecked;
                    
                    if (isChecked == true)
                    {
                        string note = checkBoxName.Substring(4, 1);

                        currentElement.Value = note;

                        if (currentElement.ElementsAfterSelf().FirstOrDefault() != null)
                        {
                            currentElement = currentElement.ElementsAfterSelf().FirstOrDefault();
                        }
                    }
                }
            }

            return GetDataAsBase64(xdoc);
        }

        private string GetDataAsBase64(XDocument xdoc)
        {
            string xmlString = xdoc.ToString();
            byte[] xmlBytes = Encoding.UTF8.GetBytes(xmlString);
            string xmlBase64 = Convert.ToBase64String(xmlBytes);

            return xmlBase64;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool allRowsValid = true;

            foreach (UIElement child in questionGrid.Children)
            {
                if (child is CheckBox checkBox)
                {
                    int currentRow = Grid.GetRow(checkBox);
                    int checkedCount = 0;

                    foreach (UIElement innerChild in questionGrid.Children)
                    {
                        if (innerChild is CheckBox innerCheckBox && Grid.GetRow(innerCheckBox) == currentRow)
                        {
                            if (innerCheckBox.IsChecked == true)
                            {
                                checkedCount++;
                            }
                        }
                    }

                    if (checkedCount != 1)
                    {
                        allRowsValid = false;
                        break;
                    }
                }
            }

            if (allRowsValid)
            {
                FormularDto formularDto = new FormularDto()
                {
                    Schluessel = this.Schluessel,
                    Data = GetData()
                };

                DatabaseManager dbm = new DatabaseManager();
                dbm.InsertFormular(formularDto);
            }
            else
            {
                Error.MessageQueue.Enqueue("Bitte jede Aussage bewerten!");
            }
        }
    }
}
