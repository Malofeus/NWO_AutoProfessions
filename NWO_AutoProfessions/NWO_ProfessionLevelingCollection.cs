using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

namespace NWO_AutoProfessions
{
    public class NWO_ProfessionLevelingCollection
    {
        private ArrayList                               _professionLevelingInformation;

        private Int32                                   _curProfessionLevel; // Will Update it everytime

        public NWO_ProfessionLevelingCollection()
        {
            _professionLevelingInformation              = new ArrayList();
        }

        public ArrayList ProfessionLevelingInfo
        {
            get
            {
                return _professionLevelingInformation;
            }
            set
            {
                _professionLevelingInformation = value;
            }
        }

        public Int32 StartLevel
        {
            get
            {
                Int32 smallest = Int32.MaxValue;
                foreach (NWO_ProfessionLeveling pl in _professionLevelingInformation)
                {
                    if (pl.Start < smallest)
                        smallest = pl.Start;
                }

                if (smallest == Int32.MaxValue)
                    smallest = - 1;

                return smallest;
            }
        }

        public Int32 EndLevel
        {
            get
            {
                Int32 Largest = Int32.MinValue;
                foreach (NWO_ProfessionLeveling pl in _professionLevelingInformation)
                {
                    if (pl.End > Largest)
                        Largest = pl.End;
                }

                if (Largest == Int32.MinValue)
                    Largest = -1;

                return Largest;
            }
        }

        public Int32 ProfessionLevel
        {
            get
            {
                return _curProfessionLevel;
            }
            set
            {
                _curProfessionLevel = value;
            }
        }

        public String Profession
        {
            get
            {
                if (_professionLevelingInformation.Count > 0)
                    return ((NWO_ProfessionLeveling)_professionLevelingInformation[0]).Profession;
                    
                return String.Empty;
            }
        }

        public String DisplayInfo
        {
            get
            {
                return String.Format("{0} Levels {1} : {2}", Profession, StartLevel, EndLevel);
            }
        }

        public NWO_ProfessionLeveling Current
        {
            get
            {
                if (_curProfessionLevel > -1)
                {
                    foreach (NWO_ProfessionLeveling pl in _professionLevelingInformation)
                    {
                        if (pl.Start <= _curProfessionLevel && pl.End > _curProfessionLevel)
                            return pl;
                    }
                }

                return null;
            }
        }

        public void WriteToConfig(StreamWriter sw, Version configVer)
        {
            if (configVer.Major > 1 || configVer.Minor > 6)
            {
                sw.WriteLine(_professionLevelingInformation.Count);

                foreach (NWO_ProfessionLeveling pl in _professionLevelingInformation)
                {
                    pl.WriteToConfig(sw, configVer);
                }
            }
        }

        public void ReadFromConfig(StreamReader sr, Version configVer)
        {
            if (configVer.Major > 1 || configVer.Minor > 6)
            {
                int iTemp = Convert.ToInt32(sr.ReadLine());

                for (int i = 0; i < iTemp; i++)
                {
                    NWO_ProfessionLeveling pl = new NWO_ProfessionLeveling();
                    pl.ReadFromConfig(sr, configVer);
                    _professionLevelingInformation.Add(pl);
                }
            }
        }
    }
}
