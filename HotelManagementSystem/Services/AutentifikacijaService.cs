using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HotelManagementSystem.Models;
using Microsoft.Data.SqlClient;

namespace HotelManagementSystem.Services
{
    public class AutentifikacijaService
    {
        string connString = "Data Source=localhost;Initial Catalog=HMS;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        string query = "SELECT * FROM [osoblje] WHERE username = @username AND sifra = @sifra";
        private MainWindow _mainWindow;
        private string uloga { get; set; } = "";
        public AutentifikacijaService(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }
        public void Prijava ()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open ();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", _mainWindow.UsernameTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@sifra", _mainWindow.PasswordBox.Password.Trim());
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Osoblje korisnik = new Osoblje
                            {
                                IdOsoblja = reader.GetInt32(reader.GetOrdinal("id")),
                                Username = reader.GetString(reader.GetOrdinal("username")),
                                Password = reader.GetString(reader.GetOrdinal("sifra")),
                                Uloga = reader.GetString(reader.GetOrdinal("uloga"))
                            };

                            _mainWindow.Hide();
                            Meni meniProzor = new Meni(korisnik);
                            meniProzor.Show();
                        }
                        else
                        {
                            MessageBox.Show("Pogresan username ili lozinka");
                        }
                    }
                }
            }
        }

    }
}
