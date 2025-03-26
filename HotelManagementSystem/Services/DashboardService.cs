using HotelManagementSystem.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Services
{
    public class DashboardService
    {
        private string connString = "Data Source=localhost;Initial Catalog=HMS;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public void UpdatePodaci(DashboardModel dashboardModel)
        {
            int brojGostiju = 0;
            int brojSoba = 0;
            int brojZaposlenih = 0;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string queryGostiju = "SELECT COUNT(*) FROM gost";
                using (SqlCommand cmd = new SqlCommand(queryGostiju, conn))
                {
                    brojGostiju = (int)cmd.ExecuteScalar();
                }

                string querySoba = "SELECT COUNT(*) FROM soba";
                using (SqlCommand cmd = new SqlCommand(querySoba, conn))
                {
                    brojSoba = (int)cmd.ExecuteScalar();
                }

                string queryZaposlenih = "SELECT COUNT(*) FROM osoblje";
                using (SqlCommand cmd = new SqlCommand(queryZaposlenih, conn))
                {
                    brojZaposlenih = (int)cmd.ExecuteScalar();
                }
            }

            dashboardModel.BrojGostiju = brojGostiju;
            dashboardModel.BrojSoba = brojSoba;
            dashboardModel.BrojZaposlenih = brojZaposlenih;
        }
    }
}
