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
    internal class VM_ManageEmployee : VM_Base
    {
        private ObservableCollection<NHANVIEN> _EmployeeList;
        public ObservableCollection<NHANVIEN> EmployeeList { get { return _EmployeeList; } set { _EmployeeList = value; OnPropertyChanged(); } }
        public ICommand OpenAEREmployeeCommand { get; set; }
        public ICommand ExportExcelManageEmployeeCommand { get; set; }
        public ICollectionView EmployeeView { get; private set; }

        public VM_ManageEmployee()
        {
            EmployeeList = new ObservableCollection<NHANVIEN>(DataProvider.Ins.DB.NHANVIENs);
            OpenAEREmployeeCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (position > 1)
                    ShowCustomMessageBox("Bạn không có quyền chỉnh sửa!");
                else
                {
                    AEREmployee aer = new AEREmployee();
                    aer.DataContext = new VM_AEREmployee();
                    aer.ShowDialog();
                }
            });
            EmployeeView = CollectionViewSource.GetDefaultView(EmployeeList);
            EmployeeView.Filter = FilterEmployee;
            ExportExcelManageEmployeeCommand = new RelayCommand(ExecuteExportExcelManageCustomerCommand);
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
                    excelPackage.Workbook.Properties.Title = "Danh sách nhân viên";
                    excelPackage.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet excelWorkSheet = excelPackage.Workbook.Worksheets[0];
                    excelWorkSheet.Name = "Sheet 1";
                    excelWorkSheet.Cells.Style.Font.Size = 14;
                    excelWorkSheet.Cells.Style.Font.Name = "Times New Roman";

                    string[] columnHeader = { "Mã nhân viên", "Họ tên", "Ngày sinh", "Giới tính", "Căn cước công dân", "Địa chỉ", "Số điện thoại", "Email", "Lương", "Vị trí" };
                    var countColumnHeader = columnHeader.Length;
                    excelWorkSheet.Cells[1, 1].Value = "Danh sách nhân viên";
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
                    foreach (var employee in EmployeeList)
                    {
                        excelWorkSheet.Cells[rowIndex, 1].Value = employee.MANV;
                        excelWorkSheet.Cells[rowIndex, 2].Value = employee.HOTEN;
                        excelWorkSheet.Cells[rowIndex, 3].Value = employee.NGAYSINH.ToString();
                        excelWorkSheet.Cells[rowIndex, 4].Value = employee.GIOITINH;
                        excelWorkSheet.Cells[rowIndex, 5].Value = employee.CCCD;
                        excelWorkSheet.Cells[rowIndex, 6].Value = employee.DIACHI;
                        excelWorkSheet.Cells[rowIndex, 7].Value = employee.SDT;
                        excelWorkSheet.Cells[rowIndex, 8].Value = employee.EMAIL;
                        excelWorkSheet.Cells[rowIndex, 9].Value = employee.LUONG;
                        excelWorkSheet.Cells[rowIndex, 10].Value = employee.VITRI;

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

        private string _SearchEmployee;
        public string SearchEmployee
        {
            get => _SearchEmployee;
            set
            {
                _SearchEmployee = value;
                OnPropertyChanged();
                FilterEmployee();
            }
        }
        private bool FilterEmployee(object item)
        {
            if (item is NHANVIEN employee)
            {
                return string.IsNullOrEmpty(SearchEmployee) || employee.HOTEN.Contains(SearchEmployee);
            }
            return false;
        }
        private void FilterEmployee()
        {
            EmployeeView.Refresh();
        }
        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
    }
}