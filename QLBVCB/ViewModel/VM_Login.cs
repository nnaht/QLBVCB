using System;
using System.Windows;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    public class VM_Login : VM_Base
    {
        public ICommand LoginCommand { get; set; }

        public VM_Login()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private void Login(object parameter)
        {
            // Tạo một thể hiện của MainWindow
            MainWindow mainWindow = new MainWindow();

            // Đóng cửa sổ đang mở (cửa sổ đăng nhập)
            CloseWindow(Application.Current.MainWindow);

            // Đặt cửa sổ chính là cửa sổ hiện tại
            Application.Current.MainWindow = mainWindow;

            // Hiển thị cửa sổ chính
            mainWindow.Show();
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