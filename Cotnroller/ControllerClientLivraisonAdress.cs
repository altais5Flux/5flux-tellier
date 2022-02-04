using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebservicesSage.Object;
using Objets100cLib;
using WebservicesSage.Singleton;
using System.Windows.Forms;

namespace WebservicesSage.Cotnroller
{
    public static class ControllerClientLivraisonAdress
    {

        /// <summary>
        /// Récupère une liste d'adresse 
        /// </summary>
        /// <param name="adressListFromSage">list d'adresse SAGE</param>
        /// <returns></returns>
        public static List<ClientLivraisonAdress> getAllClientLivraisonAdressToProcess(IBICollection adressListFromSage)
        {
            List<ClientLivraisonAdress> adressToProcess = new List<ClientLivraisonAdress>();

            foreach (IBOClientLivraison3 clientLivraison in adressListFromSage)
            {
                ClientLivraisonAdress addr = new ClientLivraisonAdress(clientLivraison);
                if (!handleAdressError(addr))
                {
                    adressToProcess.Add(addr);
                }
            }

            return adressToProcess;
        }

        private static bool handleAdressError(ClientLivraisonAdress adress)
        {
            bool error = false;

            if (String.IsNullOrEmpty(adress.Contact)){

               // SingletonUI.Instance.LogBox.Invoke((MethodInvoker)(() => SingletonUI.Instance.LogBox.AppendText("Adress :  " + adress.Intitule + " No contact Found" + Environment.NewLine)));
                error = true;
            }

            return error;
        }
    }
}
