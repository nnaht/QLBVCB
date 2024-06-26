﻿using QLBVCB.Model;
using QLBVCB.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    internal class VM_AERPlane : VM_Base
    {
        private ObservableCollection<MAYBAY> _PlaneList;
        public ObservableCollection<MAYBAY> PlaneList { get { return _PlaneList; } set { _PlaneList = value; OnPropertyChanged(); } }

        private string _MAMB;
        public string MAMB { get => _MAMB; set { _MAMB = value; OnPropertyChanged(); } }

        private string _LOAIMB;
        public string LOAIMB { get => _LOAIMB; set { _LOAIMB = value; OnPropertyChanged(); } }

        private string _HANGMB;
        public string HANGMB { get => _HANGMB; set { _HANGMB = value; OnPropertyChanged(); } }

        public ICommand AddPlaneCommand { get; set; }
        public ICommand EditPlaneCommand { get; set; }
        public ICommand RemovePlaneCommand { get; set; }

        private MAYBAY _PlaneSelectedItem;
        public MAYBAY PlaneSelectedItem
        {
            get => _PlaneSelectedItem;
            set
            {
                _PlaneSelectedItem = value;
                OnPropertyChanged();
                if (PlaneSelectedItem != null)
                {
                    MAMB = PlaneSelectedItem.MAMB;
                    LOAIMB = PlaneSelectedItem.LOAIMB;
                    HANGMB = PlaneSelectedItem.HANGMB;
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
                FilterPlane();
            }
        }

        public ICollectionView PlaneView { get; private set; }
        public VM_AERPlane()
        {
            PlaneList = new ObservableCollection<MAYBAY>(DataProvider.Ins.DB.MAYBAYs);
            var displayPlaneList = DataProvider.Ins.DB.MAYBAYs.Where(x => x.MAMB == MAMB);
            PlaneView = CollectionViewSource.GetDefaultView(PlaneList);
            PlaneView.Filter = FilterPlane;

            AddPlaneCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MAMB) || string.IsNullOrEmpty(LOAIMB) || string.IsNullOrEmpty(HANGMB))
                    return false;
                if (displayPlaneList == null)
                    return false;
                if (displayPlaneList.Count() != 0)
                    return false;
                return true;
            }, (p) =>
            {
                var plane = new MAYBAY() { MAMB = MAMB, LOAIMB = LOAIMB, HANGMB = HANGMB };
                DataProvider.Ins.DB.MAYBAYs.Add(plane);
                DataProvider.Ins.DB.SaveChanges();
                PlaneList.Add(plane);
                OnPropertyChanged();
                ShowCustomMessageBox("Thêm thành công!");
            });

            EditPlaneCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MAMB) || string.IsNullOrEmpty(LOAIMB) || string.IsNullOrEmpty(HANGMB))
                    return false;
                if (PlaneSelectedItem == null)
                    return false;
                if (displayPlaneList.Count() == 0)
                    return false;
                return true;
            }, (p) =>
            {
                var plane = DataProvider.Ins.DB.MAYBAYs.Where(x => x.MAMB == MAMB).SingleOrDefault();
                plane.MAMB = MAMB;
                plane.LOAIMB = LOAIMB;
                plane.HANGMB = HANGMB;
                DataProvider.Ins.DB.SaveChanges();
                ShowCustomMessageBox("Sửa thành công!");
            });

            RemovePlaneCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                try
                {
                    if (MessageBox.Show("Xác nhận xóa?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        DataProvider.Ins.DB.MAYBAYs.Remove(PlaneSelectedItem);
                        DataProvider.Ins.DB.SaveChanges();
                        PlaneList.Remove(PlaneSelectedItem);
                        ShowCustomMessageBox("Xóa thành công!");
                    }
                }
                catch (Exception ex)
                {
                    ShowCustomMessageBox("Không thể xóa!");
                }
            });
        }
        private bool FilterPlane(object item)
        {
            if (item is MAYBAY plane)
            {
                return string.IsNullOrEmpty(SearchKeyword) || plane.HANGMB.IndexOf(SearchKeyword, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return false;
        }

        private void FilterPlane()
        {
            PlaneView.Refresh();
        }
        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
    }
}
