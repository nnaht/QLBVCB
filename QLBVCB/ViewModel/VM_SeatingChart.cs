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
    public class Seat
    {
        public string SeatType { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string Label { get; set; }
    }

    public class Booking
    {
        public string Ma { get; set; }

        public int Hang { get; set; }
        public int Day { get; set; }
    }

    public class VM_SeatingChart : INotifyPropertyChanged
    {
        public List<Tuple<string, int, int>> selection = new List<Tuple<string, int, int>> { };
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
        public VM_SeatingChart(string flightId, int totalSeats)
        {
            Seats = new ObservableCollection<Seat>();
            _flightId = flightId;
            BookCommand = new RelayCommand(async (p) => await ExecuteBookCommand());
            GenerateSeats(totalSeats);
            SeatActionCommand = new RelayCommand(ExecuteShowSeatInfoCommand);
            Bookings = new List<Booking>();
        }
        private void ExecuteShowSeatInfoCommand(object obj)
        {
            // Xử lý khi người dùng nhấn vào một ghế
            Seat clickedSeat = obj as Seat;
            if (clickedSeat != null)
            {
                var seatTuple = new Tuple<string, int, int>(_flightId, clickedSeat.Row, clickedSeat.Column);
                if (selection.Contains(seatTuple))
                {
                    selection.Remove(seatTuple);
                }
                else
                {
                    selection.Add(seatTuple);
                    MessageBox.Show(selection.Count.ToString());
                }
            }
        }
        private async Task ExecuteBookCommand()
        {
            try
            {
                var chuyenBay = DataProvider.Ins.DB.CHUYENBAYs.SingleOrDefault(cb => cb.MACB == _flightId);
                if (chuyenBay != null)
                {
                    chuyenBay.SO_GHE -= selection.Count;
                    foreach (var item in selection)
                    {
                        var temp = new DADAT
                        {
                            MACB = item.Item1.ToString(),
                            Hang = item.Item2,
                            Day = item.Item3
                        };
                        DataProvider.Ins.DB.DADATs.Add(temp);
                        await DataProvider.Ins.DB.SaveChangesAsync();
                    }
                    MessageBox.Show("Booking completed successfully!");
                    selection.Clear();
                    Application.Current.Windows.OfType<Window>().FirstOrDefault()?.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while booking: " + ex.Message);
            }
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
