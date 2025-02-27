using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackSoftware.Classes
{
    public class Formular : INotifyPropertyChanged
    {
        public Formular(){ }

        #region Properties
        private string formularName;
        public string FormularName { get => formularName; set { formularName = value; OnPropertyChanged(); } }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
