using FeedbackSoftware.Classes;
using FeedbackSoftware.Classes.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
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

namespace FeedbackSoftware
{
    /// <summary>
    /// Interaktionslogik für Formulare.xaml
    /// </summary>
    public partial class ZielscheibenFormular : Window
    {
        // Konstruktor beim Öffnen des Templates
        public ZielscheibenFormular()
        {
            InitializeComponent();
            DisableCheckboxes();
        }

        // Konstruktor beim Ausfüllen eines Formulars
        public ZielscheibenFormular(string schluessel)
        {
            InitializeComponent();
            InitializeCheckBoxGroups();

            DatabaseManager dbm = new DatabaseManager();
            this.FeedbackVorgangName = dbm.GetNameBySchluessel(schluessel);
            this.Schluessel = Convert.ToInt32(schluessel);
        }

        // Konstruktor zum Auslesen der Data
        public ZielscheibenFormular(string data, string formularName)
        {
            InitializeComponent();

            labelFormularName.Content = formularName;
            btnSave.Visibility = Visibility.Collapsed;

            ReadData(data);

            DisableCheckboxes();
        }

        #region Properties

        private int Schluessel { get; set; }
        private string FeedbackVorgangName { get; set; }

        #endregion

        private Dictionary<string, List<CheckBox>> checkBoxGroups = new Dictionary<string, List<CheckBox>>();

        private void InitializeCheckBoxGroups()
        {
            var canvas = circle; // Canvas Name
            if (canvas == null)
            {
                Debug.WriteLine("Canvas nicht gefunden!");
                return;
            }

            var allCheckBoxes = canvas.Children.OfType<CheckBox>().ToList();
            Debug.WriteLine($"Anzahl der gefundenen CheckBoxes: {allCheckBoxes.Count}");

            foreach (var checkBox in allCheckBoxes)
            {
                string groupName = GetGroupName(checkBox.Name); // Verwende den Namen anstelle des Contents
                if (!checkBoxGroups.ContainsKey(groupName))
                {
                    checkBoxGroups[groupName] = new List<CheckBox>();
                }
                checkBoxGroups[groupName].Add(checkBox);
                checkBox.Checked += CheckBox_Checked;
            }

            Debug.WriteLine($"Anzahl der Gruppen: {checkBoxGroups.Count}");
            foreach (var group in checkBoxGroups)
            {
                Debug.WriteLine($"Gruppe {group.Key}: {group.Value.Count} CheckBoxes");
            }
        }

        private string GetGroupName(string checkBoxName)
        {
            // Extrahiert den Gruppennamen (z.B. "ObenLinks", "ObenRechts", etc.)
            return checkBoxName.Substring(0, checkBoxName.LastIndexOf('_'));
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkedBox = sender as CheckBox;
            if (checkedBox != null)
            {
                string groupName = GetGroupName(checkedBox.Name); // Verwende den Namen anstelle des Contents
                if (checkBoxGroups.ContainsKey(groupName))
                {
                    foreach (var checkBox in checkBoxGroups[groupName])
                    {
                        if (checkBox != checkedBox)
                        {
                            checkBox.IsChecked = false;
                        }
                    }
                }
                else
                {
                    Debug.WriteLine($"Gruppe {groupName} nicht gefunden!");
                }
            }
        }

        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (PrüfeCheckboxReihen() == false)
            {
                return;
            }

            FormularDto formularDto = new FormularDto()
            {
                Schluessel = this.Schluessel,
                Data = GetData(),
                Name = GetFormularName()
            };

            DatabaseManager dbm = new DatabaseManager();
            dbm.InsertFormular(formularDto);
        }

        public string GetData()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = System.IO.Path.Combine(basePath, "Data", "ZielscheibenFormular.xml");

            XDocument xdoc = XDocument.Load(filePath);

            foreach (var child in circle.Children)
            {
                if (child is CheckBox checkbox)
                {
                    if (checkbox.IsChecked == true)
                    {
                        // Hole den Namen der Checkbox
                        string name = checkbox.Name;

                        // Extrahiere die Note aus dem Namen
                        string notenIndex = name.Substring(name.Length - 2, 2);
                        int note = Convert.ToInt32(notenIndex) + 1;

                        // Hole das richtige XElement aus dem Template
                        XElement currentElement = xdoc.Root?.Element(name);

                        if (currentElement != null)
                        {
                            // Setze die Note in das entsprechende Element
                            currentElement.Value = Convert.ToString(note);
                        }
                    }
                }
            }

            return GetDataAsBase64(xdoc);
        }

        public bool PrüfeCheckboxReihen()
        {
            // Dictionary zur Gruppierung der Checkboxen nach ihrer Reihe
            Dictionary<string, List<CheckBox>> checkboxReihen = new Dictionary<string, List<CheckBox>>();

            // Alle Checkboxes aus dem Grid sammeln
            foreach (UIElement child in circle.Children)
            {
                if (child is CheckBox checkBox)
                {
                    string name = checkBox.Name;

                    //Hole dir den ReihenKey Beispiel ObenLinks1
                    string reihenKey = name.Split('_')[0];

                    // Falls die Gruppe für die Reihe noch nicht existiert, erstellen
                    if (!checkboxReihen.ContainsKey(reihenKey))
                    {
                        checkboxReihen[reihenKey] = new List<CheckBox>();
                    }

                    // Füge die Checkbox der entsprechenden Gruppe hinzu, unterschieden wird durch den Key
                    checkboxReihen[reihenKey].Add(checkBox);
                }
            }

            // Überprüfung jeder Reihe
            foreach (var reihe in checkboxReihen)
            {
                //Hier wird geprüft ob mindestens eine Checkbox angehakt ist
                bool mindestensEineAktiv = reihe.Value.Any(cb => cb.IsChecked == true);

                if (!mindestensEineAktiv)
                {
                    MessageBox.Show("In jeder Reihe eine Checkbox anhaken!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);

                    return false;
                }
            }

            return true;
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
            XDocument xdoc = XDocument.Parse(data);
            IEnumerable<XElement> elements = xdoc.Root?.Elements();

            if (elements == null)
                return;

            foreach (XElement element in elements)
            {
                if (!String.IsNullOrEmpty(element.Value))
                {
                    // Die Checkbox mit dem passenden Namen in circle suchen
                    CheckBox? targetCheckBox = circle.Children
                        .OfType<CheckBox>()
                        .FirstOrDefault(cb => cb.Name == element.Name);

                    // Falls die Checkbox gefunden wurde, auf angehakt setzen
                    if (targetCheckBox != null)
                    {
                        targetCheckBox.IsChecked = true;
                    }
                }
            }
        }

        private void DisableCheckboxes()
        {
            btnSave.Visibility = Visibility.Collapsed;

            foreach (var child in circle.Children)
            {
                if (child is CheckBox checkbox)
                {
                    checkbox.IsHitTestVisible = false;
                }
            }
        }
    }
}
