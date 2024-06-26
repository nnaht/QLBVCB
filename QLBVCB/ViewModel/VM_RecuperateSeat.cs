using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using QLBVCB.Model;
using System.Windows.Input;
using System.Windows.Controls;
using QLBVCB.View;

namespace QLBVCB.ViewModel
{

    public class VM_RecuperateSeat : INotifyPropertyChanged
    {
        private List<Tuple<string, int, int>> selection = new List<Tuple<string, int, int>> { };
        public static List<Booking> Bookings { get; private set; }
        public ObservableCollection<Seat> Seats
        {
            get => _seats;
            set
            {
                _seats = value;
                OnPropertyChanged(nameof(Seats));
            }
        }
        private ObservableCollection<Seat> _seats;
        private string _flightId;
        public string FlightId
        {
            get => _flightId;
            set
            {
                _flightId = value;
                OnPropertyChanged(nameof(FlightId));
            }
        }
        public ICommand BookCommand { get; set; }
        public ICommand SeatActionCommand { get; }
        public VM_RecuperateSeat(string flightId, int totalSeats, List<Tuple<string, int, int>> selection)
        {
            Seats = new ObservableCollection<Seat>();
            this.selection = selection;
            _flightId = flightId;
            BookCommand = new RelayCommand(async (p) => await ExecuteBookCommand());
            GenerateSeats(totalSeats);
            SeatActionCommand = new RelayCommand(ExecuteShowSeatInfoCommand);
            Bookings = new List<Booking>();
        }
        private void ExecuteShowSeatInfoCommand(object obj)
        {
            Seat clickedSeat = obj as Seat;
            if (clickedSeat != null)
            {
                clickedSeat.IsPicking = !clickedSeat.IsPicking;
                var seatTuple = new Tuple<string, int, int>(_flightId, clickedSeat.Row, clickedSeat.Column);
                if (selection.Contains(seatTuple))
                {
                    selection.Remove(seatTuple);
                    ShowCustomMessageBox("Đã hủy chọn ghế " + setSeat(clickedSeat.Row, clickedSeat.Column));
                }
                else
                {
                    selection.Add(seatTuple);
                    ShowCustomMessageBox("Bạn đã chọn ghế " + setSeat(clickedSeat.Row, clickedSeat.Column));
                }
                clickedSeat.NotifyPropertyChanged(nameof(clickedSeat.IsPicking));
            }
        }
        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
        public string setSeat(int Hang, int Day)
        {
            string a = "";
            switch (Day)
            {
                case 0:
                    a = "A";
                    break;
                case 1:
                    a = "B";
                    break;
                case 2:
                    a = "C";
                    break;
                case 3:
                    a = "";
                    break;
                case 4:
                    a = "D";
                    break;
                case 5:
                    a = "E";
                    break;
                case 6:
                    a = "F";
                    break;
            }
            return Hang.ToString() + a ;
        }
        private async Task ExecuteBookCommand()
        {
            FillInfo fillInfo = new FillInfo();
            fillInfo.DataContext = new VM_FillInfo(selection,true);
            Application.Current.Windows.OfType<RecuperateSeat>().FirstOrDefault()?.Close();
            CloseWindow(Application.Current.MainWindow);
            Application.Current.MainWindow = fillInfo;
            fillInfo.ShowDialog();

        }

        private void GenerateSeats(int totalSeats)
        {
            int totalRows = (int)Math.Round((double)totalSeats / 6) + 1;
            int seatsPerRow = 7;
            int currentSeat = 0;
            Seats.Add(new Seat { SeatType = "Label", Row = 0, Column = 0, Label = "A" });
            Seats.Add(new Seat { SeatType = "Label", Row = 0, Column = 1, Label = "B" });
            Seats.Add(new Seat { SeatType = "Label", Row = 0, Column = 2, Label = "C" });
            Seats.Add(new Seat { SeatType = "Label", Row = 0, Column = 3, Label = "" });
            Seats.Add(new Seat { SeatType = "Label", Row = 0, Column = 4, Label = "D" });
            Seats.Add(new Seat { SeatType = "Label", Row = 0, Column = 5, Label = "E" });
            Seats.Add(new Seat { SeatType = "Label", Row = 0, Column = 6, Label = "F" });

            for (int row = 1; row <= totalRows; row++)
            {

                var chuyenBay = DataProvider.Ins.DB.CHUYENBAYs.SingleOrDefault(cb => cb.MACB == _flightId);
                var DADAT = DataProvider.Ins.DB.DADATs.ToList();

                for (int col = 0; col < seatsPerRow; col++)
                {
                    if (currentSeat < totalSeats)
                    {
                        if (DADAT.Any(d => d.MACB == _flightId && d.Day == col && d.Hang == row))
                        {
                            Seats.Add(new Seat { SeatType = "Booked", Row = row, Column = col });
                            currentSeat++;
                        }
                        else if (col == 3)
                        {
                            Seats.Add(new Seat { SeatType = "Label", Row = row, Column = 3, Label = row.ToString() });
                        }
                        else if (col != 3)
                        {
                            var seatType = (row < 6) ? "Economy" : "Empty";
                            Seats.Add(new Seat { SeatType = seatType, Row = row, Column = col });
                            currentSeat++;
                        }

                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
