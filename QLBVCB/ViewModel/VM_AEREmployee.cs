using QLBVCB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
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

        private Nullable<bool> _HOATDONG;
        public Nullable<bool> HOATDONG { get => _HOATDONG; set { _HOATDONG = value; OnPropertyChanged(); } }
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
                    HOATDONG = EmployeeSelectedItem.HOATDONG;
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
                    || string.IsNullOrEmpty(VITRI) || string.IsNullOrEmpty(HOATDONG.ToString()))
                    return false;
                if (displayEmployeeList == null)
                    return false;
                if (displayEmployeeList.Count() != 0)
                    return false;
                return true;
            }, (p) =>
            {
                var employee = new NHANVIEN() { MANV = GetNextId(), HOTEN = HOTEN, NGAYSINH = NGAYSINH, GIOITINH = GIOITINH, CCCD = CCCD, DIACHI = DIACHI, SDT = SDT, EMAIL = EMAIL, LUONG = LUONG, VITRI = VITRI, HOATDONG = HOATDONG };
                DataProvider.Ins.DB.NHANVIENs.Add(employee);
                DataProvider.Ins.DB.SaveChanges();
                EmployeeList.Add(employee);
            });

            EditEmployeeCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(HOTEN) || string.IsNullOrEmpty(NGAYSINH.ToString()) || string.IsNullOrEmpty(GIOITINH) || string.IsNullOrEmpty(CCCD)
                    || string.IsNullOrEmpty(DIACHI) || string.IsNullOrEmpty(SDT) || string.IsNullOrEmpty(EMAIL) || string.IsNullOrEmpty(LUONG.ToString())
                    || string.IsNullOrEmpty(VITRI) || string.IsNullOrEmpty(HOATDONG.ToString()))
                    return false;
                if (EmployeeSelectedItem == null)
                    return false;
                if (displayEmployeeList == null && displayEmployeeList.Count() != 0)
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
                employee.HOATDONG = HOATDONG;
                DataProvider.Ins.DB.SaveChanges();
            });

            RemoveEmployeeCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(HOTEN) || string.IsNullOrEmpty(NGAYSINH.ToString()) || string.IsNullOrEmpty(GIOITINH) || string.IsNullOrEmpty(CCCD)
                    || string.IsNullOrEmpty(DIACHI) || string.IsNullOrEmpty(SDT) || string.IsNullOrEmpty(EMAIL) || string.IsNullOrEmpty(HOATDONG.ToString()))
                    if (EmployeeSelectedItem == null)
                        return false;
                if (displayEmployeeList != null && displayEmployeeList.Count() != 0)
                    return true;
                //foreach (var item in TicketList)
                //{
                //    if (LOAIMB == item.LOAIMB && HANGMB == item.HANGMB)
                //        return true;
                //}
                return false;
            }, (p) =>
            {
                DataProvider.Ins.DB.NHANVIENs.Remove(EmployeeSelectedItem);
                DataProvider.Ins.DB.SaveChanges();
                EmployeeList.Remove(EmployeeSelectedItem);
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
                return string.IsNullOrEmpty(SearchEmployee) || employee.HOTEN.Contains(SearchEmployee);
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
    }
}
