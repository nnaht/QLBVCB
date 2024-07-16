using OfficeOpenXml.Drawing.Chart;
using QLBVCB.Model;
using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    internal class VM_AEREmployee : VM_Base
    {
        private ObservableCollection<NHANVIEN> _EmployeeList;
        public ObservableCollection<NHANVIEN> EmployeeList { get { return _EmployeeList; } set { _EmployeeList = value; OnPropertyChanged(); } }

        private string _MANV;
        public string MANV { get => _MANV; set { _MANV = value; OnPropertyChanged(); } }

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

        private Nullable<decimal> _LUONG;
        public Nullable<decimal> LUONG { get => _LUONG; set { _LUONG = value; OnPropertyChanged(); } }

        private string _VITRI;
        public string VITRI { get => _VITRI; set { _VITRI = value; OnPropertyChanged(); } }

        private string _TENTK;
        public string TENTK { get => _TENTK; set { _TENTK = value; OnPropertyChanged(); } }

        private string _MATKHAU;
        public string MATKHAU { get => _MATKHAU; set { _MATKHAU = value; OnPropertyChanged(); } }
        public ICommand AddEmployeeCommand { get; set; }
        public ICommand EditEmployeeCommand { get; set; }
        public ICommand RemoveEmployeeCommand { get; set; }
        public ICollectionView EmployeeView { get; private set; }

        private NHANVIEN _EmployeeSelectedItem;
        public NHANVIEN EmployeeSelectedItem
        {
            get => _EmployeeSelectedItem;
            set
            {
                _EmployeeSelectedItem = value;
                OnPropertyChanged();
                if (EmployeeSelectedItem != null)
                {
                    MANV = EmployeeSelectedItem.MANV;
                    HOTEN = EmployeeSelectedItem.HOTEN;
                    NGAYSINH = EmployeeSelectedItem.NGAYSINH;
                    GIOITINH = EmployeeSelectedItem.GIOITINH;
                    CCCD = EmployeeSelectedItem.CCCD;
                    DIACHI = EmployeeSelectedItem.DIACHI;
                    SDT = EmployeeSelectedItem.SDT;
                    EMAIL = EmployeeSelectedItem.EMAIL;
                    LUONG = EmployeeSelectedItem.LUONG;
                    VITRI = EmployeeSelectedItem.VITRI;
                    TENTK = EmployeeSelectedItem.TENTK;
                    MATKHAU = EmployeeSelectedItem.MATKHAU;
                }
            }
        }

        public VM_AEREmployee()
        {
            EmployeeList = new ObservableCollection<NHANVIEN>(DataProvider.Ins.DB.NHANVIENs);
            var displayEmployeeList = DataProvider.Ins.DB.NHANVIENs.Where(x => x.MANV == MANV);
            EmployeeView = CollectionViewSource.GetDefaultView(EmployeeList);
            EmployeeView.Filter = FilterEmployee;

            AddEmployeeCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(HOTEN) || string.IsNullOrEmpty(NGAYSINH.ToString()) || string.IsNullOrEmpty(GIOITINH) || string.IsNullOrEmpty(CCCD)
                    || string.IsNullOrEmpty(DIACHI) || string.IsNullOrEmpty(SDT) || string.IsNullOrEmpty(EMAIL) || string.IsNullOrEmpty(LUONG.ToString())
                    || string.IsNullOrEmpty(VITRI) || string.IsNullOrWhiteSpace(TENTK) || string.IsNullOrWhiteSpace(MATKHAU))
                    return false;
                if (displayEmployeeList == null)
                    return false;
                if (displayEmployeeList.Count() != 0)
                    return false;
                return true;
            }, (p) =>
            {
                if (DataProvider.Ins.DB.NHANVIENs.Where(x => x.TENTK == TENTK).Count() != 0)
                    ShowCustomMessageBox("Tên tài khoản đã tồn tại!");
                else
                {
                    var employee = new NHANVIEN() { MANV = GetNextId(), HOTEN = HOTEN, NGAYSINH = NGAYSINH, GIOITINH = GIOITINH, CCCD = CCCD, DIACHI = DIACHI, SDT = SDT, EMAIL = EMAIL, LUONG = LUONG, VITRI = VITRI, TENTK = TENTK, MATKHAU = MATKHAU };
                    DataProvider.Ins.DB.NHANVIENs.Add(employee);
                    DataProvider.Ins.DB.SaveChanges();
                    EmployeeList.Add(employee);
                    ShowCustomMessageBox("Thêm thành công!");
                }
            });

            EditEmployeeCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(HOTEN) || string.IsNullOrEmpty(NGAYSINH.ToString()) || string.IsNullOrEmpty(GIOITINH) || string.IsNullOrEmpty(CCCD)
                    || string.IsNullOrEmpty(DIACHI) || string.IsNullOrEmpty(SDT) || string.IsNullOrEmpty(EMAIL) || string.IsNullOrEmpty(LUONG.ToString())
                    || string.IsNullOrEmpty(VITRI) || string.IsNullOrWhiteSpace(TENTK) || string.IsNullOrWhiteSpace(MATKHAU))
                    return false;
                if (EmployeeSelectedItem == null)
                    return false;
                if (displayEmployeeList.Count() == 0)
                    return false;
                return true;
            }, (p) =>
            {
                var employee = DataProvider.Ins.DB.NHANVIENs.Where(x => x.MANV == MANV).SingleOrDefault();
                employee.HOTEN = HOTEN;
                employee.NGAYSINH = NGAYSINH;
                employee.GIOITINH = GIOITINH;
                employee.CCCD = CCCD;
                employee.DIACHI = DIACHI;
                employee.SDT = SDT;
                employee.EMAIL = EMAIL;
                employee.LUONG = LUONG;
                employee.VITRI = VITRI;
                employee.TENTK = TENTK;
                employee.MATKHAU = MATKHAU;
                DataProvider.Ins.DB.SaveChanges();
                ShowCustomMessageBox("Sửa thành công!");
            });

            RemoveEmployeeCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                try
                {
                    if (MessageBox.Show("Xác nhận xóa?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        DataProvider.Ins.DB.NHANVIENs.Remove(EmployeeSelectedItem);
                        DataProvider.Ins.DB.SaveChanges();
                        EmployeeList.Remove(EmployeeSelectedItem);
                        ShowCustomMessageBox("Xóa thành công!");
                    }
                }
                catch (Exception ex)
                {
                    ShowCustomMessageBox("Không thể xóa!");
                }
            });
        }
        private string _SearchEmployee;
        public string SearchEmployee
        {
            get => _SearchEmployee;
            set
            {
                _SearchEmployee = value;
                OnPropertyChanged();
                FilterEmployee();
            }
        }
        private bool FilterEmployee(object item)
        {
            if (item is NHANVIEN employee)
            {
                return string.IsNullOrEmpty(SearchEmployee) || employee.HOTEN.IndexOf(SearchEmployee, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return false;
        }
        private void FilterEmployee()
        {
            EmployeeView.Refresh();
        }
        private String GetNextId()
        {
            var lastEmployee = DataProvider.Ins.DB.NHANVIENs.OrderByDescending(e => e.MANV).Take(1).FirstOrDefault();
            string temp = lastEmployee.MANV.ToString();
            if (temp != null)
            {
                temp = "NV" + (int.Parse(temp.Substring(2)) + 1).ToString().PadLeft(4, '0');
            }
            else
            {
                _ = "NV0001";
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