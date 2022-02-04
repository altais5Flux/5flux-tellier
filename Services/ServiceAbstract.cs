using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebservicesSage.Services
{
    abstract class ServiceAbstract
    {
        private bool alive = false;

        public bool isAlive()
        {
            return alive;
        }

        public void setAlive(bool isAlive)
        {
            alive = isAlive;
        }

        abstract public void ToDoOnFirstCommit();
    }
}
