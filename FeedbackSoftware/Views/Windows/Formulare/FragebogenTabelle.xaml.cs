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
        // Konstruktor beim Öffnen des Templates
        public FragebogenTabelle()
        {
            InitializeComponent();
            DisableCheckboxes();
        }

        // Konstruktor beim Ausfüllen eines Formulars
        public FragebogenTabelle(string schluessel)
        {
            InitializeComponent();

            DatabaseManager dbm = new DatabaseManager();
            this.FeedbackVorgangName = dbm.GetNameBySchluessel(schluessel);
            this.Schluessel = Convert.ToInt32(schluessel);
        }

        //Konstruktor zum Auslesen der Data
        public FragebogenTabelle(string data, string formularName)
        {
            InitializeComponent();

            labelFormularName.Content = formularName;
            btnSave.Visibility = Visibility.Collapsed;

            ReadData(data);

            DisableCheckboxes();
        }

        private int Schluessel { get; set; }
        private string FeedbackVorgangName { get; set; }

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
                    Data = GetData(),
                    Name = GetFormularName()
                };

                DatabaseManager dbm = new DatabaseManager();
                dbm.InsertFormular(formularDto);
				MessageBox.Show("Formular erfolgreich eingereicht");
			}
            else
            {
                MessageBox.Show("Bitte jede Aussage bewerten!");
            }
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
            XDocument xdoc = XDocument.Parse(data);
            IEnumerable<XElement> elements = xdoc.Root?.Elements();

            if (elements == null)
                return;

            int row = 2; // Starte mit der ersten Checkbox Reihe

            foreach (XElement element in elements)
            {
                string value = element.Value;
                if (int.TryParse(value, out int note))
                {
                    if (row == 8)
                    {
                        row++;
                    }
                    // Den Namen der Checkbox für diese Reihe und Note generieren
                    // D2 stellt sicher dass es ab 02 beginnt
                    string checkBoxName = $"Note{note}Row{row:D2}";

                    // Die Checkbox mit dem passenden Namen in questionGrid suchen
                    CheckBox? targetCheckBox = questionGrid.Children
                        .OfType<CheckBox>()
                        .FirstOrDefault(cb => cb.Name == checkBoxName);

                    // Falls die Checkbox gefunden wurde, auf angehakt setzen
                    if (targetCheckBox != null)
                    {
                        targetCheckBox.IsChecked = true;
                    }
                }

                row++;
            }
        }

        private void DisableCheckboxes()
        {
            btnSave.Visibility = Visibility.Collapsed;

            foreach (var child in questionGrid.Children)
            {
                if (child is CheckBox checkbox)
                {
                    checkbox.IsHitTestVisible = false;
                }
            }
        }
    }
}
