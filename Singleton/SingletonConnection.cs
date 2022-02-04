using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Objets100cLib;


namespace WebservicesSage.Singleton
{
    public sealed class SingletonConnection
    {
        private static readonly Lazy<SingletonConnection> lazy =
            new Lazy<SingletonConnection>(() => new SingletonConnection());

        public static SingletonConnection Instance { get { return lazy.Value; } }

        public BSCIALApplication100cClass Gescom { get; set; }
        public BSCPTAApplication100cClass Compta { get; set; }
        public Object.DB dB { get; set; }
        private SingletonConnection()
        {
            try
            {
                if (ConfigurationManager.AppSettings["MAE_SET"].Equals("TRUE"))
                {
                    Compta = new BSCPTAApplication100cClass
                    {
                        Name = ConfigurationManager.AppSettings["MAE_PATH"]
                    };
                    Compta.Loggable.UserName = ConfigurationManager.AppSettings["MAE_USER"];
                    Compta.Loggable.UserPwd = ConfigurationManager.AppSettings["MAE_PASS"];
                }

                if (ConfigurationManager.AppSettings["GCM_SET"].Equals("TRUE"))
                {
                    Gescom = new BSCIALApplication100cClass
                    {
                        Name = ConfigurationManager.AppSettings["GCM_PATH"]
                    };
                    Gescom.Loggable.UserName = ConfigurationManager.AppSettings["GCM_USER"];
                    Gescom.Loggable.UserPwd = ConfigurationManager.AppSettings["GCM_PASS"];
                    if (ConfigurationManager.AppSettings["MAE_SET"].Equals("TRUE"))
                    {
                        Gescom.CptaApplication = Compta;
                    }
                }
                dB = new Object.DB();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);
            }

        }
    }
}
