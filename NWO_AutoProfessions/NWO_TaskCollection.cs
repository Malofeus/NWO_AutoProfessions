using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

namespace NWO_AutoProfessions
{
    public class NWO_TaskCollection
    {
        private ArrayList           _Tasks;

        private Boolean             _RepeatTaskCycle;

        public NWO_TaskCollection()
        {
            _Tasks                  = new ArrayList();

            _RepeatTaskCycle        = false;
        }

        public ArrayList Tasks
        {
            get
            {
                return _Tasks;
            }
            set
            {
                _Tasks = value;
            }
        }

        public Boolean RepeatTaskCycle
        {
            get
            {
                return _RepeatTaskCycle;
            }
            set
            {
                _RepeatTaskCycle = value;
            }
        }

        public Boolean AllDone
        {
            get
            {
                foreach (NWO_Task task in _Tasks)
                {
                    if (task.AmntToDo != 0 && !task.Done)
                        return false;
                }

                return true;
            }
        }

        public Boolean Infinite
        {
            get
            {
                foreach (NWO_Task task in _Tasks)
                {
                    if (task.AmntToDo == 0)
                        return true;
                }

                return false;
            }

        }

        public void ResetTasks()
        {
            foreach (NWO_Task task in _Tasks)
            {
                task.CurAmntDone = 0;
            }
        }

        public void WriteToConfig(StreamWriter sw, Version configVer)
        {
            sw.WriteLine(_RepeatTaskCycle.ToString());
            sw.WriteLine(_Tasks.Count.ToString());

            foreach (NWO_Task task in _Tasks)
            {
                task.WriteToConfig(sw, configVer);
            }
        }

        public void ReadFromConfig(StreamReader sr, Version configVer)
        {
            _RepeatTaskCycle = Convert.ToBoolean(sr.ReadLine());
            int count = Convert.ToInt32(sr.ReadLine());

            for (int i = 0; i < count; i++)
            {
                NWO_Task task = new NWO_Task();
                task.ReadFromConfig(sr, configVer);
                _Tasks.Add(task);
            }
        }
    }
}
