using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPForAll.Models
{
    public class BestellungArtikel
    {
        public int PKey_Bestellung_Artikel { get; set; }
        public int FKey_Bestellung { get; set; }
        public int FKey_Artikel { get; set; }
        public int Menge { get; set; }
        public decimal PreisProStueck { get; set; }
    }
}
