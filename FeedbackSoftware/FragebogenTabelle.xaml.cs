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

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Message Sent!");
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Get the current checkbox
            var currentCheckBox = sender as CheckBox;

            // Get the Grid.Row of the current checkbox
            int currentRow = Grid.GetRow(currentCheckBox);

            // Get the parent Grid
            var parent = VisualTreeHelper.GetParent(currentCheckBox);

            while (parent != null && parent.GetType() != typeof(Grid))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent is Grid grid)
            {
                // Loop through all children in the Grid
                foreach (UIElement child in grid.Children)
                {
                    if (child is CheckBox checkBox && checkBox != currentCheckBox)
                    {
                        // Uncheck checkboxes in the same Grid.Row
                        if (Grid.GetRow(checkBox) == currentRow)
                        {
                            checkBox.IsChecked = false;
                        }
                    }
                }
            }
        }
    }
}
