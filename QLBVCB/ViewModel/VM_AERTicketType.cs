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
    internal class VM_AERTicketType : VM_Base
    {
        private ObservableCollection<LOAIVE> _TicketTypeList;
        public ObservableCollection<LOAIVE> TicketTypeList { get { return _TicketTypeList; } set { _TicketTypeList = value; OnPropertyChanged(); } }

        private string _MALV;
        public string MALV { get => _MALV; set { _MALV = value; OnPropertyChanged(); } }

        private string _TEN_LOAIVE;
        public string TEN_LOAIVE { get => _TEN_LOAIVE; set { _TEN_LOAIVE = value; OnPropertyChanged(); } }

        private Nullable<decimal> _GIAVE;
        public Nullable<decimal> GIAVE { get => _GIAVE; set { _GIAVE = value; OnPropertyChanged(); } }
        private Nullable<decimal> _PHI_THAYDOI;
        public Nullable<decimal> PHI_THAYDOI { get => _PHI_THAYDOI; set { _PHI_THAYDOI = value; OnPropertyChanged(); } }
        private Nullable<decimal> _PHI_HUY;
        public Nullable<decimal> PHI_HUY { get => _PHI_HUY; set { _PHI_HUY = value; OnPropertyChanged(); } }
        public ICommand AddTicketTypeCommand { get; set; }
        public ICommand EditTicketTypeCommand { get; set; }
        public ICommand RemoveTicketTypeCommand { get; set; }

        private LOAIVE _TicketTypeSelectedItem;
        public LOAIVE TicketTypeSelectedItem
        {
            get => _TicketTypeSelectedItem;
            set
            {
                _TicketTypeSelectedItem = value;
                OnPropertyChanged();
                if (TicketTypeSelectedItem != null)
                {
                    MALV = TicketTypeSelectedItem.MALV;
                    TEN_LOAIVE = TicketTypeSelectedItem.TEN_LOAIVE;
                    GIAVE = TicketTypeSelectedItem.GIAVE;
                    PHI_THAYDOI = TicketTypeSelectedItem.PHI_THAYDOI;
                    PHI_HUY = TicketTypeSelectedItem.PHI_HUY;
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
                FilterTicketTypes();
            }
        }

        public ICollectionView TicketTypeView { get; private set; }
        public VM_AERTicketType()
        {
            TicketTypeList = new ObservableCollection<LOAIVE>(DataProvider.Ins.DB.LOAIVEs);
            var displayTicketTypeList = DataProvider.Ins.DB.LOAIVEs.Where(x => x.MALV == MALV);
            TicketTypeView = CollectionViewSource.GetDefaultView(TicketTypeList);
            TicketTypeView.Filter = FilterTicketType;
            AddTicketTypeCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MALV) || string.IsNullOrEmpty(TEN_LOAIVE) || GIAVE != null || 
                    PHI_THAYDOI != null || PHI_HUY != null)
                    return false;
                if (displayTicketTypeList == null)
                    return false;
                if (displayTicketTypeList.Count() != 0)
                    return false;
                return true;
            }, (p) =>
            {
                var ticketType = new LOAIVE() { MALV = MALV, TEN_LOAIVE = TEN_LOAIVE, GIAVE = GIAVE, PHI_THAYDOI = PHI_THAYDOI, PHI_HUY = PHI_HUY };
                DataProvider.Ins.DB.LOAIVEs.Add(ticketType);
                DataProvider.Ins.DB.SaveChanges();
                TicketTypeList.Add(ticketType);
            });

            EditTicketTypeCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MALV) || string.IsNullOrEmpty(TEN_LOAIVE) || GIAVE != null || PHI_THAYDOI != null || PHI_HUY != null)
                    return false;
                if (TicketTypeSelectedItem == null)
                    return false;
                if (displayTicketTypeList == null && displayTicketTypeList.Count() != 0)
                    return false;
                return true;
            }, (p) =>
            {
                var ticketType = DataProvider.Ins.DB.LOAIVEs.Where(x => x.MALV == MALV).SingleOrDefault();
                ticketType.MALV = MALV;
                ticketType.TEN_LOAIVE = TEN_LOAIVE;
                ticketType.GIAVE = GIAVE;
                ticketType.PHI_THAYDOI = PHI_THAYDOI;
                ticketType.PHI_HUY = PHI_HUY;
                DataProvider.Ins.DB.SaveChanges();
            });

            RemoveTicketTypeCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MALV) || string.IsNullOrEmpty(TEN_LOAIVE) || GIAVE != null || PHI_THAYDOI != null || PHI_HUY != null)
                    return false;
                if (TicketTypeSelectedItem == null)
                    return false;
                if (displayTicketTypeList != null && displayTicketTypeList.Count() != 0)
                    return true;
                foreach (var item in TicketTypeList)
                {
                    if (TEN_LOAIVE == item.TEN_LOAIVE && GIAVE == item.GIAVE && PHI_THAYDOI == item.PHI_THAYDOI && PHI_HUY == item.PHI_HUY)
                        return true;
                }
                return false;
            }, (p) =>
            {
                DataProvider.Ins.DB.LOAIVEs.Remove(TicketTypeSelectedItem);
                DataProvider.Ins.DB.SaveChanges();

                TicketTypeList.Remove(TicketTypeSelectedItem);
            });
        }
        private bool FilterTicketType(object item)
        {
            if (item is LOAIVE ticketType)
            {
                return string.IsNullOrEmpty(SearchKeyword) || ticketType.TEN_LOAIVE.StartsWith(SearchKeyword, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private void FilterTicketTypes()
        {
            TicketTypeView.Refresh();
        }
    }
}
