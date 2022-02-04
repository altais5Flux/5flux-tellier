using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebservicesSage.Object
{
    public class SpecialPriceClient
    {
        public string groupeName { get; set; }
        public double price { get; set; }
        public string articleReference { get; set; }
        public double remiseClient{get; set;}
        public double remiseFamille { get; set; }
        public SpecialPriceClient(string groupeName,double price, string articleReference,double remiseClient, double remiseFamille)
        {
            this.groupeName = groupeName;
            this.price = price;
            this.articleReference = articleReference;
            this.remiseClient = remiseClient;
            this.remiseFamille = remiseFamille;

        }
        public SpecialPriceClient()
        { }
    }
}
