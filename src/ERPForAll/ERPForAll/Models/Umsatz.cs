using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPForAll.Models
{
    public class Umsatz
    {
        public int PKey_Umsatz { get; set; }
        public int FKey_Bestellung { get; set; }
        public decimal Betrag { get; set; }
        public DateTime Datum { get; set; }
    }
}
