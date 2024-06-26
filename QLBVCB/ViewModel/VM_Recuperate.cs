﻿using QLBVCB.Model;
using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows;

namespace QLBVCB.ViewModel
{
    internal class VM_Recuperate : VM_Base
    {
        private ObservableCollection<string> _startLocation;
        private ObservableCollection<string> _destination;
        private List<Tuple<string, int, int>> selection = new List<Tuple<string, int, int>> { };
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
                FlightView.Filter = FilterFlights;

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
                FlightView.Filter = FilterFlights;

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
        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
                FlightView.Filter = FilterFlights;
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
        public VM_Recuperate(List<Tuple<string, int, int>> selection)
        {
            this.selection = selection;
            FlightList = new ObservableCollection<CHUYENBAY>(DataProvider.Ins.DB.CHUYENBAYs);
            FlightView = CollectionViewSource.GetDefaultView(FlightList);
          //  FlightView.Filter = FilterFlights;
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
            SelectedDate = null;
        }

        private bool FilterFlights(object item)
        {
            if (item is CHUYENBAY flight)
            {
                bool startLocationMatch = string.IsNullOrEmpty(SelectedStartLocation) ||
                                          flight.MASB_CATCANH.Equals(ConvertToCode(SelectedStartLocation), StringComparison.OrdinalIgnoreCase);
                bool destinationMatch = string.IsNullOrEmpty(SelectedDestination) ||
                                        flight.MASB_HACANH.Equals(ConvertToCode(SelectedDestination), StringComparison.OrdinalIgnoreCase);
                bool dateMatch = (SelectedDate == null && flight.THOIGIAN_CATCANH > DateTime.Now) ||
                                 (SelectedDate != null && SelectedDate.Value.Date == flight.THOIGIAN_CATCANH?.Date);

                return startLocationMatch && destinationMatch && dateMatch;
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
            if (MACB != null)
            {
                var chuyenBay = await GetChuyenBayAsync(MACB);
                int totalSeats = chuyenBay?.SO_GHE ?? 250;

                RecuperateSeat recuperateSeat = new RecuperateSeat();
                recuperateSeat.DataContext = new VM_RecuperateSeat(MACB, totalSeats, selection);

                // Đóng cửa sổ hiện tại (RecuperateFlight)
                var currentWindow = System.Windows.Application.Current.Windows.OfType<RecuperateFlight>().FirstOrDefault(w => w.DataContext == this);
                currentWindow?.Close();

                // Mở cửa sổ mới
                recuperateSeat.ShowDialog();
            }
            else
            {
                System.Windows.MessageBox.Show("Vui lòng chọn chuyến bay");
            }
        }
        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
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
