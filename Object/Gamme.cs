using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objets100cLib;

namespace WebservicesSage.Object
{
    [Serializable()]
    public class Gamme
    {
        public string Intitule { get; set; }
        public string Intitule2 { get; set; }
        public List<string> Values { get; set; }
        public double Price { get; set; }
        public string Reference { get; set; }
        public string CodeBarre { get; set; }
        public string Value_Intitule { get; set; }
        public string Value_Intitule2 { get; set; }
        public double Stock { get; set; }
        public bool Sommeil { get; set; }


        public Gamme(IBPGamme GammeSage)
        {
            Values = new List<string>();
            Intitule = GammeSage.G_Intitule;
            foreach(string value in GammeSage.Enums)
            {
                Values.Add(value);
            }
        }

        public Gamme(string gamme1_Intitule, string value_Intitule)
        {

            // constructeur game simple
            Intitule = gamme1_Intitule;
            Value_Intitule = value_Intitule;
        }

        public Gamme(string gamme1_Intitule, string value_Intitule, string gamme2_Intitule, string value2_Intitule)
        {
            // constructeur gamme double 
            Intitule = gamme1_Intitule;
            Value_Intitule = value_Intitule;
            Intitule2 = gamme2_Intitule;
            Value_Intitule2 = value2_Intitule;
        }

        public Gamme()
        {

        }
    }
}
