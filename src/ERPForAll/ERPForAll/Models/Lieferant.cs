using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPForAll.Models
{
    public class Lieferant
    {
        public int PKey_Lieferant { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string Email { get; set; }
        public string Adresse { get; set; }
        public string Ort { get; set; }
        public int? PLZ { get; set; }

        public string VollerName =>
            string.IsNullOrWhiteSpace(Vorname)
                ? Nachname
                : $"{Nachname}, {Vorname}";
    }
}
