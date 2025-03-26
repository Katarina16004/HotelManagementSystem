using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models
{
    public class Gost
    {
        public int Id { get; set; }
        public string Ime { get; set; } = "";
        public string Prezime { get; set; } = "";
        public string Pol { get; set; } = "";
        public string Telefon { get; set; } = "";
        public string Drzavljanstvo { get; set; } = "";
        public string? Pasos { get; set; } = null; 
        public string? LicnaKarta { get; set; } = null; 
        public Gost(int id, string ime, string prezime, string telefon, string drzavljanstvo, string pol, string? pasos = null, string? licnaKarta = null)
        {
            Id = id;
            Ime = ime;
            Prezime = prezime;
            Telefon = telefon;
            Drzavljanstvo = drzavljanstvo;
            Pol = pol;
            Pasos = pasos;
            LicnaKarta = licnaKarta;
        }

        public Gost(string ime, string prezime, string telefon, string drzavljanstvo, string pol, string? pasos = null, string? licnaKarta = null)
        {
            Ime = ime;
            Prezime = prezime;
            Telefon = telefon;
            Drzavljanstvo = drzavljanstvo;
            Pol = pol;
            Pasos = pasos;
            LicnaKarta = licnaKarta;
        }
    }
}
