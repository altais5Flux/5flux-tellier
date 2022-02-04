using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebservicesSage.Object.DBObject
{
    class LinkedProductPrixRemisesClient
    {
        public int Id { get; set; }
        public string ArticleReference { get; set; }
        public double Price { get; set; }
        public double Born_Sup { get; set; }
        public String ClientCtNum { get; set; }
        public string reduction_type { get; set; }
        public double RemisePercentage { get; set; }
    }
}
