using QLBVCB.Model;
using QLBVCB.UserControls;
using QLBVCB.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    internal class VM_ManageBooking : VM_Base
    {
        private ObservableCollection<string> _startLocation;
        private ObservableCollection<string> _destination;

        public ObservableCollection<string> StartLocation
        {
            get => _startLocation;
            set
            {
                _startLocation = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Destination
        {
            get => _destination;
            set
            {
                _destination = value;
                OnPropertyChanged();
            }
        }
        private string _MAMB;
        public string MAMB { get => _MAMB; set { _MAMB = value; OnPropertyChanged(); } }
        private string _MACB;
        public string MACB { get => _MACB; set { _MACB = value; OnPropertyChanged(); } }

        private string _selectedStartLocation;
        public string SelectedStartLocation
        {
            get => _selectedStartLocation;
            set
            {
                _selectedStartLocation = value;
                OnPropertyChanged();
                FilterFlights();
            }
        }

        private string _selectedDestination;
        public string SelectedDestination
        {
            get => _selectedDestination;
            set
            {
                _selectedDestination = value;
                OnPropertyChanged();
                FilterFlights();
            }
        }

        private ObservableCollection<CHUYENBAY> _FlightList;
        public ObservableCollection<CHUYENBAY> FlightList
        {
            get => _FlightList;
            set
            {
                _FlightList = value;
                OnPropertyChanged();
            }
        }

        private ICollectionView _flightView;
        public ICollectionView FlightView
        {
            get => _flightView;
            set
            {
                _flightView = value;
                OnPropertyChanged();
            }
        }

        private CHUYENBAY _FlightSelectedItem;

        public CHUYENBAY FlightSelectedItem
        {
            get => _FlightSelectedItem;
            set
            {
                _FlightSelectedItem = value;
                OnPropertyChanged();
                if (_FlightSelectedItem != null)
                {
                    MAMB = FlightSelectedItem.MAMB;
                    MACB = FlightSelectedItem.MACB;
                }
            }
        }
        public ICommand BuyTicketCommand { get; set; }
        public VM_ManageBooking()
        {
            FlightList = new ObservableCollection<CHUYENBAY>(DataProvider.Ins.DB.CHUYENBAYs);
            FlightView = CollectionViewSource.GetDefaultView(FlightList);
            FlightView.Filter = FilterFlights;
            BuyTicketCommand = new RelayCommand(ExecuteBuyTicketCommand);
            StartLocation = new ObservableCollection<string>
            {
                "Đà Nẵng, Việt Nam",
                "Hà Nội, Việt Nam",
                "TP. Hồ Chí Minh, Việt Nam"
            };
            Destination = new ObservableCollection<string>
            {
                "Đà Nẵng, Việt Nam",
                "Hà Nội, Việt Nam",
                "TP. Hồ Chí Minh, Việt Nam"
            };
        }

        private bool FilterFlights(object item)
        {
            if (item is CHUYENBAY flight)
            {
                bool startLocationMatch = string.IsNullOrEmpty(SelectedStartLocation) ||
                                          flight.MASB_CATCANH.Equals(ConvertToCode(SelectedStartLocation), StringComparison.OrdinalIgnoreCase);
                bool destinationMatch = string.IsNullOrEmpty(SelectedDestination) ||
                                        flight.MASB_HACANH.Equals(ConvertToCode(SelectedDestination), StringComparison.OrdinalIgnoreCase);
                return startLocationMatch && destinationMatch;
            }
            return false;
        }

        private string ConvertToCode(string location)
        {
            if (location == "Đà Nẵng, Việt Nam")
            {
                return "DAD";
            }
            else if (location == "Hà Nội, Việt Nam")
            {
                return "HAN";
            }
            else if (location == "TP. Hồ Chí Minh, Việt Nam")
            {
                return "SGN";
            }
            else
            {
                return "";
            }
        }

        private void FilterFlights()
        {
            FlightView.Refresh();
        }
        private async void ExecuteBuyTicketCommand(object obj)
        {
            if(MACB != null)
            {
                var chuyenBay = await GetChuyenBayAsync(MACB);
                int totalSeats = chuyenBay?.SO_GHE ?? 250;

                SeatingPlan seatingPlan = new SeatingPlan();
                seatingPlan.DataContext = new VM_SeatingChart(MACB, totalSeats);
                seatingPlan.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn chuyến bay");
            }
        }
        private Task<CHUYENBAY> GetChuyenBayAsync(string macb)
        {
            return Task.Run(() =>
            {
                return DataProvider.Ins.DB.CHUYENBAYs.SingleOrDefault(cb => cb.MACB == macb);
            });
        }
    }
}
