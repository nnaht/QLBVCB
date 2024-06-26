using OfficeOpenXml.Style;
using OfficeOpenXml;
using QLBVCB.Model;
using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace QLBVCB.ViewModel
{
    internal class VM_ManageFlight : VM_Base
    {
        private ObservableCollection<CHUYENBAY> _FlightList;
        public ObservableCollection<CHUYENBAY> FlightList { get { return _FlightList; } set { _FlightList = value; OnPropertyChanged(); } }
        private string _MACB;
        public string MACB { get => _MACB; set { _MACB = value; OnPropertyChanged(); } }

        private string _MAMB;
        public string MAMB { get => _MAMB; set { _MAMB = value; OnPropertyChanged(); } }

        private string _THOIGIAN_CATCANH;
        public string THOIGIAN_CATCANH { get => _THOIGIAN_CATCANH; set { _THOIGIAN_CATCANH = value; OnPropertyChanged(); } }

        private string _THOIGIAN_HACANH;
        public string THOIGIAN_HACANH { get => _THOIGIAN_HACANH; set { _THOIGIAN_HACANH = value; OnPropertyChanged(); } }

        private string _TRANGTHAI;
        public string TRANGTHAI { get => _TRANGTHAI; set { _TRANGTHAI = value; OnPropertyChanged(); } }
        private string _SO_GHE;
        public string SO_GHE { get => _SO_GHE; set { _SO_GHE = value; OnPropertyChanged(); } }
        private string _MASB_CATCANH;
        public string MASB_CATCANH { get => _MASB_CATCANH; set { _MASB_CATCANH = value; OnPropertyChanged(); } }
        private string _MASB_HACANH;
        public string MASB_HACANH { get => _MASB_HACANH; set { _MASB_HACANH = value; OnPropertyChanged(); } }
        private string _SearchKeyword;
        public string SearchKeyword
        {
            get => _SearchKeyword;
            set
            {
                _SearchKeyword = value;
                OnPropertyChanged();
                FilterFlight();
            }
        }

        public ICollectionView FlightView { get; private set; }
        public ICommand OpenAERFlightCommand { get; set; }
        public ICommand ExportExcelManageFlightCommand { get; set; }
        public VM_ManageFlight()
        {
            OpenAERFlightCommand = new RelayCommand(ExecuteOpenAERFlightCommand);
            ExportExcelManageFlightCommand = new RelayCommand(ExecuteExportExcelManageFlightCommand);
            FlightList = new ObservableCollection<CHUYENBAY>(DataProvider.Ins.DB.CHUYENBAYs);
            FlightView = CollectionViewSource.GetDefaultView(FlightList);
            FlightView.Filter = FilterFlight;
        }
        private void ExecuteOpenAERFlightCommand(object obj)
        {
            try
            {
                AERFlight aERFlight = new AERFlight();
                aERFlight.DataContext = new VM_AERFlight();
                aERFlight.ShowDialog();
            }
            catch (Exception ex)
            {
                ShowCustomMessageBox("Có lỗi xảy ra: " + ex.Message);
            }
        }

        private void ExecuteExportExcelManageFlightCommand(object obj)
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
                    ShowCustomMessageBox("Đường dẫn không hợp lệ");
                    return;
                }

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Title = "Danh sách chuyến bay";
                    excelPackage.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet excelWorkSheet = excelPackage.Workbook.Worksheets[0];
                    excelWorkSheet.Name = "Sheet 1";
                    excelWorkSheet.Cells.Style.Font.Size = 14;
                    excelWorkSheet.Cells.Style.Font.Name = "Times New Roman";

                    string[] columnHeader = { "Mã CB", "Mã MB", "Thời gian cất cánh", "Thời gian hạ cánh", "Trạng thái", "Số ghế", "Mã SB cất cánh", "Mã SB hạ cánh" };
                    var countColumnHeader = columnHeader.Length;
                    excelWorkSheet.Cells[1, 1].Value = "Danh sách chuyến bay";
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
                    foreach (var flight in FlightList)
                    {
                        excelWorkSheet.Cells[rowIndex, 1].Value = flight.MACB;
                        excelWorkSheet.Cells[rowIndex, 2].Value = flight.MAMB;
                        excelWorkSheet.Cells[rowIndex, 3].Value = flight.THOIGIAN_CATCANH.ToString();
                        excelWorkSheet.Cells[rowIndex, 4].Value = flight.THOIGIAN_HACANH.ToString();
                        excelWorkSheet.Cells[rowIndex, 5].Value = flight.TRANGTHAI;
                        excelWorkSheet.Cells[rowIndex, 6].Value = flight.SO_GHE;
                        excelWorkSheet.Cells[rowIndex, 7].Value = flight.MASB_CATCANH;
                        excelWorkSheet.Cells[rowIndex, 8].Value = flight.MASB_HACANH;

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

                ShowCustomMessageBox("Xuất dữ liệu thành công!");
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                ShowCustomMessageBox("Có lỗi xảy ra: " + ex.Message);
            }
        }
        private bool FilterFlight(object item)
        {
            if (item is CHUYENBAY flight)
            {
                return string.IsNullOrEmpty(SearchKeyword) || flight.MASB_CATCANH.IndexOf(SearchKeyword, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return false;
        }

        private void FilterFlight()
        {
            FlightView.Refresh();
        }
        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
    }
}
