using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebservicesSage.Cotnroller;
using WebservicesSage.Utils;

namespace WebservicesSage.Services
{
    class ServiceClient : ServiceAbstract
    {
        public ServiceClient()
        {
            setAlive(true);
        }
        public void ToDoOnLaunch()
        {
            try
            {
                if (isAlive())
                {
                    ControllerClient.LaunchService();
                }
            }
            catch (Exception e)
            {
            }
        }
        public override void ToDoOnFirstCommit()
        {
            try
            {
                if (isAlive())
                {
                    Task taskA = new Task(() => ControllerClient.SendAllClients());
                    taskA.Start();
                }
            }
            catch(Exception e)
            {
                UtilsMail.SendErrorMail(DateTime.Now + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine, "SERVICES CLIENT : ToDoOnFirstCommit");
            }
        }

        public void SendClient(string ct_num)
        {
            try
            {
                if (isAlive())
                {
                    Task taskA = new Task(() => ControllerClient.SendClient(ct_num));
                    taskA.Start();
                }
            }
            catch (Exception e)
            {
                UtilsMail.SendErrorMail(DateTime.Now + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine, "SERVICES CLIENT : ToDoOnFirstCommit");
            }
        }
    }
}
