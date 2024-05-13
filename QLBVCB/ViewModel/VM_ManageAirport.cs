using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    internal class VM_ManageAirport : VM_Base
    {
        public ICommand OpenAERAirportCommand { get; set; }
        public VM_ManageAirport()
        {
            OpenAERAirportCommand = new RelayCommand(ExecuteOpenAERAirportCommand);
        }
        private void ExecuteOpenAERAirportCommand(object obj)
        {
            try
            {
                AERAirport aERAirport = new AERAirport();
                aERAirport.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
