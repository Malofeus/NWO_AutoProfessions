using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Threading;

namespace NWO_AutoProfessions
{
    public class NWO_Character
    {
        private String                          _NWO_CharacterName;
        private String                          _NWO_CharacterType;
        private String                          _NWO_CharacterLevel;
        private String                          _NWO_CharacterRace;
        private String                          _NWO_CycleFileLocation;
        private String                          _NWO_CycleFileName;

        private NWO_TaskCollection              _NWO_Slot1;
        private NWO_TaskCollection              _NWO_Slot2;
        private NWO_TaskCollection              _NWO_Slot3;
        private NWO_TaskCollection              _NWO_Slot4;
        private NWO_TaskCollection              _NWO_Slot5;
        private NWO_TaskCollection              _NWO_Slot6;
        private NWO_TaskCollection              _NWO_Slot7;
        private NWO_TaskCollection              _NWO_Slot8;
        private NWO_TaskCollection              _NWO_Slot9;

        private NWO_ProfessionLevelingCollection _NWO_ProfLeveling_Slot1;
        private NWO_ProfessionLevelingCollection _NWO_ProfLeveling_Slot2;
        private NWO_ProfessionLevelingCollection _NWO_ProfLeveling_Slot3;
        private NWO_ProfessionLevelingCollection _NWO_ProfLeveling_Slot4;
        private NWO_ProfessionLevelingCollection _NWO_ProfLeveling_Slot5;
        private NWO_ProfessionLevelingCollection _NWO_ProfLeveling_Slot6;
        private NWO_ProfessionLevelingCollection _NWO_ProfLeveling_Slot7;
        private NWO_ProfessionLevelingCollection _NWO_ProfLeveling_Slot8;
        private NWO_ProfessionLevelingCollection _NWO_ProfLeveling_Slot9;

        private readonly Int32                  _NWO_MaxNumSlots                        = 9;

        private NWO_TasksRunning[]              _NWO_TimeTillSlotDone;

        private Boolean                         _NWO_RefineDiamonds;
        private Boolean                         _NWO_ProffessionLeveling_Slot1;
        private Boolean                         _NWO_ProffessionLeveling_Slot2;
        private Boolean                         _NWO_ProffessionLeveling_Slot3;
        private Boolean                         _NWO_ProffessionLeveling_Slot4;
        private Boolean                         _NWO_ProffessionLeveling_Slot5;
        private Boolean                         _NWO_ProffessionLeveling_Slot6;
        private Boolean                         _NWO_ProffessionLeveling_Slot7;
        private Boolean                         _NWO_ProffessionLeveling_Slot8;
        private Boolean                         _NWO_ProffessionLeveling_Slot9;
        private Boolean                         _NWO_IsRunning;

        private NWO_Inventory                   _NWO_Inventory;

        private Thread                          _NWO_TaskCountdownThread;

