using OfficeOpenXml.Style;
using OfficeOpenXml;
using QLBVCB.Model;
using QLBVCB.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System;
using QLBVCB.View;
namespace QLBVCB.ViewModel
{
    internal class VM_ManageService : VM_Base
    {
        private ObservableCollection<DICHVU> _ServiceList;
        public ObservableCollection<DICHVU> ServiceList
        {
            get { return _ServiceList; }
            set { _ServiceList = value; OnPropertyChanged(); }
        }

        private string _MADV;
        public string MADV { get => _MADV; set { _MADV = value; OnPropertyChanged(); } }

        private string _LOAIDV;
        public string LOAIDV { get => _LOAIDV; set { _LOAIDV = value; OnPropertyChanged(); } }

        private string _TENDV;
        public string TENDV { get => _TENDV; set { _TENDV = value; OnPropertyChanged(); } }

        private int _SOLUONG;
        public int SOLUONG { get => _SOLUONG; set { _SOLUONG = value; OnPropertyChanged(); } }

        private string _DONGIA;
        public string DONGIA { get => _DONGIA; set { _DONGIA = value; OnPropertyChanged(); } }

        public ICommand OpenAERServiceCommand { get; set; }
        public ICommand ExportExcelManageServiceCommand { get; set; }

        private string _SearchKeyword;
        public string SearchKeyword
        {
            get => _SearchKeyword;
            set
            {
                _SearchKeyword = value;
                OnPropertyChanged();
                ServiceView.Refresh();
            }
        }

        public ICollectionView ServiceView { get; private set; }

        public VM_ManageService()
        {
            OpenAERServiceCommand = new RelayCommand(ExecuteOpenAERServiceCommand);
            ExportExcelManageServiceCommand = new RelayCommand(ExecuteExportExcelManageServiceCommand);
            ServiceList = new ObservableCollection<DICHVU>(DataProvider.Ins.DB.DICHVUs);
            ServiceView = CollectionViewSource.GetDefaultView(ServiceList);
            ServiceView.Filter = FilterService;
        }

        private void ExecuteOpenAERServiceCommand(object obj)
        {
            try
            {
                AER_Service aERService = new AER_Service();
                aERService.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }

        private void ExecuteExportExcelManageServiceCommand(object obj)
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
                    excelPackage.Workbook.Properties.Title = "Danh sách dịch vụ";
                    excelPackage.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet excelWorkSheet = excelPackage.Workbook.Worksheets[0];
                    excelWorkSheet.Name = "Sheet 1";
                    excelWorkSheet.Cells.Style.Font.Size = 14;
                    excelWorkSheet.Cells.Style.Font.Name = "Times New Roman";

                    string[] columnHeader = { "Mã dịch vụ", "Loại dịch vụ", "Tên dịch vụ", "Số lượng", "Đơn giá" };
                    var countColumnHeader = columnHeader.Length;
                    excelWorkSheet.Cells[1, 1].Value = "Danh sách dịch vụ";
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
                    foreach (var Service in ServiceList)
                    {
                        excelWorkSheet.Cells[rowIndex, 1].Value = Service.MADV;
                        excelWorkSheet.Cells[rowIndex, 2].Value = Service.LOAIDV;
                        excelWorkSheet.Cells[rowIndex, 3].Value = Service.TENDV;
                        excelWorkSheet.Cells[rowIndex, 4].Value = Service.SOLUONG;
                        excelWorkSheet.Cells[rowIndex, 5].Value = Service.DONGIA;

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

        private bool FilterService(object item)
        {
            if (item is DICHVU service)
            {
                return string.IsNullOrEmpty(SearchKeyword) || service.TENDV.StartsWith(SearchKeyword, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}