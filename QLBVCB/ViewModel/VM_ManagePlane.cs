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
using System.Windows.Data;
using System.ComponentModel;

namespace QLBVCB.ViewModel
{
    internal class VM_ManagePlane : VM_Base
    {
        private ObservableCollection<MAYBAY> _PlaneList;
        public ObservableCollection<MAYBAY> PlaneList { get { return _PlaneList; } set { _PlaneList = value; OnPropertyChanged(); } }

        private string _MAMB;
        public string MAMB { get => _MAMB; set { _MAMB = value; OnPropertyChanged(); } }

        private string _LOAIMB;
        public string LOAIMB { get => _LOAIMB; set { _LOAIMB = value; OnPropertyChanged(); } }

        private string _HANGMB;
        public string HANGMB { get => _HANGMB; set { _HANGMB = value; OnPropertyChanged(); } }
        public ICommand OpenAERPlaneCommand { get; set; }
        public ICommand ExportExcelManagePlaneCommand { get; set; }
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
        public VM_ManagePlane()
        {
            OpenAERPlaneCommand = new RelayCommand(ExecuteOpenAERPlaneCommand);
            ExportExcelManagePlaneCommand = new RelayCommand(ExecuteExportExcelManagePlaneCommand);
            PlaneList = new ObservableCollection<MAYBAY>(DataProvider.Ins.DB.MAYBAYs);
            PlaneView = CollectionViewSource.GetDefaultView(PlaneList);
            PlaneView.Filter = FilterPlane;
        }
        private void ExecuteOpenAERPlaneCommand(object obj)
        {
            try
            {
                AERPlane aERPlane = new AERPlane();
                aERPlane.ShowDialog();
            }
            catch (Exception ex)
            {
                ShowCustomMessageBox("Có lỗi xảy ra: " + ex.Message);
            }
        }

        private void ExecuteExportExcelManagePlaneCommand(object obj)
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

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Title = "Danh sách máy bay";
                    excelPackage.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet excelWorkSheet = excelPackage.Workbook.Worksheets[0];
                    excelWorkSheet.Name = "Sheet 1";
                    excelWorkSheet.Cells.Style.Font.Size = 14;
                    excelWorkSheet.Cells.Style.Font.Name = "Times New Roman";

                    string[] columnHeader = { "Mã máy bay", "Loại máy bay", "Hãng máy bay" };
                    var countColumnHeader = columnHeader.Length;
                    excelWorkSheet.Cells[1, 1].Value = "Danh sách máy bay";
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
                    foreach (var plane in PlaneList)
                    {
                        excelWorkSheet.Cells[rowIndex, 1].Value = plane.MAMB;
                        excelWorkSheet.Cells[rowIndex, 2].Value = plane.LOAIMB;
                        excelWorkSheet.Cells[rowIndex, 3].Value = plane.HANGMB;

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
        private bool FilterPlane(object item)
        {
            if (item is MAYBAY plane)
            {
                return string.IsNullOrEmpty(SearchKeyword) || plane.MAMB.StartsWith(SearchKeyword, StringComparison.OrdinalIgnoreCase);
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