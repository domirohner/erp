using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPForAll.Models
{
    public class Bestellung
    {
        public int PKey_Bestellung { get; set; }
        public int FKey_Lieferant { get; set; }
        public int FKey_Kunde { get; set; }
        public DateTime Datum { get; set; }
        public DateTime Faelligkeit { get; set; }
        public string Status { get; set; }
    }
}
