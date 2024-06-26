using OfficeOpenXml;
using OfficeOpenXml.Style;
using QLBVCB.Model;
using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Forms;
using System.Globalization;

namespace QLBVCB.ViewModel
{
    public class NumberFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("N0", CultureInfo.CurrentCulture);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DoanhThuResult
    {
        public int THANG { get; set; }
        public decimal VE { get; set; }
        public decimal DICHVU { get; set; }
        public decimal LUONG { get; set; }
        public decimal DOANHTHU { get; set; }
    }

    internal class VM_RevenueDetail : VM_Base
    {
        private ObservableCollection<DoanhThuResult> _RevenueList;
        public ObservableCollection<DoanhThuResult> RevenueList
        {
            get { return _RevenueList; }
            set { _RevenueList = value; OnPropertyChanged(); }
        }

        private string _THANG;
        public string THANG
        {
            get => _THANG;
            set { _THANG = value; OnPropertyChanged(); }
        }

        private string _VE;
        public string VE
        {
            get => _VE;
            set { _VE = value; OnPropertyChanged(); }
        }

        private string _DICHVU;
        public string DICHVU
        {
            get => _DICHVU;
            set { _DICHVU = value; OnPropertyChanged(); }
        }

        private string _LUONG;
        public string LUONG
        {
            get => _LUONG;
            set { _LUONG = value; OnPropertyChanged(); }
        }

        private string _DOANHTHU;
        public string DOANHTHU
        {
            get => _DOANHTHU;
            set { _DOANHTHU = value; OnPropertyChanged(); }
        }

        private string _SearchKeyword;
        public string SearchKeyword
        {
            get => _SearchKeyword;
            set
            {
                _SearchKeyword = value;
                OnPropertyChanged();
                FilterRevenue();
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
                LoadRevenueData();
            }
        }

        public ICollectionView RevenueView { get; private set; }
        public ICommand ExportExcelManageRevenueCommand { get; set; }
        public List<string> NamList { get; set; }

        public VM_RevenueDetail()
        {
            NamList = Enumerable.Range(2020, 6).Select(i => i.ToString()).ToList();
            Nam = "2024";
            ExportExcelManageRevenueCommand = new RelayCommand(ExecuteExportExcelManageRevenueCommand);
            LoadRevenueData();
        }

        private void LoadRevenueData()
        {
            RevenueList = new ObservableCollection<DoanhThuResult>(
                DataProvider.Ins.DB.Database.SqlQuery<DoanhThuResult>("EXEC DBO.CHITIETDOANHTHUTUNGTHANG @NAM = {0}", Nam).ToList()
            );
            RevenueView = CollectionViewSource.GetDefaultView(RevenueList);
            RevenueView.Filter = FilterRevenue;
        }

        private void ExecuteExportExcelManageRevenueCommand(object obj)
        {
            try
            {
                string filePath = "";
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = saveFileDialog.FileName;
                }

                if (string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show("Đường dẫn không hợp lệ");
                    return;
                }

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Title = "Chi tiết doanh thu";
                    excelPackage.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet excelWorkSheet = excelPackage.Workbook.Worksheets[0];
                    excelWorkSheet.Name = "Sheet 1";
                    excelWorkSheet.Cells.Style.Font.Size = 14;
                    excelWorkSheet.Cells.Style.Font.Name = "Times New Roman";

                    string[] columnHeader = { "Tháng", "Vé", "Dịch Vụ", "Lương", "Doanh Thu" };
                    var countColumnHeader = columnHeader.Count();
                    excelWorkSheet.Cells[1, 1].Value = "Chi tiết doanh thu";
                    excelWorkSheet.Cells[1, 1, 1, countColumnHeader].Merge = true;
                    excelWorkSheet.Cells[1, 1, 1, countColumnHeader].Style.Font.Bold = true;
                    excelWorkSheet.Cells[1, 1, 1, countColumnHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int columnIndex = 1;
                    int rowIndex = 2;
                    foreach (var item in columnHeader)
                    {
                        var cell = excelWorkSheet.Cells[rowIndex, columnIndex];
                        var fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                        var border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Value = item;
                        columnIndex++;
                    }

                    rowIndex = 3;
                    foreach (var Revenue in RevenueList)
                    {
                        excelWorkSheet.Cells[rowIndex, 1].Value = Revenue.THANG;
                        excelWorkSheet.Cells[rowIndex, 2].Value = Revenue.VE;
                        excelWorkSheet.Cells[rowIndex, 3].Value = Revenue.DICHVU;
                        excelWorkSheet.Cells[rowIndex, 4].Value = Revenue.LUONG;
                        excelWorkSheet.Cells[rowIndex, 5].Value = Revenue.DOANHTHU;

                        for (int i = 1; i <= countColumnHeader; i++)
                        {
                            var cell = excelWorkSheet.Cells[rowIndex, i];
                            var border = cell.Style.Border;
                            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        rowIndex++;
                    }

                    excelWorkSheet.Cells.AutoFitColumns();
                    excelPackage.SaveAs(new System.IO.FileInfo(filePath));
                }

                MessageBox.Show("Xuất dữ liệu thành công!");
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }

        private bool FilterRevenue(object item)
        {
            if (item is DoanhThuResult Revenue)
            {
                return string.IsNullOrEmpty(SearchKeyword) || Revenue.THANG.ToString().Contains(SearchKeyword);
            }
            return false;
        }

        private void FilterRevenue()
        {
            RevenueView.Refresh();
        }
    }
}
