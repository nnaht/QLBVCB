using QLBVCB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBVCB.ViewModel
{
    internal class VM_CustomerTicket : VM_Base
    {
        private string brand;
        public string Brand
        {
            get => brand;
            set
            {
                brand = value;
                OnPropertyChanged(nameof(Brand));
            }
        }

        private string start;
        public string Start
        {
            get => start;
            set
            {
                start = value;
                OnPropertyChanged(nameof(Start));
            }
        }

        private string end;
        public string End
        {
            get => end;
            set
            {
                end = value;
                OnPropertyChanged(nameof(End));
            }
        }
        private int hang;
        public int HANG
        {
            get => hang;
            set
            {
                hang = value;
                OnPropertyChanged(nameof(HANG));
            }
        }
        private int day;
        public int DAY
        {
            get => day;
            set
            {
                day = value;
                OnPropertyChanged(nameof(DAY));
            }
        }

        private string macb;
        public string MACB
        {
            get => macb;
            set
            {
                macb = value;
                OnPropertyChanged(nameof(MACB));
            }
        }

        private string date;
        public string Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private string time;
        public string Time
        {
            get => time;
            set
            {
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        private string seat;
        public string Seat
        {
            get => seat;
            set
            {
                seat = value;
                OnPropertyChanged(nameof(Seat));
            }
        }

        private string seatType;
        public string SeatType
        {
            get => seatType;
            set
            {
                seatType = value;
                OnPropertyChanged(nameof(SeatType));
            }
        }

        private string ticketType;
        public string TicketType
        {
            get => ticketType;
            set
            {
                ticketType = value;
                OnPropertyChanged(nameof(TicketType));
            }
        }

       

        private string passengerName;
        public string PassengerName
        {
            get => passengerName;
            set
            {
                passengerName = value;
                OnPropertyChanged(nameof(PassengerName));
            }
        }

        private string _suatan;
        public string SUATAN
        {
            get => _suatan;
            set
            {
                _suatan = value;
                OnPropertyChanged(nameof(SUATAN));
            }
        }


        private string _hanhly;
        public string HANHLY
        {
            get => _hanhly;
            set
            {
                _hanhly = value;
                OnPropertyChanged(nameof(HANHLY));
            }
        }
        public VM_CustomerTicket(string MACB, int Hang, int Day, bool isRecuperate, string SA, string HL, string Name)
        {
            this.passengerName = Name;
            this.MACB = MACB;
            this.Brand = SetBrand(MACB);
            this.Start = SetStart(MACB);
            this.End = SetEnd(MACB);
            this.Date = SetDate(MACB);
            this.Time = SetTime(MACB);
            this.Seat = setSeat(Hang, Day);
            this.SeatType = SetSeatType(Hang);
            this.HANG = Hang;
            this.DAY = Day;
            this.SUATAN = SA;
            this.HANHLY = HL;
            this.TicketType = SetTicketType(isRecuperate);
        }
        
        public string SetTicketType(bool isRecuperate)
        {
            return isRecuperate ? "Khứ hồi" : "Một chiều";
        }

        public string SetBrand(string MACB)
        {
            var chuyenBay = DataProvider.Ins.DB.CHUYENBAYs.SingleOrDefault(cb => cb.MACB == MACB);
            if (chuyenBay != null)
            {
                var mayBay = DataProvider.Ins.DB.MAYBAYs.SingleOrDefault(mb => mb.MAMB == chuyenBay.MAMB);
                if (mayBay != null)
                {
                    return mayBay.HANGMB;
                }
            }
            return string.Empty;
        }

        public string SetStart(string MACB)
        {
            var chuyenBay = DataProvider.Ins.DB.CHUYENBAYs.SingleOrDefault(cb => cb.MACB == MACB);
            if (chuyenBay != null)
            {
                var sanBay = DataProvider.Ins.DB.SANBAYs.SingleOrDefault(sb => sb.MASB == chuyenBay.MASB_CATCANH);
                if (sanBay != null)
                {
                    return $"{sanBay.TEN_SANBAY} / {sanBay.THANHPHO}";
                }
            }
            return string.Empty;
        }

        public string SetEnd(string MACB)
        {
            var chuyenBay = DataProvider.Ins.DB.CHUYENBAYs.SingleOrDefault(cb => cb.MACB == MACB);
            if (chuyenBay != null)
            {
                var sanBay = DataProvider.Ins.DB.SANBAYs.SingleOrDefault(sb => sb.MASB == chuyenBay.MASB_HACANH);
                if (sanBay != null)
                {
                    return $"{sanBay.TEN_SANBAY} / {sanBay.THANHPHO}";
                }
            }
            return string.Empty;
        }

        public string SetDate(string MACB)
        {
            var chuyenBay = DataProvider.Ins.DB.CHUYENBAYs.SingleOrDefault(cb => cb.MACB == MACB);
            if (chuyenBay != null)
            {
                return chuyenBay.THOIGIAN_CATCANH.ToString();
            }
            return string.Empty;
        }

        public string SetTime(string MACB)
        {
            var chuyenBay = DataProvider.Ins.DB.CHUYENBAYs.SingleOrDefault(cb => cb.MACB == MACB);
            if (chuyenBay != null)
            {
                return chuyenBay.THOIGIAN_CATCANH.ToString();
            }
            return string.Empty;
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
            return Hang.ToString() + a;
        }

        public string SetSeatType(int Hang)
        {
            return Hang < 6 ? "Business Class" : "Economy Class";
        }
    }
}
