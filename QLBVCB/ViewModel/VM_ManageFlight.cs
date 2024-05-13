using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    internal class VM_ManageFlight : VM_Base
    {
        public ICommand OpenAERFlightCommand {  get; set; }
        public VM_ManageFlight() {
            OpenAERFlightCommand = new RelayCommand(ExecuteOpenAERFlightCommand);
        }
        private void ExecuteOpenAERFlightCommand(object obj)
        {
            try
            {
                AERFlight aERFlight = new AERFlight();
                aERFlight.ShowDialog();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
