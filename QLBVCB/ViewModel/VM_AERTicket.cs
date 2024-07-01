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
    internal class VM_AERTicket : VM_Base
    {
        private ObservableCollection<VEBAY> _TicketList;
        public ObservableCollection<VEBAY> TicketList { get { return _TicketList; } set { _TicketList = value; OnPropertyChanged(); } }

        private string _MAVB;
        public string MAVB { get => _MAVB; set { _MAVB = value; OnPropertyChanged(); } }

        private string _MACB;
        public string MACB { get => _MACB; set { _MACB = value; OnPropertyChanged(); } }

        private string _THUTU_GHE;
        public string THUTU_GHE { get => _THUTU_GHE; set { _THUTU_GHE = value; OnPropertyChanged(); } }

        private string _MALV;
        public string MALV { get => _MALV; set { _MALV = value; OnPropertyChanged(); } }

        private string _TENNGUOIDAT;
        public string TENNGUOIDAT { get => _TENNGUOIDAT; set { _TENNGUOIDAT = value; OnPropertyChanged(); } }

        private string _TENHANHKHACH;
        public string TENHANHKHACH { get => _TENHANHKHACH; set { _TENHANHKHACH = value; OnPropertyChanged(); } }


        //public ICommand AddTicketCommand { get; set; }
        public ICommand EditTicketCommand { get; set; }
        public ICommand RemoveTicketCommand { get; set; }
        public ICollectionView TicketView { get; private set; }

        private VEBAY _TicketSelectedItem;
        public VEBAY TicketSelectedItem
        {
            get => _TicketSelectedItem;
            set
            {
                _TicketSelectedItem = value;
                OnPropertyChanged();
                if (TicketSelectedItem != null)
                {
                    MAVB = TicketSelectedItem.MAVB;
                    MACB = TicketSelectedItem.MACB;
                    THUTU_GHE = TicketSelectedItem.THUTU_GHE;
                    MALV = TicketSelectedItem.MALV;
                    TENNGUOIDAT = TicketSelectedItem.TENNGUOIDAT;
                    TENHANHKHACH = TicketSelectedItem.TENHANHKHACH;
                }
            }
        }
        public VM_AERTicket()
        {
            TicketList = new ObservableCollection<VEBAY>(DataProvider.Ins.DB.VEBAYs);
            var displayTicketList = DataProvider.Ins.DB.VEBAYs.Where(x => x.MAVB == MAVB);
            TicketView = CollectionViewSource.GetDefaultView(TicketList);
            TicketView.Filter = FilterTicket;

            //AddTicketCommand = new RelayCommand<object>((p) =>
            //{
            //    if (string.IsNullOrEmpty(MACB) || string.IsNullOrEmpty(THUTU_GHE) || string.IsNullOrEmpty(MALV))
            //        return false;
            //    if (displayTicketList == null)
            //        return false;
            //    if (displayTicketList.Count() != 0)
            //        return false;
            //    return true;
            //}, (p) =>
            //{
            //    var ticket = new VEBAY() { MACB = MACB, THUTU_GHE = THUTU_GHE, MALV = MALV };
            //    DataProvider.Ins.DB.VEBAYs.Add(ticket);
            //    DataProvider.Ins.DB.SaveChanges();
            //    TicketList.Add(ticket);
            //    ShowCustomMessageBox("Thêm thành công!");
            //});

            EditTicketCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MACB) || string.IsNullOrEmpty(THUTU_GHE) || string.IsNullOrEmpty(MALV))
                    return false;
                if (TicketSelectedItem == null)
                    return false;
                if (displayTicketList.Count() == 0)
                    return false;
                return true;
            }, (p) =>
            {
                var ticket = DataProvider.Ins.DB.VEBAYs.Where(x => x.MAVB == MAVB).SingleOrDefault();
                ticket.MACB = MACB;
                ticket.THUTU_GHE = THUTU_GHE;
                ticket.MALV = MALV;
                ticket.TENNGUOIDAT = TENNGUOIDAT;
                ticket.TENHANHKHACH = TENHANHKHACH;
                DataProvider.Ins.DB.SaveChanges();
                ShowCustomMessageBox("Sửa thành công!");
            });

            RemoveTicketCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                try
                {
                    if (MessageBox.Show("Xác nhận xóa?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        var booking = DataProvider.Ins.DB.DADATs.Where(x => x.MAVB == TicketSelectedItem.MAVB).SingleOrDefault();
                        DataProvider.Ins.DB.DADATs.Remove(booking);
                        DataProvider.Ins.DB.VEBAYs.Remove(TicketSelectedItem);
                        DataProvider.Ins.DB.SaveChanges();
                        TicketList.Remove(TicketSelectedItem);
                        ShowCustomMessageBox("Xóa thành công!");
                    }
                }
                catch (Exception ex)
                {
                    ShowCustomMessageBox("Không thể xóa!");
                }
            });
        }

        private string _SearchTicket;
        public string SearchTicket
        {
            get => _SearchTicket;
            set
            {
                _SearchTicket = value;
                OnPropertyChanged();
                FilterTicket();
            }
        }
        private bool FilterTicket(object item)
        {
            if (item is VEBAY ticket)
            {
                return string.IsNullOrEmpty(SearchTicket) || ticket.MACB.IndexOf(SearchTicket, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return false;
        }
        private void FilterTicket()
        {
            TicketView.Refresh();
        }
        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
    }
}
