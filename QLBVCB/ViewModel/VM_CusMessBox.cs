using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    internal class VM_CusMessBox : INotifyPropertyChanged
    {
        public string Message { get; set; }
        public ICommand CloseCommand { get; set; }

        public VM_CusMessBox(string message)
        {
            Message = message;
            CloseCommand = new RelayCommand(Close);
        }

        private void Close(object parameter)
        {
            Application.Current.Windows.OfType<CusMessBox>().FirstOrDefault()?.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
