using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objets100cLib;


namespace WebservicesSage.Object
{
    [Serializable()]
    public class ClientLivraisonAdress
    {
        public string Intitule { get; set; }
        public string Adresse { get; set; }
        public string Complement { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public string Region { get; set; }
        public string Pays { get; set; }
        public string Contact { get; set; }
        public string Telephone { get; set; }
        public string Portable { get; set; }
        public bool DefaultAddress { get; set; }

        public ClientLivraisonAdress(IBOClientLivraison3 clientLivraison3)
        {
            Intitule = clientLivraison3.LI_Intitule;
            Adresse = clientLivraison3.Adresse.Adresse;
            Complement = clientLivraison3.Adresse.Complement;
            CodePostal = clientLivraison3.Adresse.CodePostal;
            Ville = clientLivraison3.Adresse.Ville;
            Region = clientLivraison3.Adresse.CodeRegion;
            Pays = clientLivraison3.Adresse.Pays;
            Contact = clientLivraison3.LI_Contact;
            Telephone = clientLivraison3.Telecom.Telephone;
            Portable = clientLivraison3.Telecom.Portable;
            
            //DefaultAddress = clientLivraison3.Adresse.
        }

        public ClientLivraisonAdress()
        {

        }
    }
}