        public NWO_Character()
        {
            _NWO_CharacterName                  = String.Empty;
            _NWO_CharacterType                  = String.Empty;
            _NWO_CharacterLevel                 = String.Empty;
            _NWO_CharacterRace                  = String.Empty;
            _NWO_CycleFileLocation              = String.Empty;
            _NWO_CycleFileName                  = String.Empty;

            _NWO_Slot1                          = new NWO_TaskCollection();
            _NWO_Slot2                          = new NWO_TaskCollection();
            _NWO_Slot3                          = new NWO_TaskCollection();
            _NWO_Slot4                          = new NWO_TaskCollection();
            _NWO_Slot5                          = new NWO_TaskCollection();
            _NWO_Slot6                          = new NWO_TaskCollection();
            _NWO_Slot7                          = new NWO_TaskCollection();
            _NWO_Slot8                          = new NWO_TaskCollection();
            _NWO_Slot9                          = new NWO_TaskCollection();

            _NWO_ProfLeveling_Slot1             = new NWO_ProfessionLevelingCollection();
            _NWO_ProfLeveling_Slot2             = new NWO_ProfessionLevelingCollection();
            _NWO_ProfLeveling_Slot3             = new NWO_ProfessionLevelingCollection();
            _NWO_ProfLeveling_Slot4             = new NWO_ProfessionLevelingCollection();
            _NWO_ProfLeveling_Slot5             = new NWO_ProfessionLevelingCollection();
            _NWO_ProfLeveling_Slot6             = new NWO_ProfessionLevelingCollection();
            _NWO_ProfLeveling_Slot7             = new NWO_ProfessionLevelingCollection();
            _NWO_ProfLeveling_Slot8             = new NWO_ProfessionLevelingCollection();
            _NWO_ProfLeveling_Slot9             = new NWO_ProfessionLevelingCollection();

            _NWO_Inventory                      = new NWO_Inventory();

            _NWO_TimeTillSlotDone               = new NWO_TasksRunning[_NWO_MaxNumSlots];

            for (int i = 0; i < _NWO_MaxNumSlots; i++)
            {
                _NWO_TimeTillSlotDone[i]        = new NWO_TasksRunning();
            }

            _NWO_RefineDiamonds                 = false;
            _NWO_ProffessionLeveling_Slot1      = false;
            _NWO_ProffessionLeveling_Slot2      = false;
            _NWO_ProffessionLeveling_Slot3      = false;
            _NWO_ProffessionLeveling_Slot4      = false;
            _NWO_ProffessionLeveling_Slot5      = false;
            _NWO_ProffessionLeveling_Slot6      = false;
            _NWO_ProffessionLeveling_Slot7      = false;
            _NWO_ProffessionLeveling_Slot8      = false;
            _NWO_ProffessionLeveling_Slot9      = false;
            _NWO_IsRunning                      = false;
        }

        public String NWO_CharacterName
        {
            get
            {
                return _NWO_CharacterName;
            }
            set
            {
                if ( value.Length > 0 )
                    _NWO_CharacterName = value;
            }
        }

        public String NWO_CharacterType
        {
            get
            {
                return _NWO_CharacterType;
            }
            set
            {
                if (value.Length > 0)
                    _NWO_CharacterType = value;
            }
        }

        public String NWO_CharacterLevel
        {
            get
            {
                return _NWO_CharacterLevel;
            }
            set
            {
                if (value.Length > 0)
                    _NWO_CharacterLevel = value;
            }
        }

        public String NWO_CharacterRace
        {
            get
            {
                return _NWO_CharacterRace;
            }
            set
            {
                if (value.Length > 0)
                    _NWO_CharacterRace = value;
            }
        }

        public String NWO_CycleFileLocation
        {
            get
            {
                return _NWO_CycleFileLocation;
            }
            set
            {
                _NWO_CycleFileLocation = value;
            }
        }

        public String NWO_CycleFileName
        {
            get
            {
                return _NWO_CycleFileName;
            }
            set
            {
                _NWO_CycleFileName = value;
            }
        }

        public String CycleFile
        {
            get
            {
                return String.Format("{0}{1}", NWO_CycleFileLocation, NWO_CycleFileName);
            }
        }

        public String CharacterListName
        {
            get
            {
                if (NWO_CycleFileName != String.Empty)
                    return String.Format("{0}   ({1})", NWO_CharacterName, NWO_CycleFileName);
                else
                    return NWO_CharacterName;
            }
        }

        public NWO_TaskCollection Slot1
        {
            get
            {
                return _NWO_Slot1;
            }
            set
            {
                _NWO_Slot1 = value;
            }
        }

        public NWO_TaskCollection Slot2
        {
            get
            {
                return _NWO_Slot2;
            }
            set
            {
                _NWO_Slot2 = value;
            }
        }

        public NWO_TaskCollection Slot3
        {
            get
            {
                return _NWO_Slot3;
            }
            set
            {
                _NWO_Slot3 = value;
            }
        }

        public NWO_TaskCollection Slot4
        {
            get
            {
                return _NWO_Slot4;
            }
            set
            {
                _NWO_Slot4 = value;
            }
        }

        public NWO_TaskCollection Slot5
        {
            get
            {
                return _NWO_Slot5;
            }
            set
            {
                _NWO_Slot5 = value;
            }
        }

