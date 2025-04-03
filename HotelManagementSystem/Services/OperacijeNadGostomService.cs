using HotelManagementSystem.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagementSystem.Services
{
    public class OperacijeNadGostomService:IOperacijeService<Gost>
    {
        private string connString = "Data Source=localhost;Initial Catalog=HMS;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public List<Gost> Pretrazi(Dictionary<string, object> parametri)
        {
            List<Gost> gosti = new List<Gost>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                if (parametri.Count == 0)
                {
                    return PrikaziSveGoste();
                }

                string query = "SELECT * FROM gost WHERE ";
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (parametri.ContainsKey("Ime"))
                {
                    query = query + "Ime=@Ime AND ";
                    parameters.Add(new SqlParameter("@Ime", parametri["Ime"]));

                }
                if (parametri.ContainsKey("Prezime"))
                {
                    query = query + "Prezime=@Prezime AND ";
                    parameters.Add(new SqlParameter("@Prezime", parametri["Prezime"]));
                }
                if (parametri.ContainsKey("Telefon"))
                {
                    query = query + "Telefon=@Telefon AND ";
                    parameters.Add(new SqlParameter("@Telefon", parametri["Telefon"]));
                }
                if (parametri.ContainsKey("Drzavljanstvo"))
                {
                    query = query + "Drzavljanstvo=@Drzavljanstvo AND ";
                    parameters.Add(new SqlParameter("@Drzavljanstvo", parametri["Drzavljanstvo"]));
                }
                if (parametri.ContainsKey("Pol"))
                {
                    query = query + "Pol=@Pol AND ";
                    parameters.Add(new SqlParameter("@Pol", parametri["Pol"]));
                }
                if (parametri.ContainsKey("Pasos"))
                {
                    query = query + "Pasos=@Pasos AND ";
                    parameters.Add(new SqlParameter("@Pasos", parametri["Pasos"]));
                }
                if (parametri.ContainsKey("licna_karta"))
                {
                    query = query + "licna_karta=@licna_karta AND ";
                    parameters.Add(new SqlParameter("@licna_karta", parametri["licna_karta"]));
                }

                query = query.Substring(0, query.Length - 5); //brisemo AND sa kraja

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Gost gost = new Gost(
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetString(reader.GetOrdinal("ime")),
                                reader.GetString(reader.GetOrdinal("prezime")),
                                reader.GetString(reader.GetOrdinal("telefon")),
                                reader.GetString(reader.GetOrdinal("drzavljanstvo")),
                                reader.GetString(reader.GetOrdinal("pol")),
                                reader.IsDBNull(reader.GetOrdinal("pasos")) ? null : reader.GetString(reader.GetOrdinal("pasos")),
                                reader.IsDBNull(reader.GetOrdinal("licna_karta")) ? null : reader.GetString(reader.GetOrdinal("licna_karta"))
                            );
                            gosti.Add(gost);
                        }
                    }
                }
            }
            return gosti;
        }
        public List<Gost> PrikaziSveGoste()
        {
            List<Gost> gosti = new List<Gost>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                string query ="SELECT * FROM gost";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Gost gost = new Gost(
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetString(reader.GetOrdinal("ime")),
                                reader.GetString(reader.GetOrdinal("prezime")),
                                reader.GetString(reader.GetOrdinal("telefon")),
                                reader.GetString(reader.GetOrdinal("drzavljanstvo")),
                                reader.GetString(reader.GetOrdinal("pol")),
                                reader.IsDBNull(reader.GetOrdinal("pasos")) ? null : reader.GetString(reader.GetOrdinal("pasos")),
                                reader.IsDBNull(reader.GetOrdinal("licna_karta")) ? null : reader.GetString(reader.GetOrdinal("licna_karta"))
                            );
                            gosti.Add(gost);
                        }
                    }
                }
            }
            return gosti;
        }
        private bool ProveriIspravnost(Gost gost)
        {
            if (string.IsNullOrEmpty(gost.Ime) || string.IsNullOrEmpty(gost.Prezime) || string.IsNullOrEmpty(gost.Pol) ||
                string.IsNullOrEmpty(gost.Telefon) || string.IsNullOrEmpty(gost.Drzavljanstvo))
            {
                MessageBox.Show("Svi obavezni podaci moraju biti popunjeni!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(gost.Pasos) && string.IsNullOrEmpty(gost.LicnaKarta))
            {
                MessageBox.Show("Mora biti popunjeno barem jedno od polja: Pasos ili Lična karta!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (gost.Pol != "Z" && gost.Pol != "z" && gost.Pol != "M" && gost.Pol != "m")
            {
                MessageBox.Show("Pol mora biti Z/z ili M/m", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        public bool Dodaj(Gost gost)
        {
            if (!ProveriIspravnost(gost))
                return false;
            if (ProveriPostojanjeGosta(gost))
            {
                MessageBox.Show("Gost sa istim pasosom/licnom kartom već postoji!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                string query = "INSERT INTO gost (Ime, Prezime, Telefon, Drzavljanstvo, Pol, Pasos, licna_karta) " +
                            "VALUES (@Ime, @Prezime, @Telefon, @Drzavljanstvo, @Pol, @Pasos, @licna_karta)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Ime", gost.Ime);
                    command.Parameters.AddWithValue("@Prezime", gost.Prezime);
                    command.Parameters.AddWithValue("@Telefon", gost.Telefon);
                    command.Parameters.AddWithValue("@Drzavljanstvo", gost.Drzavljanstvo);
                    command.Parameters.AddWithValue("@Pol", gost.Pol);
                    command.Parameters.AddWithValue("@Pasos", string.IsNullOrEmpty(gost.Pasos) ? (object)DBNull.Value : gost.Pasos);
                    command.Parameters.AddWithValue("@licna_karta", string.IsNullOrEmpty(gost.LicnaKarta) ? (object)DBNull.Value : gost.LicnaKarta);


                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Uspesno dodato");
            return true;
        }
        private bool ProveriPostojanjeGosta(Gost gost) //ne postoje dve osobe sa istim pasosom ili licnom kartom
        {
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                var query = "SELECT COUNT(*) FROM gost WHERE ( (pasos = @Pasos AND pasos IS NOT NULL) OR (licna_karta = @LicnaKarta AND licna_karta IS NOT NULL) )";

                int broj = 0;
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Pasos", (object?)gost.Pasos ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LicnaKarta", (object?)gost.LicnaKarta ?? DBNull.Value);

                    broj = (int)command.ExecuteScalar();
                }
                return broj > 0;
            }
        }

        public bool Obrisi(Gost gost)
        {
            if (string.IsNullOrEmpty(gost.Ime) || string.IsNullOrEmpty(gost.Prezime) ||
                    string.IsNullOrEmpty(gost.Pol) || string.IsNullOrEmpty(gost.Telefon) ||
                    string.IsNullOrEmpty(gost.Drzavljanstvo))
            {
                MessageBox.Show("Svi obavezni podaci moraju biti popunjeni!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!ProveriPostojanjeGosta(gost))
            {
                MessageBox.Show("Gost sa tim podacima ne postoji u bazi.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                string query = "DELETE FROM gost WHERE Ime = @Ime AND Prezime = @Prezime AND Telefon = @Telefon";

                int broj = 0;
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Ime", gost.Ime);
                    command.Parameters.AddWithValue("@Prezime", gost.Prezime);
                    command.Parameters.AddWithValue("@Telefon", gost.Telefon);

                    broj = command.ExecuteNonQuery();
                }
                if (broj > 0)
                {
                    MessageBox.Show("Gost je uspešno obrisan!", "Uspešno", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Došlo je do greške prilikom brisanja gosta.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }
        public bool Izmeni(Gost gost)
        {
            if (!ProveriIspravnost(gost))
                return false;
            if(ProveriPostojanjeZaOstale(gost))
            {
                MessageBox.Show("Gost sa istim pasosom/licnom kartom već postoji!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
                
            
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                string query = "UPDATE gost " +
                   "SET ime = @Ime, prezime = @Prezime, telefon = @Telefon, drzavljanstvo = @Drzavljanstvo, " +
                   "pol = @Pol, pasos = @Pasos, licna_karta = @licna_karta " +
                   "WHERE id = @Id";


                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", gost.Id);
                    command.Parameters.AddWithValue("@Ime", gost.Ime);
                    command.Parameters.AddWithValue("@Prezime", gost.Prezime);
                    command.Parameters.AddWithValue("@Telefon", gost.Telefon);
                    command.Parameters.AddWithValue("@Drzavljanstvo", gost.Drzavljanstvo);
                    command.Parameters.AddWithValue("@Pol", gost.Pol);
                    command.Parameters.AddWithValue("@Pasos", string.IsNullOrEmpty(gost.Pasos) ? (object)DBNull.Value : gost.Pasos);
                    command.Parameters.AddWithValue("@licna_karta", string.IsNullOrEmpty(gost.LicnaKarta) ? (object)DBNull.Value : gost.LicnaKarta);

                    int broj = command.ExecuteNonQuery();
                    if(broj>0)
                    {
                        MessageBox.Show("Uspesno izmenjeno");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Došlo je do greške prilikom izmene gosta.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
        }
        private bool ProveriPostojanjeZaOstale(Gost gost)
        {
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                var query = "SELECT COUNT(*) FROM gost WHERE ( (pasos = @Pasos AND pasos IS NOT NULL) OR (licna_karta = @LicnaKarta AND licna_karta IS NOT NULL) ) AND id != @Id";

                int broj = 0;
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Pasos", (object?)gost.Pasos ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LicnaKarta", (object?)gost.LicnaKarta ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Id", gost.Id);

                    broj = (int)command.ExecuteScalar();
                }
                return broj > 0; 
            }
        }
        public void ResetTab(Meni meni)
        {
            meni.TextBox_ime.Text = "";
            meni.TextBox_prezime.Text = "";
            meni.TextBox_telefon.Text = "";
            meni.TextBox_drzavljanstvo.Text = "";
            meni.TextBox_pol.Text = "";
            meni.TextBox_pasos.Text = "";
            meni.TextBox_licna.Text = "";
            meni.DataGrid_gosti.ItemsSource = null;

        }
    }
}
