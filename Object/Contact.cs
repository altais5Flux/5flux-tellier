using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebservicesSage.Object
{
    [Serializable()]
    public class Contact
    {
        public string Email { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephone { get; set; }
        public string Portable { get; set; }

        public Contact()
        {

        }
    }
}