        public NWO_TaskCollection Slot6
        {
            get
            {
                return _NWO_Slot6;
            }
            set
            {
                _NWO_Slot6 = value;
            }
        }

        public NWO_TaskCollection Slot7
        {
            get
            {
                return _NWO_Slot7;
            }
            set
            {
                _NWO_Slot7 = value;
            }
        }

        public NWO_TaskCollection Slot8
        {
            get
            {
                return _NWO_Slot8;
            }
            set
            {
                _NWO_Slot8 = value;
            }
        }

        public NWO_TaskCollection Slot9
        {
            get
            {
                return _NWO_Slot9;
            }
            set
            {
                _NWO_Slot9 = value;
            }
        }

        public NWO_ProfessionLevelingCollection ProfLeveling_Slot1
        {
            get
            {
                return _NWO_ProfLeveling_Slot1;
            }
            set
            {
                _NWO_ProfLeveling_Slot1 = value;
            }
        }

        public NWO_ProfessionLevelingCollection ProfLeveling_Slot2
        {
            get
            {
                return _NWO_ProfLeveling_Slot2;
            }
            set
            {
                _NWO_ProfLeveling_Slot2 = value;
            }
        }

        public NWO_ProfessionLevelingCollection ProfLeveling_Slot3
        {
            get
            {
                return _NWO_ProfLeveling_Slot3;
            }
            set
            {
                _NWO_ProfLeveling_Slot3 = value;
            }
        }

        public NWO_ProfessionLevelingCollection ProfLeveling_Slot4
        {
            get
            {
                return _NWO_ProfLeveling_Slot4;
            }
            set
            {
                _NWO_ProfLeveling_Slot4 = value;
            }
        }

        public NWO_ProfessionLevelingCollection ProfLeveling_Slot5
        {
            get
            {
                return _NWO_ProfLeveling_Slot5;
            }
            set
            {
                _NWO_ProfLeveling_Slot5 = value;
            }
        }

        public NWO_ProfessionLevelingCollection ProfLeveling_Slot6
        {
            get
            {
                return _NWO_ProfLeveling_Slot6;
            }
            set
            {
                _NWO_ProfLeveling_Slot6 = value;
            }
        }

        public NWO_ProfessionLevelingCollection ProfLeveling_Slot7
        {
            get
            {
                return _NWO_ProfLeveling_Slot7;
            }
            set
            {
                _NWO_ProfLeveling_Slot7 = value;
            }
        }

        public NWO_ProfessionLevelingCollection ProfLeveling_Slot8
        {
            get
            {
                return _NWO_ProfLeveling_Slot8;
            }
            set
            {
                _NWO_ProfLeveling_Slot8 = value;
            }
        }

        public NWO_ProfessionLevelingCollection ProfLeveling_Slot9
        {
            get
            {
                return _NWO_ProfLeveling_Slot9;
            }
            set
            {
                _NWO_ProfLeveling_Slot9 = value;
            }
        }

        public Boolean HasTasksToRun
        {
            get
            {
                if (Slot1.Tasks.Count > 0)
                    return true;
                if (Slot2.Tasks.Count > 0)
                    return true;
                if (Slot3.Tasks.Count > 0)
                    return true;
                if (Slot4.Tasks.Count > 0)
                    return true;
                if (Slot5.Tasks.Count > 0)
                    return true;
                if (Slot6.Tasks.Count > 0)
                    return true;
                if (Slot7.Tasks.Count > 0)
                    return true;
                if (Slot8.Tasks.Count > 0)
                    return true;
                if (Slot9.Tasks.Count > 0)
                    return true;

                if (_NWO_ProffessionLeveling_Slot1)
                    return true;
                if (_NWO_ProffessionLeveling_Slot2)
                    return true;
                if (_NWO_ProffessionLeveling_Slot3)
                    return true;
                if (_NWO_ProffessionLeveling_Slot4)
                    return true;
                if (_NWO_ProffessionLeveling_Slot5)
                    return true;
                if (_NWO_ProffessionLeveling_Slot6)
                    return true;
                if (_NWO_ProffessionLeveling_Slot7)
                    return true;
                if (_NWO_ProffessionLeveling_Slot8)
                    return true;
                if (_NWO_ProffessionLeveling_Slot9)
                    return true;

                return false;
            }
        }

