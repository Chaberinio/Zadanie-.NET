using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie.NET.Models
{
    //Model klasy Ksiazka
    public class Ksiazka
    {
        //Id książki
        public int Id { get; set; }
        //Tytuł książki
        public string Tytul { get; set; }
        //Autor książki
        public string Autor { get; set; }
        //Rok wydania książki
        public int RokWydania { get; set; }
        //Numer ISBN książki
        public string ISBN { get; set; }
    }
}
