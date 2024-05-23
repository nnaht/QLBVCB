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
    internal class VM_BookedSeat : VM_Base
    {
        public ICommand PickSeatCommand {  get; set; }
        public VM_BookedSeat() {
            PickSeatCommand = new RelayCommand(ExecutePickSeatCommand);
        }
        private void ExecutePickSeatCommand(object obj)
        {
            ShowCustomMessageBox("Chỗ này book rồi");
        }
        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
    }
}