        public Boolean RefineAstralDiamonds
        {
            get
            {
                return _NWO_RefineDiamonds;
            }
            set
            {
                _NWO_RefineDiamonds = value;
            }
        }

        public Boolean ProffessionLeveling_Slot1
        {
            get
            {
                return _NWO_ProffessionLeveling_Slot1;
            }
            set
            {
                _NWO_ProffessionLeveling_Slot1 = value;
            }
        }

        public Boolean ProffessionLeveling_Slot2
        {
            get
            {
                return _NWO_ProffessionLeveling_Slot2;
            }
            set
            {
                _NWO_ProffessionLeveling_Slot2 = value;
            }
        }

        public Boolean ProffessionLeveling_Slot3
        {
            get
            {
                return _NWO_ProffessionLeveling_Slot3;
            }
            set
            {
                _NWO_ProffessionLeveling_Slot3 = value;
            }
        }

        public Boolean ProffessionLeveling_Slot4
        {
            get
            {
                return _NWO_ProffessionLeveling_Slot4;
            }
            set
            {
                _NWO_ProffessionLeveling_Slot4 = value;
            }
        }

        public Boolean ProffessionLeveling_Slot5
        {
            get
            {
                return _NWO_ProffessionLeveling_Slot5;
            }
            set
            {
                _NWO_ProffessionLeveling_Slot5 = value;
            }
        }

        public Boolean ProffessionLeveling_Slot6
        {
            get
            {
                return _NWO_ProffessionLeveling_Slot6;
            }
            set
            {
                _NWO_ProffessionLeveling_Slot6 = value;
            }
        }

        public Boolean ProffessionLeveling_Slot7
        {
            get
            {
                return _NWO_ProffessionLeveling_Slot7;
            }
            set
            {
                _NWO_ProffessionLeveling_Slot7 = value;
            }
        }

        public Boolean ProffessionLeveling_Slot8
        {
            get
            {
                return _NWO_ProffessionLeveling_Slot8;
            }
            set
            {
                _NWO_ProffessionLeveling_Slot8 = value;
            }
        }

        public Boolean ProffessionLeveling_Slot9
        {
            get
            {
                return _NWO_ProffessionLeveling_Slot9;
            }
            set
            {
                _NWO_ProffessionLeveling_Slot9 = value;
            }
        }

        public Boolean NWO_IsRunning
        {
            set
            {
                _NWO_IsRunning = value;

                if (!_NWO_IsRunning)
                {
                    for (int i = 0; i < _NWO_MaxNumSlots; i++)
                    {
                        _NWO_TimeTillSlotDone[i].NWO_HasTask = false;
                    }
                }
            }
        }

        public Boolean IsTaskCompleted
        {
            get
            {
                for (int i = 0; i < _NWO_MaxNumSlots; i++)
                {
                    if (_NWO_TimeTillSlotDone[i].NWO_TaskCompleted)
                        return true;

                }

                return false;
            }
        }

        public Boolean TaskRunning
        {
            get
            {
                for (int i = 0; i < _NWO_MaxNumSlots; i++)
                {
                    if (_NWO_TimeTillSlotDone[i].NWO_HasTask)
                        return true;
                }

                return false;
            }
        }

        public NWO_Inventory Inventory
        {
            get
            {
                return _NWO_Inventory;
            }
            set
            {
                _NWO_Inventory = value;
            }
        }

        public Int32 TaskCompleted
        {
            set
            {
                _NWO_TimeTillSlotDone[value].NWO_HasTask = false;
            }
        }

        public Int32 GetSmallestTaskDuration
        {
            get
            {
                Int32 smallest = Int32.MaxValue;
                for (int i = 0; i < _NWO_MaxNumSlots; i++)
                {
                    if (_NWO_TimeTillSlotDone[i].NWO_HasTask)
                    {
                        if (smallest > _NWO_TimeTillSlotDone[i].NWO_TaskRunTime && _NWO_TimeTillSlotDone[i].NWO_TaskRunTime > 0)
                            smallest = _NWO_TimeTillSlotDone[i].NWO_TaskRunTime;
                    }
                }

                if (smallest == Int32.MaxValue)
                    return 0;
                else
                    return smallest;
            }
        }

