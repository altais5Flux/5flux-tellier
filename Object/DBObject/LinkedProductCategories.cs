using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebservicesSage.Object.DBObject
{
    class LinkedProductCategories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category_id { get; set; }
        public string Parent_Category_id { get; set; }
        public string ArticleReference { get; set; }
        public string Is_root_category { get; set; }
    }
}
