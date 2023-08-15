using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWO_AutoProfessions
{
    public class NWO_ProfessionLevels
    {
        private String _NWO_Profession_Name;

        private Int32 _NWO_Profession_Level;

        public NWO_ProfessionLevels()
        {
            _NWO_Profession_Name = String.Empty;
            _NWO_Profession_Level = 0;
        }

        public NWO_ProfessionLevels(String pName, Int32 pLevel)
        {
            _NWO_Profession_Name = pName;
            _NWO_Profession_Level = pLevel;
        }

        public String NWO_Profession_Name
        {
            get
            {
                return _NWO_Profession_Name;
            }
            set
            {
                if (value != String.Empty && value != _NWO_Profession_Name)
                    _NWO_Profession_Name = value;
            }
        }

        public Int32 NWO_Profession_Level
        {
            get
            {
                return _NWO_Profession_Level;
            }
            set
            {
                if (value != _NWO_Profession_Level)
                    _NWO_Profession_Level = 0;
            }
        }
    }
}
