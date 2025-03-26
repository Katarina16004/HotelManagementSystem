using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Services
{
    public interface IOperacijeService<T>
    {
        bool Dodaj(T item);
        bool Obrisi(int id);
        //bool Izmeni(T item);
        IEnumerable<T> Pretrazi(Dictionary<string, object> parametri);
    }
}
