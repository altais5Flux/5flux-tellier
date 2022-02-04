using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebservicesSage.Cotnroller;
using WebservicesSage.Utils;

namespace WebservicesSage.Services
{
    class ServiceArticle : ServiceAbstract
    {
        public ServiceArticle()
        {
            setAlive(true);
        }

        public void ToDoOnLaunch()
        {
            try
            {
                if (isAlive())
                {
                    ControllerArticle.LaunchService();
                }
            }
            catch (Exception e)
            {
            }
        }

        public override void ToDoOnFirstCommit()
        {
            if (isAlive())
            {
                Task taskA = new Task(() => ControllerArticle.SendAllArticles());
                taskA.Start();
                //ControllerArticle.SendAllArticles();
            }
        }

        public void SendProducts(string reference=null)
        {
            try
            {
                if (isAlive())
                {
                    if (String.IsNullOrEmpty(reference))
                    {
                        Task taskA = new Task(() => ControllerArticle.SendAllArticles());
                        taskA.Start();
                    }
                    else
                    {
                        SendCustomProduct(reference);
                    }
                }
            }
            catch(Exception e)
            {
                UtilsMail.SendErrorMail(DateTime.Now + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine, "SERVICES ARTICLE : SendProducts");
            }

        }

        public void SendCustomProduct(string reference)
        {
            try
            {
                if (isAlive())
                {
                    Task taskA = new Task(() => ControllerArticle.SendCustomArticles(reference));
                    taskA.Start();
                }
            }
            catch(Exception e)
            {
                UtilsMail.SendErrorMail(DateTime.Now + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine, "SERVICES ARTICLE : SendCustomProduct");
            }
        }

        public void SendPriceProduct(string reference)
        {
            try
            {
                if (isAlive())
                {
                    if (String.IsNullOrEmpty(reference))
                    {
                        Task taskA = new Task(() => ControllerArticle.SendPrice());
                        taskA.Start();
                    }
                    else
                    {
                        Task taskA = new Task(() => ControllerArticle.SendCustomPrice(reference));
                        taskA.Start();
                    }
                }
            }
            catch (Exception e)
            {
                UtilsMail.SendErrorMail(DateTime.Now + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine, "SERVICES ARTICLE : SendPriceProduct");
            }
        }

        public void SendStock(string reference)
        {
            try
            {
                if (isAlive())
                {
                    if (String.IsNullOrEmpty(reference))
                    {
                        Task taskA = new Task(() => ControllerArticle.SendStock());
                        taskA.Start();
                    }
                    else
                    {
                        Task taskA = new Task(() => ControllerArticle.SendCustomStock(reference));
                        taskA.Start();
                    }
                    //ControllerArticle.SendAllArticles();
                }
            }
            catch (Exception e)
            {
                UtilsMail.SendErrorMail(DateTime.Now + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine, "SERVICES ARTICLE : SendStock");
            }

        }
    }
}
