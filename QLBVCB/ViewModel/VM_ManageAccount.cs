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
    internal class VM_ManageAccount : VM_Base
    {
        private ObservableCollection<TAIKHOAN> _AccountList;
        public ObservableCollection<TAIKHOAN> AccountList { get { return _AccountList; } set { _AccountList = value; OnPropertyChanged(); } }
        public ICommand OpenAERAccountCommand { get; set; }
        public ICommand ExportExcelManageAccountCommand { get; set; }
        public ICollectionView AccountView { get; private set; }

        public VM_ManageAccount()
        {
            AccountList = new ObservableCollection<TAIKHOAN>(DataProvider.Ins.DB.TAIKHOANs);
            OpenAERAccountCommand = new RelayCommand<object>((p) => { return true; }, (p) => { AERAccount aer = new AERAccount(); aer.ShowDialog(); });
            AccountView = CollectionViewSource.GetDefaultView(AccountList);
            AccountView.Filter = FilterAccount;
            ExportExcelManageAccountCommand = new RelayCommand(ExecuteExportExcelManageAccountCommand);
        }
        private void ExecuteExportExcelManageAccountCommand(object obj)
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
                    excelPackage.Workbook.Properties.Title = "Danh sách tài khoản";
                    excelPackage.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet excelWorkSheet = excelPackage.Workbook.Worksheets[0];
                    excelWorkSheet.Name = "Sheet 1";
                    excelWorkSheet.Cells.Style.Font.Size = 14;
                    excelWorkSheet.Cells.Style.Font.Name = "Times New Roman";

                    string[] columnHeader = { "Tên tài khoản", "Mật khẩu", "Hoạt động", "Mã nhân viên" };
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
                    foreach (var account in AccountList)
                    {
                        excelWorkSheet.Cells[rowIndex, 1].Value = account.TENTK;
                        excelWorkSheet.Cells[rowIndex, 2].Value = account.MATKHAU;
                        excelWorkSheet.Cells[rowIndex, 3].Value = account.HOATDONG;
                        excelWorkSheet.Cells[rowIndex, 4].Value = account.MANV;

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

        private string _SearchAccount;
        public string SearchAccount
        {
            get => _SearchAccount;
            set
            {
                _SearchAccount = value;
                OnPropertyChanged();
                FilterAccount();
            }
        }
        private bool FilterAccount(object item)
        {
            if (item is TAIKHOAN ticket)
            {
                return string.IsNullOrEmpty(SearchAccount) || ticket.TENTK.Contains(SearchAccount);
            }
            return false;
        }
        private void FilterAccount()
        {
            AccountView.Refresh();
        }
    }
}
