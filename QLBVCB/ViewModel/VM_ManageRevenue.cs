using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using MaterialDesignThemes.Wpf;
using QLBVCB.Model;
using QLBVCB.View;
using QLBVCB.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    internal class VM_ManageRevenue : VM_Base
    {
        private string _thang;
        public string Thang
        {
            get => _thang;
            set
            {
                _thang = value;
                OnPropertyChanged();
                RefreshData(null); // Refresh data when Thang changes
            }
        }

        private string _nam;
        public string Nam
        {
            get => _nam;
            set
            {
                _nam = value;
                OnPropertyChanged();
                RefreshData(null); // Refresh data when Nam changes
            }
        }

        public SeriesCollection LineSeriesCollection { get; set; }
        public SeriesCollection ColumnSeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand OpenRevenueDetailCommand { get; set; }
        public List<string> ThangList { get; set; }
        public List<string> NamList { get; set; }

        public VM_ManageRevenue()
        {
            ThangList = Enumerable.Range(1, 12).Select(i => i.ToString("D2")).ToList();
            NamList = Enumerable.Range(2020, 2025).Select(i => i.ToString()).ToList();

            Thang = "06"; // Initialize with current month
            Nam = "2024"; // Initialize with current year
            OpenRevenueDetailCommand = new RelayCommand(ExecuteOpenRevenueDetailCommand);
            RefreshCommand = new RelayCommand(RefreshData);
            RefreshData(null); // Refresh data initially
        }
        private void ExecuteOpenRevenueDetailCommand(object obj)
        {
            try
            {
                RevenueDetail man = new RevenueDetail();
                man.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void RefreshData(object obj)
        {
            SetYearData(Nam);
            SetMonthData(Nam, Thang);
        }

        private void SetYearData(string year)
        {
            var results = DataProvider.Ins.DB.Database.SqlQuery<DoanhThuResult>("EXEC DBO.DOANHTHUTUNGTHANG @NAM = {0}", year).ToList();

            LineSeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Thu Nhập",
                   Values = new ChartValues<double>(results.Select(r => Convert.ToDouble(r.THUNHAP)).ToList())
                },
                new LineSeries
                {
                    Title = "Chi Phí",
                    Values = new ChartValues<double>(results.Select(r => Convert.ToDouble(r.CHIPHI)).ToList())
                },
                new LineSeries
                {
                    Title = "Doanh Thu",
                    Values = new ChartValues<double>(results.Select(r => Convert.ToDouble(r.DOANHTHU)).ToList())
                }
            };

            Labels = results.Select(r => r.THANG.ToString()).ToList();
        }

        private void SetMonthData(string year, string month)
        {

            var ve = DataProvider.Ins.DB.Database.SqlQuery<decimal>("SELECT DBO.TONGTIENVETHANG({0}, {1})", year, month).Single();
            var dichvu = DataProvider.Ins.DB.Database.SqlQuery<decimal>("SELECT DBO.TONGTIENDICHVUTHANG({0}, {1})", year, month).Single();

            ColumnSeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Chỗ Ngồi",
                    Values = new ChartValues<double> { Convert.ToDouble(ve) }
                },
                new PieSeries
                {
                    Title = "Dịch Vụ",
                    Values = new ChartValues<double> { Convert.ToDouble(dichvu) }
                }
            };
        }

        private class DoanhThuResult
        {
            public int THANG { get; set; }
            public decimal THUNHAP { get; set; }
            public decimal CHIPHI { get; set; }
            public decimal DOANHTHU { get; set; }
        }
    }
}
