using QLBVCB.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace QLBVCB.ViewModel
{
    internal class VM_CustomerInfo : VM_Base
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

        private ObservableCollection<string> mealOptions;
        public ObservableCollection<string> MealOptions
        {
            get => mealOptions;
            set
            {
                mealOptions = value;
                OnPropertyChanged(nameof(MealOptions));
            }
        }

        private ObservableCollection<string> baggageOptions;
        public ObservableCollection<string> BaggageOptions
        {
            get => baggageOptions;
            set
            {
                baggageOptions = value;
                OnPropertyChanged(nameof(BaggageOptions));
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

        private ObservableCollection<string> _suatan;
        public ObservableCollection<string> SUATAN
        {
            get => _suatan;
            set
            {
                _suatan = value;
                OnPropertyChanged(nameof(SUATAN));
            }
        }

        private string selectedMealOption;
        public string SelectedMealOption
        {
            get => selectedMealOption;
            set
            {
                selectedMealOption = value;
                OnPropertyChanged(nameof(SelectedMealOption));
            }
        }

        private ObservableCollection<string> _hanhly;
        public ObservableCollection<string> HANHLY
        {
            get => _hanhly;
            set
            {
                _hanhly = value;
                OnPropertyChanged(nameof(HANHLY));
            }
        }

        private string selectedLuggageOption;
        public string SelectedLuggageOption
        {
            get => selectedLuggageOption;
            set
            {
                selectedLuggageOption = value;
                OnPropertyChanged(nameof(SelectedLuggageOption));
            }
        }
        public VM_CustomerInfo(string MACB, int Hang, int Day, bool isRecuperate)
        {
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
            this.TicketType = SetTicketType(isRecuperate);
            LoadLuggageOptions();
            LoadMealOptions();
        }
        

        private void LoadLuggageOptions()
        {
            var hanhly = DataProvider.Ins.DB.DICHVUs
                .Where(p => p.LOAIDV == "Hành lý")
                .Select(p => p.TENDV)
                .ToList();

            HANHLY = new ObservableCollection<string>(hanhly);
        }
        private void LoadMealOptions()
        {
            var suatan = DataProvider.Ins.DB.DICHVUs
                .Where(p => p.LOAIDV == "Suất ăn")
                .Select(p => p.TENDV)
                .ToList();

            SUATAN = new ObservableCollection<string>(suatan);
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
            return a + Hang.ToString();
        }

        public string SetSeatType(int Hang)
        {
            return Hang < 6 ? "Business Class" : "Economy Class";
        }
    }
}
