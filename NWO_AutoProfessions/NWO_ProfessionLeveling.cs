using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

namespace NWO_AutoProfessions
{
    public class NWO_ProfessionLeveling
    {
        private ArrayList                               _levelingTasks;
        private ArrayList                               _suppliesToBuy;

        private Int32                                   _startLevel;
        private Int32                                   _endLevel;

        private Boolean                                 _purchaseSupplies;

        private String                                  _profession;

        public NWO_ProfessionLeveling()
        {
            _levelingTasks                              = new ArrayList();
            _suppliesToBuy                              = new ArrayList();
            
            _startLevel                                 = -1;
            _endLevel                                   = -1;

            _purchaseSupplies                           = false;

            _profession                                 = String.Empty;
        }

        public ArrayList Tasks
        {
            get
            {
                return _levelingTasks;
            }
            set
            {
                _levelingTasks = value;
            }
        }

        public ArrayList Supplies
        {
            get
            {
                return _suppliesToBuy;
            }
            set
            {
                _suppliesToBuy = value;
            }
        }

        public Int32 Start
        {
            get
            {
                return _startLevel;
            }
            set
            {
                _startLevel = value;
            }
        }

        public Int32 End
        {
            get
            {
                return _endLevel;
            }
            set
            {
                _endLevel = value;
            }
        }

        public Boolean PurchaseSupplies
        {
            get
            {
                return _purchaseSupplies;
            }
            set
            {
                _purchaseSupplies = value;
            }
        }

        public Boolean AllTasksDone
        {
            get
            {
                Boolean retVal = true;

                foreach (NWO_Task task in _levelingTasks)
                {
                    if (!retVal)
                        break;

                    retVal = task.Done;
                }

                return retVal;
            }
        }

        public String Profession
        {
            get
            {
                return _profession;
            }
            set
            {
                _profession = value;
            }
        }

        public String DisplayInformation
        {
            get
            {
                return String.Format("{0} Leveling : {1} to {2}", Profession, Start, End);
            }
        }

        public void ResetTasks()
        {
            foreach (NWO_Task task in _levelingTasks)
            {
                task.CurAmntDone = 0;
            }
        }

        public void WriteToConfig(StreamWriter sw, Version configVer)
        {
            if (configVer.Major > 1 || configVer.Minor > 6)
            {
                sw.WriteLine(_startLevel);
                sw.WriteLine(_endLevel);
                sw.WriteLine(_profession);
                sw.WriteLine(_purchaseSupplies);

                sw.WriteLine(_levelingTasks.Count);

                foreach (NWO_Task task in _levelingTasks)
                {
                    task.WriteToConfig(sw, configVer);
                }

                sw.WriteLine(_suppliesToBuy.Count);

                foreach (NWO_Supplies supply in _suppliesToBuy)
                {
                    supply.WriteToConfig(sw, configVer);
                }
            }
        }

        public void ReadFromConfig(StreamReader sr, Version configVer)
        {
            if (configVer.Major > 1 || configVer.Minor > 6)
            {
                _startLevel             = Convert.ToInt32(sr.ReadLine());
                _endLevel               = Convert.ToInt32(sr.ReadLine());
                _profession             = sr.ReadLine();
                _purchaseSupplies       = Convert.ToBoolean(sr.ReadLine());

                int iTemp               = Convert.ToInt32(sr.ReadLine());

                for (int i = 0; i < iTemp; i++)
                {
                    NWO_Task task       = new NWO_Task();
                    task.ReadFromConfig(sr, configVer);
                    _levelingTasks.Add(task);
                }

                iTemp                   = Convert.ToInt32(sr.ReadLine());

                for (int i = 0; i < iTemp; i++)
                {
                    NWO_Supplies supply = new NWO_Supplies();
                    supply.ReadFromConfig(sr, configVer);
                    _suppliesToBuy.Add(supply);
                }
            }
        }
    }
}
