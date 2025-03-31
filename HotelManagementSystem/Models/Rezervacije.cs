using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models
{
    internal class Rezervacije
    {
        public int id { get; set; }
        public int gost_id { get; set; }
        public int broj_sobe { get; set; }
        public DateTime? datum_pocetka_rez { get; set; } = null;
        public DateTime? datum_kraja_rez { get; set; } = null;
        public decimal ukupna_cena { get; set; } = 0m;

        public Rezervacije(int gostId, int brojSobe, DateTime datumPocetkaRez, DateTime datumKrajaRez)
        {
            gost_id = gostId;
            broj_sobe = brojSobe;
            datum_pocetka_rez = datumPocetkaRez;
            datum_kraja_rez = datumKrajaRez;
        }

    }
}
