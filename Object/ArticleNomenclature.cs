using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebservicesSage.Object
{
    [Serializable()]
    public class ArticleNomenclature
    {
        public string ArticleRef { get; set; }
        public List<string> NomenclatureRefList { get; set; }

        public ArticleNomenclature()
        {
            NomenclatureRefList = new List<string>();
        }
    }
}
