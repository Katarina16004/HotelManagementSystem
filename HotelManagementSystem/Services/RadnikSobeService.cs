using HotelManagementSystem.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Services
{
    public class RadnikSobeService
    {
        private string connString = "Data Source=localhost;Initial Catalog=HMS;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public List<Soba> Pretrazi(List<string> tip, List<string> sprat, DateTime? pocetak, DateTime? kraj )
        {
            List<Soba> sobe=new List<Soba>();
            if(tip.Count==0 && sprat.Count==0 && pocetak==null && kraj==null)
                return PrikaziSveSobe();
            return sobe;
        }
        public List<Soba> PrikaziSveSobe()
        {
            List<Soba> sobe = new List<Soba>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                string query = "SELECT * FROM soba";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Soba soba = new Soba(
                                reader.GetInt32(reader.GetOrdinal("broj_sobe")),
                                reader.GetInt32(reader.GetOrdinal("sprat")),
                                reader.GetString(reader.GetOrdinal("tip_sobe")),
                                reader.GetString(reader.GetOrdinal("status_rada")),
                                reader.IsDBNull(reader.GetOrdinal("napomena")) ? null : reader.GetString(reader.GetOrdinal("napomena")),
                                reader.GetDecimal(reader.GetOrdinal("cena_po_noci")),
                                reader.IsDBNull(reader.GetOrdinal("poslednji_datum_odrzavanja")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("poslednji_datum_odrzavanja"))

                            );

                            sobe.Add(soba);
                        }
                    }
                }
            }
            return sobe;
        }
    }
}
