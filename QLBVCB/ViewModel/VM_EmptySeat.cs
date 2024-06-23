using QLBVCB.View;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    internal class VM_EmptySeat : VM_Base
    {
        public ICommand PickSeatCommand { get; set; }

        public VM_EmptySeat()
        {
            PickSeatCommand = new RelayCommand(ExecutePickSeatCommand);
        }

        public void ExecutePickSeatCommand(object obj)
        {
            IsChosen = !IsChosen;
        }

        private bool _isChosen;
        public bool IsChosen
        {
            get { return _isChosen; }
            set { _isChosen = value; OnPropertyChanged(); }
        }

        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
    }
}
