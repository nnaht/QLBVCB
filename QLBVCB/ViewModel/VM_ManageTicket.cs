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
    internal class VM_ManageTicket : VM_Base
    {
        public ICommand OpenAERTicketCommand { get; set; }
        private ObservableCollection<VEBAY> _TicketList;
        public ObservableCollection<VEBAY> TicketList { get { return _TicketList; } set { _TicketList = value; OnPropertyChanged(); } }
        public VM_ManageTicket()
        {
            TicketList = new ObservableCollection<VEBAY>(DataProvider.Ins.DB.VEBAYs);
            OpenAERTicketCommand = new RelayCommand<object>((p) => { return true; }, (p) => { AERTicket aer = new AERTicket(); aer.ShowDialog(); });
        }
    }
}
