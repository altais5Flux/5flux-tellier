using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebservicesSage.Cotnroller;

namespace WebservicesSage.Services
{
    class ServicesGammes : ServiceAbstract
    {
        public ServicesGammes()
        {
            setAlive(true);
        }

        public override void ToDoOnFirstCommit()
        {
            if (isAlive())
            {
                //ControllerGammes.SendAllGammes();
                Task taskA = new Task(() => ControllerGammes.SendAllGammes());
                taskA.Start();
            }
        }
    }
}
