using QLBVCB.Model;
using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;
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
        //private ObservableCollection<TAIKHOAN> _AccountList;
        //public ObservableCollection<TAIKHOAN> AccountList { get { return _AccountList; } set { _AccountList = value; OnPropertyChanged(); } }
        public ICommand ManageServicesCommand {  get; set; }
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
        private void ManageTicketType(object obj) => CurrentView = new VM_ManageTicketType();
        private void ManageFlight(object obj) => CurrentView = new VM_ManageFlight();
        private void ManagePlane(object obj) => CurrentView = new VM_ManagePlane();
        private void ManageAirport(object obj) => CurrentView = new VM_ManageAirport();
        private void ManageTicket(object obj) => CurrentView = new VM_ManageTicket();
        private void ManageEmployee (object obj) => CurrentView = new VM_ManageEmployee();
        private void ManageCustomer (object obj) => CurrentView = new VM_ManageCustomer();
        private void ManageAccount (object obj) => CurrentView = new VM_ManageAccount();
        private void ManageBooking(object obj) => CurrentView = new VM_ManageBooking();
        private void ManageServices(object obj) => CurrentView = new VM_ManageService();
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        public VM_Main()
        {
            //LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            //{
            //    IsLoaded = true;
            //    if (p == null)
            //    {
            //        return;
            //    }

            //    p.Hide();

            //    Login loginWindow = new Login();
            //    loginWindow.ShowDialog();
            //    LoadMainWindow();

            //    if (loginWindow.DataContext == null)
            //    {
            //        return;
            //    }

            //    var loginVM = loginWindow.DataContext as VM_Loginn;

            //    if (loginVM.IsLogin == true)
            //    {
            //        p.Show();
            //    }
            //    else
            //    {
            //        p.Close();
            //    }

            //});
            ManageServicesCommand = new RelayCommand(ManageServices);
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
        //void LoadMainWindow()
        //{
        //    AccountList = new ObservableCollection<TAIKHOAN>(DataProvider.Ins.DB.TAIKHOANs);
        //}
    }
}
