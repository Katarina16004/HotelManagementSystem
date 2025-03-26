using HotelManagementSystem.Models;
using HotelManagementSystem.Services;
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
using System.Windows.Shapes;

namespace HotelManagementSystem
{
    /// <summary>
    /// Interaction logic for Meni.xaml
    /// </summary>
    public partial class Meni : Window
    {
        private Osoblje korisnik;
        private DashboardService _dashboardService;
        public DashboardModel DashboardData { get; set; }
        public Meni(Osoblje korisnik)
        {
            InitializeComponent();
            TB_info.Text = "ID: " + korisnik.IdOsoblja + " \tUsername: " + korisnik.Username;
            if (korisnik.Uloga == "admin")
            {
                TabItem_urediSobe.Visibility = Visibility.Visible;
                TabItem_urediZaposlenog.Visibility = Visibility.Visible;
            }
            else
            {
                TabItem_urediSobe.Visibility = Visibility.Hidden;
                TabItem_urediZaposlenog.Visibility = Visibility.Hidden;
            }

            this.korisnik = korisnik;

            _dashboardService = new DashboardService();
            DashboardData = new DashboardModel();

            this.DataContext = DashboardData; //zbog bindinga

            RefreshDashboardData();

        }
        private void RefreshDashboardData()
        {
            _dashboardService.UpdatePodaci(DashboardData);
        }
        private void ButtonOdjava_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
