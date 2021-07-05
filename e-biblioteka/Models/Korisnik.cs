using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_biblioteka
{
    public class Korisnik
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public int Status { get; set; }

        public Korisnik(string username, string password, string ime, string prezime, int status)
        {
            Username = username;
            Password = password;
            Ime = ime;
            Prezime = prezime;
            Status = status;
        }

        public Korisnik()
        {

        }
    }
}
