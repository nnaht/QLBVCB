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
    internal class VM_AERService : VM_Base
    {
        private ObservableCollection<DICHVU> _ServiceList;
        public ObservableCollection<DICHVU> ServiceList { get { return _ServiceList; } set { _ServiceList = value; OnPropertyChanged(); } }

        private string _MADV;
        public string MADV { get => _MADV; set { _MADV = value; OnPropertyChanged(); } }

        private string _LOAIDV;
        public string LOAIDV { get => _LOAIDV; set { _LOAIDV = value; OnPropertyChanged(); } }

        private string _TENDV;
        public string TENDV { get => _TENDV; set { _TENDV = value; OnPropertyChanged(); } }
        private int _SOLUONG;
        public int SOLUONG { get => _SOLUONG; set { _SOLUONG = value; OnPropertyChanged(); } }
        private decimal _DONGIA;
        public decimal DONGIA { get => _DONGIA; set { _DONGIA = value; OnPropertyChanged(); } }
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
                    SOLUONG = int.Parse(ServiceSelectedItem.SOLUONG.ToString());
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
            var displayServiceList = DataProvider.Ins.DB.DICHVUs.Where(x => x.MADV == MADV);
            ServiceView = CollectionViewSource.GetDefaultView(ServiceList);
            ServiceView.Filter = FilterService;
            AddServiceCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MADV) || string.IsNullOrEmpty(LOAIDV) || string.IsNullOrEmpty(TENDV))
                    return false;
                if (displayServiceList == null)
                    return false;
                if (displayServiceList.Count() != 0)
                    return false;
                return true;
            }, (p) =>
            {
                var service = new DICHVU() { MADV = MADV, TENDV = TENDV, SOLUONG = SOLUONG, DONGIA = DONGIA, LOAIDV= LOAIDV };
                DataProvider.Ins.DB.DICHVUs.Add(service);
                DataProvider.Ins.DB.SaveChanges();
                ServiceList.Add(service);
            });

            EditServiceCommand = new RelayCommand<object>((p) =>
            {
                //if (string.IsNullOrEmpty(MASB) || string.IsNullOrEmpty(TEN_DICHVU) || string.IsNullOrEmpty(THANHPHO) || string.IsNullOrEmpty(QUOCGIA))
                  //  return false;
                if (ServiceSelectedItem == null)
                    return false;
                if (displayServiceList == null && displayServiceList.Count() != 0)
                    return false;
                return true;
            }, (p) =>
            {
                var service = DataProvider.Ins.DB.DICHVUs.Where(x => x.MADV == MADV).SingleOrDefault();
                service.MADV = MADV;
                service.TENDV = TENDV;
                service.LOAIDV = LOAIDV;
                service.SOLUONG = SOLUONG ;
                service.DONGIA = DONGIA;
                DataProvider.Ins.DB.SaveChanges();
            });

            RemoveServiceCommand = new RelayCommand<object>((p) =>
            {
                //if (string.IsNullOrEmpty(MASB) || string.IsNullOrEmpty(TEN_DICHVU) || string.IsNullOrEmpty(THANHPHO) || string.IsNullOrEmpty(QUOCGIA))
                 //   return false;
                if (ServiceSelectedItem == null)
                    return false;
                if (displayServiceList != null && displayServiceList.Count() != 0)
                    return true;
                foreach (var item in ServiceList)
                {
                   // if (TEN_DICHVU == item.TEN_DICHVU && THANHPHO == item.THANHPHO && QUOCGIA == item.QUOCGIA)
                        return true;
                }
                return false;
            }, (p) =>
            {
                DataProvider.Ins.DB.DICHVUs.Remove(ServiceSelectedItem);
                DataProvider.Ins.DB.SaveChanges();

                ServiceList.Remove(ServiceSelectedItem);
            });
        }
        private bool FilterService(object item)
        {
            if (item is DICHVU service)
            {
                return string.IsNullOrEmpty(SearchKeyword) || service.TENDV.StartsWith(SearchKeyword, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private void FilterService()
        {
            ServiceView.Refresh();
        }
    }
}
