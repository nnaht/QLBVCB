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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLBVCB.ViewModel
{
    internal class VM_ManageTicket : VM_Base
    {
        private ObservableCollection<VEBAY> _TicketList;
        public ObservableCollection<VEBAY> TicketList { get { return _TicketList; } set { _TicketList = value; OnPropertyChanged(); } }
        public ICommand OpenAERTicketCommand { get; set; }
        public ICommand ExportExcelManageTicketCommand { get; set; }
        public ICollectionView TicketView { get; private set; }

        public VM_ManageTicket()
        {
            TicketList = new ObservableCollection<VEBAY>(DataProvider.Ins.DB.VEBAYs);
            OpenAERTicketCommand = new RelayCommand<object>((p) => { return true; }, (p) => { AERTicket aer = new AERTicket(); aer.DataContext = new VM_AERTicket(); aer.ShowDialog(); });
            TicketView = CollectionViewSource.GetDefaultView(TicketList);
            TicketView.Filter = FilterTicket;
            ExportExcelManageTicketCommand = new RelayCommand(ExecuteExportExcelManageTicketCommand);
        }
        private void ExecuteExportExcelManageTicketCommand(object obj)
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
                    excelPackage.Workbook.Properties.Title = "Danh sách vé bay";
                    excelPackage.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet excelWorkSheet = excelPackage.Workbook.Worksheets[0];
                    excelWorkSheet.Name = "Sheet 1";
                    excelWorkSheet.Cells.Style.Font.Size = 14;
                    excelWorkSheet.Cells.Style.Font.Name = "Times New Roman";

                    string[] columnHeader = { "Mã vé bay", "Mã chuyến bay", "Ghế", "Mã loại vé", "Tên người đặt", "Tên hành khách" };
                    var countColumnHeader = columnHeader.Length;
                    excelWorkSheet.Cells[1, 1].Value = "Danh sách vé bay";
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
                    foreach (var ticket in TicketList)
                    {
                        excelWorkSheet.Cells[rowIndex, 1].Value = ticket.MAVB;
                        excelWorkSheet.Cells[rowIndex, 2].Value = ticket.MACB;
                        excelWorkSheet.Cells[rowIndex, 3].Value = ticket.THUTU_GHE;
                        excelWorkSheet.Cells[rowIndex, 4].Value = ticket.MALV;
                        excelWorkSheet.Cells[rowIndex, 5].Value = ticket.TENNGUOIDAT;
                        excelWorkSheet.Cells[rowIndex, 6].Value = ticket.TENHANHKHACH;

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

        private string _SearchTicket;
        public string SearchTicket
        {
            get => _SearchTicket;
            set
            {
                _SearchTicket = value;
                OnPropertyChanged();
                FilterTicket();
            }
        }
        private bool FilterTicket(object item)
        {
            if (item is VEBAY ticket)
            {
                return string.IsNullOrEmpty(SearchTicket) || ticket.MACB.IndexOf(SearchTicket, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return false;
        }
        private void FilterTicket()
        {
            TicketView.Refresh();
        }
        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
    }
}
