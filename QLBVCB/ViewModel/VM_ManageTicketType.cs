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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Data;
using System.ComponentModel;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace QLBVCB.ViewModel
{
    public class LOAIVEWrapper
{
    private LOAIVE _loaive;
    public LOAIVEWrapper(LOAIVE loaive)
    {
        _loaive = loaive;
    }

    public string MALV => _loaive.MALV;
    public string TEN_LOAIVE => _loaive.TEN_LOAIVE;
    public string GIAVE => _loaive.GIAVE?.ToString("N0").Replace(",", ".");
    public string PHI_THAYDOI => _loaive.PHI_THAYDOI?.ToString("N0").Replace(",", ".");
    public string PHI_HUY => _loaive.PHI_HUY?.ToString("N0").Replace(",", ".");
}
    internal class VM_ManageTicketType : VM_Base
    {
        private ObservableCollection<LOAIVEWrapper> _TicketTypeList;
        public ObservableCollection<LOAIVEWrapper> TicketTypeList { get { return _TicketTypeList; } set { _TicketTypeList = value; OnPropertyChanged(); } }

        private string _MALV;
        public string MALV { get => _MALV; set { _MALV = value; OnPropertyChanged(); } }

        private string _TEN_LOAIVE;
        public string TEN_LOAIVE { get => _TEN_LOAIVE; set { _TEN_LOAIVE = value; OnPropertyChanged(); } }

        public ICommand OpenAERTicketTypeCommand { get; set; }
        public ICommand ExportExcelManageTicketTypeCommand { get; set; }

        private string _SearchKeyword;
        public string SearchKeyword
        {
            get => _SearchKeyword;
            set
            {
                _SearchKeyword = value;
                OnPropertyChanged();
                FilterTicketTypes();
            }
        }

        public ICollectionView TicketTypeView { get; private set; }

        public VM_ManageTicketType()
        {
            ExportExcelManageTicketTypeCommand = new RelayCommand(ExecuteExportExcelManageTicketTypeCommand);
            OpenAERTicketTypeCommand = new RelayCommand(ExecutedOpenAERTicketTypeCommand);
            var loaives = new ObservableCollection<LOAIVE>(DataProvider.Ins.DB.LOAIVEs);
            _TicketTypeList = new ObservableCollection<LOAIVEWrapper>(loaives.Select(lv => new LOAIVEWrapper(lv)));
            TicketTypeView = CollectionViewSource.GetDefaultView(TicketTypeList);
            TicketTypeView.Filter = FilterTicketType;
        }

        private void ExecutedOpenAERTicketTypeCommand(object obj)
        {
            try
            {
                AERTicketType aERTicketType = new AERTicketType();
                aERTicketType.ShowDialog();
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }

        private void ExecuteExportExcelManageTicketTypeCommand(object obj)
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

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Title = "Danh sách loại vé";
                    excelPackage.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet excelWorkSheet = excelPackage.Workbook.Worksheets[0];
                    excelWorkSheet.Name = "Sheet 1";
                    excelWorkSheet.Cells.Style.Font.Size = 14;
                    excelWorkSheet.Cells.Style.Font.Name = "Times New Roman";

                    string[] columnHeader = { "Mã loại vé", "Tên loại vé", "Giá vé", "Phí thay đổi", "Phí huỷ" };
                    var countColumnHeader = columnHeader.Length;
                    excelWorkSheet.Cells[1, 1].Value = "Danh sách loại vé";
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
                    foreach (var ticketType in TicketTypeList)
                    {
                        excelWorkSheet.Cells[rowIndex, 1].Value = ticketType.MALV;
                        excelWorkSheet.Cells[rowIndex, 2].Value = ticketType.TEN_LOAIVE;
                        excelWorkSheet.Cells[rowIndex, 3].Value = ticketType.GIAVE;
                        excelWorkSheet.Cells[rowIndex, 4].Value = ticketType.PHI_THAYDOI;
                        excelWorkSheet.Cells[rowIndex, 5].Value = ticketType.PHI_HUY;

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

        private bool FilterTicketType(object item)
        {
            if (item is LOAIVEWrapper ticketType)
            {
                return string.IsNullOrEmpty(SearchKeyword) || ticketType.TEN_LOAIVE.Contains(SearchKeyword);
            }
            return false;
        }

        private void FilterTicketTypes()
        {
            TicketTypeView.Refresh();
        }
    }
}
