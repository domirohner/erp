using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPForAll.Models
{
    public class Artikel
    {
        public int PKey_Artikel { get; set; }
        public string Name { get; set; }
        public string Beschreibung { get; set; }
        public string Kategorie { get; set; }
        public int Menge { get; set; }
    }
}
