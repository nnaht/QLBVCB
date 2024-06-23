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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLBVCB.ViewModel
{
    internal class VM_AERFlight : VM_Base
    {
        private ObservableCollection<CHUYENBAY> _FlightList;
        public ObservableCollection<CHUYENBAY> FlightList { get { return _FlightList; } set { _FlightList = value; OnPropertyChanged(); } }
        private string _MACB;
        public string MACB { get => _MACB; set { _MACB = value; OnPropertyChanged(); } }

        private string _MAMB;
        public string MAMB { get => _MAMB; set { _MAMB = value; OnPropertyChanged(); } }

        private string _THOIGIAN_CATCANH;
        public string THOIGIAN_CATCANH { get => _THOIGIAN_CATCANH; set { _THOIGIAN_CATCANH = value; OnPropertyChanged(); } }

        private string _THOIGIAN_HACANH;
        public string THOIGIAN_HACANH { get => _THOIGIAN_HACANH; set { _THOIGIAN_HACANH = value; OnPropertyChanged(); } }

        private string _TRANGTHAI;
        public string TRANGTHAI { get => _TRANGTHAI; set { _TRANGTHAI = value; OnPropertyChanged(); } }
        private string _SO_GHE;
        public string SO_GHE { get => _SO_GHE; set { _SO_GHE = value; OnPropertyChanged(); } }
        private string _MASB_CATCANH;
        public string MASB_CATCANH { get => _MASB_CATCANH; set { _MASB_CATCANH = value; OnPropertyChanged(); } }
        private string _MASB_HACANH;
        public string MASB_HACANH { get => _MASB_HACANH; set { _MASB_HACANH = value; OnPropertyChanged(); } }
        public ICommand AddFlightCommand { get; set; }
        public ICommand EditFlightCommand { get; set; }
        public ICommand RemoveFlightCommand { get; set; }
        private string _SearchKeyword;
        public string SearchKeyword
        {
            get => _SearchKeyword;
            set
            {
                _SearchKeyword = value;
                OnPropertyChanged();
                FilterFlight();
            }
        }

        public ICollectionView FlightView { get; private set; }

        private CHUYENBAY _FlightSelectedItem;
        public CHUYENBAY FlightSelectedItem
        {
            get => _FlightSelectedItem;
            set
            {
                _FlightSelectedItem = value;
                OnPropertyChanged();
                if (FlightSelectedItem != null)
                {
                    MACB = FlightSelectedItem.MACB;
                    MAMB = FlightSelectedItem.MAMB;
                    THOIGIAN_CATCANH = FlightSelectedItem.THOIGIAN_CATCANH.ToString();
                    THOIGIAN_HACANH = FlightSelectedItem.THOIGIAN_HACANH.ToString();
                    TRANGTHAI = FlightSelectedItem.TRANGTHAI;
                    SO_GHE = FlightSelectedItem.SO_GHE.ToString();
                    MASB_CATCANH = FlightSelectedItem.MASB_CATCANH;
                    MASB_HACANH = FlightSelectedItem.MASB_HACANH;
                }
            }
        }
        public VM_AERFlight()
        {
            FlightList = new ObservableCollection<CHUYENBAY>(DataProvider.Ins.DB.CHUYENBAYs);
            var displayFlightList = DataProvider.Ins.DB.CHUYENBAYs.Where(x => x.MACB == MACB);
            FlightView = CollectionViewSource.GetDefaultView(FlightList);
            FlightView.Filter = FilterFlight;
            AddFlightCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MACB) || string.IsNullOrEmpty(MAMB) || string.IsNullOrEmpty(THOIGIAN_CATCANH) || string.IsNullOrEmpty(THOIGIAN_HACANH) ||
                string.IsNullOrEmpty(TRANGTHAI) || string.IsNullOrEmpty(SO_GHE) || string.IsNullOrEmpty(MASB_CATCANH) || string.IsNullOrEmpty(MASB_HACANH))
                    return false;
                if (displayFlightList == null)
                    return false;
                if (displayFlightList.Count() != 0)
                    return false;
                return true;
            }, (p) =>
            {
                var flight = new CHUYENBAY()
                {
                    MACB = MACB,
                    MAMB = MAMB,
                    THOIGIAN_CATCANH = DateTime.Parse(THOIGIAN_CATCANH),
                    THOIGIAN_HACANH = DateTime.Parse(THOIGIAN_HACANH),
                    TRANGTHAI = TRANGTHAI,
                    SO_GHE = int.Parse(SO_GHE),
                    MASB_CATCANH = MASB_CATCANH,
                    MASB_HACANH = MASB_HACANH
                };
                DataProvider.Ins.DB.CHUYENBAYs.Add(flight);
                DataProvider.Ins.DB.SaveChanges();
                FlightList.Add(flight);
                ShowCustomMessageBox("Thêm thành công!");
            });

            EditFlightCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MACB) || string.IsNullOrEmpty(MAMB) || string.IsNullOrEmpty(THOIGIAN_CATCANH) || string.IsNullOrEmpty(THOIGIAN_HACANH) ||
                string.IsNullOrEmpty(TRANGTHAI) || string.IsNullOrEmpty(SO_GHE) || string.IsNullOrEmpty(MASB_CATCANH) || string.IsNullOrEmpty(MASB_HACANH))
                    return false;
                if (FlightSelectedItem == null)
                    return false;
                if (displayFlightList.Count() == 0)
                    return false;
                return true;
            }, (p) =>
            {
                var flight = DataProvider.Ins.DB.CHUYENBAYs.Where(x => x.MACB == MACB).SingleOrDefault();
                flight.MACB = MACB;
                flight.MAMB = MAMB;
                flight.THOIGIAN_CATCANH = DateTime.Parse(THOIGIAN_CATCANH);
                flight.THOIGIAN_HACANH = DateTime.Parse(THOIGIAN_HACANH);
                flight.TRANGTHAI = TRANGTHAI;
                flight.SO_GHE = int.Parse(SO_GHE);
                flight.MASB_CATCANH = MASB_CATCANH;
                flight.MASB_HACANH = MASB_HACANH;
                DataProvider.Ins.DB.SaveChanges();
                ShowCustomMessageBox("Sửa thành công!");
            });

            RemoveFlightCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (MessageBox.Show("Xác nhận xóa?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    DataProvider.Ins.DB.CHUYENBAYs.Remove(FlightSelectedItem);
                    DataProvider.Ins.DB.SaveChanges();

                    FlightList.Remove(FlightSelectedItem);
                    ShowCustomMessageBox("Xóa thành công!");
                }
            });
        }
        private bool FilterFlight(object item)
        {
            if (item is CHUYENBAY flight)
            {
                return string.IsNullOrEmpty(SearchKeyword) || flight.MASB_HACANH.StartsWith(SearchKeyword, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private void FilterFlight()
        {
            FlightView.Refresh();
        }
        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
    }
}
