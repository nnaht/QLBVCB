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
    internal class VM_AERAccount : VM_Base
    {
        private ObservableCollection<NHANVIEN> _AccountList;
        public ObservableCollection<NHANVIEN> AccountList { get { return _AccountList; } set { _AccountList = value; OnPropertyChanged(); } }

        private string _MANV;
        public string MANV { get => _MANV; set { _MANV = value; OnPropertyChanged(); } }

        private string _TENTK;
        public string TENTK { get => _TENTK; set { _TENTK = value; OnPropertyChanged(); } }

        private string _MATKHAU;
        public string MATKHAU { get => _MATKHAU; set { _MATKHAU = value; OnPropertyChanged(); } }
        public ICommand AddAccountCommand { get; set; }
        public ICommand EditAccountCommand { get; set; }
        public ICommand RemoveAccountCommand { get; set; }
        public ICollectionView AccountView { get; private set; }

        private NHANVIEN _AccountSelectedItem;
        public NHANVIEN AccountSelectedItem
        {
            get => _AccountSelectedItem;
            set
            {
                _AccountSelectedItem = value;
                OnPropertyChanged();
                if (AccountSelectedItem != null)
                {
                    TENTK = AccountSelectedItem.TENTK;
                    MATKHAU = AccountSelectedItem.MATKHAU;
                    MANV = AccountSelectedItem.MANV;
                }
            }
        }

        public VM_AERAccount()
        {
            AccountList = new ObservableCollection<NHANVIEN>(DataProvider.Ins.DB.NHANVIENs);
            var displayAccountList = DataProvider.Ins.DB.NHANVIENs.Where(x => x.MANV == MANV);
            var displayEmployeeList = DataProvider.Ins.DB.NHANVIENs.Where(x => x.MANV == MANV);
            AccountView = CollectionViewSource.GetDefaultView(AccountList);
            AccountView.Filter = FilterAccount;

            AddAccountCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TENTK) || string.IsNullOrEmpty(MATKHAU) || string.IsNullOrEmpty(MANV))
                    return false;
                if (displayAccountList == null)
                    return false;
                if (displayAccountList.Count() != 0)
                    return false;
                return true;
            }, (p) =>
            {
                if (displayAccountList.Count() != 0)
                    ShowCustomMessageBox("Nhân viên đã có tài khoản!");
                else if (displayEmployeeList.Count() == 0)
                    ShowCustomMessageBox("Không tồn tại mã nhân viên!");
                else if (DataProvider.Ins.DB.NHANVIENs.Where(x => x.TENTK == TENTK).Count() != 0)
                    ShowCustomMessageBox("Tên tài khoản đã tồn tại!");
                else
                {
                    var employee = DataProvider.Ins.DB.NHANVIENs.Where(x => x.MANV == MANV).SingleOrDefault();
                    employee.TENTK = TENTK;
                    employee.MATKHAU = MATKHAU;
                    DataProvider.Ins.DB.SaveChanges();
                    ShowCustomMessageBox("Thêm thành công!");
                }
            });

            EditAccountCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TENTK) || string.IsNullOrEmpty(MATKHAU) || string.IsNullOrEmpty(MANV))
                    return false;
                if (AccountSelectedItem == null)
                    return false;
                if (displayAccountList.Count() == 0)
                    return false;
                return true;
            }, (p) =>
            {
                var employee = DataProvider.Ins.DB.NHANVIENs.Where(x => x.MANV == MANV).SingleOrDefault();
                employee.TENTK = TENTK;
                employee.MATKHAU = MATKHAU;
                DataProvider.Ins.DB.SaveChanges();
                ShowCustomMessageBox("Sửa thành công!");
            });

            RemoveAccountCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                try
                {
                    if (MessageBox.Show("Xác nhận xóa?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        var employee = DataProvider.Ins.DB.NHANVIENs.Where(x => x.TENTK == TENTK).SingleOrDefault();
                        employee.TENTK = "";
                        employee.MATKHAU = "";
                        DataProvider.Ins.DB.SaveChanges();
                        AccountList.Remove(AccountSelectedItem);
                        ShowCustomMessageBox("Xóa thành công!");
                    }
                }
                catch (Exception ex)
                {
                    ShowCustomMessageBox("Không thể xóa!");
                }
            });
        }

        private string _SearchAccount;
        public string SearchAccount
        {
            get => _SearchAccount;
            set
            {
                _SearchAccount = value;
                OnPropertyChanged();
                FilterAccount();
            }
        }
        private bool FilterAccount(object item)
        {
            if (item is NHANVIEN ticket)
            {
                return string.IsNullOrEmpty(SearchAccount) || ticket.MANV.IndexOf(SearchAccount, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return false;
        }
        private void FilterAccount()
        {
            AccountView.Refresh();
        }
        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
    }
}
