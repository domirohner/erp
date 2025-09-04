using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPForAll.Models
{
    public class EinkaufListeDto
    {
        public int BestellungID { get; set; }
        public string Lieferant { get; set; }   // "Nachname, Vorname" des Kreditors
        public System.DateTime Datum { get; set; }
        public System.DateTime Faelligkeit { get; set; }
        public string Status { get; set; }
        public decimal Betrag { get; set; }     // Summe Menge * Preis pro Stück
    }
}
