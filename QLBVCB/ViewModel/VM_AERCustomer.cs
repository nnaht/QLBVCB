using QLBVCB.Model;
using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    internal class VM_AERCustomer : VM_Base
    {
        private ObservableCollection<KHACHHANG> _CustomerList;
        public ObservableCollection<KHACHHANG> CustomerList { get { return _CustomerList; } set { _CustomerList = value; OnPropertyChanged(); } }

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

        public ICommand AddCustomerCommand { get; set; }
        public ICommand EditCustomerCommand { get; set; }
        public ICommand RemoveCustomerCommand { get; set; }
        public ICollectionView CustomerView { get; private set; }

        private KHACHHANG _CustomerSelectedItem;
        public KHACHHANG CustomerSelectedItem
        {
            get => _CustomerSelectedItem;
            set
            {
                _CustomerSelectedItem = value;
                OnPropertyChanged();
                if (CustomerSelectedItem != null)
                {
                    MAKH = CustomerSelectedItem.MAKH;
                    HOTEN = CustomerSelectedItem.HOTEN;
                    NGAYSINH = CustomerSelectedItem.NGAYSINH;
                    GIOITINH = CustomerSelectedItem.GIOITINH;
                    CCCD = CustomerSelectedItem.CCCD;
                    DIACHI = CustomerSelectedItem.DIACHI;
                    SDT = CustomerSelectedItem.SDT;
                    EMAIL = CustomerSelectedItem.EMAIL;
                    TENTK = CustomerSelectedItem.TENTK;
                    MATKHAU = CustomerSelectedItem.MATKHAU;
                }
            }
        }

        public VM_AERCustomer()
        {
            CustomerList = new ObservableCollection<KHACHHANG>(DataProvider.Ins.DB.KHACHHANGs);
            var displayCustomerList = DataProvider.Ins.DB.KHACHHANGs.Where(x => x.MAKH == MAKH);
            CustomerView = CollectionViewSource.GetDefaultView(CustomerList);
            CustomerView.Filter = FilterCustomer;

            AddCustomerCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(HOTEN) || string.IsNullOrEmpty(NGAYSINH.ToString()) || string.IsNullOrEmpty(GIOITINH) || string.IsNullOrEmpty(CCCD)
                  || string.IsNullOrEmpty(DIACHI) || string.IsNullOrEmpty(SDT) || string.IsNullOrEmpty(EMAIL) || string.IsNullOrEmpty(TENTK) || string.IsNullOrEmpty(MATKHAU))
                    return false;
                if (displayCustomerList == null)
                    return false;
                if (displayCustomerList.Count() != 0)
                    return false;
                return true;
            }, (p) =>
            {
                if (DataProvider.Ins.DB.KHACHHANGs.Where(x => x.TENTK == TENTK).Count() != 0)
                    ShowCustomMessageBox("Tên tài khoản đã tồn tại!");
                else
                {
                    var customer = new KHACHHANG() { MAKH = GetNextId(), HOTEN = HOTEN, NGAYSINH = NGAYSINH, GIOITINH = GIOITINH, CCCD = CCCD, DIACHI = DIACHI, SDT = SDT, EMAIL = EMAIL, TENTK = TENTK, MATKHAU = MATKHAU };
                    DataProvider.Ins.DB.KHACHHANGs.Add(customer);
                    DataProvider.Ins.DB.SaveChanges();
                    CustomerList.Add(customer);
                    ShowCustomMessageBox("Thêm thành công!");
                }
            });

            EditCustomerCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(HOTEN) || string.IsNullOrEmpty(NGAYSINH.ToString()) || string.IsNullOrEmpty(GIOITINH) || string.IsNullOrEmpty(CCCD)
                  || string.IsNullOrEmpty(DIACHI) || string.IsNullOrEmpty(SDT) || string.IsNullOrEmpty(EMAIL) || string.IsNullOrEmpty(TENTK) || string.IsNullOrEmpty(MATKHAU))
                    return false;
                if (CustomerSelectedItem == null)
                    return false;
                if (displayCustomerList.Count() == 0)
                    return false;
                return true;
            }, (p) =>
            {
                var customer = DataProvider.Ins.DB.KHACHHANGs.Where(x => x.MAKH == MAKH).SingleOrDefault();
                customer.HOTEN = HOTEN;
                customer.NGAYSINH = NGAYSINH;
                customer.GIOITINH = GIOITINH;
                customer.CCCD = CCCD;
                customer.DIACHI = DIACHI;
                customer.SDT = SDT;
                customer.EMAIL = EMAIL;
                customer.TENTK = TENTK;
                customer.MATKHAU = MATKHAU;
                DataProvider.Ins.DB.SaveChanges();
                ShowCustomMessageBox("Sửa thành công!");
            });

            RemoveCustomerCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (MessageBox.Show("Xác nhận xóa?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    DataProvider.Ins.DB.KHACHHANGs.Remove(CustomerSelectedItem);
                    DataProvider.Ins.DB.SaveChanges();
                    CustomerList.Remove(CustomerSelectedItem);
                    ShowCustomMessageBox("Xóa thành công!");
                }
            });
        }

        private string _SearchCustomer;
        public string SearchCustomer
        {
            get => _SearchCustomer;
            set
            {
                _SearchCustomer = value;
                OnPropertyChanged();
                FilterCustomer();
            }
        }
        private bool FilterCustomer(object item)
        {
            if (item is KHACHHANG customer)
            {
                return string.IsNullOrEmpty(SearchCustomer) || customer.HOTEN.IndexOf(SearchCustomer, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return false;
        }
        private void FilterCustomer()
        {
            CustomerView.Refresh();
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
