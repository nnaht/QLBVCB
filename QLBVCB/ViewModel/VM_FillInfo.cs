using PdfSharp.Drawing;
using PdfSharp.Pdf;
using QLBVCB.Model;
using QLBVCB.UserControls;
using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;

namespace QLBVCB.ViewModel
{
    internal class VM_FillInfo : VM_Base
    {
        public ICommand BookCommand { get; set; }
        public ICommand AddCustomerCommand { get; set; }
        public ObservableCollection<VM_CustomerInfo> Customers { get; set; }
        public ObservableCollection<CustomerInfo> CustomerInfos { get; set; }
        private string sdt;
        public string SDT
        {
            get => sdt;
            set
            {
                sdt = value;
                OnPropertyChanged(nameof(SDT));
            }
        }
        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
        private int totalPeople;
        public ICommand ExportPdfCommand { get; set; }
        public VM_FillInfo(List<Tuple<string, int, int>> selection, bool isRecuperate)
        {
            ExportPdfCommand = new RelayCommand(ExecuteExportPdfCommand);
            BookCommand = new RelayCommand(ExecuteBookCommand);
            AddCustomerCommand = new RelayCommand<Button>((p) => { return true; }, (p) => { CustomerRegister cr = new CustomerRegister(); cr.DataContext = new VM_CustomerRegister(); cr.ShowDialog(); });
            Customers = new ObservableCollection<VM_CustomerInfo>();
            setTotalPeople(selection, isRecuperate);
            for (int i = 0; i < totalPeople; i++)
            {
                var selectionItem = selection[i];
                Customers.Add(new VM_CustomerInfo(selectionItem.Item1, selectionItem.Item2, selectionItem.Item3, isRecuperate));
            }

            CustomerInfos = new ObservableCollection<CustomerInfo>();
            foreach (var customer in Customers)
            {
                var customerInfo = new CustomerInfo();
                customerInfo.DataContext = customer;
                CustomerInfos.Add(customerInfo);
            }
        }
        private void ExecuteExportPdfCommand(object obj)
        {
            var mainWindow = Application.Current.Windows.OfType<FillInfo>().FirstOrDefault();
            if (mainWindow != null)
            {
                var itemsControl = mainWindow.FindName("CustomerInfoControl") as ItemsControl;
                if (itemsControl != null)
                {
                    string filePath = "CustomerInfo.pdf";
                    ExportItemsControlToPdf(itemsControl, filePath);
                    ShowCustomMessageBox($"Customer info has been successfully exported to {filePath}");
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                }
                else
                {
                    ShowCustomMessageBox("Failed to find CustomerInfoControl.");
                }
            }
            else
            {
                ShowCustomMessageBox("Failed to find the main window.");
            }
        }

