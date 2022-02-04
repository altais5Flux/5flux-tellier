using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebservicesSage.Object
{
    [Serializable()]
    public class PrixRemiseClient
    {
        public double Price { get; set; }
        public double Born_Sup { get; set; }
        public string ClientCtNum { get; set; }
        public string reduction_type { get; set; }
        public double RemisePercentage { get; set; }
        public double FixedPrice { get; set; }

        public PrixRemiseClient()
        {

        }
    }
}