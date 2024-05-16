using QLBVCB.Model;
using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    internal class VM_ManageCustomer : VM_Base
    {
        public ICommand OpenAERCustomerCommand { get; set; }
        private ObservableCollection<KHACHHANG> _CustomerList;
        public ObservableCollection<KHACHHANG> CustomerList { get { return _CustomerList; } set { _CustomerList = value; OnPropertyChanged(); } }
        public VM_ManageCustomer()
        {
            CustomerList = new ObservableCollection<KHACHHANG>(DataProvider.Ins.DB.KHACHHANGs);
            OpenAERCustomerCommand = new RelayCommand<object>((p) => { return true; }, (p) => { AERCustomer aer = new AERCustomer(); aer.ShowDialog(); });
        }
    }
}
