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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLBVCB.ViewModel
{
    internal class VM_AERAirport : VM_Base
    {
        private ObservableCollection<SANBAY> _AirportList;
        public ObservableCollection<SANBAY> AirportList { get { return _AirportList; } set { _AirportList = value; OnPropertyChanged(); } }

        private string _MASB;
        public string MASB { get => _MASB; set { _MASB = value; OnPropertyChanged(); } }

        private string _TEN_SANBAY;
        public string TEN_SANBAY { get => _TEN_SANBAY; set { _TEN_SANBAY = value; OnPropertyChanged(); } }

        private string _THANHPHO;
        public string THANHPHO { get => _THANHPHO; set { _THANHPHO = value; OnPropertyChanged(); } }
        private string _QUOCGIA;
        public string QUOCGIA { get => _QUOCGIA; set { _QUOCGIA = value; OnPropertyChanged(); } }
        public ICommand AddAirportCommand { get; set; }
        public ICommand EditAirportCommand { get; set; }
        public ICommand RemoveAirportCommand { get; set; }
        private SANBAY _AirportSelectedItem;
        public SANBAY AirportSelectedItem
        {
            get => _AirportSelectedItem;
            set
            {
                _AirportSelectedItem = value;
                OnPropertyChanged();
                if (AirportSelectedItem != null)
                {
                    MASB = AirportSelectedItem.MASB;
                    TEN_SANBAY = AirportSelectedItem.TEN_SANBAY;
                    THANHPHO = AirportSelectedItem.THANHPHO;
                    QUOCGIA = AirportSelectedItem.QUOCGIA;
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
                FilterAirport();
            }
        }

        public ICollectionView AirportView { get; private set; }
        public VM_AERAirport()
        {
            AirportList = new ObservableCollection<SANBAY>(DataProvider.Ins.DB.SANBAYs);
            var displayAirportList = DataProvider.Ins.DB.SANBAYs.Where(x => x.MASB == MASB);
            AirportView = CollectionViewSource.GetDefaultView(AirportList);
            AirportView.Filter = FilterAirport;
            AddAirportCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MASB) || string.IsNullOrEmpty(TEN_SANBAY) || string.IsNullOrEmpty(THANHPHO) || string.IsNullOrEmpty(QUOCGIA))
                    return false;
                if (displayAirportList == null)
                    return false;
                if (displayAirportList.Count() != 0)
                    return false;
                return true;
            }, (p) =>
            {
                var airport = new SANBAY() { MASB = MASB, TEN_SANBAY = TEN_SANBAY, THANHPHO = THANHPHO, QUOCGIA = QUOCGIA };
                DataProvider.Ins.DB.SANBAYs.Add(airport);
                DataProvider.Ins.DB.SaveChanges();
                AirportList.Add(airport);
            });

            EditAirportCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MASB) || string.IsNullOrEmpty(TEN_SANBAY) || string.IsNullOrEmpty(THANHPHO) || string.IsNullOrEmpty(QUOCGIA))
                    return false;
                if (AirportSelectedItem == null)
                    return false;
                if (displayAirportList == null && displayAirportList.Count() != 0)
                    return false;
                return true;
            }, (p) =>
            {
                var airport = DataProvider.Ins.DB.SANBAYs.Where(x => x.MASB == MASB).SingleOrDefault();
                airport.MASB = MASB;
                airport.TEN_SANBAY = TEN_SANBAY;
                airport.THANHPHO = THANHPHO;
                airport.QUOCGIA = QUOCGIA;
                DataProvider.Ins.DB.SaveChanges();
            });

            RemoveAirportCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MASB) || string.IsNullOrEmpty(TEN_SANBAY) || string.IsNullOrEmpty(THANHPHO) || string.IsNullOrEmpty(QUOCGIA))
                    return false;
                if (AirportSelectedItem == null)
                    return false;
                if (displayAirportList != null && displayAirportList.Count() != 0)
                    return true;
                foreach (var item in AirportList)
                {
                    if (TEN_SANBAY == item.TEN_SANBAY && THANHPHO == item.THANHPHO && QUOCGIA == item.QUOCGIA)
                        return true;
                }
                return false;
            }, (p) =>
            {
                DataProvider.Ins.DB.SANBAYs.Remove(AirportSelectedItem);
                DataProvider.Ins.DB.SaveChanges();

                AirportList.Remove(AirportSelectedItem);
            });
        }
        private bool FilterAirport(object item)
        {
            if (item is SANBAY airport)
            {
                return string.IsNullOrEmpty(SearchKeyword) || airport.TEN_SANBAY.StartsWith(SearchKeyword, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private void FilterAirport()
        {
            AirportView.Refresh();
        }
    }
}
