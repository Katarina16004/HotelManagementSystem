using HotelManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Windows;


namespace HotelManagementSystem.Services
{
    internal class RezervacijeService
    {
        private string connString = "Data Source=localhost;Initial Catalog=HMS;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public bool Dodaj(Rezervacije rez)
        {
            bool uspesno = false;
            string proveraPreklapanja = @"
                SELECT COUNT(*) FROM rezervacija
                WHERE broj_sobe = @BrojSobe 
                AND (
                    (@DatumPocetkaRez BETWEEN datum_pocetka_rez AND datum_kraja_rez) 
                    OR 
                    (@DatumKrajaRez BETWEEN datum_pocetka_rez AND datum_kraja_rez)
                    OR
                    (datum_pocetka_rez BETWEEN @DatumPocetkaRez AND @DatumKrajaRez)
                )";

            string unos = @"
                INSERT INTO rezervacija (gost_id, broj_sobe, datum_pocetka_rez, datum_kraja_rez, ukupna_cena)
                VALUES (@GostId, @BrojSobe, @DatumPocetkaRez, @DatumKrajaRez, @UkupnaCena)";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand proveraCmd = new SqlCommand(proveraPreklapanja, conn))
                    {
                        proveraCmd.Parameters.AddWithValue("@BrojSobe", rez.broj_sobe);
                        proveraCmd.Parameters.AddWithValue("@DatumPocetkaRez", rez.datum_pocetka_rez);
                        proveraCmd.Parameters.AddWithValue("@DatumKrajaRez", rez.datum_kraja_rez);

                        int brojPreklapanja = (int)proveraCmd.ExecuteScalar();

                        if (brojPreklapanja > 0)
                        {
                            MessageBox.Show("Soba je već rezervisana u izabranom terminu, proverite termine za izabranu sobu!");
                            return false;
                        }
                    }

                    using (SqlCommand unosCmd = new SqlCommand(unos, conn))
                    {
                        unosCmd.Parameters.AddWithValue("@GostId", rez.gost_id);
                        unosCmd.Parameters.AddWithValue("@BrojSobe", rez.broj_sobe);
                        unosCmd.Parameters.AddWithValue("@DatumPocetkaRez", rez.datum_pocetka_rez);
                        unosCmd.Parameters.AddWithValue("@DatumKrajaRez", rez.datum_kraja_rez);
                        rez.ukupna_cena = 100;
                        unosCmd.Parameters.AddWithValue("@UkupnaCena", rez.ukupna_cena);

                        int rowsAffected = unosCmd.ExecuteNonQuery();
                        uspesno = rowsAffected > 0;

                        if (uspesno)
                        {
                            MessageBox.Show("Rezervacija je uspešno dodata!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Molimo vas da unesete sva polja!");
                }
            }
            return uspesno;
        }
    }
}
