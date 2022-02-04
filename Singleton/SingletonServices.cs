using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objets100cLib;
using WebservicesSage.Services;


namespace WebservicesSage.Singleton
{
    public sealed class SingletonServices
    {
        private static readonly Lazy<SingletonServices> lazy =
            new Lazy<SingletonServices>(() => new SingletonServices());

        public static SingletonServices Instance { get { return lazy.Value; } }

        internal ServiceClient ServiceClient { get; set; }
        internal ServiceGroupeTarrifaire ServiceGroupeTarifaire { get; set; }
        internal ServiceCommande ServiceCommande { get; set; }
        internal ServiceArticle ServiceArticle { get; set; }
        internal ServicesGammes ServiceGammes { get; set; }

        private SingletonServices()
        {
            
        }
    }
}
