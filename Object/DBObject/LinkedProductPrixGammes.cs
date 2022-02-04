using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebservicesSage.Object.DBObject
{
    class LinkedProductPrixGammes
    {
        public int Id { get; set; }
        public string ArticleReference { get; set; }
        public double Price { get; set; }
        public string Gamme1_Intitule { get; set; }
        public string Gamme2_Intitule { get; set; }
        public string Gamme1_Value { get; set; }
        public string Gamme2_Value { get; set; }
        public string Categori_Intitule { get; set; }
    }
}
