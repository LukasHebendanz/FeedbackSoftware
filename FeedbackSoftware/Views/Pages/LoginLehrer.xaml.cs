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

namespace FeedbackSoftware.Views.Pages
{
    /// <summary>
    /// Interaction logic for LoginLehrer.xaml
    /// </summary>
    public partial class LoginLehrer : Page
    {
        public LoginLehrer()
        {
			InitializeComponent();
        }

        private void ValuesTbx()
        {
            string username = tbxUsername.Text;
            string password = tbxPassword.Value;
        }
	}
}