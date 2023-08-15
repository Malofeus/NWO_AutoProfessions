using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NWO_AutoProfessions
{
    public class NWO_Supplies
    {
        private String _Name;

        private Int32 _AmntToBuy;
        private Int32 _CurAmntBought;

        private Boolean _BuyAsNeeded;

        public NWO_Supplies()
        {
            _Name                   = String.Empty;

            _AmntToBuy              = 0;
            _CurAmntBought          = 0;
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

        public Int32 AmntToBuy
        {
            get
            {
                return _AmntToBuy;
            }
            set
            {
                _AmntToBuy = value;
            }
        }

        public Int32 CurAmntBought
        {
            get
            {
                return _CurAmntBought;
            }
        }

        public Boolean BuyAsNeeded
        {
            get
            {
                return _BuyAsNeeded;
            }
            set
            {
                _BuyAsNeeded = value;
            }
        }

        public String DisplayInfo
        {
            get
            {
                return String.Format("{0} x {1}", Name, AmntToBuy);
            }
        }

        public void WriteToConfig(StreamWriter sw, Version configVer)
        {
            if (configVer.Major > 1 || configVer.Minor > 5)
            {
                sw.WriteLine(_Name);
                sw.WriteLine(_AmntToBuy);
                sw.WriteLine(_BuyAsNeeded);
            }
        }

        public void ReadFromConfig(StreamReader sr, Version configVer)
        {
            if (configVer.Major > 1 || configVer.Minor > 5)
            {
                _Name = sr.ReadLine();
                _AmntToBuy = Convert.ToInt32(sr.ReadLine());
                _BuyAsNeeded = Convert.ToBoolean(sr.ReadLine());
            }
        }
    }
}
