using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models
{
    public class Osoblje
    {
        public int IdOsoblja { get; set; } = 0;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Uloga { get; set; } = "";
    }
}
