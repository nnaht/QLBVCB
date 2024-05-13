using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    internal class VM_ManagePlane : VM_Base
    {
        public ICommand OpenAERPlaneCommand { get; set; }
        public VM_ManagePlane() {
            OpenAERPlaneCommand = new RelayCommand(ExecuteOpenAERPlaneCommand);
        }
        private void ExecuteOpenAERPlaneCommand(object obj)
        {
            try
            {
                AERPlane aerPlanePage = new AERPlane();
                aerPlanePage.Show();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
