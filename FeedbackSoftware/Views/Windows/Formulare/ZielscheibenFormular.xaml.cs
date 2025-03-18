using System;
using System.Collections.Generic;
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

namespace FeedbackSoftware
{
    /// <summary>
    /// Interaktionslogik für Formulare.xaml
    /// </summary>
    public partial class ZielscheibenFormular : Window
    {
        public ZielscheibenFormular()
        {
            InitializeComponent();
            InitializeCheckBoxGroups();
        }

        private Dictionary<string, List<CheckBox>> checkBoxGroups = new Dictionary<string, List<CheckBox>>();

        private void InitializeCheckBoxGroups()
        {
            var canvas = nuckelan; // Canvas Name
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

        public void SubmitButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
