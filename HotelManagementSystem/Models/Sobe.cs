using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models
{
    public class Sobe
    {
        public int BrSobe { get; set; }
        public int Sprat { get; set; } = 1;
        public string TipSobe { get; set; } = "Jednokrevetna";
        public string StatusRada { get; set; } = "Slobodna";
        public string? Napomena { get; set; } = null;
        public decimal CenaPoNoci { get; set; } = 0m;
        public DateTime? DatumOdrzavanja { get; set; } = null;
    }
}
