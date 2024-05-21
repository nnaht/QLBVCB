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
        public ICommand ManageTicketCommand { get; set; }
        public ICommand ManageBookingCommand { get; set; }
        public ICommand ManageEmployeeCommand { get; set; }
        public ICommand ManageCustomerCommand { get; set; }
        public ICommand ManageAccountCommand { get; set; }
        private void ManageTicketType(object obj) =>CurrentView = new VM_ManageTicketType();
        private void ManageFlight(object obj) => CurrentView = new VM_ManageFlight();
        private void ManagePlane(object obj) => CurrentView = new VM_ManagePlane();
        private void ManageAirport(object obj) => CurrentView = new VM_ManageAirport();
        private void ManageTicket(object obj) => CurrentView = new VM_ManageTicket();
        private void ManageEmployee (object obj) => CurrentView = new VM_ManageEmployee();
        private void ManageCustomer (object obj) => CurrentView = new VM_ManageCustomer();
        private void ManageAccount (object obj) => CurrentView = new VM_ManageAccount();
        private void ManageBooking(object obj) => CurrentView = new VM_ManageBooking();

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
            ManageTicketCommand = new RelayCommand(ManageTicket);
            ManageEmployeeCommand = new RelayCommand(ManageEmployee);
            ManageCustomerCommand = new RelayCommand(ManageCustomer);
            ManageAccountCommand = new RelayCommand(ManageAccount);
            ManageBookingCommand = new RelayCommand(ManageBooking);
        }
    }
}
