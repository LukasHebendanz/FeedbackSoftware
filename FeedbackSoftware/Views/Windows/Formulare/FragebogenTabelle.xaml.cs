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

namespace FeedbackSoftware
{
    /// <summary>
    /// Interaction logic for FragebogenTabelle.xaml
    /// </summary>
    public partial class FragebogenTabelle : Window
    {
        public FragebogenTabelle()
        {
            InitializeComponent();
        }

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
        private void GetAllCheckBoxValues()
        {

            foreach (UIElement child in questionGrid.Children)
            {
                if (child is CheckBox checkBox)
                {

                    string checkBoxName = checkBox.Name;
                    bool? isChecked = checkBox.IsChecked;

                    //Hier werden die Daten zum Test in der Konsole ausgegeben
                    Console.WriteLine($"CheckBox: {checkBoxName}, IsChecked: {isChecked}");
                }
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            GetAllCheckBoxValues();
        }

    }
}
