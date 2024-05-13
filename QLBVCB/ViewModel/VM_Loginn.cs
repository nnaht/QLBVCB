using System;
using System.Windows;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    public class VM_Loginn : VM_Base
    {
        public ICommand LoginCommand { get; set; }

        public VM_Loginn()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private void Login(object parameter)
        {
            MainWindow mainWindow = new MainWindow();
            CloseWindow(Application.Current.MainWindow);
            Application.Current.MainWindow = mainWindow;
            mainWindow.ShowDialog();
        }

        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}