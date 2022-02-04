using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebservicesSage.Object
{
    public class PrixClientTarif
    {
        public double Price { get; set; }
        public string ClinetCtNum { get; set; }
        public double FixedRemisePercentage { get; set; }
        public PrixClientTarif()
        {
        }
        
    }
}
