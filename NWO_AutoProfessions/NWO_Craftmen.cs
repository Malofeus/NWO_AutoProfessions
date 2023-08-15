using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWO_AutoProfessions
{
    public class NWO_Craftsmen
    {
        private String                          _Profession;
        private String                          _Rank1Common;
        private String                          _Rank2Common;
        private String                          _Rank3Common;
        private String                          _Rank3Uncommon;
        private String                          _Rank3Rare;
        private String                          _Rank3Epic;

        public NWO_Craftsmen()
        {
            _Profession                         = String.Empty;
            _Rank1Common                        = String.Empty;
            _Rank2Common                        = String.Empty;
            _Rank3Common                        = String.Empty;
            _Rank3Uncommon                      = String.Empty;
            _Rank3Rare                          = String.Empty;
            _Rank3Epic                          = String.Empty;
        }

        public String Profession
        {
            get
            {
                return _Profession;
            }
            set
            {
                _Profession = value;
            }
        }

        public String Rank1Common
        {
            get
            {
                return _Rank1Common;
            }
            set
            {
                _Rank1Common = value;
            }
        }

        public String Rank2Common
        {
            get
            {
                return _Rank2Common;
            }
            set
            {
                _Rank2Common = value;
            }
        }

        public String Rank3Common
        {
            get
            {
                return _Rank3Common;
            }
            set
            {
                _Rank3Common = value;
            }
        }

        public String Rank3Uncommon
        {
            get
            {
                return _Rank3Uncommon;
            }
            set
            {
                _Rank3Uncommon = value;
            }
        }

        public String Rank3Rare
        {
            get
            {
                return _Rank3Rare;
            }
            set
            {
                _Rank3Rare = value;
            }
        }

        public String Rank3Epic
        {
            get
            {
                return _Rank3Epic;
            }
            set
            {
                _Rank3Epic = value;
            }
        }
    }
}
