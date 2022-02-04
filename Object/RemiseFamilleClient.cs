using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebservicesSage.Object
{
    public class RemiseFamilleClient
    {
        public string codeFamille { get; set; }
        public double remise { get; set; }
        public RemiseFamilleClient(string codeFamille,double remise)
        {
            this.codeFamille = codeFamille;
            this.remise = remise;
        }
    }
}
