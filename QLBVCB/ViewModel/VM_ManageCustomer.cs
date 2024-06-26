using OfficeOpenXml;
using OfficeOpenXml.Style;
using QLBVCB.Model;
using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace QLBVCB.ViewModel
{
    internal class VM_ManageCustomer : VM_Base
    {
        private ObservableCollection<KHACHHANG> _CustomerList;
        public ObservableCollection<KHACHHANG> CustomerList { get { return _CustomerList; } set { _CustomerList = value; OnPropertyChanged(); } }
        public ICommand OpenAERCustomerCommand { get; set; }
        public ICommand ExportExcelManageCustomerCommand { get; set; }
        public ICollectionView CustomerView { get; private set; }
        public VM_ManageCustomer()
        {
            CustomerList = new ObservableCollection<KHACHHANG>(DataProvider.Ins.DB.KHACHHANGs);
            OpenAERCustomerCommand = new RelayCommand<object>((p) => { return true; }, (p) => { AERCustomer aer = new AERCustomer(); aer.DataContext = new VM_AERCustomer(); aer.ShowDialog(); });
            CustomerView = CollectionViewSource.GetDefaultView(CustomerList);
            CustomerView.Filter = FilterCustomer;
            ExportExcelManageCustomerCommand = new RelayCommand(ExecuteExportExcelManageCustomerCommand);
        }
        private void ExecuteExportExcelManageCustomerCommand(object obj)
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
                    excelPackage.Workbook.Properties.Title = "Danh sách khách hàng";
                    excelPackage.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet excelWorkSheet = excelPackage.Workbook.Worksheets[0];
                    excelWorkSheet.Name = "Sheet 1";
                    excelWorkSheet.Cells.Style.Font.Size = 14;
                    excelWorkSheet.Cells.Style.Font.Name = "Times New Roman";

                    string[] columnHeader = { "Mã khách hàng", "Họ tên", "Ngày sinh", "Giới tính", "Căn cước công dân", "Địa chỉ", "Số điện thoại", "Email" };
                    var countColumnHeader = columnHeader.Length;
                    excelWorkSheet.Cells[1, 1].Value = "Danh sách khách hàng";
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
                    foreach (var customer in CustomerList)
                    {
                        excelWorkSheet.Cells[rowIndex, 1].Value = customer.MAKH;
                        excelWorkSheet.Cells[rowIndex, 2].Value = customer.HOTEN;
                        excelWorkSheet.Cells[rowIndex, 3].Value = customer.NGAYSINH.ToString();
                        excelWorkSheet.Cells[rowIndex, 4].Value = customer.GIOITINH;
                        excelWorkSheet.Cells[rowIndex, 5].Value = customer.CCCD;
                        excelWorkSheet.Cells[rowIndex, 6].Value = customer.DIACHI;
                        excelWorkSheet.Cells[rowIndex, 7].Value = customer.SDT;
                        excelWorkSheet.Cells[rowIndex, 8].Value = customer.EMAIL;

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

        private string _SearchCustomer;
        public string SearchCustomer
        {
            get => _SearchCustomer;
            set
            {
                _SearchCustomer = value;
                OnPropertyChanged();
                FilterCustomer();
            }
        }
        private bool FilterCustomer(object item)
        {
            if (item is KHACHHANG customer)
            {
                return string.IsNullOrEmpty(SearchCustomer) || customer.HOTEN.Contains(SearchCustomer);
            }
            return false;
        }
        private void FilterCustomer()
        {
            CustomerView.Refresh();
        }
        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
    }
}
