using System;

namespace ERPForAll.Models
{
    public class Kunde
    {
        public int PKey_Kunde { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public DateTime? Geburtsdatum { get; set; }
        public string Email { get; set; }
        public string Adresse { get; set; }
        public string Ort { get; set; }
        public int? PLZ { get; set; }

        public string VollerName => $"{Nachname}, {Vorname}";
    }
}
