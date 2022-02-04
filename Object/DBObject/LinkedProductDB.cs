using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objets100cLib;

namespace WebservicesSage.Object.DBObject
{
    public class LinkedProductDB
    {
        public int Id { get; set; }
        public string Designation { get; set; }
        public string Reference { get; set; }
        public string Resume { get; set; }
        public double PrixAchat { get; set; }
        public double PrixVente { get; set; }
        public string Langue1 { get; set; }
        public string Langue2 { get; set; }
        public string CodeBarres { get; set; }
        public double Poid { get; set; }
        public bool Sommeil { get; set; }
        public bool IsPriceTTC { get; set; }
        public bool isGamme { get; set; }
        public bool IsDoubleGamme { get; set; }
        public double Stock { get; set; }
        public bool HaveNomenclature { get; set; }
        public string Longueur { get; set; }
        public string Largeur { get; set; }
        public string Taille { get; set; }

    }
}