        public Int32 SubtractWaitTimeFromTaskDuration
        {
            set
            {
                for (int i = 0; i < _NWO_MaxNumSlots; i++)
                {
                    if (_NWO_TimeTillSlotDone[i].NWO_HasTask)
                    {
                        _NWO_TimeTillSlotDone[i].NWO_TaskRunTime -= value;

                        if (_NWO_TimeTillSlotDone[i].NWO_TaskRunTime < 0)
                            _NWO_TimeTillSlotDone[i].NWO_TaskRunTime = 0;
                    }
                }
            }
        }

        public void TaskTimerCountdown()
        {
            Int32 count = 0;
            if (_NWO_IsRunning)
            {
                do
                {
                    count = GetSmallestTaskDuration;
                    if (count > 0)
                    {
                        Thread.Sleep(1000);
                        SubtractWaitTimeFromTaskDuration = 1;
                    }
                } while (_NWO_IsRunning && TaskRunning);
            }
        }

        public void WriteToConfig(String configFile, Version configVer)
        {
            StreamWriter sw = new StreamWriter(configFile);

            sw.WriteLine(_NWO_CharacterName);

            WriteToConfig(sw, configVer);

            sw.Close();
        }

        public void WriteToConfig(StreamWriter sw, Version configVer)
        {
            sw.WriteLine(_NWO_CycleFileLocation);
            sw.WriteLine(_NWO_CycleFileName);

            if (configVer.Minor > 0 || configVer.Major > 1)
                sw.WriteLine(_NWO_RefineDiamonds);

            if (configVer.Major > 1 || configVer.Minor > 6)
            {
                sw.WriteLine(_NWO_ProffessionLeveling_Slot1);
                sw.WriteLine(_NWO_ProffessionLeveling_Slot2);
                sw.WriteLine(_NWO_ProffessionLeveling_Slot3);
                sw.WriteLine(_NWO_ProffessionLeveling_Slot4);
                sw.WriteLine(_NWO_ProffessionLeveling_Slot5);
                sw.WriteLine(_NWO_ProffessionLeveling_Slot6);
                sw.WriteLine(_NWO_ProffessionLeveling_Slot7);
                sw.WriteLine(_NWO_ProffessionLeveling_Slot8);
                sw.WriteLine(_NWO_ProffessionLeveling_Slot9);
            }

            _NWO_Slot1.WriteToConfig(sw, configVer);
            _NWO_Slot2.WriteToConfig(sw, configVer);
            _NWO_Slot3.WriteToConfig(sw, configVer);
            _NWO_Slot4.WriteToConfig(sw, configVer);
            _NWO_Slot5.WriteToConfig(sw, configVer);
            _NWO_Slot6.WriteToConfig(sw, configVer);
            _NWO_Slot7.WriteToConfig(sw, configVer);
            _NWO_Slot8.WriteToConfig(sw, configVer);
            _NWO_Slot9.WriteToConfig(sw, configVer);

            if (configVer.Major > 1 || configVer.Minor > 6)
            {
                _NWO_ProfLeveling_Slot1.WriteToConfig(sw, configVer);
                _NWO_ProfLeveling_Slot2.WriteToConfig(sw, configVer);
                _NWO_ProfLeveling_Slot3.WriteToConfig(sw, configVer);
                _NWO_ProfLeveling_Slot4.WriteToConfig(sw, configVer);
                _NWO_ProfLeveling_Slot5.WriteToConfig(sw, configVer);
                _NWO_ProfLeveling_Slot6.WriteToConfig(sw, configVer);
                _NWO_ProfLeveling_Slot7.WriteToConfig(sw, configVer);
                _NWO_ProfLeveling_Slot8.WriteToConfig(sw, configVer);
                _NWO_ProfLeveling_Slot9.WriteToConfig(sw, configVer);
            }

            _NWO_Inventory.WriteToConfig(sw, configVer);
        }

