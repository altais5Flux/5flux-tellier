using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objets100cLib;
using WebservicesSage.Singleton;
using WebservicesSage.Object;
using WebservicesSage.Utils;
using WebservicesSage.Utils.Enums;
using System.Windows.Forms;

namespace WebservicesSage.Cotnroller
{
    class ControllerGammes
    {

        /// <summary>
        /// Envoie toute les gammes SAGE à prestashop
        /// </summary>
        public static void SendAllGammes()
        {
           /* var gescom = SingletonConnection.Instance.Gescom;
            var gammesSAGE = gescom.FactoryGamme.List;

            int increm = 1;
            foreach (Gamme gamme in GetListOfGammesToProcess(gammesSAGE))
            {
                string gammeXML = UtilsSerialize.SerializeObject<Gamme>(gamme);

                SingletonUI.Instance.ArticleNumber.Invoke((MethodInvoker)(() => SingletonUI.Instance.ArticleNumber.Text = "Sending data : " + increm));

                increm++;
                Console.WriteLine(UtilsWebservices.SendData(UtilsConfig.BaseUrl + EnumEndPoint.Gamme.Value, gammeXML));
            }*/
        }

        /// <summary>
        /// Tranforme une liste de gamme SAGE en gamme perso
        /// </summary>
        /// <param name="gammesSageObj"></param>
        /// <returns></returns>
        private static List<Gamme> GetListOfGammesToProcess(IBICollection gammesSageObj)
        {
            List<Gamme> gammeToProcess = new List<Gamme>();

            foreach (IBPGamme gammeSAGE in gammesSageObj)
            {
                if (gammeSAGE.G_Type == GammeType.GammeTypeDivers && !String.IsNullOrEmpty(gammeSAGE.G_Intitule))
                {
                    Gamme gamme = new Gamme(gammeSAGE);
                    gammeToProcess.Add(gamme);
                }
            }
            return gammeToProcess;
        }
    }
}
