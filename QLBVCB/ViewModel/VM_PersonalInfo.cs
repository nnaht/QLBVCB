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
    internal class VM_PersonalInfo : VM_Base
    {
        private string _HOTEN;
        public string HOTEN { get => _HOTEN; set { _HOTEN = value; OnPropertyChanged(); } }

        private string _NGAYSINH;
        public string NGAYSINH { get => _NGAYSINH; set { _NGAYSINH = value; OnPropertyChanged(); } }

        private string _GIOITINH;
        public string GIOITINH { get => _GIOITINH; set { _GIOITINH = value; OnPropertyChanged(); } }

        private string _CCCD;
        public string CCCD { get => _CCCD; set { _CCCD = value; OnPropertyChanged(); } }

        private string _DIACHI;
        public string DIACHI { get => _DIACHI; set { _DIACHI = value; OnPropertyChanged(); } }

        private string _SDT;
        public string SDT { get => _SDT; set { _SDT = value; OnPropertyChanged(); } }

        private string _EMAIL;
        public string EMAIL { get => _EMAIL; set { _EMAIL = value; OnPropertyChanged(); } }

        private string _TENTK;
        public string TENTK { get => _TENTK; set { _TENTK = value; OnPropertyChanged(); } }
        public ICommand ChangePasswordCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public VM_PersonalInfo()
        { 
            UpdateData();
            ChangePasswordCommand = new RelayCommand<Button>((p) => { return true; }, (p) => { ChangePassword cp = new ChangePassword(); cp.DataContext = new VM_ChangePassword(); cp.ShowDialog(); });
            ExitCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                Application.Current.Windows.OfType<PersonalInfo>().FirstOrDefault()?.Close();
            });
        }
        private void UpdateData()
        {
            HOTEN = EmployeeAccountLogin.HOTEN + CustomerAccountLogin.HOTEN;
            NGAYSINH = EmployeeAccountLogin.NGAYSINH.ToString() + CustomerAccountLogin.NGAYSINH.ToString();
            GIOITINH = EmployeeAccountLogin.GIOITINH + CustomerAccountLogin.GIOITINH;
            CCCD = EmployeeAccountLogin.CCCD + CustomerAccountLogin.CCCD;
            DIACHI = EmployeeAccountLogin.DIACHI + CustomerAccountLogin.DIACHI;
            SDT = EmployeeAccountLogin.SDT + CustomerAccountLogin.SDT;
            EMAIL = EmployeeAccountLogin.EMAIL + CustomerAccountLogin.EMAIL;
            TENTK = EmployeeAccountLogin.TENTK + CustomerAccountLogin.TENTK;
        }
    }
}