        public void ReadFromConfig(String configFile, Version configVer)
        {
            StreamReader sr = new StreamReader(configFile);

            _NWO_CharacterName = sr.ReadLine();

            ReadFromConfig(sr, configVer);

            sr.Close();
        }

        public void ReadFromConfig(StreamReader sr, Version configVer)
        {
            _NWO_CycleFileLocation = sr.ReadLine();
            _NWO_CycleFileName = sr.ReadLine();
            if (configVer.Minor > 0 || configVer.Major > 1)
                _NWO_RefineDiamonds = Convert.ToBoolean(sr.ReadLine());

            if (configVer.Major > 1 || configVer.Minor > 6)
            {
                _NWO_ProffessionLeveling_Slot1 = Convert.ToBoolean(sr.ReadLine());
                _NWO_ProffessionLeveling_Slot2 = Convert.ToBoolean(sr.ReadLine());
                _NWO_ProffessionLeveling_Slot3 = Convert.ToBoolean(sr.ReadLine());
                _NWO_ProffessionLeveling_Slot4 = Convert.ToBoolean(sr.ReadLine());
                _NWO_ProffessionLeveling_Slot5 = Convert.ToBoolean(sr.ReadLine());
                _NWO_ProffessionLeveling_Slot6 = Convert.ToBoolean(sr.ReadLine());
                _NWO_ProffessionLeveling_Slot7 = Convert.ToBoolean(sr.ReadLine());
                _NWO_ProffessionLeveling_Slot8 = Convert.ToBoolean(sr.ReadLine());
                _NWO_ProffessionLeveling_Slot9 = Convert.ToBoolean(sr.ReadLine());
            }

            _NWO_Slot1.ReadFromConfig(sr, configVer);
            _NWO_Slot2.ReadFromConfig(sr, configVer);
            _NWO_Slot3.ReadFromConfig(sr, configVer);
            _NWO_Slot4.ReadFromConfig(sr, configVer);
            _NWO_Slot5.ReadFromConfig(sr, configVer);
            _NWO_Slot6.ReadFromConfig(sr, configVer);
            _NWO_Slot7.ReadFromConfig(sr, configVer);
            _NWO_Slot8.ReadFromConfig(sr, configVer);
            _NWO_Slot9.ReadFromConfig(sr, configVer);

            if (configVer.Major > 1 || configVer.Minor > 6)
            {
                _NWO_ProfLeveling_Slot1.ReadFromConfig(sr, configVer);
                _NWO_ProfLeveling_Slot2.ReadFromConfig(sr, configVer);
                _NWO_ProfLeveling_Slot3.ReadFromConfig(sr, configVer);
                _NWO_ProfLeveling_Slot4.ReadFromConfig(sr, configVer);
                _NWO_ProfLeveling_Slot5.ReadFromConfig(sr, configVer);
                _NWO_ProfLeveling_Slot6.ReadFromConfig(sr, configVer);
                _NWO_ProfLeveling_Slot7.ReadFromConfig(sr, configVer);
                _NWO_ProfLeveling_Slot8.ReadFromConfig(sr, configVer);
                _NWO_ProfLeveling_Slot9.ReadFromConfig(sr, configVer);
            }
            
            _NWO_Inventory.ReadFromConfig(sr, configVer);
        }

        public void AddTaskRunTime(int timeAmnt, int SlotNum)
        {
            if (SlotNum > -1 && SlotNum < _NWO_MaxNumSlots)
            {
                _NWO_TimeTillSlotDone[SlotNum].NWO_TaskRunTime = timeAmnt;
                _NWO_TimeTillSlotDone[SlotNum].NWO_HasTask = true;

                if (_NWO_IsRunning)
                {
                    if (_NWO_TaskCountdownThread == null || !_NWO_TaskCountdownThread.IsAlive)
                    {
                        _NWO_TaskCountdownThread = new Thread(TaskTimerCountdown);
                        _NWO_TaskCountdownThread.Start();
                    }
                }
            }
        }
    }
}
