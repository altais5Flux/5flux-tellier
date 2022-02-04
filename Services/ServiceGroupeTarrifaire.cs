using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebservicesSage.Cotnroller;

namespace WebservicesSage.Services
{
    class ServiceGroupeTarrifaire : ServiceAbstract
    {

        public ServiceGroupeTarrifaire()
        {
            setAlive(true);
        }

        public override void ToDoOnFirstCommit()
        {
            if (isAlive())
            {
                Task taskA = new Task(() => ControllerGroupeTarrifaire.SendAllGroupeTarrifaire());
                taskA.Start();
            }
        }
    }
}
