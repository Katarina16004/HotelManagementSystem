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
        private OperacijeNadGostomService _operacijeNadGostomService;
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
            _operacijeNadGostomService = new OperacijeNadGostomService();

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

        private void Button_pretraziGosta_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> parametri = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(TextBox_ime.Text))
                parametri.Add("Ime", TextBox_ime.Text);

            if (!string.IsNullOrEmpty(TextBox_prezime.Text))
                parametri.Add("Prezime", TextBox_prezime.Text);

            if (!string.IsNullOrEmpty(TextBox_pol.Text))
                parametri.Add("Pol", TextBox_pol.Text);

            if (!string.IsNullOrEmpty(TextBox_telefon.Text))
                parametri.Add("Telefon", TextBox_telefon.Text);

            if (!string.IsNullOrEmpty(TextBox_drzavljanstvo.Text))
                parametri.Add("Drzavljanstvo", TextBox_drzavljanstvo.Text);

            if (!string.IsNullOrEmpty(TextBox_pasos.Text))
                parametri.Add("Pasos", TextBox_pasos.Text);

            if (!string.IsNullOrEmpty(TextBox_licna.Text))
                parametri.Add("LicnaKarta", TextBox_licna.Text);

            var rezultati = _operacijeNadGostomService.Pretrazi(parametri);

            DataGrid_gosti.ItemsSource = rezultati;
        }

        private void DataGrid_gosti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid_gosti.SelectedItem is Gost selektovaniGost)
            {
                TextBox_ime.Text = selektovaniGost.Ime;
                TextBox_prezime.Text = selektovaniGost.Prezime;
                TextBox_pol.Text = selektovaniGost.Pol;
                TextBox_telefon.Text = selektovaniGost.Telefon;
                TextBox_drzavljanstvo.Text = selektovaniGost.Drzavljanstvo;
                TextBox_pasos.Text = selektovaniGost.Pasos;
                TextBox_licna.Text = selektovaniGost.LicnaKarta;
            }
        }

        private void Button_dodajGosta_Click(object sender, RoutedEventArgs e)
        {
            var ime = TextBox_ime.Text;
            var prezime = TextBox_prezime.Text;
            var pol = TextBox_pol.Text;
            var telefon = TextBox_telefon.Text;
            var drzavljanstvo = TextBox_drzavljanstvo.Text;
            var pasos = TextBox_pasos.Text;
            var licnaKarta = TextBox_licna.Text;

            Gost noviGost = new Gost(ime, prezime, pol, telefon, drzavljanstvo, pasos, licnaKarta);

            _operacijeNadGostomService.Dodaj(noviGost);
        }
    }
}
