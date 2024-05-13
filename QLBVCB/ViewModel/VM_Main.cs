using QLBVCB.View;
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
        
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand ManageTicketTypeCommand { get; set; }
        public ICommand ManageFlightCommand { get; set; }
        public ICommand ManagePlaneCommand { get; set; }
        public ICommand ManageAirportCommand { get; set; }
        private void ManageTicketType(object obj) =>CurrentView = new VM_ManageTicketType();
        private void ManageFlight(object obj) => CurrentView = new VM_ManageFlight();
        private void ManagePlane(object obj) => CurrentView = new VM_ManagePlane();
        private void ManageAirport(object obj) => CurrentView = new VM_ManageAirport();
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        public VM_Main()
        {
            ManageTicketTypeCommand = new RelayCommand(ManageTicketType);
            ManageFlightCommand = new RelayCommand(ManageFlight);
            ManagePlaneCommand = new RelayCommand(ManagePlane);
            ManageAirportCommand = new RelayCommand(ManageAirport);
        }
    }
}
