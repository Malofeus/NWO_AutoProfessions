using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace NWO_AutoProfessions
{
    public class NWO_Profession
    {
        private String                          _Name;
        
        private ArrayList                       _TaskNameList;
        private ArrayList                       _TaskNameURLList;

        private Int32                           _Rare;

        private DateTime                        _DateStart;
        private DateTime                        _DateEnd;

        public NWO_Profession()
        {
            _Name                       = String.Empty;

            _TaskNameList               = new ArrayList();
            _TaskNameURLList            = new ArrayList();

            _Rare                       = 0;

            _DateStart                  = new DateTime();
            _DateEnd                    = new DateTime();
        }

        public String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        public ArrayList TaskNameList
        {
            get
            {
                return _TaskNameList;
            }
            set
            {
                _TaskNameList = value;
            }
        }

        public ArrayList TaskNameURLList
        {
            get
            {
                return _TaskNameURLList;
            }
            set
            {
                _TaskNameURLList = value;
            }
        }

        public Int32 Rare
        {
            get
            {
                return _Rare;
            }
            set
            {
                if (value > 0)
                    _Rare = value;
            }
        }

        public DateTime DateStart
        {
            get
            {
                return _DateStart;
            }
            set
            {
                _DateStart = value;
            }
        }

        public DateTime DateEnd
        {
            get
            {
                return _DateEnd;
            }
            set
            {
                _DateEnd = value;
            }
        }
    }
}
