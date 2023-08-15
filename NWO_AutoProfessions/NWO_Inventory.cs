using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace NWO_AutoProfessions
{
    public class NWO_Inventory
    {
        private Boolean             _NWO_Sell_Junk;
        private Boolean             _NWO_Sell_NonMagicalEquipment;
        private Boolean             _NWO_OpenBoxes;
        private Boolean             _NWO_Sell_MagicalNonClass;

        public NWO_Inventory()
        {
            _NWO_Sell_Junk                          = false;
            _NWO_Sell_NonMagicalEquipment           = false;
            _NWO_OpenBoxes                          = false;
            _NWO_Sell_MagicalNonClass               = false;
        }

        public Boolean NWO_Sell_Junk
        {
            get
            {
                return _NWO_Sell_Junk;
            }
            set
            {
                _NWO_Sell_Junk = value;
            }
        }

        public Boolean NWO_Sell_NonMagicalEquipment
        {
            get
            {
                return _NWO_Sell_NonMagicalEquipment;
            }
            set
            {
                _NWO_Sell_NonMagicalEquipment = value;
            }
        }

        public Boolean NWO_OpenBoxes
        {
            get
            {
                return _NWO_OpenBoxes;
            }
            set
            {
                _NWO_OpenBoxes = value;
            }
        }

        public Boolean NWO_Sell_MagicalNonClass
        {
            get
            {
                return _NWO_Sell_MagicalNonClass;
            }
            set
            {
                _NWO_Sell_MagicalNonClass = value;
            }
        }

        public void WriteToConfig(StreamWriter sw, Version configVer)
        {
            if (configVer.Major > 1 || configVer.Minor > 1)
            {
                sw.WriteLine(_NWO_Sell_Junk);
                sw.WriteLine(_NWO_Sell_NonMagicalEquipment);
            }

            if (configVer.Major > 1 || configVer.Minor > 4)
                sw.WriteLine(_NWO_OpenBoxes);

            if (configVer.Major > 1 || configVer.Minor > 7)
                sw.WriteLine(_NWO_Sell_MagicalNonClass);
        }

        public void ReadFromConfig(StreamReader sr, Version configVer)
        {
            if (configVer.Major > 1 || configVer.Minor > 1)
            {
                _NWO_Sell_Junk = Convert.ToBoolean(sr.ReadLine());
                _NWO_Sell_NonMagicalEquipment = Convert.ToBoolean(sr.ReadLine());
            }

            if (configVer.Major > 1 || configVer.Minor > 4)
                _NWO_OpenBoxes = Convert.ToBoolean(sr.ReadLine());

            if (configVer.Major > 1 || configVer.Minor > 7)
                _NWO_Sell_MagicalNonClass = Convert.ToBoolean(sr.ReadLine());
        }
    }
}
