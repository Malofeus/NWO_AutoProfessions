using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

namespace NWO_AutoProfessions
{
    public class NWO_Task
    {
        private String              _TaskName;
        private String              _Name;
        private String              _Profession;
        private String              _URL;
        
        private int                 _Level;
        private int                 _AmntToDo;
        private int                 _CurAmtDone;

        private Boolean             _Rare;

        private NWO_Asset           _Assets;

        public NWO_Task()
        {
            _Name                   = String.Empty;
            _Profession             = String.Empty;
            _URL                    = String.Empty;

            _Level                  = -1;
            _AmntToDo               = 1;
            _CurAmtDone             = 0;

            _Assets                 = new NWO_Asset();

            _Rare                   = false;
        }

        public String DisplayName
        {
            get
            {
                if (_AmntToDo == 0 )
                    return String.Format("{0} {1} x Inf", _Level, _Name);
                else
                    return String.Format("{0} {1} x {2}", _Level, _Name, _AmntToDo);
            }
        }

        public String TaskName
        {
            get
            {
                return _Name;
            }
        }

        public String TaskFullName
        {
            get
            {
                return _TaskName;
            }
            set
            {
                if (value != String.Empty)
                {
                    _TaskName = value;
                    _Name = _TaskName.Remove(0, _TaskName.IndexOf(" ") + 1);
                    _Name = _Name.Remove(_Name.IndexOf(" ("), _Name.Count() - _Name.IndexOf(" ("));
                    _Level = Convert.ToInt32(_TaskName.Substring(0, _TaskName.IndexOf(" ")));
                }
            }
        }

        public String TaskProfession
        {
            get
            {
                return _Profession;
            }
            set
            {
                if (value != String.Empty)
                    _Profession = value;
            }
        }

        public String TaskURL
        {
            get
            {
                return _URL;
            }
            set
            {
                if (value != String.Empty)
                    _URL = value;
            }
        }

        public Boolean Rare
        {
            get
            {
                return _Rare;
            }
            set
            {
                _Rare = value;
            }
        }

        public Boolean Done
        {
            get
            {
                if (_AmntToDo > 0)
                    return (_AmntToDo <= _CurAmtDone);
                else
                    return false;
            }
        }

        public Int32 AmntToDo
        {
            get
            {
                return _AmntToDo;
            }
            set
            {
                _AmntToDo = value;
            }
        }

        public Int32 CurAmntDone
        {
            get
            {
                return _CurAmtDone;
            }
            set
            {
                if ( _AmntToDo != 0 )
                    _CurAmtDone = value;
            }
        }

        public Int32 TaskLevel
        {
            get
            {
                return _Level;
            }
        }

        public NWO_Asset Assets
        {
            get
            {
                return _Assets;
            }
            set
            {
                _Assets = value;
            }
        }

        public void WriteToConfig(StreamWriter sw, Version configVer)
        {
            sw.WriteLine(_TaskName);
            sw.WriteLine(_Profession);
            sw.WriteLine(_URL);
            
            if (configVer.Minor > 1 || configVer.Major > 1)
                sw.WriteLine(_Rare);

            sw.WriteLine(_AmntToDo.ToString());
            sw.WriteLine(_CurAmtDone.ToString());

            _Assets.WriteToConfig(sw, configVer);
        }

        public void ReadFromConfig(StreamReader sr, Version configVer)
        {
            TaskFullName = sr.ReadLine();
            TaskProfession = sr.ReadLine();
            TaskURL = sr.ReadLine();
            if (configVer.Minor > 1 || configVer.Major > 1)
                _Rare = Convert.ToBoolean(sr.ReadLine());

            AmntToDo = Convert.ToInt32(sr.ReadLine());
            CurAmntDone = Convert.ToInt32(sr.ReadLine());

            _Assets.ReadFromConfig(sr, configVer);
        }
    }
}
