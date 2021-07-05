using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_biblioteka.Models
{
    public class Knjiga
    {
        public int Idknjiga { get; set; }
        public string Naslov { get; set; }
        public double Ocena { get; set; }
        public int BrOcena { get; set; }
        public Pisac Pisac{ get; set; }

        public Knjiga(int idknjiga, string naslov, double ocena, int brOcena, Pisac pisac)
        {
            Idknjiga = idknjiga;
            Naslov = naslov;
            Ocena = ocena;
            BrOcena = brOcena;
            Pisac = pisac;
        }

        public Knjiga()
        {
        }
    }
}
