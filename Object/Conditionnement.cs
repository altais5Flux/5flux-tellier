using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebservicesSage.Object
{
    public class Conditionnement
    {
        public string Enumere { get; set; }
        public string Reference { get; set; }
        public string Quantity { get; set; }
        public Conditionnement()
        {

        }
        public Conditionnement(string enumere, string reference, string quantity)
        {
            this.Enumere = enumere;
            this.Reference = reference;
            this.Quantity = quantity;
        }
    }
}
