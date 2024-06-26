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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.ComponentModel;
using System.Windows.Data;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace QLBVCB.ViewModel
{
    internal class VM_ManageAirport : VM_Base
    {
        private ObservableCollection<SANBAY> _AirportList;
        public ObservableCollection<SANBAY> AirportList { get { return _AirportList; } set { _AirportList = value; OnPropertyChanged(); } }

        private string _MASB;
        public string MASB { get => _MASB; set { _MASB = value; OnPropertyChanged(); } }

        private string _TEN_SANBAY;
        public string TEN_SANBAY { get => _TEN_SANBAY; set { _TEN_SANBAY = value; OnPropertyChanged(); } }

        private string _THANHPHO;
        public string THANHPHO { get => _THANHPHO; set { _THANHPHO = value; OnPropertyChanged(); } }

        private string _QUOCGIA;
        public string QUOCGIA { get => _QUOCGIA; set { _QUOCGIA = value; OnPropertyChanged(); } }

        private string _SearchKeyword;
        public string SearchKeyword
        {
            get => _SearchKeyword;
            set
            {
                _SearchKeyword = value;
                OnPropertyChanged();
                FilterAirport();
            }
        }

        public ICollectionView AirportView { get; private set; }

        public ICommand OpenAERAirportCommand { get; set; }
        public ICommand ExportExcelManageAirportCommand { get; set; }

        public VM_ManageAirport()
        {
            OpenAERAirportCommand = new RelayCommand(ExecuteOpenAERAirportCommand);
            ExportExcelManageAirportCommand = new RelayCommand(ExecuteExportExcelManageAirportCommand);
            AirportList = new ObservableCollection<SANBAY>(DataProvider.Ins.DB.SANBAYs);
            AirportView = CollectionViewSource.GetDefaultView(AirportList);
            AirportView.Filter = FilterAirport;
        }

        private void ExecuteOpenAERAirportCommand(object obj)
        {
            try
            {
                AERAirport aERAirport = new AERAirport();
                aERAirport.DataContext = new VM_AERAirport();
                aERAirport.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        private void ExecuteExportExcelManageAirportCommand(object obj)
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
                    excelPackage.Workbook.Properties.Title = "Danh sách sân bay";
                    excelPackage.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet excelWorkSheet = excelPackage.Workbook.Worksheets[0];
                    excelWorkSheet.Name = "Sheet 1";
                    excelWorkSheet.Cells.Style.Font.Size = 14;
                    excelWorkSheet.Cells.Style.Font.Name = "Times New Roman";

                    string[] columnHeader = { "Mã", "Tên sân bay", "Thành phố", "Quốc gia" };
                    var countColumnHeader = columnHeader.Count();
                    excelWorkSheet.Cells[1, 1].Value = "Danh sách sân bay";
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
                    foreach (var airport in AirportList)
                    {
                        excelWorkSheet.Cells[rowIndex, 1].Value = airport.MASB;
                        excelWorkSheet.Cells[rowIndex, 2].Value = airport.TEN_SANBAY;
                        excelWorkSheet.Cells[rowIndex, 3].Value = airport.THANHPHO;
                        excelWorkSheet.Cells[rowIndex, 4].Value = airport.QUOCGIA;

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
        private bool FilterAirport(object item)
        {
            if (item is SANBAY airport)
            {
                return string.IsNullOrEmpty(SearchKeyword) || airport.TEN_SANBAY.Contains(SearchKeyword);
            }
            return false;
        }

        private void FilterAirport()
        {
            AirportView.Refresh();
        }
        public bool IsCustomerVisible
        {
            get { return position != 3; }
        }
        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
    }
}
