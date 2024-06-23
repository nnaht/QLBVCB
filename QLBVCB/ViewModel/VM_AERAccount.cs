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
        private ObservableCollection<TAIKHOAN> _AccountList;
        public ObservableCollection<TAIKHOAN> AccountList { get { return _AccountList; } set { _AccountList = value; OnPropertyChanged(); } }

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

        private TAIKHOAN _AccountSelectedItem;
        public TAIKHOAN AccountSelectedItem
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
            AccountList = new ObservableCollection<TAIKHOAN>(DataProvider.Ins.DB.TAIKHOANs);
            var displayAccountList = DataProvider.Ins.DB.TAIKHOANs.Where(x => x.MANV == MANV);
            var displayEmployeeList = DataProvider.Ins.DB.NHANVIENs.Where(x => x.MANV == MANV);
            AccountView = CollectionViewSource.GetDefaultView(AccountList);
            AccountView.Filter = FilterAccount;

            AddAccountCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TENTK) || string.IsNullOrEmpty(MATKHAU) || string.IsNullOrEmpty(MANV))
                    return false;
                return true;
            }, (p) =>
            {
                if (displayAccountList.Count() != 0)
                    ShowCustomMessageBox("Nhân viên đã có tài khoản!");
                else if (displayEmployeeList.Count() == 0)
                    ShowCustomMessageBox("Không tồn tại mã nhân viên!");
                else
                {
                    var account = new TAIKHOAN() { TENTK = TENTK, MATKHAU = MATKHAU, MANV = MANV };
                    DataProvider.Ins.DB.TAIKHOANs.Add(account);
                    DataProvider.Ins.DB.SaveChanges();
                    AccountList.Add(account);
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
                var account = DataProvider.Ins.DB.TAIKHOANs.Where(x => x.TENTK == TENTK).SingleOrDefault();
                account.TENTK = TENTK;
                account.MATKHAU = MATKHAU;
                account.MANV = MANV;
                DataProvider.Ins.DB.SaveChanges();
                ShowCustomMessageBox("Sửa thành công!");
            });

            RemoveAccountCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (MessageBox.Show("Xác nhận xóa?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    DataProvider.Ins.DB.TAIKHOANs.Remove(AccountSelectedItem);
                    DataProvider.Ins.DB.SaveChanges();
                    AccountList.Remove(AccountSelectedItem);
                    ShowCustomMessageBox("Xóa thành công!");
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
            if (item is TAIKHOAN ticket)
            {
                return string.IsNullOrEmpty(SearchAccount) || ticket.TENTK.Contains(SearchAccount);
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
