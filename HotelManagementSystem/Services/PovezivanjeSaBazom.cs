using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace HotelManagementSystem.Services
{
    class PovezivanjeSaBazom
    {
        string connString = "Data Source=DESKTOP-RP1BINM\\SQLEXPRESS;Initial Catalog=HMS;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        string query = "SELECT * FROM [osoblje] WHERE username = @username AND sifra = @sifra";
        private MainWindow _mainWindow;
        public PovezivanjeSaBazom(MainWindow mainWindow)
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
                        if (reader.HasRows)
                        {
                            _mainWindow.Hide();
                            Meni meniProzor = new Meni();
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
