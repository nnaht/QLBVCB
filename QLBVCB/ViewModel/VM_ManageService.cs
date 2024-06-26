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
using System.Globalization;
namespace QLBVCB.ViewModel
{
    public class DongiaFormatterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (decimal.TryParse(value.ToString(), out decimal decimalValue))
            {
                return string.Format(CultureInfo.InvariantCulture, "{0:N0}", decimalValue);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (decimal.TryParse(value.ToString(), NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out decimal decimalValue))
            {
                return decimalValue;
            }

            return value;
        }
    }
    public class NullToDefaultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (value is int intValue && intValue == 0))
            {
                return "Không có";
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
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

        private int? _SOLUONG;
        public int? SOLUONG { get => _SOLUONG; set { _SOLUONG = value; OnPropertyChanged(); } }

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
                aERService.DataContext = new VM_AERService();
                aERService.ShowDialog();
            }
            catch (Exception ex)
            {
                ShowCustomMessageBox("Có lỗi xảy ra: " + ex.Message);
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
                    ShowCustomMessageBox("Đường dẫn không hợp lệ");
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
                    foreach (var service in ServiceList)
                    {
                        excelWorkSheet.Cells[rowIndex, 1].Value = service.MADV;
                        excelWorkSheet.Cells[rowIndex, 2].Value = service.LOAIDV;
                        excelWorkSheet.Cells[rowIndex, 3].Value = service.TENDV;
                        excelWorkSheet.Cells[rowIndex, 4].Value = service.SOLUONG ?? (object)"Không có";
                        excelWorkSheet.Cells[rowIndex, 5].Value = service.DONGIA;

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

        private bool FilterService(object item)
        {
            if (item is DICHVU service)
            {
                return string.IsNullOrEmpty(SearchKeyword) || service.TENDV.Contains(SearchKeyword);
            }
            return false;
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