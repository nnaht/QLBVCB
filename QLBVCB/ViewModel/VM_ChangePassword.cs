using QLBVCB.Model;
using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    internal class VM_ChangePassword : VM_Base
    {
        private string _ConfirmText;
        public string ConfirmText { get => _ConfirmText; set { _ConfirmText = value; OnPropertyChanged(); } }

        private string _CurrentPassword;
        public string CurrentPassword { get => _CurrentPassword; set { _CurrentPassword = value; OnPropertyChanged(); } }

        private string _NewPassword;
        public string NewPassword { get => _NewPassword; set { _NewPassword = value; OnPropertyChanged(); } }

        private string _ConfirmNewPassword;
        public string ConfirmNewPassword { get => _ConfirmNewPassword; set { _ConfirmNewPassword = value; OnPropertyChanged(); } }
        public ICommand ConfirmCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand CurrentPasswordChangedCommand { get; set; }
        public ICommand NewPasswordChangedCommand { get; set; }
        public ICommand ConfirmNewPasswordChangedCommand { get; set; }
        public VM_ChangePassword()
        {
            CurrentPasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { CurrentPassword = p.Password; });
            NewPasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { NewPassword = p.Password; });
            ConfirmNewPasswordChangedCommand = new RelayCommand<PasswordBox>((p) =>
            {
                return true;
            }, (p) =>
            {
                ConfirmNewPassword = p.Password;
                if (!string.IsNullOrEmpty(ConfirmNewPassword))
                { 
                    if (ConfirmNewPassword != NewPassword)
                        ConfirmText = "Mật khẩu không khớp.";
                    else
                        ConfirmText = "";
                }
                else
                {
                    ConfirmText = "";
                }
            });
            ConfirmCommand = new RelayCommand(ExecuteConfirmCommand);
            ExitCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                Application.Current.Windows.OfType<ChangePassword>().FirstOrDefault()?.Close();
            });
        }
        private void ExecuteConfirmCommand(object obj)
        {
            if (position < 3)
            {
                if (CurrentPassword != EmployeeAccountLogin.MATKHAU)
                    ShowCustomMessageBox("Sai mật khẩu!");
                else
                {
                    EmployeeAccountLogin.MATKHAU = NewPassword;
                    DataProvider.Ins.DB.SaveChanges();
                    ShowCustomMessageBox("Đổi mật khẩu thành công!");
                    Application.Current.Windows.OfType<ChangePassword>().FirstOrDefault()?.Close();
                }
            }    
            else if (position == 3)
            {
                if (CurrentPassword != CustomerAccountLogin.MATKHAU)
                    ShowCustomMessageBox("Sai mật khẩu!");
                else
                {
                    CustomerAccountLogin.MATKHAU = NewPassword;
                    DataProvider.Ins.DB.SaveChanges();
                    ShowCustomMessageBox("Đổi mật khẩu thành công!");
                    Application.Current.Windows.OfType<ChangePassword>().FirstOrDefault()?.Close();
                }
            }
        }
        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
    }
}
