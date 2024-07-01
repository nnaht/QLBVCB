using PdfSharp.Drawing;
using PdfSharp.Pdf;
using QLBVCB.Model;
using QLBVCB.UserControls;
using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace QLBVCB.ViewModel
{
    internal class VM_WCustomerTicket : VM_Base
    {
        public ObservableCollection<VM_CustomerTicket> Customers { get; set; }
        public ObservableCollection<UC_CustomerTicker> CustomerInfos { get; set; }
        public void ShowCustomMessageBox(string message)
        {
            CusMessBox customMessageBox = new CusMessBox();
            customMessageBox.DataContext = new VM_CusMessBox(message);
            customMessageBox.ShowDialog();
        }
        private int totalPeople;
        private String totalPrice;
        public string TotalPrice
        {
            get => totalPrice;
            set
            {
                totalPrice = value;
                OnPropertyChanged(nameof(totalPrice));
            }
        }
        public ICommand ExportPdfCommand { get; set; }
        public VM_WCustomerTicket(List<Tuple<string, int, int,string,string,string>> selection, bool isRecuperate)
        {
            ExportPdfCommand = new RelayCommand(ExecuteExportPdfCommand);
            Customers = new ObservableCollection<VM_CustomerTicket>();
            setTotalPeople(selection, isRecuperate);
            for (int i = 0; i < totalPeople; i++)
            {
                var selectionItem = selection[i];
                Customers.Add(new VM_CustomerTicket(selectionItem.Item1, selectionItem.Item2, selectionItem.Item3, isRecuperate, selectionItem.Item4, selectionItem.Item5,selectionItem.Item6));
            }

            CustomerInfos = new ObservableCollection<UC_CustomerTicker>();
            foreach (var customer in Customers)
            {
                var customerInfo = new UC_CustomerTicker();
                customerInfo.DataContext = customer;
                CustomerInfos.Add(customerInfo);
            }
            SetTotalPrice();
        }

        private decimal GetLuggageFee(String selectedLuggageOption)
        {
            return DataProvider.Ins.DB.DICHVUs
                            .Where(dv => dv.LOAIDV == "Hành lý" && dv.TENDV == selectedLuggageOption)
                            .SingleOrDefault().DONGIA;

        }

        private decimal GetMealFee(String selectedMealOption)
        {
            return DataProvider.Ins.DB.DICHVUs
                            .Where(dv => dv.LOAIDV == "Suất ăn" && dv.TENDV == selectedMealOption)
                            .SingleOrDefault().DONGIA;

        }

        private decimal GetSeatFee(String selectedTicketType)
        {
            String tempString;
            if(selectedTicketType == "Business Class")
            {
                tempString = "Business";
            }
            else
            {
                tempString = "Economy";
            }
            var temp = DataProvider.Ins.DB.LOAIVEs
                            .Where(dv => dv.TEN_LOAIVE == tempString)
                            .SingleOrDefault().GIAVE;
            if(temp != null)
            {
                return (decimal)temp;
            }
            else
            {
                return 0;
            }
        }
        private String ConvertToCurrency(decimal value)
        {
            string temp = value.ToString("N0").Replace(",", ".") + "VND";
            return temp;
        }
        private void SetTotalPrice()
        {
            decimal temp = 0;
            foreach (var customer in Customers)
            {
                temp += GetLuggageFee(customer.HANHLY) + GetMealFee(customer.SUATAN) + GetSeatFee(customer.SeatType);
                
            }

            TotalPrice = ConvertToCurrency(temp);
        }


        private void ExecuteExportPdfCommand(object obj)
        {
            var mainWindow = Application.Current.Windows.OfType<CustomerTicket>().FirstOrDefault();
            if (mainWindow != null)
            {
                var itemsControl = mainWindow.FindName("UC_CustomerTicker") as ItemsControl;
                if (itemsControl != null)
                {
                    string filePath = "CustomerInfo.pdf";
                    ExportItemsControlToPdf(itemsControl, filePath);
                    ShowCustomMessageBox($"Xuất vé thành công. {filePath}");
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
        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
        

        public void setTotalPeople(List<Tuple<string, int, int,string,string,string>> selection, bool isRecuperate)
        {
            totalPeople = selection.Count;
        }

    }
}
