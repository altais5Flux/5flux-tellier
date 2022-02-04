using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebservicesSage.Cotnroller;
using WebservicesSage.Utils;
using WebservicesSage.Utils.Enums;
using Objets100cLib;

namespace WebservicesSage.Services
{
    class ServiceCommande : ServiceAbstract
    {

        public ServiceCommande()
        {
            setAlive(true);
        }

        public void ToDoOnLaunch()
        {
            try
            {
                if (isAlive())
                {
                    // check if configuration is here
                    AddStatusConfiguration();
                    //Task taskA = new Task(() => ControllerCommande.LaunchService());
                    //taskA.Start();
                    ControllerCommande.LaunchService();
                }
            }
            catch(Exception e)
            {
                UtilsMail.SendErrorMail(DateTime.Now + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine, "SERVICES COMMANDE : ToDoOnFirstCommit");
            }
        }

        public override void ToDoOnFirstCommit()
        {
            if (isAlive())
            {
                
            }
        }

        private void checkForConfiguration()
        {

        }

        private void AddStatusConfiguration()
        {
            try
            {
                //string response = UtilsWebservices.SendData(UtilsConfig.BaseUrl + EnumEndPoint.Commande.Value, "getStatus");
                int i = 0;
                //UtilsConfig.AddNodeInCustomSection("OrderSetting/OrderMapping", "2", DocumentType.DocumentTypeVenteCommande.ToString());
            }
            catch (Exception e)
            {

            }
        }
    }
}
