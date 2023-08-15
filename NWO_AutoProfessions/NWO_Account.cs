using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace NWO_AutoProfessions
{
    public class NWO_Account
    {
        private readonly Version                                    _NWO_Version                        = new Version(1, 8);

        private String                                              _NWO_UserName;
        private String                                              _NWO_Password;
        private String                                              _NWO_AccountNick;
        private String                                              _NWO_AutoProfessionFileLoc;
        private String                                              _NWO_BottingCharacter;

        private readonly String                                     _NWO_ConfigLoc                      = @"Config";
        private readonly String                                     _NWO_ConfigFileName                 = "Config.cfg";

        private Boolean                                             _NWO_AutoLogin;
        private Boolean                                             _NWO_AutoCalcWaitTime;
        private Boolean                                             _NWO_AppendLogFile;
        private Boolean                                             _NWO_DevLogging;
        private Boolean                                             _NWO_VerboseLogging;
        private Boolean                                             _NWO_IsShrunk;
        private Boolean                                             _NWO_Botting;

        private Int32                                               _NWO_MaxLoginRetry;
        private Int32                                               _NWO_WaitTimeForRetry;
        private Int32                                               _NWO_WaitTimePageLoad;
        private Int32                                               _NWO_WaitTimeBetweenChecks;

        private readonly Int32                                      _NWO_WaitTimeMaintenance            = 30;  // Minutes X * 60 * 1000
        private readonly Int32                                      _NWO_BottingWaitToSellItems         = 2;   // Minutes X * 60 * 1000

        private ArrayList                                           _NWO_CharacterList;

        public NWO_Account()
        {
            _NWO_UserName                                           = String.Empty;
            _NWO_Password                                           = String.Empty;
            _NWO_AccountNick                                        = String.Empty;
            _NWO_BottingCharacter                                   = String.Empty;
            _NWO_AutoProfessionFileLoc                              = @"Config\Cycles";

            _NWO_AutoLogin                                          = false;
            _NWO_AutoCalcWaitTime                                   = false;
            _NWO_AppendLogFile                                      = false;
            _NWO_DevLogging                                         = false;
            _NWO_VerboseLogging                                     = false;
            _NWO_IsShrunk                                           = false;
            _NWO_Botting                                            = false;

            _NWO_MaxLoginRetry                                      = 5;        // Default value is 5 times;
            _NWO_WaitTimeForRetry                                   = 10;       // Default value is 10 min;
            _NWO_WaitTimePageLoad                                   = 5;        // Default value is 12 sec;
            _NWO_WaitTimeBetweenChecks                              = 120;      // Default value is 120 sec;

            _NWO_CharacterList                                      = new ArrayList();

            ReadFromConfig();
        }

        public String NWO_UserName
        {
            get
            {
                return _NWO_UserName;
            }
            set
            {
                if (value != String.Empty && value != _NWO_UserName)
                    _NWO_UserName = value;
            }
        }

        public String NWO_Password
        {
            get
            {
                return _NWO_Password;
            }
            set
            {
                if (value != String.Empty && value != _NWO_Password)
                    _NWO_Password = value;
            }
        }

        public String NWO_AccountNick
        {
            get
            {
                return _NWO_AccountNick;
            }
            set
            {
                if (value != String.Empty && value != _NWO_AccountNick)
                    _NWO_AccountNick = value;
            }
        }

        public String NWO_AutoProfessionFileLoc
        {
            get
            {
                return _NWO_AutoProfessionFileLoc;
            }
            set
            {
                if (value != string.Empty)
                    _NWO_AutoProfessionFileLoc = value;
            }
        }

        public String NWO_BottingCharacter
        {
            get
            {
                return _NWO_BottingCharacter;
            }
            set
            {
                _NWO_BottingCharacter = value;
            }
        }

        public Boolean NWO_AutoLogin
        {
            get
            {
                return _NWO_AutoLogin;
            }
            set
            {
                _NWO_AutoLogin = value;
            }
        }

        public Boolean NWO_AutoCalcWaitTime
        {
            get
            {
                return _NWO_AutoCalcWaitTime;
            }
            set
            {
                _NWO_AutoCalcWaitTime = value;
            }
        }

        public Boolean NWO_AppendLogFile
        {
            get
            {
                return _NWO_AppendLogFile;
            }
            set
            {
                _NWO_AppendLogFile = value;
            }
        }

        public Boolean NWO_DevLogging
        {
            get
            {
                return _NWO_DevLogging;
            }
            set
            {
                _NWO_DevLogging = value;
            }
        }

        public Boolean NWO_VerboseLogging
        {
            get
            {
                return _NWO_VerboseLogging;
            }
            set
            {
                _NWO_VerboseLogging = value;
            }
        }

        public Boolean NWO_IsShrunk
        {
            get
            {
                return _NWO_IsShrunk;
            }
            set
            {
                _NWO_IsShrunk = value;
            }
        }

        public Boolean NWO_ForBotting
        {
            get
            {
                return _NWO_Botting;
            }
            set
            {
                _NWO_Botting = value;
            }
        }

        public Boolean NWO_IsRunning
        {
            set
            {
                foreach (NWO_Character chars in _NWO_CharacterList)
                {
                    chars.NWO_IsRunning = value;
                }
            }
        }

        public Boolean NWO_TaskComplete
        {
            get
            {
                foreach (NWO_Character chars in _NWO_CharacterList)
                {
                    if (chars.IsTaskCompleted)
                        return true;
                }

                return false;
            }
        }

        public Int32 NWO_MaxLoginRetry
        {
            get
            {
                return _NWO_MaxLoginRetry;
            }
            set
            {
                _NWO_MaxLoginRetry = value;
            }
        }

        public Int32 NWO_WaitTimeForRetry
        {
            get
            {
                return _NWO_WaitTimeForRetry;
            }
            set
            {
                if ( value > 0 )
                    _NWO_WaitTimeForRetry = value;
            }
        }
       
        public Int32 NWO_WaitTimePageLoad
        {
            get
            {
                return _NWO_WaitTimePageLoad;
            }
            set
            {
                if ( value > 0 )
                    _NWO_WaitTimePageLoad = value;
            }
        }
       
        public Int32 NWO_WaitTimeBetweenChecks
        {
            get
            {
                Int32 timeToWait = 0;
                if (NWO_AutoCalcWaitTime)
                {
                    timeToWait = Int32.MaxValue;
                    foreach (NWO_Character nCharacter in NWO_CharacterList)
                    {
                        if (timeToWait > nCharacter.GetSmallestTaskDuration && nCharacter.GetSmallestTaskDuration > 0)
                            timeToWait = nCharacter.GetSmallestTaskDuration;
                    }

                    if (timeToWait == Int32.MaxValue)
                        timeToWait = _NWO_WaitTimeBetweenChecks;
                }
                else
                    timeToWait = _NWO_WaitTimeBetweenChecks;

                return timeToWait;
            }
            set
            {
                if ( value > 0 )
                    _NWO_WaitTimeBetweenChecks = value;
            }
        }

        public Int32 NWO_WaitTimeMantenance
        {
            get
            {
                return _NWO_WaitTimeMaintenance;
            }
        }

        public Int32 NWO_BottingWaitToSellItems
        {
            get
            {
                return _NWO_BottingWaitToSellItems;
            }
        }

        public ArrayList NWO_CharacterList
        {
            get
            {
                return _NWO_CharacterList;
            }
            set
            {
                _NWO_CharacterList = value;
            }
        }

        public Version NWO_Version
        {
            get
            {
                return _NWO_Version;
            }
        }

        public void WriteToConfig()
        {
            if (!Directory.Exists(_NWO_ConfigLoc))
                Directory.CreateDirectory(_NWO_ConfigLoc);

            StreamWriter sw = new StreamWriter(String.Format("{0}\\{1}", _NWO_ConfigLoc, _NWO_ConfigFileName));

            sw.WriteLine(StringCipher.Encrypt(_NWO_Version.ToString()));
            
            sw.WriteLine(StringCipher.Encrypt(_NWO_AccountNick));
            sw.WriteLine(StringCipher.Encrypt(_NWO_AppendLogFile.ToString()));
            sw.WriteLine(StringCipher.Encrypt(_NWO_AutoCalcWaitTime.ToString()));
            sw.WriteLine(StringCipher.Encrypt(_NWO_AutoLogin.ToString()));
            sw.WriteLine(StringCipher.Encrypt(_NWO_DevLogging.ToString()));
            sw.WriteLine(StringCipher.Encrypt(_NWO_MaxLoginRetry.ToString()));
            sw.WriteLine(StringCipher.Encrypt(_NWO_Password));
            sw.WriteLine(StringCipher.Encrypt(_NWO_UserName));
            sw.WriteLine(StringCipher.Encrypt(_NWO_VerboseLogging.ToString()));
            sw.WriteLine(StringCipher.Encrypt(_NWO_WaitTimeBetweenChecks.ToString()));
            sw.WriteLine(StringCipher.Encrypt(_NWO_WaitTimeForRetry.ToString()));
            sw.WriteLine(StringCipher.Encrypt(_NWO_WaitTimePageLoad.ToString()));
            sw.WriteLine(StringCipher.Encrypt(_NWO_IsShrunk.ToString()));
            
            if (_NWO_Version.Major > 1 || _NWO_Version.Minor > 3)
                sw.WriteLine(StringCipher.Encrypt(_NWO_Botting.ToString()));

            if (_NWO_Version.Major > 1 || _NWO_Version.Minor > 5)
                sw.WriteLine(StringCipher.Encrypt(_NWO_BottingCharacter.ToString()));

            foreach (NWO_Character character in _NWO_CharacterList)
            {
                sw.WriteLine(StringCipher.Encrypt(character.NWO_CharacterName));
                sw.WriteLine(StringCipher.Encrypt(character.NWO_CycleFileLocation));
                sw.WriteLine(StringCipher.Encrypt(character.NWO_CycleFileName));

                character.WriteToConfig(String.Format("{0}\\{1}_TaskConfig.cfg", _NWO_ConfigLoc, character.NWO_CharacterName), _NWO_Version);
            }

            sw.Close();
        }

        public void ReadFromConfig()
        {
            try
            {
                if (!Directory.Exists(_NWO_ConfigLoc))
                    return;

                if (!File.Exists(String.Format("{0}\\{1}", _NWO_ConfigLoc, _NWO_ConfigFileName)))
                    return;

                StreamReader sr = new StreamReader(String.Format("{0}\\{1}", _NWO_ConfigLoc, _NWO_ConfigFileName));

                Version ConfigVer = new Version(StringCipher.Decrypt(sr.ReadLine()));
                if (ConfigVer != _NWO_Version)
                    MessageBox.Show("The Version has changed, it is recommended that you press the Config and Profession Config buttons and close them again before Running.");

                _NWO_AccountNick = StringCipher.Decrypt(sr.ReadLine());
                _NWO_AppendLogFile = Convert.ToBoolean(StringCipher.Decrypt(sr.ReadLine()));
                _NWO_AutoCalcWaitTime = Convert.ToBoolean(StringCipher.Decrypt(sr.ReadLine()));
                _NWO_AutoLogin = Convert.ToBoolean(StringCipher.Decrypt(sr.ReadLine()));
                _NWO_DevLogging = Convert.ToBoolean(StringCipher.Decrypt(sr.ReadLine()));
                _NWO_MaxLoginRetry = Convert.ToInt32(StringCipher.Decrypt(sr.ReadLine()));
                _NWO_Password = StringCipher.Decrypt(sr.ReadLine());
                _NWO_UserName = StringCipher.Decrypt(sr.ReadLine());
                _NWO_VerboseLogging = Convert.ToBoolean(StringCipher.Decrypt(sr.ReadLine()));
                _NWO_WaitTimeBetweenChecks = Convert.ToInt32(StringCipher.Decrypt(sr.ReadLine()));
                _NWO_WaitTimeForRetry = Convert.ToInt32(StringCipher.Decrypt(sr.ReadLine()));
                _NWO_WaitTimePageLoad = Convert.ToInt32(StringCipher.Decrypt(sr.ReadLine()));
                _NWO_IsShrunk = Convert.ToBoolean(StringCipher.Decrypt(sr.ReadLine()));

                if (ConfigVer.Major > 1 || ConfigVer.Minor > 3)
                    _NWO_Botting = Convert.ToBoolean(StringCipher.Decrypt(sr.ReadLine()));

                if (ConfigVer.Major > 1 || ConfigVer.Minor > 5)
                    _NWO_BottingCharacter = StringCipher.Decrypt(sr.ReadLine());

                while (!sr.EndOfStream)
                {
                    NWO_Character tempCharacter = new NWO_Character();
                    tempCharacter.NWO_CharacterName = StringCipher.Decrypt(sr.ReadLine());
                    tempCharacter.NWO_CycleFileLocation = StringCipher.Decrypt(sr.ReadLine());
                    tempCharacter.NWO_CycleFileName = StringCipher.Decrypt(sr.ReadLine());

                    tempCharacter.ReadFromConfig(String.Format("{0}\\{1}_TaskConfig.cfg", _NWO_ConfigLoc, tempCharacter.NWO_CharacterName), ConfigVer);

                    _NWO_CharacterList.Add(tempCharacter);

                }

                sr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
