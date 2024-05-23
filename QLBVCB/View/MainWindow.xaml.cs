using QLBVCB.UserControls;
using QLBVCB.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QLBVCB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            Login loginWindow = new Login();
            loginWindow.Show();
        }

        private void btRevenue_Click(object sender, RoutedEventArgs e)
        {
            //uc_Home.Visibility = Visibility.Collapsed;
            //uc_Airplane.Visibility = Visibility.Collapsed;
            //uc_Airport.Visibility = Visibility.Collapsed;
            //uc_TicketType.Visibility = Visibility.Collapsed;
            //uc_Flight.Visibility = Visibility.Collapsed;
            //uc_Ticket.Visibility = Visibility.Collapsed;
            //uc_Booking.Visibility = Visibility.Collapsed;
            //uc_Employee.Visibility = Visibility.Collapsed;
            //uc_Customer.Visibility = Visibility.Collapsed;
            //uc_Account.Visibility = Visibility.Collapsed;
            //uc_Revenue.Visibility = Visibility.Visible;
        }
    }
}
