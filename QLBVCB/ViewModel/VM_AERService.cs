using OfficeOpenXml.Style;
using OfficeOpenXml;
using QLBVCB.Model;
using QLBVCB.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Forms;
using System;
using QLBVCB.View;
using System.Globalization;
using System.Windows;

namespace QLBVCB.ViewModel
{


    internal class VM_AERService : VM_Base
    {
        private ObservableCollection<DICHVU> _ServiceList;
        public ObservableCollection<DICHVU> ServiceList
        {
            get { return _ServiceList; }
            set { _ServiceList = value; OnPropertyChanged(); }
        }

        private string _MADV;
        public string MADV { get => _MADV; set { _MADV = value; OnPropertyChanged(); } }

        private string _LOAIDV;
        public string LOAIDV { get => _LOAIDV; set { _LOAIDV = value; OnPropertyChanged(); } }

        private string _TENDV;
        public string TENDV { get => _TENDV; set { _TENDV = value; OnPropertyChanged(); } }

        private int? _SOLUONG;
        public int? SOLUONG { get => _SOLUONG; set { _SOLUONG = value; OnPropertyChanged(); } }

        private decimal _DONGIA;
        public decimal DONGIA
        {
            get => _DONGIA;
            set
            {
                _DONGIA = value;
                OnPropertyChanged();
            }
        }

        // Formatted property for DONGIA
        public string FormattedDONGIA => DONGIA.ToString("#,##0.###");

        public ICommand AddServiceCommand { get; set; }
        public ICommand EditServiceCommand { get; set; }
        public ICommand RemoveServiceCommand { get; set; }

        private DICHVU _ServiceSelectedItem;
        public DICHVU ServiceSelectedItem
        {
            get => _ServiceSelectedItem;
            set
            {
                _ServiceSelectedItem = value;
                OnPropertyChanged();
                if (ServiceSelectedItem != null)
                {
                    MADV = ServiceSelectedItem.MADV;
                    LOAIDV = ServiceSelectedItem.LOAIDV;
                    TENDV = ServiceSelectedItem.TENDV;
                    SOLUONG = ServiceSelectedItem.SOLUONG;
                    DONGIA = ServiceSelectedItem.DONGIA;
                }
            }
        }

        private string _SearchKeyword;
        public string SearchKeyword
        {
            get => _SearchKeyword;
            set
            {
                _SearchKeyword = value;
                OnPropertyChanged();
                FilterService();
            }
        }

        public ICollectionView ServiceView { get; private set; }

        public VM_AERService()
        {
            ServiceList = new ObservableCollection<DICHVU>(DataProvider.Ins.DB.DICHVUs);
            var displayServicecList = DataProvider.Ins.DB.DICHVUs.Where(x => x.MADV == MADV);
            ServiceView = CollectionViewSource.GetDefaultView(ServiceList);
            ServiceView.Filter = FilterService;

            AddServiceCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MADV) || string.IsNullOrEmpty(LOAIDV) || string.IsNullOrEmpty(TENDV))
                    return false;
                if (displayServicecList == null)
                    return false;
                if (displayServicecList.Count() != 0)
                    return false;
                return true;
            }, (p) =>
            {
                var service = new DICHVU() { MADV = MADV, TENDV = TENDV, SOLUONG = SOLUONG, DONGIA = DONGIA, LOAIDV = LOAIDV };
                DataProvider.Ins.DB.DICHVUs.Add(service);
                DataProvider.Ins.DB.SaveChanges();
                ServiceList.Add(service);
                ShowCustomMessageBox("Thêm thành công!");
            });

            EditServiceCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MADV) || string.IsNullOrEmpty(LOAIDV) || string.IsNullOrEmpty(TENDV))
                    return false;
                if (ServiceSelectedItem == null)
                    return false;
                if (displayServicecList.Count() == 0)
                    return false;
                return true;
            }, (p) =>
            {
                var service = DataProvider.Ins.DB.DICHVUs.Find(MADV);
                if (service != null)
                {
                    service.LOAIDV = LOAIDV;
                    service.TENDV = TENDV;
                    service.SOLUONG = SOLUONG;
                    service.DONGIA = DONGIA;
                    DataProvider.Ins.DB.SaveChanges();
                    ShowCustomMessageBox("Sửa thành công!");
                }
            });

            RemoveServiceCommand = new RelayCommand<object>((p) =>
            {
                return ServiceSelectedItem != null;
            }, (p) =>
            {
                try
                {
                    if (System.Windows.MessageBox.Show("Xác nhận xóa?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        DataProvider.Ins.DB.DICHVUs.Remove(ServiceSelectedItem);
                        DataProvider.Ins.DB.SaveChanges();
                        ServiceList.Remove(ServiceSelectedItem);
                        ShowCustomMessageBox("Xóa thành công!");
                    }
                }
                catch (Exception ex)
                {
                    ShowCustomMessageBox("Không thể xóa!");
                }
            });
        }

        private bool FilterService(object item)
        {
            if (item is DICHVU service)
            {
                return string.IsNullOrEmpty(SearchKeyword) || service.TENDV.IndexOf(SearchKeyword, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return false;
        }

        private void FilterService()
        {
            ServiceView.Refresh();
        }
        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
    }
}
