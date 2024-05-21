using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

namespace QLBVCB.ViewModel
{
    public class Seat
    {
        public string SeatType { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string Label { get; set; } 
    }
    public class VM_SeatingChart : INotifyPropertyChanged
    {
        public ObservableCollection<Seat> Seats { get; set; }
        public string _flightId;
        public VM_SeatingChart()
        {
            Seats = new ObservableCollection<Seat>();
            GenerateSeats();
        }
        public VM_SeatingChart(string flightId)
        {
            Seats = new ObservableCollection<Seat>();
            GenerateSeats();
            _flightId = flightId;
        }

        private void GenerateSeats()
        {
            int totalSeats = 250;
            int totalRows = 42;
            int seatsPerRow = 7; // số cột bao gồm cột giữa là số hàng
            int currentSeat = 0;
            // Thêm hàng đầu tiên với các chữ cái a-f
            Seats.Add(new Seat { SeatType = "Label", Row = 0, Column = 0, Label = "A" });
            Seats.Add(new Seat { SeatType = "Label", Row = 0, Column = 1, Label = "B" });
            Seats.Add(new Seat { SeatType = "Label", Row = 0, Column = 2, Label = "C" });
            Seats.Add(new Seat { SeatType = "Label", Row = 0, Column = 3, Label = "" });
            Seats.Add(new Seat { SeatType = "Label", Row = 0, Column = 4, Label = "D" });
            Seats.Add(new Seat { SeatType = "Label", Row = 0, Column = 5, Label = "E" });
            Seats.Add(new Seat { SeatType = "Label", Row = 0, Column = 6, Label = "F" });

            for (int row = 1; row <= totalRows; row++)
            {
                

                for (int col = 0; col < seatsPerRow; col++)
                {
                    if (currentSeat < totalSeats)
                    {
                        if (col == 3)
                        {
                            Seats.Add(new Seat { SeatType = "Label", Row = row, Column = 3, Label = row.ToString() });
                        }
                        if (col != 3)
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
