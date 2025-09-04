using System;

namespace ERPForAll.Models
{
    /// <summary>
    /// Ein Eintrag in der Mahnliste: fasst Kunde + Bestellung + offenen Betrag zusammen.
    /// </summary>
    public class MahnlisteEintragDto
    {
        public int KundeID { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }

        public int BestellungID { get; set; }
        public DateTime Datum { get; set; }
        public DateTime Faelligkeit { get; set; }
        public string Status { get; set; }

        public decimal Betrag { get; set; }

        public string KundeVollname => $"{Nachname}, {Vorname}";
    }
}
