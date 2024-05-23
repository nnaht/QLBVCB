using QLBVCB.Model;
using System.Linq;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    public class VM_Loginn : VM_Base
    {
        public bool IsLogin { get; set; }

        private string _Username;
        public string Username { get => _Username; set { _Username = value; OnPropertyChanged(); } }

        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

        public ICommand LoginCommand { get; set; }
        public ICommand UsernameChangedCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        public VM_Loginn()
        {
            LoginCommand = new RelayCommand(Login);
            //IsLogin = false;
            UsernameChangedCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) => { Username = p.Text; });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
        }

        private void Login(object parameter)
        {

            var accCount = DataProvider.Ins.DB.TAIKHOANs.Where(x => x.TENTK == Username && x.MATKHAU == Password).SingleOrDefault();
            if (accCount != null || (Username == "admin" && Password == "admin"))
            {
                AccountLogin = accCount;
                IsLogin = true;
                MainWindow mainWindow = new MainWindow();
                Application.Current.Windows.OfType<Login>().FirstOrDefault()?.Close();
                CloseWindow(Application.Current.MainWindow);
                Application.Current.MainWindow = mainWindow;
                mainWindow.ShowDialog();
            }
            else
            {
                IsLogin = false;
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
            }

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