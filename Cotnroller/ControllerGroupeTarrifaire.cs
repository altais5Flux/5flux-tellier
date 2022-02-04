using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebservicesSage.Singleton;
using WebservicesSage.Object;
using Objets100cLib;
using WebservicesSage.Utils;
using WebservicesSage.Utils.Enums;
using System.Windows.Forms;

namespace WebservicesSage.Cotnroller
{
    public static class ControllerGroupeTarrifaire
    {

        /// <summary>
        /// Envoie touts les groupes tarrifaire vers prestashop
        /// </summary>
        public static void SendAllGroupeTarrifaire()
        {
           /* var gescom = SingletonConnection.Instance.Gescom;

            var CategorieTarifssage = gescom.FactoryCategorieTarif.List;

            SingletonUI.Instance.LogBox.Invoke((MethodInvoker)(() => SingletonUI.Instance.LogBox.AppendText("Found " + CategorieTarifssage.Count + " Groupe Tariffaire" + Environment.NewLine)));


            var CategorieTarifs = GetListOfGroupTarrifaireToProcess(CategorieTarifssage);

            int increm = 100 / CategorieTarifs.Count;

            SingletonUI.Instance.LogBox.Invoke((MethodInvoker)(() => SingletonUI.Instance.LogBox.AppendText("Processing " + CategorieTarifs.Count + " Groupe Tariffaire" + Environment.NewLine)));


            foreach (GroupeTarrifaire groupeTarrifaire in CategorieTarifs)
            {

                string groupeTarrifaireXML = UtilsSerialize.SerializeObject<GroupeTarrifaire>(groupeTarrifaire);
				UtilsWebservices.SendData(UtilsConfig.BaseUrl + EnumEndPoint.GroupeTarrifaire.Value, groupeTarrifaireXML);

                SingletonUI.Instance.catProgress.Invoke((MethodInvoker)(() => SingletonUI.Instance.catProgress.Value += increm));

            }*/

        }

        /// <summary>
        /// Check les erreurs possible sur les groupe tarriffaire
        /// </summary>
        /// <param name="groupeTarrifaire"></param>
        /// <returns></returns>
        private static bool HandleGroupeTarrifaireError(GroupeTarrifaire groupeTarrifaire)
        {
            bool error = false;

            if (String.IsNullOrEmpty(groupeTarrifaire.Intitule))
            {
                error = true;
                //SingletonUI.Instance.LogBox.Invoke((MethodInvoker)(() => SingletonUI.Instance.LogBox.AppendText("Client :  " + groupeTarrifaire.Intitule + " No Intitule Found" + Environment.NewLine)));


                // on affiche une erreur + log 
            }

            return error;
        }

        /// <summary>
        /// Tranforme la liste de groupe SAGE en liste de groupe perso
        /// </summary>
        /// <param name="groupeTarrifairesSageObj"></param>
        /// <returns></returns>
        private static List<GroupeTarrifaire> GetListOfGroupTarrifaireToProcess(IBICollection groupeTarrifairesSageObj)
        {
            List<GroupeTarrifaire> groupeTarrifaireToProcess = new List<GroupeTarrifaire>();

            foreach (IBPCategorieTarif groupeTarrifaireSageObj in groupeTarrifairesSageObj)
            {
                GroupeTarrifaire groupeTarrifaire = new GroupeTarrifaire(groupeTarrifaireSageObj);

                if (!HandleGroupeTarrifaireError(groupeTarrifaire))
                {
                    groupeTarrifaireToProcess.Add(groupeTarrifaire);
                }
            }
            return groupeTarrifaireToProcess;
        }
    }
}
