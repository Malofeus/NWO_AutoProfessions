using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWO_AutoProfessions
{
    public class NWO_TasksRunning
    {
        public Boolean NWO_HasTask;

        public Int32   NWO_TaskRunTime;

        public NWO_TasksRunning()
        {
            NWO_HasTask = false;
            NWO_TaskRunTime = 0;
        }

        public Boolean NWO_TaskCompleted
        {
            get
            {
                return (NWO_HasTask && NWO_TaskRunTime <= 0);
            }
        }
    }
}
