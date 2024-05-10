using QLBVCB.UserControls;
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
            uc_Home.Visibility = Visibility.Visible;
            uc_Airplane.Visibility = Visibility.Collapsed;
            uc_Airport.Visibility = Visibility.Collapsed;
            uc_TicketType.Visibility = Visibility.Collapsed;
            uc_Flight.Visibility = Visibility.Collapsed;
            uc_Ticket.Visibility = Visibility.Collapsed;
            uc_Booking.Visibility = Visibility.Collapsed;
            uc_Employee.Visibility = Visibility.Collapsed;
            uc_Customer.Visibility = Visibility.Collapsed;
            uc_Account.Visibility = Visibility.Collapsed;
            uc_Revenue.Visibility = Visibility.Collapsed;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            Login loginWindow = new Login();
            loginWindow.ShowDialog();
        }

        private void btHome_Click(object sender, RoutedEventArgs e)
        {
            uc_Home.Visibility = Visibility.Visible;
            uc_Airplane.Visibility = Visibility.Collapsed;
            uc_Airport.Visibility = Visibility.Collapsed;
            uc_TicketType.Visibility = Visibility.Collapsed;
            uc_Flight.Visibility = Visibility.Collapsed;
            uc_Ticket.Visibility = Visibility.Collapsed;
            uc_Booking.Visibility = Visibility.Collapsed;
            uc_Employee.Visibility = Visibility.Collapsed;
            uc_Customer.Visibility = Visibility.Collapsed;
            uc_Account.Visibility = Visibility.Collapsed;
            uc_Revenue.Visibility = Visibility.Collapsed;
        }

        private void btAirplane_Click(object sender, RoutedEventArgs e)
        {
            uc_Home.Visibility = Visibility.Collapsed;
            uc_Airplane.Visibility = Visibility.Visible;
            uc_Airport.Visibility = Visibility.Collapsed;
            uc_TicketType.Visibility = Visibility.Collapsed;
            uc_Flight.Visibility = Visibility.Collapsed;
            uc_Ticket.Visibility = Visibility.Collapsed;
            uc_Booking.Visibility = Visibility.Collapsed;
            uc_Employee.Visibility = Visibility.Collapsed;
            uc_Customer.Visibility = Visibility.Collapsed;
            uc_Account.Visibility = Visibility.Collapsed;
            uc_Revenue.Visibility = Visibility.Collapsed;
        }

        private void btAirport_Click(object sender, RoutedEventArgs e)
        {
            uc_Home.Visibility = Visibility.Collapsed;
            uc_Airplane.Visibility = Visibility.Collapsed;
            uc_Airport.Visibility = Visibility.Visible;
            uc_TicketType.Visibility = Visibility.Collapsed;
            uc_Flight.Visibility = Visibility.Collapsed;
            uc_Ticket.Visibility = Visibility.Collapsed;
            uc_Booking.Visibility = Visibility.Collapsed;
            uc_Employee.Visibility = Visibility.Collapsed;
            uc_Customer.Visibility = Visibility.Collapsed;
            uc_Account.Visibility = Visibility.Collapsed;
            uc_Revenue.Visibility = Visibility.Collapsed;
        }

        private void btTicketType_Click(object sender, RoutedEventArgs e)
        {
            uc_Home.Visibility = Visibility.Collapsed;
            uc_Airplane.Visibility = Visibility.Collapsed;
            uc_Airport.Visibility = Visibility.Collapsed;
            uc_TicketType.Visibility = Visibility.Visible;
            uc_Flight.Visibility = Visibility.Collapsed;
            uc_Ticket.Visibility = Visibility.Collapsed;
            uc_Booking.Visibility = Visibility.Collapsed;
            uc_Employee.Visibility = Visibility.Collapsed;
            uc_Customer.Visibility = Visibility.Collapsed;
            uc_Account.Visibility = Visibility.Collapsed;
            uc_Revenue.Visibility = Visibility.Collapsed;
        }

        private void btFlight_Click(object sender, RoutedEventArgs e)
        {
            uc_Home.Visibility = Visibility.Collapsed;
            uc_Airplane.Visibility = Visibility.Collapsed;
            uc_Airport.Visibility = Visibility.Collapsed;
            uc_TicketType.Visibility = Visibility.Collapsed;
            uc_Flight.Visibility = Visibility.Visible;
            uc_Ticket.Visibility = Visibility.Collapsed;
            uc_Booking.Visibility = Visibility.Collapsed;
            uc_Employee.Visibility = Visibility.Collapsed;
            uc_Customer.Visibility = Visibility.Collapsed;
            uc_Account.Visibility = Visibility.Collapsed;
            uc_Revenue.Visibility = Visibility.Collapsed;
        }

        private void btTicket_Click(object sender, RoutedEventArgs e)
        {
            uc_Home.Visibility = Visibility.Collapsed;
            uc_Airplane.Visibility = Visibility.Collapsed;
            uc_Airport.Visibility = Visibility.Collapsed;
            uc_TicketType.Visibility = Visibility.Collapsed;
            uc_Flight.Visibility = Visibility.Collapsed;
            uc_Ticket.Visibility = Visibility.Visible;
            uc_Booking.Visibility = Visibility.Collapsed;
            uc_Employee.Visibility = Visibility.Collapsed;
            uc_Customer.Visibility = Visibility.Collapsed;
            uc_Account.Visibility = Visibility.Collapsed;
            uc_Revenue.Visibility = Visibility.Collapsed;
        }

        private void btBooking_Click(object sender, RoutedEventArgs e)
        {
            uc_Home.Visibility = Visibility.Collapsed;
            uc_Airplane.Visibility = Visibility.Collapsed;
            uc_Airport.Visibility = Visibility.Collapsed;
            uc_TicketType.Visibility = Visibility.Collapsed;
            uc_Flight.Visibility = Visibility.Collapsed;
            uc_Ticket.Visibility = Visibility.Collapsed;
            uc_Booking.Visibility = Visibility.Visible;
            uc_Employee.Visibility = Visibility.Collapsed;
            uc_Customer.Visibility = Visibility.Collapsed;
            uc_Account.Visibility = Visibility.Collapsed;
            uc_Revenue.Visibility = Visibility.Collapsed;
        }

        private void btEmployee_Click(object sender, RoutedEventArgs e)
        {
            uc_Home.Visibility = Visibility.Collapsed;
            uc_Airplane.Visibility = Visibility.Collapsed;
            uc_Airport.Visibility = Visibility.Collapsed;
            uc_TicketType.Visibility = Visibility.Collapsed;
            uc_Flight.Visibility = Visibility.Collapsed;
            uc_Ticket.Visibility = Visibility.Collapsed;
            uc_Booking.Visibility = Visibility.Collapsed;
            uc_Employee.Visibility = Visibility.Visible;
            uc_Customer.Visibility = Visibility.Collapsed;
            uc_Account.Visibility = Visibility.Collapsed;
            uc_Revenue.Visibility = Visibility.Collapsed;
        }

        private void btCustomer_Click(object sender, RoutedEventArgs e)
        {
            uc_Home.Visibility = Visibility.Collapsed;
            uc_Airplane.Visibility = Visibility.Collapsed;
            uc_Airport.Visibility = Visibility.Collapsed;
            uc_TicketType.Visibility = Visibility.Collapsed;
            uc_Flight.Visibility = Visibility.Collapsed;
            uc_Ticket.Visibility = Visibility.Collapsed;
            uc_Booking.Visibility = Visibility.Collapsed;
            uc_Employee.Visibility = Visibility.Collapsed;
            uc_Customer.Visibility = Visibility.Visible;
            uc_Account.Visibility = Visibility.Collapsed;
            uc_Revenue.Visibility = Visibility.Collapsed;
        }

        private void btAccount_Click(object sender, RoutedEventArgs e)
        {
            uc_Home.Visibility = Visibility.Collapsed;
            uc_Airplane.Visibility = Visibility.Collapsed;
            uc_Airport.Visibility = Visibility.Collapsed;
            uc_TicketType.Visibility = Visibility.Collapsed;
            uc_Flight.Visibility = Visibility.Collapsed;
            uc_Ticket.Visibility = Visibility.Collapsed;
            uc_Booking.Visibility = Visibility.Collapsed;
            uc_Employee.Visibility = Visibility.Collapsed;
            uc_Customer.Visibility = Visibility.Collapsed;
            uc_Account.Visibility = Visibility.Visible;
            uc_Revenue.Visibility = Visibility.Collapsed;
        }

        private void btRevenue_Click(object sender, RoutedEventArgs e)
        {
            uc_Home.Visibility = Visibility.Collapsed;
            uc_Airplane.Visibility = Visibility.Collapsed;
            uc_Airport.Visibility = Visibility.Collapsed;
            uc_TicketType.Visibility = Visibility.Collapsed;
            uc_Flight.Visibility = Visibility.Collapsed;
            uc_Ticket.Visibility = Visibility.Collapsed;
            uc_Booking.Visibility = Visibility.Collapsed;
            uc_Employee.Visibility = Visibility.Collapsed;
            uc_Customer.Visibility = Visibility.Collapsed;
            uc_Account.Visibility = Visibility.Collapsed;
            uc_Revenue.Visibility = Visibility.Visible;
        }
    }
}
