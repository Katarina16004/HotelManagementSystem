using HotelManagementSystem.Models;
using HotelManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private RadnikSobeService _radnikSobeService;
        private RezervacijeService _rezervacijeService;
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

            _rezervacijeService = new RezervacijeService();
            _dashboardService = new DashboardService();
            DashboardData = new DashboardModel();
            _operacijeNadGostomService = new OperacijeNadGostomService();
            _radnikSobeService = new RadnikSobeService();

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

            if (!string.IsNullOrEmpty(TextBox_telefon.Text))
                parametri.Add("Telefon", TextBox_telefon.Text);

            if (!string.IsNullOrEmpty(TextBox_drzavljanstvo.Text))
                parametri.Add("Drzavljanstvo", TextBox_drzavljanstvo.Text);

            if (!string.IsNullOrEmpty(TextBox_pol.Text))
                parametri.Add("Pol", TextBox_pol.Text);

            if (!string.IsNullOrEmpty(TextBox_pasos.Text))
                parametri.Add("Pasos", TextBox_pasos.Text);

            if (!string.IsNullOrEmpty(TextBox_licna.Text))
                parametri.Add("licna_karta", TextBox_licna.Text);

            var rezultati = _operacijeNadGostomService.Pretrazi(parametri);

            DataGrid_gosti.ItemsSource = rezultati;
            if(rezultati.Count()>0 )
            {
                TextBox_ime.Text = "";
                TextBox_prezime.Text = "";
                TextBox_telefon.Text = "";
                TextBox_drzavljanstvo.Text = "";
                TextBox_pol.Text = "";
                TextBox_pasos.Text = "";
                TextBox_licna.Text = "";
            }
        }

        private void DataGrid_gosti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid_gosti.SelectedItem is Gost selektovaniGost)
            {
                TextBox_ime.Text = selektovaniGost.Ime;
                TextBox_prezime.Text = selektovaniGost.Prezime;
                TextBox_telefon.Text = selektovaniGost.Telefon;
                TextBox_drzavljanstvo.Text = selektovaniGost.Drzavljanstvo;
                TextBox_pol.Text = selektovaniGost.Pol;
                TextBox_pasos.Text = selektovaniGost.Pasos;
                TextBox_licna.Text = selektovaniGost.LicnaKarta;
            }
        }

        private void Button_dodajGosta_Click(object sender, RoutedEventArgs e)
        {
            var ime = TextBox_ime.Text;
            var prezime = TextBox_prezime.Text;
            var telefon = TextBox_telefon.Text;
            var drzavljanstvo = TextBox_drzavljanstvo.Text;
            var pol = TextBox_pol.Text;
            string?  pasos, licnaKarta;
            if (TextBox_pasos.Text == "")
                pasos = null;
            else
                pasos = TextBox_pasos.Text;
            if(TextBox_licna.Text == "")
                licnaKarta = null;
            else
                licnaKarta= TextBox_licna.Text;

            Gost noviGost = new Gost(ime, prezime, telefon, drzavljanstvo, pol, pasos, licnaKarta);

            if(_operacijeNadGostomService.Dodaj(noviGost))
            {
                DashboardData.BrojGostiju++;
                OsveziDataGrid();
            }
        }

        private void Button_obrisiGosta_Click(object sender, RoutedEventArgs e)
        {
            var ime = TextBox_ime.Text;
            var prezime = TextBox_prezime.Text;
            var telefon = TextBox_telefon.Text;
            var drzavljanstvo = TextBox_drzavljanstvo.Text;
            var pol = TextBox_pol.Text;
            var pasos = TextBox_pasos.Text;
            var licnaKarta = TextBox_licna.Text;

            Gost noviGost = new Gost(ime, prezime, telefon, drzavljanstvo, pol, pasos, licnaKarta);
            if(_operacijeNadGostomService.Obrisi(noviGost))
            {
                DashboardData.BrojGostiju--;
                OsveziDataGrid();
            }
        }
        private void OsveziDataGrid()
        {
            TextBox_ime.Text = "";
            TextBox_prezime.Text = "";
            TextBox_telefon.Text = "";
            TextBox_drzavljanstvo.Text = "";
            TextBox_pol.Text = "";
            TextBox_pasos.Text = "";
            TextBox_licna.Text = "";
            var rezultati = _operacijeNadGostomService.PrikaziSveGoste();
            DataGrid_gosti.ItemsSource = rezultati;
        }

        private void Button_izmeniGosta_Click(object sender, RoutedEventArgs e)
        {
            var selectedGost = (Gost)DataGrid_gosti.SelectedItem;
            if (selectedGost != null)
            {
                var id = selectedGost.Id;
                var ime = TextBox_ime.Text;
                var prezime = TextBox_prezime.Text;
                var telefon = TextBox_telefon.Text;
                var drzavljanstvo = TextBox_drzavljanstvo.Text;
                var pol = TextBox_pol.Text;
                string? pasos, licnaKarta;
                if (TextBox_pasos.Text == "")
                    pasos = null;
                else
                    pasos = TextBox_pasos.Text;
                if (TextBox_licna.Text == "")
                    licnaKarta = null;
                else
                    licnaKarta = TextBox_licna.Text;


                Gost noviGost = new Gost(id, ime, prezime, telefon, drzavljanstvo, pol, pasos, licnaKarta);
                if (noviGost != null)
                {
                    if (_operacijeNadGostomService.Izmeni(noviGost))
                        OsveziDataGrid();
                }
            }
        }

        private void Button_pretraziSobu_Click(object sender, RoutedEventArgs e)
        {
            List<string> tipovi = new List<string>();
            if (CheckBox_jednokrevetna.IsChecked == true) 
                   tipovi.Add("Jednokrevetna");
            if (CheckBox_dvokrevetna.IsChecked == true) 
                tipovi.Add("Dvokrevetna");
            if (CheckBox_trokrevetna.IsChecked == true) 
                tipovi.Add("Trokrevetna");
            List<int> spratovi = new List<int>();
            if (CheckBox_prizemlje.IsChecked == true) 
                spratovi.Add(0);
            if (CheckBox_prviSprat.IsChecked == true) 
                spratovi.Add(1);
            if (CheckBox_drugiSprat.IsChecked == true) 
                spratovi.Add(2);
            if (CheckBox_treciSprat.IsChecked == true) 
                spratovi.Add(3);
            if (CheckBox_cetvrtiSprat.IsChecked == true) 
                spratovi.Add(4);
            if (CheckBox_petiSprat.IsChecked == true) 
                spratovi.Add(5);
            DateTime? pocetak = DatePicker_pocetak.SelectedDate;
            DateTime? kraj = DatePicker_kraj.SelectedDate;

            var rezultati =_radnikSobeService.Pretrazi(tipovi,spratovi, pocetak,kraj);
            DataGrid_sobeRadnik.ItemsSource = rezultati;
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl && tabControl.SelectedItem is TabItem selectedTab)
            {
                if (selectedTab.Header.ToString() == "Sobe")
                {
                    _radnikSobeService.ResetTab(this);
                }
                else if (selectedTab.Header.ToString() == "Gosti")
                {
                    _operacijeNadGostomService.ResetTab(this);
                }
            }
        }
        private void dodajRez(object sender, EventArgs e)
        {

            int gostId = int.Parse(TextBox_idOsobe.Text);
            int brojSobe = int.Parse(TextBox_idSobe.Text);

            if (!DatePicker_pocetakRez.SelectedDate.HasValue || !DatePicker_krajRez.SelectedDate.HasValue)
            {
                MessageBox.Show("Molimo izaberite datume za rezervaciju.");
                return;
            }
            DateTime datumPocetka = DatePicker_pocetakRez.SelectedDate.Value;
            DateTime datumKraja = DatePicker_krajRez.SelectedDate.Value;
            Rezervacije novaRezervacija = new Rezervacije(gostId, brojSobe, datumPocetka, datumKraja);

            bool uspesno = _rezervacijeService.Dodaj(novaRezervacija);

            if (uspesno)
            {
                MessageBox.Show("Rezervacija je uspešno dodata!");
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl && tabControl.SelectedItem is TabItem selectedTab)
            {
                if (selectedTab.Header.ToString() == "Sobe")
                {
                    _radnikSobeService.ResetTab(this);
                }
                else if (selectedTab.Header.ToString() == "Gosti")
                {
                    _operacijeNadGostomService.ResetTab(this);
                }
            }
        }
        private void dodajRez(object sender, EventArgs e)
        {
            
            int gostId  = int.Parse(TextBox_idOsobe.Text);
            int brojSobe = int.Parse(TextBox_idSobe.Text);

            if (!DatePicker_pocetakRez.SelectedDate.HasValue || !DatePicker_krajRez.SelectedDate.HasValue)
            {
                MessageBox.Show("Molimo izaberite datume za rezervaciju.");
                return;
            }
            DateTime datumPocetka = DatePicker_pocetakRez.SelectedDate.Value;
            DateTime datumKraja = DatePicker_krajRez.SelectedDate.Value;
            Rezervacije novaRezervacija = new Rezervacije(gostId, brojSobe, datumPocetka, datumKraja);

            bool uspesno = _rezervacijeService.Dodaj(novaRezervacija);

            if (uspesno)
            {
                MessageBox.Show("Rezervacija je uspešno dodata!");
            }
        }
    }
}
