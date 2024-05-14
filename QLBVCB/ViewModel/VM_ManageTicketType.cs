using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    internal class VM_ManageTicketType : VM_Base
    {
        public ICommand OpenAERTicketTypeCommand {get;set;}
        public VM_ManageTicketType() {
            OpenAERTicketTypeCommand = new RelayCommand(ExecutedOpenAERTicketTypeCommand);
        }
        private void ExecutedOpenAERTicketTypeCommand(object obj)
        {
            try
            {
                AERTicketType aERTicketType = new AERTicketType();
                aERTicketType.ShowDialog();
            }
            catch(Exception ex)
            {

            }
        }
    }
}
