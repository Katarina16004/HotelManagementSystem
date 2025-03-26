using HotelManagementSystem.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelManagementSystem.Services
{
    public class OperacijeNadGostomService:IOperacijeService<Gost>
    {
        private string connString = "Data Source=localhost;Initial Catalog=HMS;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public IEnumerable<Gost> Pretrazi(Dictionary<string, object> parametri)
        {
            List<Gost> gosti = new List<Gost>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                var queryBuilder = new StringBuilder("SELECT * FROM gost WHERE ");
                var parameters = new List<SqlParameter>();
                foreach (var param in parametri) //oni textBoxovi koji sadrze nesto
                {
                    queryBuilder.Append($"{param.Key} = @{param.Key} AND ");
                    parameters.Add(new SqlParameter($"@{param.Key}", param.Value));
                }
                if (parametri.Count == 0)
                {
                    return PrikaziSveGoste();
                }

                // and na kraju
                queryBuilder.Length=queryBuilder.Length - 4;

                var command = new SqlCommand(queryBuilder.ToString(), connection);
                command.Parameters.AddRange(parameters.ToArray());

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        gosti.Add(new Gost(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.IsDBNull(6) ? "" : reader.GetString(6),
                            reader.IsDBNull(7) ? "" : reader.GetString(7)
                        ));
                    }
                }
            }
            return gosti;
        }
        public IEnumerable<Gost> PrikaziSveGoste()
        {
            List<Gost> gosti = new List<Gost>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                var queryBuilder = new StringBuilder("SELECT * FROM gost");

                var command = new SqlCommand(queryBuilder.ToString(), connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        gosti.Add(new Gost(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.IsDBNull(6) ? "" : reader.GetString(6),
                            reader.IsDBNull(7) ? "" : reader.GetString(7)
                        ));
                    }
                }
            }
            return gosti;
        }
        public bool Dodaj(Gost gost)
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
            if (ProveriPostojanjeGosta(gost))
            {
                MessageBox.Show("Gost sa istim podacima već postoji!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                var query = "INSERT INTO gost (Ime, Prezime, Pol, Telefon, Drzavljanstvo, Pasos, licna_karta) " +
                            "VALUES (@Ime, @Prezime, @Pol, @Telefon, @Drzavljanstvo, @Pasos, @licna_karta)";

                var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Ime", gost.Ime);
                command.Parameters.AddWithValue("@Prezime", gost.Prezime);
                command.Parameters.AddWithValue("@Pol", gost.Pol);
                command.Parameters.AddWithValue("@Telefon", gost.Telefon);
                command.Parameters.AddWithValue("@Drzavljanstvo", gost.Drzavljanstvo);
                command.Parameters.AddWithValue("@Pasos", string.IsNullOrEmpty(gost.Pasos) ? (object)DBNull.Value : gost.Pasos);
                command.Parameters.AddWithValue("@licna_karta", string.IsNullOrEmpty(gost.LicnaKarta) ? (object)DBNull.Value : gost.LicnaKarta);

                command.ExecuteNonQuery();
            }
            MessageBox.Show("Uspesno dodato");
            return true;
        }
        private bool ProveriPostojanjeGosta(Gost gost)
        {
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                var query = "SELECT COUNT(*) FROM gost WHERE Ime = @Ime AND Prezime = @Prezime AND Telefon = @Telefon AND Pol=@Pol";
                var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Ime", gost.Ime);
                command.Parameters.AddWithValue("@Prezime", gost.Prezime);
                command.Parameters.AddWithValue("@Telefon", gost.Telefon);
                command.Parameters.AddWithValue("@Pol", gost.Pol);

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

    }
}
