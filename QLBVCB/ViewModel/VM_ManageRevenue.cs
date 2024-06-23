using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using MaterialDesignThemes.Wpf;
using QLBVCB.Model;
using QLBVCB.View;
using QLBVCB.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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
                RefreshData2(null); // Refresh data when Thang changes
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
                RefreshData2(null); // Refresh data when Nam changes
            }
        }
        private string _label3;
        public string Label3
        {
            get => _label3;
            set
            {
                _label3 = value;
                OnPropertyChanged();
            }
        }
        private string _label1;
        public string Label1
        {
            get => _label1;
            set
            {
                _label1 = value;
                OnPropertyChanged();
            }
        }
        private string _label2;
        public string Label2
        {
            get => _label2;
            set
            {
                _label2 = value;
                OnPropertyChanged();
            }
        }
        public SeriesCollection LineSeriesCollection { get; set; }
        public SeriesCollection ColumnSeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand OpenRevenueDetailCommand { get; set; }

        public List<string> ThangList { get; set; }
        public List<string> NamList { get; set; }
        DataSet dsMonth = new DataSet();

        public VM_ManageRevenue()
        {
            Initialize();
        }
        private async void Initialize()
        {
            ThangList = Enumerable.Range(1, 12).Select(i => i.ToString("D2")).ToList();
            NamList = Enumerable.Range(2020, 2025).Select(i => i.ToString()).ToList();

            Thang = "06"; // Initialize with current month
            Nam = "2024"; // Initialize with current year

            OpenRevenueDetailCommand = new RelayCommand(ExecuteOpenRevenueDetailCommand);
            RefreshCommand = new RelayCommand(RefreshData2);

            await RefreshData();
        }

        private void SetLabel()
        {
            try
            {
                using (var context = new QLBVCBEntities())
                {
                    var ve = context.Database.SqlQuery<decimal>("SELECT DBO.TONGTIENVETHANG(@p0, @p1)", Nam, Thang).SingleOrDefault();
                    var dichvu = context.Database.SqlQuery<decimal>("SELECT DBO.TONGTIENDICHVUTHANG(@p0, @p1)", Nam, Thang).SingleOrDefault();
                    var luong = context.Database.SqlQuery<decimal>("SELECT DBO.TONGTIENLUONGTHANG()").SingleOrDefault();

                    if (ve != default(decimal) && dichvu != default(decimal) && luong != default(decimal))
                    {
                        Label1 = $"{ve:N0} VNĐ";
                        Label2 = $"{dichvu:N0} VNĐ";
                        Label3 = $"{luong:N0} VNĐ";
                    }
                    else
                    {
                        Label1 = "0 VNĐ";
                        Label2 = "0 VNĐ";
                        Label3 = "0 VNĐ";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving label data: {ex.Message}");
                // Handle exceptions appropriately, e.g., log the error, display a message to the user, etc.
            }
        }


        private async Task RefreshData()
        {
            await Task.Run(() =>
            {
                SetLabel();
            });
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
        private void RefreshData2(object obj)
        {
            SetYearData(Nam);
            SetMonthData(Nam, Thang);
            RefreshData();
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
