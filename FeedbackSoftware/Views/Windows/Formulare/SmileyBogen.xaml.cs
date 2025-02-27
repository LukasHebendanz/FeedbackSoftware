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
    /// Interaction logic for SmileyBogen.xaml
    /// </summary>
    public partial class SmileyBogen : Window
    {
        public SmileyBogen()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string goodInput = GoodTextBox.Text;
            string sadInput = SadTextBox.Text;
            string midInput = midTextBox.Text;

            MessageBox.Show($"Good: {goodInput}\nSad: {sadInput}\nmid: {midInput},", "Submitted Values");
            ValuesForDB(goodInput, sadInput, midInput);
        }

        private void ValuesForDB(string goodInput, string badInput, string midInput)
        {
            Console.WriteLine($"Good: {goodInput}\nSad: {badInput}\nmid: {midInput}", "Submitted Values");
        }
    }
}
