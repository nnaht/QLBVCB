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
    internal class VM_ManageAccount : VM_Base
    {
        public ICommand OpenAERAccountCommand { get; set; }
        private ObservableCollection<TAIKHOAN> _AccountList;
        public ObservableCollection<TAIKHOAN> AccountList { get { return _AccountList; } set { _AccountList = value; OnPropertyChanged(); } }
        public VM_ManageAccount()
        {
            AccountList = new ObservableCollection<TAIKHOAN>(DataProvider.Ins.DB.TAIKHOANs);
            OpenAERAccountCommand = new RelayCommand<object>((p) => { return true; }, (p) => { AERAccount aer = new AERAccount(); aer.ShowDialog(); });
        }
    }
}
