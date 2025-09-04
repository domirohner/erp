using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPForAll.Models
{
    public class UmsatzProKundeDto
    {
        public int KundeID { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public decimal Umsatz { get; set; }

        // Praktisch für DataGridView oder Dropdown-Anzeige
        public string VollerName => string.IsNullOrWhiteSpace(Vorname)
            ? Nachname
            : $"{Nachname}, {Vorname}";
    }
}
