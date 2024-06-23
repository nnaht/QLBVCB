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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLBVCB.ViewModel
{
    internal class VM_CustomerRegister : VM_Base
    {
        private string _MAKH;
        public string MAKH { get => _MAKH; set { _MAKH = value; OnPropertyChanged(); } }

        private string _HOTEN;
        public string HOTEN { get => _HOTEN; set { _HOTEN = value; OnPropertyChanged(); } }

        private Nullable<System.DateTime> _NGAYSINH;
        public Nullable<System.DateTime> NGAYSINH { get => _NGAYSINH; set { _NGAYSINH = value; OnPropertyChanged(); } }

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

        private string _MATKHAU;
        public string MATKHAU { get => _MATKHAU; set { _MATKHAU = value; OnPropertyChanged(); } }
        public ICommand SignInCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        public VM_CustomerRegister()
        {
            ExitCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                Application.Current.Windows.OfType<CustomerRegister>().FirstOrDefault()?.Close();
            });
            SignInCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(HOTEN) || string.IsNullOrEmpty(NGAYSINH.ToString()) || string.IsNullOrEmpty(GIOITINH) || string.IsNullOrEmpty(CCCD)
                || string.IsNullOrEmpty(DIACHI) || string.IsNullOrEmpty(SDT) || string.IsNullOrEmpty(EMAIL) || string.IsNullOrEmpty(TENTK) || string.IsNullOrEmpty(MATKHAU))
                    return false;
                return true;
            }, (p) =>
            {
                ExecuteSignInCommand(p);
            });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { MATKHAU = p.Password; });
        }

        private void ExecuteSignInCommand(object obj)
        {
            var isExistCCCD = DataProvider.Ins.DB.KHACHHANGs.Where(x => x.CCCD == CCCD).SingleOrDefault();
            var isExistSDT = DataProvider.Ins.DB.KHACHHANGs.Where(x => x.SDT == SDT).SingleOrDefault();
            var isExistEMAIL = DataProvider.Ins.DB.KHACHHANGs.Where(x => x.EMAIL == EMAIL).SingleOrDefault();
            var isExistTENTK = DataProvider.Ins.DB.KHACHHANGs.Where(x => x.TENTK == TENTK).SingleOrDefault();
            if (isExistCCCD != null)
                ShowCustomMessageBox("Căn cước công dân này đã tồn tại!");
            else if (isExistSDT != null)
                ShowCustomMessageBox("Số điện thoại này đã tồn tại!");
            else if (isExistEMAIL != null)
                ShowCustomMessageBox("Email này đã được đăng ký!");
            else if (isExistTENTK != null)
                ShowCustomMessageBox("Tên tài khoản này đã được đăng ký!");
            else
            {
                var customer = new KHACHHANG() { MAKH = GetNextId(), HOTEN = HOTEN, NGAYSINH = NGAYSINH, GIOITINH = GIOITINH, CCCD = CCCD, DIACHI = DIACHI, SDT = SDT, EMAIL = EMAIL, TENTK = TENTK, MATKHAU = MATKHAU };
                DataProvider.Ins.DB.KHACHHANGs.Add(customer);
                DataProvider.Ins.DB.SaveChanges();
                ShowCustomMessageBox("Đăng ký thành công!");
                Application.Current.Windows.OfType<CustomerRegister>().FirstOrDefault()?.Close();
            }
        }
        private String GetNextId()
        {
            var lastCustomer = DataProvider.Ins.DB.KHACHHANGs.OrderByDescending(e => e.MAKH).Take(1).FirstOrDefault();
            string temp = lastCustomer.MAKH.ToString();
            if (temp != null)
            {
                temp = "KH" + (int.Parse(temp.Substring(2)) + 1).ToString().PadLeft(4, '0');
            }
            else
            {
                _ = "KH0001";
            }
            return temp;
        }
        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
    }
}
