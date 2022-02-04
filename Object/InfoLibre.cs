using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebservicesSage.Object
{
    [Serializable()]
    public class InfoLibre
    {
        public string Libelle { get; set; }
        public string Value { get; set; }

        public string Id_feature { get; set; }

        public InfoLibre(string libelle, string value, string id_feature = "0")
        {
            Libelle = libelle;
            Value = value;
            Id_feature = id_feature;
        }
        public InfoLibre()
        {

        }
    }
}
