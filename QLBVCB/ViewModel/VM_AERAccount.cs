using QLBVCB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    internal class VM_AERAccount : VM_Base
    {
        private ObservableCollection<TAIKHOAN> _AccountList;
        public ObservableCollection<TAIKHOAN> AccountList { get { return _AccountList; } set { _AccountList = value; OnPropertyChanged(); } }

        private string _TENTK;
        public string TENTK { get => _TENTK; set { _TENTK = value; OnPropertyChanged(); } }

        private string _MATKHAU;
        public string MATKHAU { get => _MATKHAU; set { _MATKHAU = value; OnPropertyChanged(); } }

        private Nullable<bool> _HOATDONG;
        public Nullable<bool> HOATDONG { get => _HOATDONG; set { _HOATDONG = value; OnPropertyChanged(); } }

        private string _MANV;
        public string MANV { get => _MANV; set { _MANV = value; OnPropertyChanged(); } }
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
                    HOATDONG = AccountSelectedItem.HOATDONG;
                    MANV = AccountSelectedItem.MANV;
                }
            }
        }

        public VM_AERAccount()
        {
            AccountList = new ObservableCollection<TAIKHOAN>(DataProvider.Ins.DB.TAIKHOANs);
            var displayAccounttList = DataProvider.Ins.DB.TAIKHOANs.Where(x => x.TENTK == TENTK);
            AccountView = CollectionViewSource.GetDefaultView(AccountList);
            AccountView.Filter = FilterAccount;

            AddAccountCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TENTK) || string.IsNullOrEmpty(MATKHAU) || string.IsNullOrEmpty(HOATDONG.ToString()) || string.IsNullOrEmpty(MANV))
                    return false;
                if (displayAccounttList == null)
                    return false;
                if (displayAccounttList.Count() != 0)
                    return false;
                return true;
            }, (p) =>
            {
                var account = new TAIKHOAN() { TENTK = TENTK, MATKHAU = MATKHAU, HOATDONG = HOATDONG, MANV = MANV };
                DataProvider.Ins.DB.TAIKHOANs.Add(account);
                DataProvider.Ins.DB.SaveChanges();
                AccountList.Add(account);
            });

            EditAccountCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TENTK) || string.IsNullOrEmpty(MATKHAU) || string.IsNullOrEmpty(HOATDONG.ToString()) || string.IsNullOrEmpty(MANV))
                    return false;
                if (AccountSelectedItem == null)
                    return false;
                if (displayAccounttList == null && displayAccounttList.Count() != 0)
                    return false;
                return true;
            }, (p) =>
            {
                var account = DataProvider.Ins.DB.TAIKHOANs.Where(x => x.TENTK == TENTK).SingleOrDefault();
                account.TENTK = TENTK;
                account.MATKHAU = MATKHAU;
                account.HOATDONG = HOATDONG;
                account.MANV = MANV;
                DataProvider.Ins.DB.SaveChanges();
            });

            RemoveAccountCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TENTK) || string.IsNullOrEmpty(MATKHAU) || string.IsNullOrEmpty(HOATDONG.ToString()) || string.IsNullOrEmpty(MANV))
                    return false;
                if (AccountSelectedItem == null)
                    return false;
                if (displayAccounttList != null && displayAccounttList.Count() != 0)
                    return true;
                //foreach (var item in TicketList)
                //{
                //    if (LOAIMB == item.LOAIMB && HANGMB == item.HANGMB)
                //        return true;
                //}
                return false;
            }, (p) =>
            {
                DataProvider.Ins.DB.TAIKHOANs.Remove(AccountSelectedItem);
                DataProvider.Ins.DB.SaveChanges();
                AccountList.Remove(AccountSelectedItem);
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
    }
}
