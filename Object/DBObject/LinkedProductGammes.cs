using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebservicesSage.Object.DBObject
{
    class LinkedProductGammes
    {
        public int Id { get; set; }
        public string ArticleReference { get; set; }
        public string Intitule { get; set; }
        public string Intitule2 { get; set; }
        public double Price { get; set; }
        public string Reference { get; set; }
        public string CodeBarre { get; set; }
        public string Value_Intitule { get; set; }
        public string Value_Intitule2 { get; set; }
        public double Stock { get; set; }
        public bool Sommeil { get; set; }
    }
}
