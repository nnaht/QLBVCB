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
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand ManageTicketTypeCommand { get; set; }
        private void ManageTicketType(object obj) =>CurrentView = new VM_ManageTicketType();
        public VM_Main()
        {
            ManageTicketTypeCommand = new RelayCommand(ManageTicketType);
        }
    }
}