        private void ExportItemsControlToPdf(ItemsControl itemsControl, string filePath)
        {
            double dpi = 96;
            double width = itemsControl.ActualWidth;
            double height = itemsControl.ActualHeight;

            var size = new Size(width, height);
            itemsControl.Measure(size);
            itemsControl.Arrange(new Rect(size));

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)width, (int)height, dpi, dpi, PixelFormats.Pbgra32);
            rtb.Render(itemsControl);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);

                using (PdfDocument document = new PdfDocument())
                {
                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    XImage image = XImage.FromStream(ms);

                    double aspectRatio = image.PixelWidth / (double)image.PixelHeight;
                    double pageWidth = page.Width;
                    double pageHeight = pageWidth / aspectRatio;
                    if (pageHeight > page.Height)
                    {
                        pageHeight = page.Height;
                        pageWidth = pageHeight * aspectRatio;
                    }

                    gfx.DrawImage(image, 0, 0, pageWidth, pageHeight);
                    document.Save(filePath);
                }
            }
        }
        private void ExecuteBookCommand(object obj)
        {
            List<string> names = Customers.Select(customer => customer.PassengerName).ToList();

            try
            {
                var macbList = Customers.Select(c => c.MACB).ToList();
                var chuyenBays = DataProvider.Ins.DB.CHUYENBAYs.Where(cb => macbList.Contains(cb.MACB)).ToList();

                for (int i = 0; i < Customers.Count; i++)
                {
                    var customer = Customers[i];
                    var chuyenBay = chuyenBays.SingleOrDefault(cb => cb.MACB == customer.MACB);
                    if (chuyenBay != null)
                    {
                        var selectedMealOption = customer.SelectedMealOption;
                        var selectedLuggageOption = customer.SelectedLuggageOption;

                        var suatan = DataProvider.Ins.DB.DICHVUs
                            .Where(dv => dv.LOAIDV == "Suất ăn" && dv.TENDV == selectedMealOption)
                            .SingleOrDefault();
                        var hanhly = DataProvider.Ins.DB.DICHVUs
                            .Where(dv => dv.LOAIDV == "Hành lý" && dv.TENDV == selectedLuggageOption)
                            .SingleOrDefault();

                        if (suatan == null || hanhly == null)
                        {
                            ShowCustomMessageBox("Vui lòng chọn dịch vụ và suất ăn!");
                            return;
                        }

                        var khachHang = DataProvider.Ins.DB.KHACHHANGs.SingleOrDefault(kh => kh.SDT == SDT);
                        if (khachHang == null)
                        {
                            ShowCustomMessageBox("Số điện thoại không tồn tại!");
                            return;
                        }

                        string mavb = GetNextId(i);

                        var veBay = new VEBAY
                        {
                            MAVB = mavb,
                            MACB = customer.MACB,
                            MALV= customer.HANG >= 6 ? "LV01": "LV02",
                            THUTU_GHE = customer.DAY.ToString()+customer.HANG.ToString(),
                            
                        };
                        DataProvider.Ins.DB.VEBAYs.Add(veBay);

                        var dadat = new DADAT
                        {
                            MACB = customer.MACB,
                            Hang = customer.HANG,
                            Day = customer.DAY,
                            TENHANHKHACH = names[i],
                            MAKH = khachHang.MAKH,
                            MAVB = mavb,
                            MABK = GetBNextId(i),
                            MASA = suatan.MADV,
                            MAHL = hanhly.MADV,
                            NGTHANHTOAN = DateTime.Now
                        };

                        DataProvider.Ins.DB.DADATs.Add(dadat);
                        chuyenBay.SO_GHE -= 1;
                        DataProvider.Ins.DB.SaveChanges();
                    }
                }


                ShowCustomMessageBox("Đặt vé thành công.");
                Application.Current.Windows.OfType<FillInfo>().FirstOrDefault()?.Close();
                CloseWindow(Application.Current.MainWindow);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += "\n" + ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                    {
                        errorMessage += "\n" + ex.InnerException.InnerException.Message;
                    }
                }
                ShowCustomMessageBox("Đặt chỗ không thành công");
            }
        }
        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
        private string GetBNextId(int i)
        {
            var lastTicket = DataProvider.Ins.DB.DADATs.OrderByDescending(e => e.MABK).FirstOrDefault();
            if (lastTicket != null)
            {
                int nextId = int.Parse(lastTicket.MABK.Substring(2)) + 1 + i;
                return "BK" + nextId.ToString().PadLeft(4, '0');
            }
            else
            {
                return "BK0001";
            }
        }

        private string GetNextId(int i)
        {
            var lastTicket = DataProvider.Ins.DB.VEBAYs.OrderByDescending(e => e.MAVB).FirstOrDefault();
            if (lastTicket != null)
            {
                int nextId = int.Parse(lastTicket.MAVB.Substring(2)) + 1 + i;
                return "VB" + nextId.ToString().PadLeft(4, '0');
            }
            else
            {
                return "VB0001";
            }
        }


       
        public void setTotalPeople(List<Tuple<string, int, int>> selection, bool isRecuperate)
        {
            totalPeople = selection.Count;
        }

    }
}
