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
    internal class VM_ManageEmployee : VM_Base
    {
        public ICommand OpenAEREmployeeCommand { get; set; }
        private ObservableCollection<NHANVIEN> _EmployeeList;
        public ObservableCollection<NHANVIEN> EmployeeList { get { return _EmployeeList; } set { _EmployeeList = value; OnPropertyChanged(); } }
        public VM_ManageEmployee()
        {
            EmployeeList = new ObservableCollection<NHANVIEN>(DataProvider.Ins.DB.NHANVIENs);
            OpenAEREmployeeCommand = new RelayCommand<object>((p) => { return true; }, (p) => { AEREmployee aer = new AEREmployee(); aer.ShowDialog(); });
        }
    }
}
