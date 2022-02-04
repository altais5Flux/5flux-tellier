using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebservicesSage.Utils.Enums
{
    class EnumEndPoint
    {
        private EnumEndPoint(string value) { Value = value; }

        public string Value { get; set; }

        public static EnumEndPoint Client { get { return new EnumEndPoint("Webservices-Customers.php"); } }
        public static EnumEndPoint GroupeTarrifaire { get { return new EnumEndPoint("Webservices-GroupeTarrifaire.php"); } }
        public static EnumEndPoint Commande { get { return new EnumEndPoint("Webservices-Commandes.php"); } }
        public static EnumEndPoint Article { get { return new EnumEndPoint("Webservices-Articles.php"); } }
        public static EnumEndPoint Stock { get { return new EnumEndPoint("Webservices-Stock.php"); } }
        public static EnumEndPoint Price { get { return new EnumEndPoint("Webservices-Price.php"); } }
        public static EnumEndPoint UpdateStatutCommande { get { return new EnumEndPoint("Webservices-UpdateStatutCommande.php"); } }
        public static EnumEndPoint ArticleNomenclature { get { return new EnumEndPoint("Webservices-ArticlesNommenclature.php"); } }
        public static EnumEndPoint Gamme { get { return new EnumEndPoint("Webservices-Gammes.php"); } }
        public static EnumEndPoint Transporteur { get { return new EnumEndPoint("Webservices-Transporteur.php"); } }
        public static EnumEndPoint EditeArticle { get { return new EnumEndPoint("Webservices-EditArticles.php"); } }
        public static EnumEndPoint ArticleDetails { get { return new EnumEndPoint("Webservices-EditeArticle.php"); } }
        public static EnumEndPoint Category { get { return new EnumEndPoint("Webservices-Category.php"); } }

    }
}
