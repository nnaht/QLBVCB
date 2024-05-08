using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    public class VM_Main : VM_Base
    {
        public bool IsLoaded = false;
        public ICommand LoadedWindowCommand { get; set; }
        public VM_Main()
        {
            LoadedWindowCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsLoaded = true;
                Login loginWindow = new Login();
                loginWindow.ShowDialog();
            }
            );
        }
    }
}
