using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebservicesSage.Object
{
    [Serializable()]
    public class RemiseFamille
    {
        public string CtNum { get; set; }
        public double RemisePercentage { get; set; }
        public string CategorieTarifaire { get; set; }
        public double FixedPrice { get; set; }
        public RemiseFamille()
        {
        }
    }
}
