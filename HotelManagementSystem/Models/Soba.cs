using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models
{
    public class Soba
    {
        public int BrojSobe { get; set; }
        public int Sprat { get; set; } = 1;
        public string TipSobe { get; set; } = "Jednokrevetna";
        public string StatusRada { get; set; } = "Slobodna";
        public string? Napomena { get; set; } = null;
        public decimal CenaPoNoci { get; set; } = 0m;
        public DateTime? PoslednjiDatumOdrzavanja { get; set; } = null;

        public Soba(int brSobe, int sprat, string tipSobe, string statusRada, string? napomena, decimal cenaPoNoci, DateTime? datumOdrzavanja)
        {
            BrojSobe = brSobe;
            Sprat = sprat;
            TipSobe = tipSobe;
            StatusRada = statusRada;
            Napomena = napomena;
            CenaPoNoci = cenaPoNoci;
            PoslednjiDatumOdrzavanja = datumOdrzavanja;
        }

        public Soba(int sprat, string tipSobe, string statusRada, string? napomena, decimal cenaPoNoci, DateTime? datumOdrzavanja)
        {
            Sprat = sprat;
            TipSobe = tipSobe;
            StatusRada = statusRada;
            Napomena = napomena;
            CenaPoNoci = cenaPoNoci;
            PoslednjiDatumOdrzavanja = datumOdrzavanja;
        }
    }
}
