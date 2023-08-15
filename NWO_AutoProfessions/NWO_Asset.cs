using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NWO_AutoProfessions
{
    public class NWO_Asset
    {
        private Boolean                 _Rank1Common;
        private Boolean                 _Rank2Common;
        private Boolean                 _Rank3Common;
        private Boolean                 _Rank3Uncommon;
        private Boolean                 _Rank3Rare;
        private Boolean                 _Rank3Epic;

        private Int32                   _Rank1Common_Quantity;
        private Int32                   _Rank2Common_Quantity;
        private Int32                   _Rank3Common_Quantity;
        private Int32                   _Rank3Uncommon_Quantity;
        private Int32                   _Rank3Rare_Quantity;
        private Int32                   _Rank3Epic_Quantity;

        public NWO_Asset()
        {
            _Rank1Common                            = false;
            _Rank2Common                            = false;
            _Rank3Common                            = false;
            _Rank3Uncommon                          = false;
            _Rank3Rare                              = false;
            _Rank3Epic                              = false;

            _Rank1Common_Quantity                   = -1;
            _Rank2Common_Quantity                   = -1;
            _Rank3Common_Quantity                   = -1;
            _Rank3Uncommon_Quantity                 = -1;
            _Rank3Rare_Quantity                     = -1;
            _Rank3Epic_Quantity                     = -1;
        }

        public Boolean Rank1Common
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

        public Boolean Rank2Common
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

        public Boolean Rank3Common
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

        public Boolean Rank3Uncommon
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

        public Boolean Rank3Rare
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

        public Boolean Rank3Epic
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

        public Boolean HasAssets
        {
            get
            {
                Boolean bRetValue = false;

                if (_Rank1Common && _Rank1Common_Quantity > 0)
                    bRetValue = true;
                if (_Rank2Common && _Rank2Common_Quantity > 0)
                    bRetValue = true;
                if (_Rank3Common && _Rank3Common_Quantity > 0)
                    bRetValue = true;
                if (_Rank3Uncommon && _Rank3Uncommon_Quantity > 0)
                    bRetValue = true;
                if (_Rank3Rare && _Rank3Rare_Quantity > 0)
                    bRetValue = true;
                if (_Rank3Epic && _Rank3Epic_Quantity > 0)
                    bRetValue = true;

                return bRetValue;
            }
        }

        public Int32 Rank1Common_Quantity
        {
            get
            {
                return _Rank1Common_Quantity;
            }
            set
            {
                _Rank1Common_Quantity = value;
            }
        }

        public Int32 Rank2Common_Quantity
        {
            get
            {
                return _Rank2Common_Quantity;
            }
            set
            {
                _Rank2Common_Quantity = value;
            }
        }

        public Int32 Rank3Common_Quantity
        {
            get
            {
                return _Rank3Common_Quantity;
            }
            set
            {
                _Rank3Common_Quantity = value;
            }
        }

        public Int32 Rank3Uncommon_Quantity
        {
            get
            {
                return _Rank3Uncommon_Quantity;
            }
            set
            {
                _Rank3Uncommon_Quantity = value;
            }
        }

        public Int32 Rank3Rare_Quantity
        {
            get
            {
                return _Rank3Rare_Quantity;
            }
            set
            {
                _Rank3Rare_Quantity = value;
            }
        }

        public Int32 Rank3Epic_Quantity
        {
            get
            {
                return _Rank3Epic_Quantity;
            }
            set
            {
                _Rank3Epic_Quantity = value;
            }
        }

        public void WriteToConfig(StreamWriter sw, Version configVer)
        {
            sw.WriteLine(_Rank1Common);
            sw.WriteLine(_Rank2Common);
            sw.WriteLine(_Rank3Common);
            sw.WriteLine(_Rank3Uncommon);
            sw.WriteLine(_Rank3Rare);
            sw.WriteLine(_Rank3Epic);
            sw.WriteLine(_Rank1Common_Quantity.ToString());
            sw.WriteLine(_Rank2Common_Quantity.ToString());
            sw.WriteLine(_Rank3Common_Quantity.ToString());
            sw.WriteLine(_Rank3Uncommon_Quantity.ToString());
            sw.WriteLine(_Rank3Rare_Quantity.ToString());
            sw.WriteLine(_Rank3Epic_Quantity.ToString());
        }

        public void ReadFromConfig(StreamReader sr, Version configVer)
        {
            _Rank1Common = Convert.ToBoolean(sr.ReadLine());
            _Rank2Common = Convert.ToBoolean(sr.ReadLine());
            _Rank3Common = Convert.ToBoolean(sr.ReadLine());
            _Rank3Uncommon = Convert.ToBoolean(sr.ReadLine());
            _Rank3Rare = Convert.ToBoolean(sr.ReadLine());
            _Rank3Epic = Convert.ToBoolean(sr.ReadLine());
            _Rank1Common_Quantity = Convert.ToInt32(sr.ReadLine());
            _Rank2Common_Quantity = Convert.ToInt32(sr.ReadLine());
            _Rank3Common_Quantity = Convert.ToInt32(sr.ReadLine());
            _Rank3Uncommon_Quantity = Convert.ToInt32(sr.ReadLine());
            _Rank3Rare_Quantity = Convert.ToInt32(sr.ReadLine());
            _Rank3Epic_Quantity = Convert.ToInt32(sr.ReadLine());
        }
    }
}
