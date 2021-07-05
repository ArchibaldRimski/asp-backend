using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_biblioteka.Models
{
    public class Pisac
    {
        

        public int IdPisac { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }

        public Pisac()
        {
        }

        public Pisac(int idPisac, string ime, string prezime)
        {
            IdPisac = idPisac;
            Ime = ime;
            Prezime = prezime;
        }

    }
}
