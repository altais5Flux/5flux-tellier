using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objets100cLib;

namespace WebservicesSage.Object
{
    [Serializable()]
    public class GroupeTarrifaire
    {
        public string Intitule { get; set; }
        public bool TaxInclude { get; set; }

        public GroupeTarrifaire(IBPCategorieTarif categorieTarif)
        {
            Intitule = categorieTarif.CT_Intitule;
            TaxInclude = categorieTarif.CT_PrixTTC;
        }
        public GroupeTarrifaire()
        {

        }
    }
}
