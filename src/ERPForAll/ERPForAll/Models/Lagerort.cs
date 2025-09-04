using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPForAll.Models
{
    public class Lagerort
    {
        public int PKey_Lagerort { get; set; }
        public string Name { get; set; }
        public string Adresse { get; set; }
        public string Ort { get; set; }
        public int PLZ { get; set; }
    }
}
