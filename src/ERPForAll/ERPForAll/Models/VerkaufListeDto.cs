using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPForAll.Models
{
    public class VerkaufListeDto
    {
        public int BestellungID { get; set; }
        public string Kunde { get; set; }      // "Nachname, Vorname"
        public System.DateTime Datum { get; set; }
        public System.DateTime Faelligkeit { get; set; }
        public string Status { get; set; }
        public decimal Betrag { get; set; }    // Summe aus Menge * Preis pro Stück
    }
}
