using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net.NetworkInformation;

using Gecko;
using Gecko.DOM;

namespace NWO_AutoProfessions
{
    public partial class NWO_AutoProfession_Form1 : Form
    {
        private Boolean                     _NWO_isRunning = false;
        private Boolean                     _NWO_PageLoaded = false;
        private Boolean                     _NWO_isLoggedIn = false;
        private Boolean                     _NWO_HasNetwork = false;
        private Boolean                     _NWO_WaitingToCheck = false;
        private Boolean                     _NWO_BotSellingItems = false;
        private Boolean                     _NWO_MinimizedOnce = false;

        private System.Windows.Forms.Timer  _NWO_runningTimer = null;
        
        private Int32                       _NWO_runningTime = 0;
        private Int32                       _NWO_CurrentLoginAttempt = 0;
        private Int32                       _NWO_RunNumber = 0;
        private readonly Int32              _NWO_MaxNumberSlots = 9;

        private NWO_Account                 _NWO_Account;

        private Thread                      _NWO_MainThread;
        private Thread                      _NWO_BotSellingThread;

        private String                      _NWO_LoggedInChar;

        public NWO_AutoProfession_Form1()
        {
            InitializeComponent();

            Xpcom.Initialize(@"xulrunner");

            _NWO_Account = new NWO_Account();

            if (_NWO_Account.NWO_IsShrunk)
            {
                this.Size = new Size(575, this.Size.Height);
                NWO_Status_Textbox.Enabled = false;
                NWO_Status_Textbox.Hide();
                NWO_TabControl.Enabled = false;
                NWO_TabControl.Hide();
                ShrinkExpandForm_Button.Text = ">";
            }

            NWO_Start_Button.Enabled = false;

            _NWO_runningTimer = new System.Windows.Forms.Timer();
            _NWO_runningTimer.Interval = 1000;
            _NWO_runningTimer.Tick += new EventHandler(_NWO_runningTime_Tick);

            NWO_Timer_StatusLabel.Text = NWO_Timer_Text();
            
            NWO_Navigate(NWO_URLs.NWO_Gateway_URL);

            //_NWO_Navigate("www.google.com");

            AddNewTabs();
            
            // Reading the version number.
            //NWO_TabControl.Controls[0].Controls[0].Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            _NWO_LoggedInChar = String.Empty;

            _NWO_HasNetwork = NetworkInterface.GetIsNetworkAvailable();
            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);

            _NWO_NotifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu();

            MenuItem MI_Start = new MenuItem("Start");
            MI_Start.Click += new EventHandler(MI_Start_Click);
            _NWO_NotifyIcon.ContextMenu.MenuItems.Add(MI_Start);
            _NWO_NotifyIcon.ContextMenu.MenuItems[0].Enabled = false;

            MenuItem MI_Show = new MenuItem("Show");
            MI_Show.Click += new EventHandler(MI_Show_Click);
            _NWO_NotifyIcon.ContextMenu.MenuItems.Add(MI_Show);

            MenuItem MI_Close = new MenuItem("Close");
            MI_Close.Click += new EventHandler(MI_Close_Click);
            _NWO_NotifyIcon.ContextMenu.MenuItems.Add(MI_Close);
            
            _NWO_NotifyIcon.DoubleClick += new EventHandler(_NWO_NotifyIcon_DoubleClick);

            SetSystemTrayText();
        }

        ~NWO_AutoProfession_Form1()
        {
            NWO_IsRunning = false;
            _NWO_isLoggedIn = false;
            if (_NWO_MainThread != null)
            {
                if (_NWO_MainThread.IsAlive)
                    _NWO_MainThread.Abort();

                _NWO_MainThread = null;
            }

            if (_NWO_NotifyIcon != null)
            {
                _NWO_NotifyIcon.Dispose();
            }
        }

        private void NWO_AutoProfession_Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (!_NWO_MinimizedOnce)
                {
                    _NWO_NotifyIcon.BalloonTipTitle = "Minimized to Tray";
                    _NWO_NotifyIcon.BalloonTipText = "NWO AutoProfessions was minimized to the System Tray.";
                    _NWO_NotifyIcon.ShowBalloonTip(500);
                    _NWO_MinimizedOnce = true;
                }

                this.Hide();
            }
        }

        void _NWO_NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
        }

        void MI_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void MI_Show_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
        }

        void MI_Start_Click(object sender, EventArgs e)
        {
            NWO_Start_Button_Click(null, e);
        }

        private void SetSystemTrayText()
        {
            if (_NWO_Account.NWO_UserName != String.Empty)
                _NWO_NotifyIcon.Text = String.Format("{0} - NWO AutoProfession", _NWO_Account.NWO_UserName);
            else if (_NWO_Account.NWO_AccountNick != String.Empty)
                _NWO_NotifyIcon.Text = String.Format("{0} - NWO AutoProfession", _NWO_Account.NWO_AccountNick);
            else
                _NWO_NotifyIcon.Text = "NWO AutoProfession";
        }

        private void AddText(String text)
        {
            try
            {
                if (NWO_Status_Textbox.InvokeRequired)
                {
                    NWO_Defines.Delegates.NWO_CallbackText addText = new NWO_Defines.Delegates.NWO_CallbackText(AddText);
                    this.Invoke(addText, new object[] { text });
                }
                else
                {
                    if (_NWO_Account.NWO_AppendLogFile && _NWO_Account.NWO_DevLogging)
                    {
                        if (!Directory.Exists(@"Config\Logs"))
                            Directory.CreateDirectory(@"Config\Logs");

                        StreamWriter sw = new StreamWriter(String.Format(@"Config\Logs\{0}_Run_{1}_AutoProfession.log", String.Format("{0}_{1}", DateTime.Now.Month, DateTime.Now.Day), ((_NWO_RunNumber == 0) ? 1 : _NWO_RunNumber)), _NWO_Account.NWO_AppendLogFile);
                        //sw.WriteLine(String.Format("{0} {1}", String.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second), text));
                        sw.WriteLine(String.Format("{0} : {1} - {2}", String.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second), _NWO_LoggedInChar, text));
                        sw.Close();
                    }

                    if (text.LastIndexOf("\n") < text.Length - 2)
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            NWO_Status_Textbox.AppendText(String.Format("{0} : {1} - {2}\n", String.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second), _NWO_LoggedInChar, text));
                        else
                            NWO_Status_Textbox.AppendText(String.Format("{0} {1}\n", String.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second), text));
                    }
                    else
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            NWO_Status_Textbox.AppendText(String.Format("{0} : {1} - {2}\n", String.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second), _NWO_LoggedInChar, text));
                        else
                            NWO_Status_Textbox.AppendText(String.Format("{0} {1}", String.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second), text));
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void ClearInfoBrowser()
        {
            if (NWO_WebBrowser.InvokeRequired)
            {
                NWO_Defines.Delegates.NWO_Callback _clearInfoBrowser = new NWO_Defines.Delegates.NWO_Callback(ClearInfoBrowser);
                this.Invoke(_clearInfoBrowser, new object[] { });
            }
            else
            {
                NWO_WebBrowser.History.Clear();
            }
        }

        private void UpdateCompletedCount(String charName, Int32 slotNum)
        {
            if (NWO_TabControl.InvokeRequired)
            {
                NWO_Defines.Delegates.NWO_CallbackTextInt32 updateCount = new NWO_Defines.Delegates.NWO_CallbackTextInt32(UpdateCompletedCount);
                this.Invoke(updateCount, new object[] { charName, slotNum });
            }
            else
            {
                foreach (TabPage tp in NWO_TabControl.Controls)
                {
                    if (tp.Text.Contains(charName))
                    {
                        ((TextBox)tp.Controls[slotNum]).Text = Convert.ToString(Convert.ToInt32(((TextBox)tp.Controls[slotNum]).Text) + 1);
                        break;
                    }
                }
            }
        }

        private void UpdateRoughAstralDiamondsRefined(String charName, Int64 num)
        {
            if (NWO_TabControl.InvokeRequired)
            {
                NWO_Defines.Delegates.NWO_CallbackTextInt64 updateAstralDiamonds = new NWO_Defines.Delegates.NWO_CallbackTextInt64(UpdateRoughAstralDiamondsRefined);
                this.Invoke(updateAstralDiamonds, new object[] { charName, num });
            }
            else
            {
                foreach (TabPage tp in NWO_TabControl.Controls)
                {
                    if (tp.Text.Contains(charName))
                    {
                        String temp = ((TextBox)tp.Controls[9]).Text;
                        temp = temp.Replace(",", "");
                        Int64 curAmnt = Convert.ToInt64(temp);
                        curAmnt += num;

                        ((TextBox)tp.Controls[9]).Text = RefinedAstralDiamondsString(curAmnt);
                        break;
                    }
                }
            }
        }

        private void ShrinkExpandForm_Button_Click(object sender, EventArgs e)
        {
            if (_NWO_Account.NWO_IsShrunk)
            {
                _NWO_Account.NWO_IsShrunk = false;
                this.Size = new Size(850, this.Size.Height);
                NWO_Status_Textbox.Enabled = true;
                NWO_Status_Textbox.Show();
                NWO_TabControl.Enabled = true;
                NWO_TabControl.Show();
                ShrinkExpandForm_Button.Text = "<";
            }
            else
            {
                _NWO_Account.NWO_IsShrunk = true;
                this.Size = new Size(575, this.Size.Height);
                NWO_Status_Textbox.Enabled = false;
                NWO_Status_Textbox.Hide();
                NWO_TabControl.Enabled = false;
                NWO_TabControl.Hide();
                ShrinkExpandForm_Button.Text = ">";
            }
        }

        private void NWO_Start_Button_Click(object sender, EventArgs e)
        {
            if (NWO_IsRunning)
            {
                NWO_Start_Button.Text = "Start";
                _NWO_NotifyIcon.ContextMenu.MenuItems[0].Text = "Start";
                NWO_Start_Button.BackColor = Color.Lime;
                NWO_ProfessionConfig_Button.Enabled = true;
                NWO_IsRunning = false;
                _NWO_runningTimer.Enabled = false;
            }
            else
            {
                NWO_Start_Button.Text = "Running";
                _NWO_NotifyIcon.ContextMenu.MenuItems[0].Text = "Pause";
                NWO_Start_Button.BackColor = Color.Red;
                NWO_ProfessionConfig_Button.Enabled = false;
                _NWO_RunNumber++;
                NWO_IsRunning = true;
                _NWO_runningTimer.Enabled = true;
                _NWO_runningTime = 0;  // Reset for every Start.
                NWO_Timer_StatusLabel.Text = NWO_Timer_Text();

                if (_NWO_MainThread == null || !_NWO_MainThread.IsAlive)
                {
                    _NWO_MainThread = new Thread(Run);
                    _NWO_MainThread.Start();
                }

                StartBotSelling();
            }
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (_NWO_Account.NWO_DevLogging)
                AddText("Enter NetworkChange_NetworkAvailabilityChanged");

            if (e.IsAvailable)
                _NWO_HasNetwork = true;
            else
            {
                _NWO_HasNetwork = false;
                WaitForNetworkConnection();
            }

            if (_NWO_Account.NWO_DevLogging)
                AddText("Exiting NetworkChange_NetworkAvailabilityChanged");
        }

        private void StartBotSelling()
        {
            if (_NWO_Account.NWO_ForBotting)
            {
                if (_NWO_BotSellingThread == null || !_NWO_BotSellingThread.IsAlive)
                {
                    _NWO_BotSellingThread = new Thread(WaitForBotSelling);
                    _NWO_BotSellingThread.Start();
                }
            }
        }

        private void AddNewTabs()
        {
            if (NWO_TabControl.Controls.Count > 0)
                NWO_TabControl.Controls.Clear();

            foreach (NWO_Character nwoChar in _NWO_Account.NWO_CharacterList)
            {
                // Character Tab.
                TabPage tabPgCtrl = new TabPage();
                NWO_TabControl.Controls.Add(tabPgCtrl);

                tabPgCtrl.Text = nwoChar.NWO_CharacterName;
                tabPgCtrl.BackColor = Color.Gray;

                int tabIndex = 0;
                TextBox slotXTextbox;
                for (int i = 0; i < 9; i++)
                {
                    slotXTextbox = new TextBox();
                    tabPgCtrl.Controls.Add(slotXTextbox);

                    slotXTextbox.Name = String.Format("slot{0}Textbox", i);
                    slotXTextbox.Location = new Point(155, (30 + (26 * i)));
                    slotXTextbox.Size = new Size(52, 20);
                    slotXTextbox.TabIndex = tabIndex;
                    slotXTextbox.Text = "0";
                    slotXTextbox.TextAlign = HorizontalAlignment.Center;

                    tabIndex++;
                }

                slotXTextbox = new TextBox();
                tabPgCtrl.Controls.Add(slotXTextbox);
                slotXTextbox.Name = "RoughAstralDiamondsTextbox";
                slotXTextbox.Location = new Point(155, (30 + (26 * 9)));
                slotXTextbox.Size = new Size(70, 20);
                slotXTextbox.TabIndex = tabIndex;
                slotXTextbox.Text = "0";
                slotXTextbox.TextAlign = HorizontalAlignment.Center;
                
                tabIndex++;

                Label slotXLabel;
                for (int i = 0; i < 9; i++)
                {
                    slotXLabel = new Label();
                    tabPgCtrl.Controls.Add(slotXLabel);

                    slotXLabel.Name = String.Format("slot{0}Label", i);
                    slotXLabel.AutoSize = true;
                    slotXLabel.Text = String.Format("Slot {0} Completed:", (i + 1));
                    slotXLabel.Location = new Point(50, (33 + (26 * i)));
                    slotXLabel.Size = new System.Drawing.Size(90, 13);
                    slotXLabel.TabIndex = tabIndex;

                    tabIndex++;
                }

                slotXLabel = new Label();
                tabPgCtrl.Controls.Add(slotXLabel);

                slotXLabel.Name = "RoughAstralDiamondsLabel";
                slotXLabel.AutoSize = true;
                slotXLabel.Text = "Astral Diamonds Refined:";
                slotXLabel.Location = new Point(15, (33 + (26 * 9)));
                slotXLabel.Size = new System.Drawing.Size(90, 13);
                slotXLabel.TabIndex = tabIndex;

                tabIndex++;
            }
        }

        private void SwitchToCurrentCharacterTab()
        {
            if (NWO_TabControl.InvokeRequired)
            {
                NWO_Defines.Delegates.NWO_Callback switchTabCtrl = new NWO_Defines.Delegates.NWO_Callback(SwitchToCurrentCharacterTab);
                this.Invoke(switchTabCtrl);
            }
            else
            {
                for (int i = 0; i < NWO_TabControl.Controls.Count; i++)
                {
                    if (NWO_TabControl.Controls[i].Text.Contains(_NWO_LoggedInChar))
                    {
                        NWO_TabControl.SelectedIndex = i;
                        return;
                    }
                }

                NWO_TabControl.SelectedIndex = 0;
            }
        }

        private void _NWO_runningTime_Tick(object sender, EventArgs e)
        {
            _NWO_runningTime++;

            NWO_Timer_StatusLabel.Text = NWO_Timer_Text();
        }

        private String NWO_Timer_Text()
        {
            int temp = _NWO_runningTime;
            int h = (temp / (60 * 60));
            temp -= h * (60 * 60);
            int m = (temp / 60);
            temp -= m * 60;

            return String.Format("{0:00} : {1:00} : {2:00}", h, m, temp);
        }

        private String TaskDurationTimeString(int taskDurTime)
        {
            String rString = String.Empty;

            int h = taskDurTime / (60 * 60);
            taskDurTime -= h * (60 * 60);
            int m = taskDurTime / 60;
            taskDurTime -= m * 60;

            if (h > 0)
                rString = String.Format("{0}h", h);
            if (m > 0)
                rString = String.Format("{0} {1}m", rString, m);
            if (taskDurTime > 0)
                rString = String.Format("{0} {1}s", rString, taskDurTime);

            return rString;
        }

        private Int32 SplitNumber(Int64 num, Int32 split)
        {
            Int32 count = 0;

            do
            {
                num /= split;

                if (num > 0)
                    count++;

            } while (num > 0);

            return count;
        }

        private String RefinedAstralDiamondsString(Int64 refinedAstralDiamonds)
        {
            String retStr = String.Empty;

            Int32 tCount = SplitNumber(refinedAstralDiamonds, 1000);

            Int64 temp = 0;
            for (Int32 i = tCount; i > -1; i--)
            {
                temp = refinedAstralDiamonds;
                for (Int32 j = 0; j < i; j++)
                {
                    temp /= 1000;
                }

                Int64 subTmp = temp;
                for (Int32 j = 0; j < i; j++)
                {
                    subTmp *= 1000;
                }
                refinedAstralDiamonds -= subTmp;

                Int32 hCount = SplitNumber(temp, 10);

                if (i == tCount)
                    retStr = String.Format("{0},", temp);
                else
                {
                    if (hCount == 0)
                        retStr = String.Format("{0}00{1},", retStr, temp);
                    else if (hCount == 1)
                        retStr = String.Format("{0}0{1},", retStr, temp);
                    else
                        retStr = String.Format("{0}{1},", retStr, temp);
                }
            }

            if ((retStr.LastIndexOf(",") + 1) == retStr.Length)
                retStr = retStr.Remove(retStr.LastIndexOf(","));

            return retStr;
        }

        private void NWO_Navigate(String url)
        {
            if (_NWO_Account.NWO_DevLogging)
                AddText("Entering NWO_Navigate");

            if (NWO_WebBrowser.InvokeRequired)
            {
                #if !DEBUG
                NWO_Defines.Delegates.NWO_CallbackWebBrowserText navi = new NWO_Defines.Delegates.NWO_CallbackWebBrowserText(NWO_URLs.NWO_Navigate);
                this.Invoke(navi, new object[] { NWO_WebBrowser, url });
                #endif
            }
            else
            {
                #if !DEBUG
                NWO_URLs.NWO_Navigate(NWO_WebBrowser, url);
                #endif
                
                NWO_URL_StatusLabel.Text = url;
            }

            if (_NWO_Account.NWO_DevLogging)
                AddText("Exiting NWO_Navigate");
        }

        private void NWO_AutoProfession_Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Write Out Config
            _NWO_Account.WriteToConfig();

            NWO_IsRunning = false;
            _NWO_Account.NWO_IsRunning = false;
            _NWO_isLoggedIn = false;
            if (_NWO_MainThread != null)
            {
                if (_NWO_MainThread.IsAlive)
                    _NWO_MainThread.Abort();

                _NWO_MainThread = null;
            }

            if (_NWO_NotifyIcon != null)
            {
                _NWO_NotifyIcon.Dispose();
            }
        }

        private void NWO_Config_Button_Click(object sender, EventArgs e)
        {
            if (_NWO_Account.NWO_DevLogging)
                AddText("Entering Config Button Clicked");

            NWO_Config config = new NWO_Config();
            config.NWO_AccountInfo = _NWO_Account;
            config.NWO_IsRunning = NWO_IsRunning;

            if ( config.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _NWO_Account = config.NWO_AccountInfo;
                _NWO_Account.WriteToConfig();

                AddNewTabs();

                if (NWO_IsRunning)
                    StartBotSelling();

                SetSystemTrayText();
            }

            if (_NWO_Account.NWO_DevLogging)
                AddText("Exiting Config Button Clicked");
        }

        private void NWO_ProfessionConfig_Button_Click(object sender, EventArgs e)
        {
            if (_NWO_Account.NWO_DevLogging)
                AddText("Entering Profession Config Button Clicked");

            NWO_ProfessionsConfig profConfig = new NWO_ProfessionsConfig();

            profConfig.CurVersion = _NWO_Account.NWO_Version;
            profConfig.AutoProfessionFileLoc = _NWO_Account.NWO_AutoProfessionFileLoc;
            profConfig.CharacterList = _NWO_Account.NWO_CharacterList;

            if (profConfig.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _NWO_Account.NWO_CharacterList = profConfig.CharacterList;
                _NWO_Account.NWO_AutoProfessionFileLoc = profConfig.AutoProfessionFileLoc;
                _NWO_Account.WriteToConfig();
            }

            if (_NWO_Account.NWO_DevLogging)
                AddText("Exiting Profession Config Button Clicked");
        }

        private void NWO_WebBrowser_DocumentCompleted(object sender, EventArgs e)
        {
            //_NWO_PageLoaded = true;
            NWO_Start_Button.Enabled = true;
            _NWO_NotifyIcon.ContextMenu.MenuItems[0].Enabled = true;
        }

        private Int32 GetTaskDuration(String duration)
        {
            if (_NWO_Account.NWO_DevLogging)
                AddText("Entering Get Task Duration");

            Int32 retValue = 0;

            String sub;
            Int32 index = duration.IndexOf("h");
            if (index > 0)
            {
                sub = duration.Substring(0, index);
                duration = duration.Remove(0, index + 1);
                retValue += ((Convert.ToInt32(sub) * 60) * 60);
            }
            index = duration.IndexOf("m");
            if (index > 0)
            {
                sub = duration.Substring(0, index);
                duration = duration.Remove(0, index + 1);
                retValue += (Convert.ToInt32(sub) * 60);
            }
            index = duration.IndexOf("s");
            if (index > 0)
            {
                sub = duration.Substring(0, index);
                duration = duration.Remove(0, index + 1);
                retValue += Convert.ToInt32(sub);
            }

            if (_NWO_Account.NWO_DevLogging)
                AddText("Exiting Get Task Duration");

            return retValue;
        }

        private void WaitForPageToLoad()
        {
            if (_NWO_Account.NWO_DevLogging)
                AddText("Starting Wait for Page to Load");

            if (!_NWO_PageLoaded)
            {
                int count = 0;
                while (!_NWO_PageLoaded && NWO_IsRunning)
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText(String.Format("Waiting for Page To Load: {0}", count));

                    Thread.Sleep(1000); // Sleep for 1 Second.

                    if (count >= _NWO_Account.NWO_WaitTimePageLoad) // Wait for X secounds.
                        _NWO_PageLoaded = true;
                    else
                        count++;
                }
            }
         
            _NWO_PageLoaded = false;

            if (_NWO_Account.NWO_DevLogging)
                AddText("Leaving Wait for Page to Load");
        }

        private void WaitForMaintenance()
        {
            if (_NWO_Account.NWO_DevLogging)
                AddText("Starting Wait for Maintenance");

            int tempMaint = _NWO_Account.NWO_WaitTimeMantenance * 60;  // 30 minutes.
            if (_NWO_Account.NWO_DevLogging)
                AddText(String.Format("Waiting for Maintenance: {0} seconds", tempMaint));

            while (tempMaint > 0 && NWO_IsRunning)
            {
                Thread.Sleep(1000);

                tempMaint--;
            }
            if (_NWO_Account.NWO_DevLogging)
                AddText("Leaving Waiting for Maintenance.");
        }

        private void WaitForNetworkConnection()
        {
            if (_NWO_Account.NWO_DevLogging)
                AddText("Starting Wait for Network Connection");

            if (_NWO_Account.NWO_DevLogging)
                AddText("Waiting for Network Connection until it comes back, or we stop running.");

            while (NWO_IsRunning && !_NWO_HasNetwork)
            {
                Thread.Sleep(1000);
            }
            if (_NWO_Account.NWO_DevLogging)
                AddText("Leaving Waiting for Network Connection.");
        }

        private void WaitForLoginRetry()
        {
            if (_NWO_Account.NWO_DevLogging)
                AddText("Starting Wait for Login Retry");

            int tempRetry = _NWO_Account.NWO_WaitTimeForRetry * 60;
            while (tempRetry > 0 && NWO_IsRunning)
            {
                //if (_NWO_Account.NWO_DevLogging)
                //    AddText(String.Format("Waiting for Login Retry: {0}", tempRetry));

                Thread.Sleep(1000);

                tempRetry--;
            }
            if (_NWO_Account.NWO_DevLogging)
                AddText("Leaving Waiting for Login Retry.");
        }

        private void WaitForNextCheck()
        {
            if (_NWO_Account.NWO_DevLogging)
                AddText("Starting Wait for Next Check");

            ClearInfoBrowser();

            _NWO_WaitingToCheck = true;
            int timeToWait = 0;
            Boolean autoSelected = false;
            if (_NWO_Account.NWO_AutoCalcWaitTime)
                autoSelected = true;

            timeToWait = _NWO_Account.NWO_WaitTimeBetweenChecks;

            if (_NWO_Account.NWO_DevLogging)
                AddText(String.Format("{0} : Calculated wait time.", TaskDurationTimeString(timeToWait)));

            int count = 0; 
            while (timeToWait > 0 && NWO_IsRunning)
            {
                Thread.Sleep(1000);

                if (_NWO_Account.NWO_TaskComplete)
                    timeToWait = 0;
                else
                    timeToWait--;
                
                if (_NWO_Account.NWO_DevLogging)
                {
                    count++;
                    if (count > 300)
                    {
                        AddText(String.Format("Still Running, we have {0} left till next check.", TaskDurationTimeString(timeToWait)));
                        
                        count = 0;
                    }
                }

                if (!_NWO_Account.NWO_AutoCalcWaitTime && autoSelected)
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Switched from Autocalculating wait time to normal.");
                    if (timeToWait > _NWO_Account.NWO_WaitTimeBetweenChecks)
                        timeToWait = _NWO_Account.NWO_WaitTimeBetweenChecks;
                    autoSelected = false;
                }

                if (timeToWait == 0 && _NWO_BotSellingItems)
                    timeToWait++;
            }

            _NWO_WaitingToCheck = false;

            if (_NWO_Account.NWO_DevLogging)
                AddText("Leaving Waiting for Next Check.");
        }

        private void WaitForBotSelling()
        {
            if (_NWO_Account.NWO_ForBotting)
            {
                do
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Starting Timed Wait for Selling while Botting");

                    _NWO_BotSellingItems = false;

                    int waitTime = _NWO_Account.NWO_BottingWaitToSellItems * 60;
                    while (waitTime > 0 && NWO_IsRunning)
                    {
                        Thread.Sleep(1000);

                        waitTime--;

                        if (waitTime == 0 && !_NWO_WaitingToCheck)
                            waitTime++;
                    }

                    _NWO_BotSellingItems = true;

                    if (NWO_IsRunning && _NWO_isLoggedIn && _NWO_WaitingToCheck )
                        BotSelling();

                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Ending Timed Wait for Selling while Botting");
                } while (NWO_IsRunning);
            }
        }

        private void WaitForCharacterSelectScreen()
        {
            if (_NWO_Account.NWO_DevLogging)
                AddText("Starting Wait for Character Select to Load");

            if (!_NWO_PageLoaded)
            {
                int count = 0;
                int waitAmount = 8;
                while (!_NWO_PageLoaded && NWO_IsRunning)
                {
                    //if (_NWO_Account.NWO_DevLogging)
                    //    AddText(String.Format("Waiting for Character Select To Load: {0}", count));

                    Thread.Sleep(1000); // Sleep for 1 Second.

                    if (count >= waitAmount) // Wait for X secounds.
                        _NWO_PageLoaded = true;
                    else
                        count++;
                }
            }

            _NWO_PageLoaded = false;

            if (_NWO_Account.NWO_DevLogging)
                AddText("Leaving Wait for Character Select to Load");
        }

        private void RefineAstralDiamonds()
        {
            if (_NWO_Account.NWO_DevLogging)
                AddText("Starting Refine Astral Diamonds");
            else
                AddText("Additional Task: Refine Astral Diamnods");

            if (_NWO_Account.NWO_DevLogging)
                AddText("Checking to see if we are on Inventory Bags Page.");

            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bCheckOnInventoryBagScreen = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.CheckForInventoryBagScreen);
            Boolean bOnInventoryBagScreen = (Boolean)this.Invoke(bCheckOnInventoryBagScreen, new object[] { NWO_WebBrowser });

            if (!bOnInventoryBagScreen)
            {
                // Navigate to Inventory Bag Screen.
                if (_NWO_Account.NWO_DevLogging)
                    AddText("Not on Bag Screen, Navigating to Bag Screen.");

                NWO_Navigate(NWO_URLs.NWO_Inventory_URL);

                WaitForPageToLoad();

                if (_NWO_Account.NWO_DevLogging)
                    AddText("Checking to see if we are on the Correct Screen.");

                bOnInventoryBagScreen = (Boolean)this.Invoke(bCheckOnInventoryBagScreen, new object[] { NWO_WebBrowser });
                if (!bOnInventoryBagScreen)
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Still not on Bag Screen.. What happen did we disconnect???");
                    return;
                }
            }

            if (_NWO_Account.NWO_DevLogging)
                AddText("Checking to see if the Refine button is enabled.");

            // Check to see if Refine button is enabled.
            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bCheckRefineButtonEnabled = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.CheckRefineAstralDiamondsButtonEnabled);
            Boolean bRefineButtonEnabled = (Boolean)this.Invoke(bCheckRefineButtonEnabled, new object[] { NWO_WebBrowser });
            
            if (!bRefineButtonEnabled)
            {
                AddText("Refine Button is Disabled. Trying again later.");
                return;
            }

            if (_NWO_Account.NWO_DevLogging)
                AddText("Refine Button is Enabled, Lets get how many Diamonds we are going to refine.");

            NWO_Defines.Delegates.NWO_Int64CallbackWebBrowser getNumRoughDiamonds = new NWO_Defines.Delegates.NWO_Int64CallbackWebBrowser(NWO_InventoryScreen.GetNumRoughAstralDiamonds);
            Int64 numRoughDiamonds = (Int64)this.Invoke(getNumRoughDiamonds, new object[] { NWO_WebBrowser });

            if (numRoughDiamonds < -1)
            {
                if (_NWO_Account.NWO_DevLogging)
                    AddText("Wasn't able to get the number of Rough Astral Diamonds.");
            }

            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bClickRefineButton = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.ClickRefineAstralDiamonds);
            Boolean bClickedRefineButton = (Boolean)this.Invoke(bClickRefineButton, new object[] { NWO_WebBrowser });

            if (_NWO_Account.NWO_DevLogging)
            {
                if (bClickedRefineButton)
                    AddText("Refine Button Clicked.");
                else
                    AddText("Failed in Clicking Refine Button.");
            }

            if (bClickedRefineButton)
            {
                if ((_NWO_Account.NWO_VerboseLogging || _NWO_Account.NWO_DevLogging) && numRoughDiamonds > -1)
                    AddText(String.Format("Refined {0} Rough Diamonds", numRoughDiamonds));

                if (numRoughDiamonds > -1)
                {
                    UpdateRoughAstralDiamondsRefined(_NWO_LoggedInChar, numRoughDiamonds);
                }
            }
        }

        private void BotSelling()
        {
            if (_NWO_Account.NWO_ForBotting && _NWO_Account.NWO_BottingCharacter != String.Empty)
            {
                String curSell = "Bot Selling.";

                if (_NWO_Account.NWO_DevLogging)
                    AddText(String.Format("Starting {0}'s", curSell));
                else
                    AddText(String.Format("Additional Task: {0}'s", curSell));

                if (_NWO_Account.NWO_DevLogging)
                    AddText("Check to see if we are disconnected.");

                Boolean connected = Disconnected();
                if (connected)
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("We were disconnected from the gateway. Trying to login again.");
                    AddText("Disconnected from gateway.");

                    connected = ClickDisconnectButton();

                    if (!connected)
                    {
                        AddText("Unable to click Disconnected OK button. Stopping Bot.");
                        NWO_IsRunning = false;
                        return;
                    }
                }

                if (_NWO_Account.NWO_DevLogging)
                    AddText("Checking to see if we are on the Correct Character that is botting.");

                if (!NWO_IsRunning)
                    return;

                if (!SwitchCharacters(_NWO_Account.NWO_BottingCharacter))
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText(String.Format("Was Unable to switch characters.  Stopping {0}.", curSell));
                    return;
                }
                else
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText(String.Format("Successfully switched to {0}.", _NWO_Account.NWO_BottingCharacter));
                }

                if (_NWO_Account.NWO_DevLogging)
                    AddText("Checking to see if we are on Inventory Bags Page.");

                if (!NWO_IsRunning)
                    return;

                NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bCheckOnInventoryBagScreen = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.CheckForInventoryBagScreen);
                Boolean bOnInventoryBagScreen = (Boolean)this.Invoke(bCheckOnInventoryBagScreen, new object[] { NWO_WebBrowser });

                if (!bOnInventoryBagScreen)
                {
                    // Navigate to Inventory Bag Screen.
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Not on Bag Screen, Navigating to Bag Screen.");

                    NWO_Navigate(NWO_URLs.NWO_Inventory_URL);

                    WaitForPageToLoad();

                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Checking to see if we are on the Correct Screen.");

                    bOnInventoryBagScreen = (Boolean)this.Invoke(bCheckOnInventoryBagScreen, new object[] { NWO_WebBrowser });
                    if (!bOnInventoryBagScreen)
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Still not on Bag Screen.. What happen did we disconnect???");
                        return;
                    }
                }

                NWO_Defines.Delegates.NWO_BoolCallbacknsIDOMNode clickButton;
                NWO_Defines.Delegates.NWO_DivEleCallBackWebBrowser getSellItem;
                GeckoDivElement itemToSell = null;

                do
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText(String.Format("Getting {0} from Inventory.", curSell));

                    if (!NWO_IsRunning)
                        return;

                    // Get the items in our inventory.
                    getSellItem = new NWO_Defines.Delegates.NWO_DivEleCallBackWebBrowser(NWO_InventoryScreen.GetBotItem);
                    itemToSell = (GeckoDivElement)this.Invoke(getSellItem, new object[] { NWO_WebBrowser });

                    if (itemToSell == null)
                        break;

                    if (_NWO_Account.NWO_DevLogging)
                        AddText(String.Format("Found {0}.  Attempting to Select the item.", curSell));

                    if (!NWO_IsRunning)
                        return;

                    clickButton = new NWO_Defines.Delegates.NWO_BoolCallbacknsIDOMNode(NWO_InventoryScreen.ClickButton);
                    Boolean buttonClicked = (Boolean)this.Invoke(clickButton, new object[] { itemToSell.DomObject });
                    if (!buttonClicked)
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            AddText(String.Format("Was not able to select {0}. Not sure what happen. Exiting Sell Junk.", curSell));

                        break;
                    }

                    if (_NWO_Account.NWO_DevLogging)
                        AddText(String.Format("{0} Button Clicked. Checking to make sure we are on the item select screen.", curSell));

                    WaitForPageToLoad();

                    if (!NWO_IsRunning)
                        return;

                    NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser checkOnItemSelectScreen = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.CheckForItemSelectScreen);
                    Boolean onItemSelectScreen = (Boolean)this.Invoke(checkOnItemSelectScreen, new object[] { NWO_WebBrowser });
                    if (!onItemSelectScreen)
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            AddText(String.Format("Not on Item Select Screen Something Happen. Exiting Sell {0}'s.", curSell));

                        break;
                    }

                    if (_NWO_Account.NWO_DevLogging)
                        AddText("On Item Select Screen. Trying to press Sell To Vendor Button.");

                    if (!NWO_IsRunning)
                        return;

                    NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bClickSellToVendorButton = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.ClickSellToVendorButton);
                    Boolean bSellToVendorButtonClicked = (Boolean)this.Invoke(bClickSellToVendorButton, new object[] { NWO_WebBrowser });
                    if (!bSellToVendorButtonClicked)
                    {
                        if (NWO_InventoryScreen.ErrorMessage != String.Empty)
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Sell to Vendor Button is disabled.  Move to next Item.");

                            //TODO: Need to press the back button.
                            //_NWO_Navigate(NWO_URLs.NWO_Inventory_URL);

                            WaitForPageToLoad();

                            continue;
                        }
                        else
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText(String.Format("Not able to click Sell to Vendor Button. Exiting Sell {0}'s.", curSell));

                            break;
                        }
                    }

                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Clicked Sell to Vendor Button. Checking for Item Sell Qty text box.");

                    WaitForPageToLoad();

                    if (!NWO_IsRunning)
                        return;

                    NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bCheckForItemQtyTextBox = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.CheckForInventorySellQuantityBox);
                    Boolean bItemQtyTextBoxFound = (Boolean)this.Invoke(bCheckForItemQtyTextBox, new object[] { NWO_WebBrowser });
                    if (bItemQtyTextBoxFound)
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Item Sell Qty Text Box found. Setting it to Maximum Value.");

                        NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bSetSellQtyMax = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.SetInventorySellQuantityBox);
                        Boolean bSellQtySetMax = (Boolean)this.Invoke(bSetSellQtyMax, new object[] { NWO_WebBrowser });

                        if (bSellQtySetMax)
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Item Sell Qty Text Box Set to Max Value.");
                        }
                        else
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Item Sell Qty Text Box Not Set to Max Value.");
                        }
                    }

                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Pressing the Confirm Button.");

                    if (!NWO_IsRunning)
                        return;

                    NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bClickConfirmButton = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.ClickConfirmButton);
                    Boolean bConfirmButtonClicked = (Boolean)this.Invoke(bClickConfirmButton, new object[] { NWO_WebBrowser });
                    if (!bConfirmButtonClicked)
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            AddText(String.Format("Was Unable to click Confirm Button. Exiting Sell {0}'s.", curSell));

                        break;
                    }

                    if (_NWO_Account.NWO_DevLogging)
                        AddText(String.Format("{0} was Sold.", curSell));

                    WaitForPageToLoad();

                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Checking for Message Box saying we sold item.");

                    NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser checkSoldMessageBox = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.CheckMessageBox);
                    Boolean bSoldMessageBox = (Boolean)this.Invoke(checkSoldMessageBox, new object[] { NWO_WebBrowser });

                    if (bSoldMessageBox)
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Sold Message Box Found. Trying to Close.");

                        NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser closeMessageBox = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.ClickMessageBox);
                        Boolean bMessageBoxClosed = (Boolean)this.Invoke(closeMessageBox, new object[] { NWO_WebBrowser });

                        if (bMessageBoxClosed)
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Message Box Closed.");
                        }
                    }

                } while (itemToSell != null);

                if (_NWO_Account.NWO_DevLogging)
                    AddText(String.Format("Done Selling {0}'s. Exiting Sell {0}'s.", curSell));
                else
                    AddText(String.Format("Additional Task: Sell {0}'s Done", curSell));
            }
        }

        private void SellItems(Boolean bJunk, Boolean bNonMagical, Boolean bMagicalNonClass, Boolean bOpenChests)
        {
            if (!bJunk && !bNonMagical && !bMagicalNonClass && !bOpenChests)
                return;

            String curSell = String.Empty;
            //if (bJunk)
            //    curSell = "Junk Item";
            //else if (bNonMagical)
            //    curSell = "Non-Magical Equipment";
            //else if (bMagicalNonClass)
            //    curSell = "Magical Non-Class Equipment";
            //else if (bOpenChests)
            //    curSell = "Open Chests";

            AddText("Starting Additional Tasks:");

            if (_NWO_Account.NWO_DevLogging)
                AddText("Checking to see if we are on Inventory Bags Page.");

            if (_NWO_Account.NWO_DevLogging)
                AddText("Check to see if we are disconnected.");

            #region Check for Disconnect
            Boolean connected = Disconnected();
            if (connected)
            {
                if (_NWO_Account.NWO_DevLogging)
                    AddText("We were disconnected from the gateway. Trying to login again.");
                AddText("Disconnected from gateway.");

                connected = ClickDisconnectButton();

                if (!connected)
                {
                    AddText("Unable to click Disconnected OK button. Stopping Bot.");
                    NWO_IsRunning = false;
                    return;
                }
            }

            if (!NWO_IsRunning)
                return;
            #endregion

            #region Check on Inventory Screen
            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bCheckOnInventoryBagScreen = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.CheckForInventoryBagScreen);
            Boolean bOnInventoryBagScreen = (Boolean)this.Invoke(bCheckOnInventoryBagScreen, new object[] { NWO_WebBrowser });

            if (!bOnInventoryBagScreen)
            {
                // Navigate to Inventory Bag Screen.
                if (_NWO_Account.NWO_DevLogging)
                    AddText("Not on Bag Screen, Navigating to Bag Screen.");

                NWO_Navigate(NWO_URLs.NWO_Inventory_URL);

                WaitForPageToLoad();

                if (!NWO_IsRunning)
                    return;

                if (_NWO_Account.NWO_DevLogging)
                    AddText("Checking to see if we are on the Correct Screen.");

                bOnInventoryBagScreen = (Boolean)this.Invoke(bCheckOnInventoryBagScreen, new object[] { NWO_WebBrowser });
                if (!bOnInventoryBagScreen)
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Still not on Bag Screen.. What happen did we disconnect???");
                    return;
                }
            }
            #endregion

            NWO_Defines.Delegates.NWO_BoolCallbacknsIDOMNode clickButton;
            NWO_Defines.Delegates.NWO_DivEleCallBackWebBrowserDivElement getSellItem;
            GeckoDivElement itemToSell = null;

            do
            {
                if (_NWO_Account.NWO_DevLogging)
                    AddText("Getting Item from Inventory.");

                if (!NWO_IsRunning)
                    return;

                // Get the items in our inventory.
                //if (bOpenChests)
                //    getSellItem = new NWO_Defines.Delegates.NWO_DivEleCallBackWebBrowserDivElement(NWO_InventoryScreen.GetOpenableChests);
                //else if (bNonMagical)
                //    getSellItem = new NWO_Defines.Delegates.NWO_DivEleCallBackWebBrowserDivElement(NWO_InventoryScreen.GetNonMagicalItem);
                //else // Default will be junk
                //    getSellItem = new NWO_Defines.Delegates.NWO_DivEleCallBackWebBrowserDivElement(NWO_InventoryScreen.GetJunkItem);
                NWO_InventoryScreen.SellJunk = bJunk;
                NWO_InventoryScreen.SellNonMagical = bNonMagical;
                NWO_InventoryScreen.SellMagicalNonClass = bMagicalNonClass;
                NWO_InventoryScreen.OpenBoxes = bOpenChests;

                getSellItem = new NWO_Defines.Delegates.NWO_DivEleCallBackWebBrowserDivElement(NWO_InventoryScreen.GetItem);
                itemToSell = (GeckoDivElement)this.Invoke(getSellItem, new object[] { NWO_WebBrowser, itemToSell });

                if (itemToSell == null)
                    break;

                if (_NWO_Account.NWO_DevLogging)
                    AddText("Found Item.  Attempting to Select the item.");

                if (!NWO_IsRunning)
                    return;

                clickButton = new NWO_Defines.Delegates.NWO_BoolCallbacknsIDOMNode(NWO_InventoryScreen.ClickButton);
                Boolean buttonClicked = (Boolean)this.Invoke(clickButton, new object[] { itemToSell.DomObject });
                if (!buttonClicked)
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Was not able to select Item. Not sure what happen. Exiting Sell Items.");

                    break;
                }

                if (_NWO_Account.NWO_DevLogging)
                    AddText("Item Button Clicked. Checking to make sure we are on the item's screen.");

                WaitForPageToLoad();

                if (!NWO_IsRunning)
                    return;

                NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser checkOnItemSelectScreen = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.CheckForItemSelectScreen);
                Boolean onItemSelectScreen = (Boolean)this.Invoke(checkOnItemSelectScreen, new object[] { NWO_WebBrowser });
                if (!onItemSelectScreen)
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Not on Item Select Screen Something Happen. Exiting Additional Tasks.");

                    break;
                }

                if (!NWO_IsRunning)
                    return;

                if (NWO_InventoryScreen.SellJunk || NWO_InventoryScreen.SellMagicalNonClass || NWO_InventoryScreen.SellNonMagical)
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("On Item Select Screen. Trying to press Sell To Vendor Button.");

                    NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bClickSellToVendorButton = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.ClickSellToVendorButton);
                    Boolean bSellToVendorButtonClicked = (Boolean)this.Invoke(bClickSellToVendorButton, new object[] { NWO_WebBrowser });
                    if (!bSellToVendorButtonClicked)
                    {
                        if (NWO_InventoryScreen.ErrorMessage != String.Empty)
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Sell to Vendor Button is disabled.  Move to next Item.");

                            //TODO: Need to press the back button.
                            //_NWO_Navigate(NWO_URLs.NWO_Inventory_URL);

                            WaitForPageToLoad();

                            continue;
                        }
                        else
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Not able to click Sell to Vendor Button. Exiting Additional Tasks.");

                            break;
                        }
                    }

                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Clicked Sell to Vendor Button. Checking for Item Sell Qty text box.");

                    WaitForPageToLoad();

                    if (!NWO_IsRunning)
                        return;

                    NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bCheckForItemQtyTextBox = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.CheckForInventorySellQuantityBox);
                    Boolean bItemQtyTextBoxFound = (Boolean)this.Invoke(bCheckForItemQtyTextBox, new object[] { NWO_WebBrowser });
                    if (bItemQtyTextBoxFound)
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Item Sell Qty Text Box found. Setting it to Maximum Value.");

                        NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bSetSellQtyMax = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.SetInventorySellQuantityBox);
                        Boolean bSellQtySetMax = (Boolean)this.Invoke(bSetSellQtyMax, new object[] { NWO_WebBrowser });

                        if (bSellQtySetMax)
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Item Sell Qty Text Box Set to Max Value.");
                        }
                        else
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Item Sell Qty Text Box Not Set to Max Value.");
                        }
                    }

                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Pressing the Confirm Button.");

                    if (!NWO_IsRunning)
                        return;

                    NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bClickConfirmButton = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.ClickConfirmButton);
                    Boolean bConfirmButtonClicked = (Boolean)this.Invoke(bClickConfirmButton, new object[] { NWO_WebBrowser });
                    if (!bConfirmButtonClicked)
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Was Unable to click Confirm Button. Exiting Additional Tasks.");

                        break;
                    }

                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Item was Sold.");

                    WaitForPageToLoad();

                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Checking for Message Box saying we sold item.");

                    NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser checkSoldMessageBox = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.CheckMessageBox);
                    Boolean bSoldMessageBox = (Boolean)this.Invoke(checkSoldMessageBox, new object[] { NWO_WebBrowser });

                    if (bSoldMessageBox)
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Sold Message Box Found. Trying to Close.");

                        NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser closeMessageBox = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.ClickMessageBox);
                        Boolean bMessageBoxClosed = (Boolean)this.Invoke(closeMessageBox, new object[] { NWO_WebBrowser });

                        if (bMessageBoxClosed)
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Message Box Closed.");
                        }
                    }

                }
                else if (NWO_InventoryScreen.OpenBoxes)
                {
                    NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bClickOpenButton = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.ClickOpenButton);
                    Boolean bOpenClicked = (Boolean)this.Invoke(bClickOpenButton, new object[] { NWO_WebBrowser });

                    if (!bOpenClicked)
                    {
                        if (NWO_InventoryScreen.ErrorMessage != String.Empty)
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Open Button is disabled.  Move to next Item.");

                            //TODO: Need to press the back button.
                            //_NWO_Navigate(NWO_URLs.NWO_Inventory_URL);

                            WaitForPageToLoad();

                            continue;
                        }
                        else
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText(String.Format("Not able to click Open Button. Exiting {0}", curSell));

                            break;
                        }
                    }

                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Open Button Clicked.");

                    WaitForPageToLoad();

                    NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bClickConfirmButton = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.ClickConfirmButton);
                    Boolean bConfirmButtonClicked = (Boolean)this.Invoke(bClickConfirmButton, new object[] { NWO_WebBrowser });
                    if (!bConfirmButtonClicked)
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            AddText(String.Format("Was Unable to click Confirm Button. Exiting {0}.", curSell));

                        break;
                    }

                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Confirm Button Clicked.");

                    WaitForPageToLoad();
                    
                    // Need to press OK button here.
                    NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bClickOKButton = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_InventoryScreen.ClickOkButton);
                    bConfirmButtonClicked = (Boolean)this.Invoke(bClickOKButton, new object[] { NWO_WebBrowser });
                    if (!bConfirmButtonClicked)
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            AddText(String.Format("Was Unable to click OK Button. Exiting {0}.", curSell));

                        break;
                    }

                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Ok Button Was Clicked.");
                    
                    WaitForPageToLoad();

                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Chest was Open.");
                }

                WaitForPageToLoad();

            } while (itemToSell != null);

            AddText("Additional Tasks Done");
        }

        private Int32 GetProfessionLevel(String curProfession)
        {
            NWO_Defines.Delegates.NWO_Int32CallbackWebBrowserString profLevels = new NWO_Defines.Delegates.NWO_Int32CallbackWebBrowserString(NWO_ProfessionScreen.GetProfessionLevel);
            return (Int32)this.Invoke(profLevels, new object[] { NWO_WebBrowser, curProfession });
        }

        private Boolean NWO_IsRunning
        {
            get
            {
                return _NWO_isRunning;
            }
            set
            {
                _NWO_isRunning = value;
                _NWO_Account.NWO_IsRunning = value;
            }
        }

        private Boolean Undermaintenance()
        {
            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser underMain = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_LoginScreen.CheckUndergoingMaintenance);
            return (Boolean)this.Invoke(underMain, new object[] { NWO_WebBrowser });
        }

        private Boolean Disconnected()
        {
            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser checkDiscon = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_LoginScreen.CheckForDisconnectScreen);
            return (Boolean)this.Invoke(checkDiscon, new object[] { NWO_WebBrowser });
        }

        private Boolean ClickDisconnectButton()
        {
            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser clickDiscon = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_LoginScreen.ClickDisconnectScreen);
            return (Boolean)this.Invoke(clickDiscon, new object[] { NWO_WebBrowser });
        }

        private Boolean LoginScreen()
        {
            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser NWO_CallbackWB = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_LoginScreen.CheckForLoginScreen);
            return (Boolean)this.Invoke(NWO_CallbackWB, new object[] { NWO_WebBrowser });
        }

        private Boolean CharacterSelectScreen()
        {
            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser checkCharSelect = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_CharacterSelect.CheckForCharacterSelectScreen);
            return (Boolean)this.Invoke(checkCharSelect, new object[] { NWO_WebBrowser });
        }

        private Boolean Login()
        {
            if (_NWO_Account.NWO_DevLogging)
                AddText("Attempthing to login with AutoLogin.");
            else if (_NWO_Account.NWO_VerboseLogging)
                AddText("Logining In.");

            // Login
            if (_NWO_Account.NWO_UserName == String.Empty)
            {
                if (_NWO_Account.NWO_DevLogging)
                    AddText("Username is empty.");

                AddText("Invalid Username. Please check to make sure Username is correct.");
                NWO_IsRunning = false;
                return false;
            }

            if (_NWO_Account.NWO_Password == String.Empty)
            {
                if (_NWO_Account.NWO_DevLogging)
                    AddText("Password is empty.");
                AddText("Invalid Password. Please check to make sure Password is not Empty.");
                NWO_IsRunning = false;
                return false;
            }

            if ( _NWO_Account.NWO_DevLogging )
                AddText("Attempting to input Username.");

            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowserText LoginInfo = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowserText(NWO_LoginScreen.SetUserText);
            Boolean LoginSuccess = (Boolean)this.Invoke( LoginInfo, new object[] { NWO_WebBrowser, _NWO_Account.NWO_UserName });

            if (!LoginSuccess)
            {
                if (_NWO_Account.NWO_DevLogging)
                    AddText(NWO_LoginScreen.ErrorMessage);
                else
                    AddText("Unable to find Username textbox on login screen.");
                NWO_IsRunning = false;
                return false;
            }

            if (_NWO_Account.NWO_DevLogging)
            {
                AddText("Username added to textbox successfully.");
                AddText("Attempting to input Password.");
            }

            LoginInfo = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowserText(NWO_LoginScreen.SetPasswordText);
            LoginSuccess = (Boolean)this.Invoke(LoginInfo, new object[] { NWO_WebBrowser, _NWO_Account.NWO_Password });

            if (!LoginSuccess)
            {
                if (_NWO_Account.NWO_DevLogging)
                    AddText(NWO_LoginScreen.ErrorMessage);
                else
                    AddText("Unable to find Password Textbox on Login Screen.");
                NWO_IsRunning = false;
                return false;
            }

            if (_NWO_Account.NWO_DevLogging)
            {
                AddText("Password added to textbox successfully.");
                AddText("Attempting to click login");
            }

            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser loginClick = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_LoginScreen.PressLoginButton);
            LoginSuccess = (Boolean)this.Invoke(loginClick, new object[] { NWO_WebBrowser });

            if (!LoginSuccess)
            {
                if (_NWO_Account.NWO_DevLogging)
                    AddText(NWO_LoginScreen.ErrorMessage);
                else
                    AddText("Unable to find Login Button.");
                NWO_IsRunning = false;
                return false;
            }
                    
            return true;
        }

        private Boolean SwitchCharacters(String charWanted)
        {
            #region Switch Characters
            // Check to see if we are already logged in as someone else.
            // First Check to see if we are on the Character Select Screen.
            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser loginScreenCheck = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_LoginScreen.CheckForLoginScreen);
            Boolean onLoginScreen = (Boolean)this.Invoke(loginScreenCheck, new object[] { NWO_WebBrowser });

            if (!onLoginScreen)
            {
                if (_NWO_Account.NWO_DevLogging)
                    AddText("We are not logged in, need to try and log in again.");
                _NWO_isLoggedIn = false;
                return false;
            }

            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bCharacterSelectScreen = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_CharacterSelect.CheckForCharacterSelectScreen);
            Boolean bOnCharacterSelect = (Boolean)this.Invoke(bCharacterSelectScreen, new object[] { NWO_WebBrowser });

            if (_NWO_LoggedInChar != charWanted || bOnCharacterSelect)
            {
                if (_NWO_LoggedInChar != String.Empty && !bOnCharacterSelect)
                {
                    if (_NWO_Account.NWO_DevLogging)
                    {
                        AddText("Another Character already logged in navigating to character select screen.");
                        AddText("Navigating to the Character Select Screen.");
                    }
                    else
                        AddText(String.Format("Switching Characters to {0}.", charWanted));

                    NWO_Navigate(NWO_URLs.NWO_Character_Select_URL);

                    WaitForCharacterSelectScreen();
                }


                if (!SelectCharacter(charWanted))
                {
                    AddText(String.Format("Unable to Select character {0}", charWanted));
                    _NWO_LoggedInChar = String.Empty;
                    return false;
                }

                SwitchToCurrentCharacterTab();

                WaitForPageToLoad();
            }
            #endregion

            return true;
        }

        private Boolean SelectCharacter(String charWanted)
        {
            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowserText selectCharClick = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowserText(NWO_CharacterSelect.PressCharacterButton);
            Boolean charSelected = (Boolean)this.Invoke(selectCharClick, new object[] { NWO_WebBrowser, charWanted });

            if (charSelected)
            {
                if (_NWO_Account.NWO_DevLogging)
                    AddText("Character selection successful.");
                
                _NWO_LoggedInChar = charWanted;

                return true;
            }
            else
            {
                if (_NWO_Account.NWO_DevLogging)
                    AddText("Character selection unsuccessful.");

                return false;
            }
        }

        private Boolean GetTaskAvailable()
        {
            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser checkStartTask = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_ProfessionScreen.CheckStartTask);
            return (Boolean)this.Invoke(checkStartTask, new object[] { NWO_WebBrowser });
        }

        private Boolean GetTaskDuration()
        {
            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser getTaskDuration = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_ProfessionScreen.GetTaskDuration);
            return (Boolean)this.Invoke(getTaskDuration, new object[] { NWO_WebBrowser });
        }

        private Boolean PressStartTask(String taskURL)
        {
            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowserText bStartTask = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowserText(NWO_ProfessionScreen.StartTask);
            return (Boolean)this.Invoke(bStartTask, new object[] { NWO_WebBrowser, taskURL });
        }

        private Boolean SelectAssets(NWO_Asset nAsset, NWO_Craftsmen nCraftmen)
        {
            if (_NWO_Account.NWO_DevLogging)
                AddText("Attempting to add Asset.");

            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bCheckOptAssets = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_ProfessionScreen.CheckForOptionalAssets);
            Boolean bOptAssets = (Boolean)this.Invoke(bCheckOptAssets, new object[] { NWO_WebBrowser });
            if (bOptAssets)
            {
                if (_NWO_Account.NWO_DevLogging)
                {
                    AddText("Optional Assets are available.");
                    AddText("Getting number available.");
                }

                NWO_Defines.Delegates.NWO_Int32CallbackWebBrowser iNumOptAssets = new NWO_Defines.Delegates.NWO_Int32CallbackWebBrowser(NWO_ProfessionScreen.GetNumberOptionalAssets);
                Int32 numOptAssets = (Int32)this.Invoke(iNumOptAssets, new object[] { NWO_WebBrowser });

                if (_NWO_Account.NWO_DevLogging)
                    AddText(String.Format("Number of optimal Assets is {0}", numOptAssets));

                NWO_Defines.Delegates.NWO_BoolCallbackWeBrowserInt32 bClickSelectOptAsset = new NWO_Defines.Delegates.NWO_BoolCallbackWeBrowserInt32(NWO_ProfessionScreen.ClickSelectOptionalAssetSpot);
                NWO_Defines.Delegates.NWO_BoolCallbackWebBrowserText bSetAsset = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowserText(NWO_ProfessionScreen.AssetSelect);
                Boolean bClickedSelectOptAsset = false;
                Boolean bAssetSet = false;
                if (numOptAssets > 0)
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("We have got the number optimal assets.");

                    Int32 r1c = 0;
                    Int32 r2c = 0;
                    Int32 r3c = 0;
                    Int32 r3u = 0;
                    Int32 r3r = 0;
                    Int32 r3e = 0;
                    for (Int32 i = 0; i < numOptAssets; i++)
                    {
                        if (nAsset.Rank3Epic)
                        {
                            if (nAsset.Rank3Epic_Quantity > r3e)
                            {
                                 if (_NWO_Account.NWO_DevLogging)
                                     AddText("Rank 3 Epic Asset");

                                 bClickedSelectOptAsset = (Boolean)this.Invoke(bClickSelectOptAsset, new object[] { NWO_WebBrowser, i });
                                 if (bClickedSelectOptAsset)
                                 {
                                     if (_NWO_Account.NWO_DevLogging)
                                         AddText("Optional Asset Button Clicked.");

                                     WaitForPageToLoad();

                                     bAssetSet = (Boolean)this.Invoke(bSetAsset, new object[] { NWO_WebBrowser, nCraftmen.Rank3Epic });
                                     if (bAssetSet)
                                     {
                                         if (_NWO_Account.NWO_DevLogging)
                                             AddText("Rank 3 Epic Asset Selected.");

                                         WaitForPageToLoad();
                                         r3e++;
                                         continue;
                                     }
                                 }                              
                            }
                        }

                        if (nAsset.Rank3Rare)
                        {
                            if (nAsset.Rank3Rare_Quantity > r3r)
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Rank 3 Rare Asset");

                                bClickedSelectOptAsset = (Boolean)this.Invoke(bClickSelectOptAsset, new object[] { NWO_WebBrowser, i });
                                if (bClickedSelectOptAsset)
                                {
                                    if (_NWO_Account.NWO_DevLogging)
                                        AddText("Optional Asset Button Clicked.");

                                    WaitForPageToLoad();

                                    bAssetSet = (Boolean)this.Invoke(bSetAsset, new object[] { NWO_WebBrowser, nCraftmen.Rank3Rare });
                                    if (bAssetSet)
                                    {
                                        if (_NWO_Account.NWO_DevLogging)
                                            AddText("Rank 3 Rare Asset Selected.");

                                        WaitForPageToLoad();
                                        r3r++;
                                        continue;
                                    }
                                }
                            }
                        }

                        if (nAsset.Rank3Uncommon)
                        {
                            if (nAsset.Rank3Uncommon_Quantity > r3u)
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Rank 3 Uncommon Asset");

                                bClickedSelectOptAsset = (Boolean)this.Invoke(bClickSelectOptAsset, new object[] { NWO_WebBrowser, i });
                                if (bClickedSelectOptAsset)
                                {
                                    if (_NWO_Account.NWO_DevLogging)
                                        AddText("Optional Asset Button Clicked.");

                                    WaitForPageToLoad();

                                    bAssetSet = (Boolean)this.Invoke(bSetAsset, new object[] { NWO_WebBrowser, nCraftmen.Rank3Uncommon });
                                    if (bAssetSet)
                                    {
                                        if (_NWO_Account.NWO_DevLogging)
                                            AddText("Rank 3 Uncommon Asset Selected.");

                                        WaitForPageToLoad();
                                        r3u++;
                                        continue;
                                    }
                                }
                            }
                        }

                        if (nAsset.Rank3Common)
                        {
                            if (nAsset.Rank3Common_Quantity > r3c)
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Rank 3 Common Asset");

                                bClickedSelectOptAsset = (Boolean)this.Invoke(bClickSelectOptAsset, new object[] { NWO_WebBrowser, i });
                                if (bClickedSelectOptAsset)
                                {
                                    if (_NWO_Account.NWO_DevLogging)
                                        AddText("Optional Asset Button Clicked.");

                                    WaitForPageToLoad();

                                    bAssetSet = (Boolean)this.Invoke(bSetAsset, new object[] { NWO_WebBrowser, nCraftmen.Rank3Common });
                                    if (bAssetSet)
                                    {
                                        if (_NWO_Account.NWO_DevLogging)
                                            AddText("Rank 3 Common Asset Selected.");

                                        WaitForPageToLoad();
                                        r3c++;
                                        continue;
                                    }
                                }
                            }
                        }

                        if (nAsset.Rank2Common)
                        {
                            if (nAsset.Rank2Common_Quantity > r2c)
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Rank 2 Common Asset");

                                bClickedSelectOptAsset = (Boolean)this.Invoke(bClickSelectOptAsset, new object[] { NWO_WebBrowser, i });
                                if (bClickedSelectOptAsset)
                                {
                                    if (_NWO_Account.NWO_DevLogging)
                                        AddText("Optional Asset Button Clicked.");

                                    WaitForPageToLoad();

                                    bAssetSet = (Boolean)this.Invoke(bSetAsset, new object[] { NWO_WebBrowser, nCraftmen.Rank2Common });
                                    if (bAssetSet)
                                    {
                                        if (_NWO_Account.NWO_DevLogging)
                                            AddText("Rank 2 Common Asset Selected.");

                                        WaitForPageToLoad();
                                        r2c++;
                                        continue;
                                    }
                                }
                            }
                        }

                        if (nAsset.Rank1Common)
                        {
                            if (nAsset.Rank1Common_Quantity > r1c)
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Rank 1 Common Asset");

                                bClickedSelectOptAsset = (Boolean)this.Invoke(bClickSelectOptAsset, new object[] { NWO_WebBrowser, i });
                                if (bClickedSelectOptAsset)
                                {
                                    if (_NWO_Account.NWO_DevLogging)
                                        AddText("Optional Asset Button Clicked.");

                                    WaitForPageToLoad();

                                    bAssetSet = (Boolean)this.Invoke(bSetAsset, new object[] { NWO_WebBrowser, nCraftmen.Rank1Common });
                                    if (bAssetSet)
                                    {
                                        if (_NWO_Account.NWO_DevLogging)
                                            AddText("Rank 1 Common Asset Selected.");

                                        WaitForPageToLoad();
                                        r1c++;
                                        continue;
                                    }
                                }
                            }
                        }
                    }

                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Exiting Select Assets");

                    if (r1c > 0 || r2c > 0 || r3c > 0 || r3u > 0 || r3r > 0 || r3e > 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Did not get the number of optimal assets??? Will just do the first one.");

                    if (nAsset.Rank3Epic)
                    {
                        if (nAsset.Rank3Epic_Quantity > 0)
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Rank 3 Epic Asset");

                            bClickedSelectOptAsset = (Boolean)this.Invoke(bClickSelectOptAsset, new object[] { NWO_WebBrowser, 0 });
                            if (bClickedSelectOptAsset)
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Optional Asset Button Clicked.");
                                
                                WaitForPageToLoad();

                                bAssetSet = (Boolean)this.Invoke(bSetAsset, new object[] { NWO_WebBrowser, nCraftmen.Rank3Epic });
                                if (bAssetSet)
                                {
                                    if (_NWO_Account.NWO_DevLogging)
                                        AddText("Rank 3 Epic Asset Selected.");

                                    WaitForPageToLoad();
                                    return true;
                                }
                            }
                        }
                    }

                    if (nAsset.Rank3Rare)
                    {
                        if (nAsset.Rank3Rare_Quantity > 0)
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Rank 3 Rare Asset");

                            bClickedSelectOptAsset = (Boolean)this.Invoke(bClickSelectOptAsset, new object[] { NWO_WebBrowser, 0 });
                            if (bClickedSelectOptAsset)
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Optional Asset Button Clicked.");
                                
                                WaitForPageToLoad();

                                bAssetSet = (Boolean)this.Invoke(bSetAsset, new object[] { NWO_WebBrowser, nCraftmen.Rank3Rare });
                                if (bAssetSet)
                                {
                                    if (_NWO_Account.NWO_DevLogging)
                                        AddText("Rank 3 Rare Asset Selected.");

                                    WaitForPageToLoad();
                                    return true;
                                }
                            }
                        }
                    }

                    if (nAsset.Rank3Uncommon)
                    {
                        if (nAsset.Rank3Uncommon_Quantity > 0)
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Rank 3 Uncommon Asset");

                            bClickedSelectOptAsset = (Boolean)this.Invoke(bClickSelectOptAsset, new object[] { NWO_WebBrowser, 0 });
                            if (bClickedSelectOptAsset)
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Optional Asset Button Clicked.");
                                
                                WaitForPageToLoad();

                                bAssetSet = (Boolean)this.Invoke(bSetAsset, new object[] { NWO_WebBrowser, nCraftmen.Rank3Uncommon });
                                if (bAssetSet)
                                {
                                    if (_NWO_Account.NWO_DevLogging)
                                        AddText("Rank 3 Uncommon Asset Selected.");

                                    WaitForPageToLoad();
                                    return true;
                                }
                            }
                        }
                    }

                    if (nAsset.Rank3Common)
                    {
                        if (nAsset.Rank3Common_Quantity > 0)
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Rank 3 Common Asset");

                            bClickedSelectOptAsset = (Boolean)this.Invoke(bClickSelectOptAsset, new object[] { NWO_WebBrowser, 0 });
                            if (bClickedSelectOptAsset)
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Optional Asset Button Clicked.");
                                
                                WaitForPageToLoad();

                                bAssetSet = (Boolean)this.Invoke(bSetAsset, new object[] { NWO_WebBrowser, nCraftmen.Rank3Common });
                                if (bAssetSet)
                                {
                                    if (_NWO_Account.NWO_DevLogging)
                                        AddText("Rank 3 Common Asset Selected.");

                                    WaitForPageToLoad();
                                    return true;
                                }
                            }
                        }
                    }

                    if (nAsset.Rank2Common)
                    {
                        if (nAsset.Rank2Common_Quantity > 0)
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Rank 2 Common Asset");

                            bClickedSelectOptAsset = (Boolean)this.Invoke(bClickSelectOptAsset, new object[] { NWO_WebBrowser, 0 });
                            if (bClickedSelectOptAsset)
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Optional Asset Button Clicked.");
                                
                                WaitForPageToLoad();

                                bAssetSet = (Boolean)this.Invoke(bSetAsset, new object[] { NWO_WebBrowser, nCraftmen.Rank2Common });
                                if (bAssetSet)
                                {
                                    if (_NWO_Account.NWO_DevLogging)
                                        AddText("Rank 2 Common Asset Selected.");

                                    WaitForPageToLoad();
                                    return true;
                                }
                            }
                        }
                    }

                    if (nAsset.Rank1Common)
                    {
                        if (nAsset.Rank1Common_Quantity > 0)
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Rank 1 Common Asset");

                            bClickedSelectOptAsset = (Boolean)this.Invoke(bClickSelectOptAsset, new object[] { NWO_WebBrowser, 0 });
                            if (bClickedSelectOptAsset)
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Optional Asset Button Clicked.");

                                WaitForPageToLoad();

                                bAssetSet = (Boolean)this.Invoke(bSetAsset, new object[] { NWO_WebBrowser, nCraftmen.Rank1Common });
                                if (bAssetSet)
                                {
                                    if (_NWO_Account.NWO_DevLogging)
                                        AddText("Rank 1 Common Asset Selected.");

                                    WaitForPageToLoad();
                                    return true;
                                }
                            }
                        }
                    }

                    return false;
                }
            }
            else
            {
                if (_NWO_Account.NWO_DevLogging)
                    AddText("Optional Assets are not available.");
                return false;
            }

            //if (_NWO_Account.NWO_DevLogging)
            //    AddText("Exiting Select Assets...??? How did we get here.");

            //return false;
        }

        private Boolean StartTask(NWO_TaskCollection slotTasks, out Int32 taskDuration)
        {
            bool firstTime = true;
            bool taskStarted = false;

            Int32 profLevel = -1;
            String lastTaskProf = String.Empty;

            taskDuration = 0;

            if (_NWO_Account.NWO_DevLogging)
                AddText("StartTask Function Start.");
            do
            {
                #region Reset Tasks if Repeat is on.
                if (slotTasks.AllDone && slotTasks.RepeatTaskCycle && slotTasks.Infinite)
                {
                    foreach (NWO_Task task in slotTasks.Tasks)
                    {
                        task.CurAmntDone = 0;
                    }
                }
                #endregion

                foreach (NWO_Task task in slotTasks.Tasks)
                {
                    // Check to see if we have finished the amount we want to do for the task.
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Checking to see if we are done running the task a certain amount of times.");
                    else if (_NWO_Account.NWO_VerboseLogging)
                        AddText(String.Format("{0}: Attempting to Run {1}.", _NWO_LoggedInChar, task));

                    if (!task.Done)
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Able to run the task again..");

                        // Navigate to the Task Tab.
                        if (lastTaskProf != task.TaskProfession)
                        {
                            NWO_URLs.NWO_Profession_Name = task.TaskProfession;
                            lastTaskProf = task.TaskProfession;
                            NWO_Navigate(NWO_URLs.NWO_Profession_Task_Tab_Url);

                            WaitForPageToLoad();

                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Attempting to get Profession Level.");

                            profLevel = GetProfessionLevel(task.TaskProfession);
                        }

                        if (profLevel > -1)
                        {
                            if (profLevel < task.TaskLevel)
                            {
                                AddText(String.Format("Task {0} level is to Heigh.", task.TaskName));
                                AddText("Moving on to try next Task.");
                                continue;
                            }
                        }

                        if (_NWO_Account.NWO_DevLogging)
                        {
                            if (profLevel == -1)
                                AddText("Get Profession Level failed.");
                            AddText("see if Tasks Start button is available.");
                        }

                        if (!NWO_IsRunning)
                            return false;

                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Navigating to Profession Task List");

                        // Navigate to the Profession Task List.
                        NWO_Navigate(NWO_URLs.NWO_Profession_Task_List_Url);

                        WaitForPageToLoad();

                        // Check to see if Task is Available.
                        // If the Task is a Rare and not available, it will still have an active start task button.
                        // We should look for it in the list before navigating to it.
                        if (task.Rare)
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Attempting Rare Task.");

                            NWO_Defines.Delegates.NWO_BoolCallbackWebBrowserText bCheckRareTask = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowserText(NWO_ProfessionScreen.CheckRareTaskAvailable);
                            Boolean bRaretaskAvail = (Boolean)this.Invoke(bCheckRareTask, new object[] { NWO_WebBrowser, task.TaskName });

                            if (!bRaretaskAvail)
                            {
                                AddText(String.Format("Task {0} not available right now, going on to next task.", task.TaskName));
                                continue;
                            }
                        }

                        if (!NWO_IsRunning)
                            return false;

                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Navigating to Task");

                        NWO_URLs.NWO_Task_Name = task.TaskURL;
                        NWO_Navigate(NWO_URLs.NWO_Profession_Task_Select_Url);

                        WaitForPageToLoad();

                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Checking to see if task is available.");

                        if (!NWO_IsRunning)
                            return false;

                        Boolean startTaskEnabled = GetTaskAvailable();

                        if (startTaskEnabled)
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Task is available. Checking to see if we have assets to add.");

                            if (!_NWO_isRunning)
                                return false;

                            // TODO: Make this work for multiple assets.
                            //Add Asset Here.
                            if (task.Assets.HasAssets)
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Has Assets");

                                foreach (NWO_Craftsmen nwoCraft in NWO_Defines.ArrayLists.Craftsmen)
                                {
                                    if (nwoCraft.Profession == task.TaskProfession)
                                    {
                                        if (SelectAssets(task.Assets, nwoCraft))
                                        {
                                            if (_NWO_Account.NWO_DevLogging)
                                                AddText("Adding Assets Successful.");
                                            break;
                                        }
                                        else
                                        {
                                            if (_NWO_Account.NWO_DevLogging)
                                                AddText("Adding Assets Failed");
                                            break;
                                        }
                                    }
                                }
                            }

                            if (!_NWO_isRunning)
                                return false;

                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Get Task Duration Time.");

                            Boolean taskDur = GetTaskDuration();
                            if (taskDur)
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Found Task Duration Time.");
                                taskDuration = GetTaskDuration(NWO_ProfessionScreen.TaskDurationTime);
                            }
                            else
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Was Unable to get task duration.");
                                taskDuration = 0;
                            }

                            if (!_NWO_isRunning)
                                return false;

                            Boolean bTaskStarted = PressStartTask(task.TaskURL);

                            if (bTaskStarted)
                            {
                                //TODO: Check for the failed to start task message here.
                                WaitForPageToLoad();

                                NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser bMessageRet = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_ProfessionScreen.CheckTaskStartedSuccessfully);
                                Boolean bSuccessful = (Boolean)this.Invoke(bMessageRet, new object[] { NWO_WebBrowser });

                                if (bSuccessful)
                                {
                                    task.CurAmntDone++;
                                    AddText(String.Format("{0} started.", task.TaskName));
                                    AddText(String.Format("Task Duration time : {0}", TaskDurationTimeString(taskDuration)));
                                    taskStarted = true;

                                    return true;
                                }
                                else
                                {
                                    if (_NWO_Account.NWO_DevLogging)
                                        AddText("Got a failed to Start Message.  Trying next task.");
                                    lastTaskProf = String.Empty;

                                    // Closing Failed Messagebox
                                    if (_NWO_Account.NWO_DevLogging)
                                        AddText("Sold Message Box Found. Trying to Close.");

                                    NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser closeMessageBox = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_ProfessionScreen.ClickMessageBox);
                                    Boolean bMessageBoxClosed = (Boolean)this.Invoke(closeMessageBox, new object[] { NWO_WebBrowser });

                                    if (bMessageBoxClosed)
                                    {
                                        if (_NWO_Account.NWO_DevLogging)
                                            AddText("Message Box Closed.");
                                    }

                                    continue;
                                }
                                
                            }
                            else
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                {
                                    AddText("Was unable to start task??? What heppen??");
                                    AddText(NWO_ProfessionScreen.ErrorMessage);
                                }
                            }
                        }
                        else
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Task is unavailable going to next task..");

                            NWO_Navigate(NWO_URLs.NWO_Profession_Task_List_Url);

                            WaitForPageToLoad();
                        }
                    }
                    else
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Task has been run the total amount of times set. Going to next task.");
                    }
                }

                if (firstTime && !taskStarted && slotTasks.RepeatTaskCycle && NWO_IsRunning)
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("First run though the list no Tasks were available of the ones not already done.  Resetting all cur amounts back to 0, since we have repeat checked.");
                    firstTime = false;
                    foreach (NWO_Task task in slotTasks.Tasks)
                    {
                        task.CurAmntDone = 0;
                    }
                }
                else if (!slotTasks.RepeatTaskCycle)
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Finished running though the list. Probably wont ever get here.");
                    else
                        AddText("List is finished.");
                    taskStarted = true; // No more tasks to do on this list
                }
                else if (!firstTime && !taskStarted)
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Went through the list twice without starting a task.  No tasks are startable???");
                    taskStarted = true; // None of the tasks were avaiable at this time we will try again later.
                }

            } while (!taskStarted && NWO_IsRunning);

            if (_NWO_Account.NWO_DevLogging)
                AddText("StartTask Function End.");

            return false;
        }

        private Boolean StartTaskLeveling(NWO_ProfessionLevelingCollection slotProfLeveling, out Int32 taskDuration)
        {
            Boolean taskStarted = false;
            Boolean firstTime = false;

            String lastTaskProf = String.Empty;

            taskDuration = 0;

            if (_NWO_Account.NWO_DevLogging)
                AddText("Start Task Leveling Started.");

            do
            {
                if (lastTaskProf != slotProfLeveling.Profession)
                {
                    // Navigate tot he Task Tab.
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Getting Profession Level.");

                    NWO_URLs.NWO_Profession_Name = slotProfLeveling.Profession;
                    lastTaskProf = slotProfLeveling.Profession;
                    NWO_Navigate(NWO_URLs.NWO_Profession_Task_Tab_Url);

                    WaitForPageToLoad();

                    slotProfLeveling.ProfessionLevel = GetProfessionLevel(slotProfLeveling.Profession);
                }

                if (slotProfLeveling.ProfessionLevel == -1)
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Failed to get Profession Level.  Profession Leveling will not work without this, exiting.");

                    return false;
                }

                if (_NWO_Account.NWO_DevLogging)
                    AddText("Check to see if current Profession Level information is for the current level.");

                if (slotProfLeveling.Current == null)
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("We didnt have any information for this level range.  Exiting Profession Leveling.");
                    return false;
                }

                if (_NWO_Account.NWO_DevLogging)
                    AddText("We have information for this level. Starting next task in list. - Profession Leveling");

                // TODO: Do we need to buy supplies.  We should navigate to the Inventory Screen and see what all we have in profession tab.
                // Then check to see if we need to buy any of the supplies in our list yet.  Need to get the supplies information from the
                // start task screen so we can suptract them from our running totals.

                if (!NWO_IsRunning)
                    return false;

                if (slotProfLeveling.Current.AllTasksDone)
                {
                    slotProfLeveling.Current.ResetTasks();
                }

                foreach (NWO_Task task in slotProfLeveling.Current.Tasks)
                {
                    if (!task.Done)
                    {
                        if (!NWO_IsRunning)
                            return false;

                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Navigating to the Profession Task List - Profession Leveling");

                        NWO_Navigate(NWO_URLs.NWO_Profession_Task_List_Url);

                        WaitForPageToLoad();

                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Navigating to Task Page. - Profession Leveling");

                        if (!NWO_IsRunning)
                            return false;

                        NWO_URLs.NWO_Task_Name = task.TaskURL;
                        NWO_Navigate(NWO_URLs.NWO_Profession_Task_Select_Url);

                        WaitForPageToLoad();

                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Checking to see if it is available. - Profession Leveling");

                        Boolean startTaskEnabled = GetTaskAvailable();

                        if (startTaskEnabled)
                        {
                            if (!NWO_IsRunning)
                                return false;

                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Task is Available. Adding any Assets Selected. - Profession Leveling");

                            // TODO: Make this work for multiple assets.
                            if (task.Assets.HasAssets)
                            {
                                if (!NWO_IsRunning)
                                    return false;

                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Has Assets - Profession Leveling");

                                foreach (NWO_Craftsmen nwoCraft in NWO_Defines.ArrayLists.Craftsmen)
                                {
                                    if (nwoCraft.Profession == task.TaskProfession)
                                    {
                                        if (SelectAssets(task.Assets, nwoCraft))
                                        {
                                            if (_NWO_Account.NWO_DevLogging)
                                                AddText("Adding Assets Successful. - Profession Leveling");
                                            break;
                                        }
                                        else
                                        {
                                            if (_NWO_Account.NWO_DevLogging)
                                                AddText("Adding Assets Failed. - Profession Leveling");
                                            break;
                                        }
                                    }
                                }

                                if (!NWO_IsRunning)
                                    return false;
                            }

                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Get Task Duration Time. - Profession Leveling");

                            if (!NWO_IsRunning)
                                return false;

                            Boolean taskDur = GetTaskDuration();
                            if (taskDur)
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Found Task Duration Time. - Profession Leveling");
                                taskDuration = GetTaskDuration(NWO_ProfessionScreen.TaskDurationTime);
                            }
                            else
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Was Unable to get task duration. - Profession Leveling");
                                taskDuration = 0;
                            }

                            if (!NWO_IsRunning)
                                return false;

                            Boolean bTaskStarted = PressStartTask(task.TaskURL);

                            if (bTaskStarted)
                            {
                                //TODO: Check for the failed to start task message here.
                                WaitForPageToLoad();

                                task.CurAmntDone++;
                                AddText(String.Format("{0} started.", task.TaskName));
                                AddText(String.Format("Task Duration time : {0}", TaskDurationTimeString(taskDuration)));
                                taskStarted = true;
                                break;
                            }
                            else
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                {
                                    AddText("Was unable to start task??? What heppen?? - Profession Leveling");
                                    AddText(NWO_ProfessionScreen.ErrorMessage);
                                }
                            }
                        }
                        else
                        {
                            if (_NWO_Account.NWO_DevLogging)
                                AddText("Task is unavailable going to next task.. This means that all tasks after this is probably unavailable. - Profession Leveling");

                            NWO_Navigate(NWO_URLs.NWO_Profession_Task_List_Url);

                            WaitForPageToLoad();
                        }
                    }
                    else
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Task Done. Moving to next task. - Profession Leveling");
                    }
                }

                if (firstTime && !taskStarted && NWO_IsRunning)
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("First time through, nothing started. Reseting all tasks and trying again. - Profession Leveling");

                    slotProfLeveling.Current.ResetTasks();
                }
                else if (!firstTime && !taskStarted)
                {
                    if (_NWO_Account.NWO_DevLogging)
                        AddText("Must have run out of supplies needed for the tasks. Leaving Prof Leveling. - Profession Leveling");
                    return false;
                }
            }
            while (!taskStarted && NWO_IsRunning);

            if (_NWO_Account.NWO_DevLogging)
                AddText("Start Profession Leveling End.");

            return true;
        }

        private void Run()
        {
            AddText("Starting Bot.");

            Boolean isUnderMain = false;
            _NWO_CurrentLoginAttempt = 0;

            if (_NWO_Account.NWO_DevLogging || _NWO_Account.NWO_VerboseLogging)
                AddText("Waiting for Login Page to Load.");

            do
            {
                #region Check for Under Maintenance
                if ((_NWO_Account.NWO_DevLogging || _NWO_Account.NWO_VerboseLogging) && !isUnderMain)
                    AddText("Checking to see if Gateway Undermaintenance.");
                do
                {
                    isUnderMain = Undermaintenance();

                    if (isUnderMain && NWO_IsRunning)
                    {
                        AddText(String.Format("Gateway Under Maintenance. Sleeping for {0} min", _NWO_Account.NWO_WaitTimeMantenance));
                        
                        WaitForMaintenance();

                        // Refresh the Screen.
                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Refreshing Gateway.");
                        NWO_Navigate(NWO_URLs.NWO_Gateway_URL);
                    }
                } while (isUnderMain && NWO_IsRunning);

                #endregion

                if (_NWO_Account.NWO_DevLogging || _NWO_Account.NWO_VerboseLogging)
                    AddText("Checking for Login Page.");

                _NWO_isLoggedIn = LoginScreen();

                if (_NWO_Account.NWO_AutoLogin && !_NWO_isLoggedIn && NWO_IsRunning)
                {
                    if (_NWO_CurrentLoginAttempt > 0)
                    {
                        if (_NWO_Account.NWO_VerboseLogging || _NWO_Account.NWO_DevLogging)
                            AddText(String.Format("Login attempt #{0}.", _NWO_CurrentLoginAttempt));
                    }

                    #region Attempt to Login
                    if (!Login())
                    {
                        if (_NWO_CurrentLoginAttempt >= _NWO_Account.NWO_MaxLoginRetry)
                        {
                            AddText(String.Format("Unable to login after {0} tries. Stopping!!!", _NWO_CurrentLoginAttempt));
                            NWO_IsRunning = false;
                            return;
                        }
                        else
                        {
                            AddText(String.Format("Will attempt to login again in {0} min.", _NWO_Account.NWO_WaitTimeForRetry));
                            WaitForLoginRetry();
                            _NWO_CurrentLoginAttempt++;
                        }
                    }
                    else
                    {
                        AddText("Login Successful.");
                        _NWO_isLoggedIn = true;

                        WaitForCharacterSelectScreen();
                    }
                    #endregion
                }
                else // Either Autologin is not turned on or we are already logged in.
                {
                    if (!NWO_IsRunning)
                        return;

                    if (_NWO_Account.NWO_DevLogging || _NWO_Account.NWO_VerboseLogging)
                        AddText("Checking to see if we are on the character select screen.");

                    #region Check for Character Select Screen
                    Boolean onCharSelect = CharacterSelectScreen();

                    if (!onCharSelect)
                    {
                        if (_NWO_Account.NWO_DevLogging)
                        {
                            AddText(NWO_CharacterSelect.ErrorMessage);
                            AddText("Going to try navigating to character select screen");
                        }

                        NWO_Navigate(NWO_URLs.NWO_Character_Select_URL);

                        WaitForPageToLoad();

                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Checking to see if we are on the character select screen again.");

                        onCharSelect = CharacterSelectScreen();
                        if (!onCharSelect)
                        {
                            if (_NWO_Account.NWO_DevLogging)
                            {
                                AddText(NWO_CharacterSelect.ErrorMessage);
                                AddText("Checking to see if we were disconnected.");
                            }

                            #region Check for Disconnect
                            Boolean isDiscon = Disconnected();

                            if (isDiscon)
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("We were disconnected from the gateway. Trying to login again.");
                                AddText("Disconnected from gateway.");

                                isDiscon = ClickDisconnectButton();

                                if (isDiscon)
                                {
                                    _NWO_isLoggedIn = false;

                                    if (_NWO_Account.NWO_DevLogging)
                                        AddText("Attempting to login again.");
                                }
                                else
                                {
                                    _NWO_isLoggedIn = false;
                                    NWO_IsRunning = false;

                                    if (_NWO_Account.NWO_DevLogging)
                                        AddText("Unable to close the disconnect screen. Stoping Bot.");
                                    else
                                        AddText("Stoping Bot.");

                                    return;
                                }

                                WaitForPageToLoad();
                            }
                            else
                            {
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Not on the login or character select screen.. not sure where we are so lets just reload gateway.");

                                NWO_Navigate(NWO_URLs.NWO_Gateway_URL);

                                WaitForPageToLoad();

                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Told WebControl to navigate back to the Neverwinter Gateway first screen, so it should be back at the login screen or maintenance screen.");
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        if (_NWO_Account.NWO_DevLogging)
                            AddText("On the character select screen. Starting Script.");
                    }
                    #endregion
                }

                if (_NWO_isLoggedIn && NWO_IsRunning)
                {
                    #region Running Script
                    AddText("Starting Script");
                    do
                    {
                        #region Check for Gateway Under Maintenance
                        if (_NWO_Account.NWO_DevLogging || _NWO_Account.NWO_VerboseLogging)
                            AddText("Checking to see if Gateway Undermaintenance.");
                        isUnderMain = Undermaintenance();
                        if (isUnderMain)
                        {
                            _NWO_isLoggedIn = false;
                            _NWO_LoggedInChar = String.Empty;
                            break;
                        }

                        #endregion

                        #region Check for disconnect
                        if (_NWO_Account.NWO_DevLogging)
                            AddText("Checking to see if we are disconnected.");
                        Boolean isDiscon = Disconnected();
                        #endregion

                        if (!isDiscon)
                        {
                            #region Character Loop
                            foreach (NWO_Character curCharacter in _NWO_Account.NWO_CharacterList)
                            {

                                if (_NWO_Account.NWO_DevLogging)
                                    AddText(String.Format("Starting character loop - {0}.", curCharacter.NWO_CharacterName));

                                if (curCharacter.HasTasksToRun && (curCharacter.IsTaskCompleted || !curCharacter.TaskRunning || !_NWO_Account.NWO_AutoCalcWaitTime))
                                {
                                    if (curCharacter.IsTaskCompleted || !curCharacter.TaskRunning || !_NWO_Account.NWO_AutoCalcWaitTime)
                                    {
                                        if (_NWO_Account.NWO_DevLogging)
                                            AddText("Character has tasks to run.");

                                        #region Switch Characters
                                        // Check to see if we are already logged in as someone else.
                                        // First Check to see if we are on the Character Select Screen.
                                        if (!SwitchCharacters(curCharacter.NWO_CharacterName))
                                        {
                                            AddText(String.Format("Unable to Select character {0}", curCharacter.NWO_CharacterName));
                                            _NWO_LoggedInChar = String.Empty;
                                            continue;
                                        }
                                        else
                                        {
                                            curCharacter.NWO_CharacterRace = NWO_CharacterSelect.CharacterRace;
                                            curCharacter.NWO_CharacterType = NWO_CharacterSelect.CharacterClass;
                                            curCharacter.NWO_CharacterLevel = NWO_CharacterSelect.CharacterLevel;
                                            AddText("Logged in as: ");
                                            AddText(curCharacter.NWO_CharacterName);
                                            AddText(curCharacter.NWO_CharacterRace);
                                            AddText(String.Format("Level {0} {1}", curCharacter.NWO_CharacterLevel, curCharacter.NWO_CharacterType));

                                            NWO_URLs.NWO_Account_Nickname = _NWO_Account.NWO_AccountNick;
                                            NWO_URLs.NWO_Charcter_Name = curCharacter.NWO_CharacterName;
                                        }
                                        #endregion

                                        #region Checking to see if we are on profession screen.
                                        if (_NWO_Account.NWO_DevLogging)
                                            AddText("Checking to see if on Profession Screen or not.");

                                        NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser checkProf = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_ProfessionScreen.CheckProfessionPage);
                                        Boolean onProf = (Boolean)this.Invoke(checkProf, new object[] { NWO_WebBrowser });
                                        if (!onProf)
                                        {
                                            if (_NWO_Account.NWO_DevLogging)
                                                AddText("Not on Profession Screen, navigating to it.");
                                            else
                                                AddText("Navigate to Profession Screen.");

                                            NWO_URLs.NWO_Account_Nickname = _NWO_Account.NWO_AccountNick;
                                            NWO_URLs.NWO_Charcter_Name = curCharacter.NWO_CharacterName;

                                            NWO_Navigate(NWO_URLs.NWO_Professions_Overview_Url);

                                            WaitForPageToLoad();

                                            if (_NWO_Account.NWO_DevLogging)
                                                AddText("Checking to see if on Profession Screen now.");

                                            onProf = (Boolean)this.Invoke(checkProf, new object[] { NWO_WebBrowser });
                                            if (!onProf)
                                            {
                                                if (_NWO_Account.NWO_DevLogging)
                                                    AddText("Not on profession screen still.. Are we logged in?? Is There Maintenance??");

                                                _NWO_isLoggedIn = false;
                                                break;
                                            }

                                            if (_NWO_Account.NWO_DevLogging)
                                                AddText("On Profession Screen.");
                                        }
                                        else
                                        {
                                            if (_NWO_Account.NWO_DevLogging)
                                                AddText("On Profession Screen.");
                                        }
                                        #endregion

                                        #region Get Available Slots Number
                                        if (_NWO_Account.NWO_DevLogging)
                                            AddText("Checking to see how many slots are available.");

                                        NWO_Defines.Delegates.NWO_Int32CallbackWebBrowser getNumSlots = new NWO_Defines.Delegates.NWO_Int32CallbackWebBrowser(NWO_ProfessionScreen.GetNumberActiveSlots);
                                        Int32 numSlots = (Int32)this.Invoke(getNumSlots, new object[] { NWO_WebBrowser });

                                        if (numSlots < 0 || numSlots > _NWO_MaxNumberSlots)
                                        {
                                            if (_NWO_Account.NWO_DevLogging)
                                                AddText("Was unable to get the number of available slots.");
                                            numSlots = _NWO_MaxNumberSlots;
                                        }
                                        else
                                        {
                                            if (_NWO_Account.NWO_DevLogging)
                                                AddText("Was able to get the number of available slots.");

                                            if (_NWO_Account.NWO_VerboseLogging || _NWO_Account.NWO_DevLogging)
                                                AddText(String.Format("Number available slots for {0} is {1}.", curCharacter.NWO_CharacterName, numSlots));
                                        }
                                        #endregion

                                        for (int slot = 0; slot < numSlots; slot++)
                                        {
                                            #region Checking Slot
                                            AddText(String.Format("Checking slot {0}", slot + 1));

                                            #region Checking to see if we are on profession screen.
                                            if (_NWO_Account.NWO_DevLogging)
                                                AddText("Checking to see if on Profession Screen or not.");

                                            checkProf = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_ProfessionScreen.CheckProfessionPage);
                                            onProf = (Boolean)this.Invoke(checkProf, new object[] { NWO_WebBrowser });
                                            if (!onProf)
                                            {
                                                if (_NWO_Account.NWO_DevLogging)
                                                    AddText("Not on Profession Screen, navigating to it.");
                                                else
                                                    AddText("Navigate to Profession Screen.");

                                                NWO_URLs.NWO_Account_Nickname = _NWO_Account.NWO_AccountNick;
                                                NWO_URLs.NWO_Charcter_Name = curCharacter.NWO_CharacterName;

                                                NWO_Navigate(NWO_URLs.NWO_Professions_Overview_Url);

                                                WaitForPageToLoad();

                                                if (_NWO_Account.NWO_DevLogging)
                                                    AddText("Checking to see if on Profession Screen now.");
                                                onProf = (Boolean)this.Invoke(checkProf, new object[] { NWO_WebBrowser });
                                                if (!onProf)
                                                {
                                                    if (_NWO_Account.NWO_DevLogging)
                                                        AddText("Not on profession screen still.. Are we logged in?? Is There Maintenance??");
                                                    break;
                                                }

                                                if (_NWO_Account.NWO_DevLogging)
                                                    AddText("On Profession Screen.");
                                            }
                                            else
                                            {
                                                if (_NWO_Account.NWO_DevLogging)
                                                    AddText("On Profession Screen.");
                                            }
                                            #endregion

                                            #region Check for Task Not Complete Slot X.
                                            if (_NWO_Account.NWO_DevLogging)
                                                AddText("Checking to see if Task is Not Complete.");

                                            NWO_Defines.Delegates.NWO_BoolCallbackWeBrowserInt32 checkNotComplete = new NWO_Defines.Delegates.NWO_BoolCallbackWeBrowserInt32(NWO_ProfessionScreen.CheckSlotTaskNotComplete);
                                            Boolean slotNotComplete = (Boolean)this.Invoke(checkNotComplete, new object[] { NWO_WebBrowser, slot });

                                            if (slotNotComplete)
                                            {
                                                if (_NWO_Account.NWO_DevLogging)
                                                    AddText("Task not compete, getting duration till complete.");
                                                else
                                                    AddText(String.Format("Slot {0} still running.", slot + 1));

                                                NWO_Defines.Delegates.NWO_BoolCallbackWeBrowserInt32 getDuration = new NWO_Defines.Delegates.NWO_BoolCallbackWeBrowserInt32(NWO_ProfessionScreen.GetTaskDurationOverview);
                                                Boolean gotDuration = (Boolean)this.Invoke(getDuration, new object[] { NWO_WebBrowser, slot });

                                                if (gotDuration)
                                                {
                                                    if (_NWO_Account.NWO_DevLogging)
                                                        AddText("Was able to get Task Duration Time.");

                                                    int iTaskDur = GetTaskDuration(NWO_ProfessionScreen.TaskDurationTime);
                                                    curCharacter.AddTaskRunTime(iTaskDur, slot);
                                                    if (_NWO_Account.NWO_VerboseLogging)
                                                        AddText(String.Format("Time till Task done: {0}", TaskDurationTimeString(iTaskDur)));
                                                }
                                                else
                                                {
                                                    if (_NWO_Account.NWO_DevLogging)
                                                        AddText("Was unable to get Task Duration Time.");
                                                }

                                                continue;
                                            }
                                            else
                                            {
                                                if (NWO_ProfessionScreen.ErrorMessage != String.Empty)
                                                {
                                                    if (_NWO_Account.NWO_DevLogging)
                                                        AddText(NWO_ProfessionScreen.ErrorMessage);
                                                }
                                            }
                                            #endregion

                                            #region Check to see if Slot X is done.
                                            // Check to see if Slot X is done.
                                            NWO_Defines.Delegates.NWO_BoolCallbackWeBrowserInt32 checkSlotComplete = new NWO_Defines.Delegates.NWO_BoolCallbackWeBrowserInt32(NWO_ProfessionScreen.CheckSlotTaskComplete);
                                            Boolean slotComplete = (Boolean)this.Invoke(checkSlotComplete, new object[] { NWO_WebBrowser, slot });

                                            if (slotComplete)
                                            {
                                                AddText(String.Format("Slot {0} Complete. Collecting Reward.", slot + 1));
                                                NWO_URLs.NWO_Slot_Num = slot;

                                                NWO_Navigate(NWO_URLs.NWO_ProfessionCollectReward_Url);

                                                WaitForPageToLoad();

                                                NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser collect = new NWO_Defines.Delegates.NWO_BoolCallbackWebBrowser(NWO_ProfessionScreen.ClickTaskComplete);
                                                Boolean collected = (Boolean)this.Invoke(collect, new object[] { NWO_WebBrowser });

                                                if (!collected)
                                                {
                                                    if (_NWO_Account.NWO_DevLogging)
                                                        AddText(NWO_ProfessionScreen.ErrorMessage);
                                                }

                                                UpdateCompletedCount(curCharacter.NWO_CharacterName, slot);

                                                curCharacter.TaskCompleted = slot;

                                                AddText("Collected Reward.");
                                                WaitForPageToLoad();
                                            }
                                            else
                                            {
                                                if (NWO_ProfessionScreen.ErrorMessage != String.Empty)
                                                {
                                                    if (_NWO_Account.NWO_DevLogging)
                                                        AddText(NWO_ProfessionScreen.ErrorMessage);
                                                }
                                            }
                                            #endregion

                                            #region Check for Start Task Slot X

                                            if (_NWO_Account.NWO_DevLogging)
                                                AddText("Checking to see if the slot is ready for a task.");

                                            NWO_Defines.Delegates.NWO_BoolCallbackWeBrowserInt32 checkSlotStartTask = new NWO_Defines.Delegates.NWO_BoolCallbackWeBrowserInt32(NWO_ProfessionScreen.CheckSlotTaskStartTask);
                                            Boolean bslotStartTask = (Boolean)this.Invoke(checkSlotStartTask, new object[] { NWO_WebBrowser, slot });

                                            if (bslotStartTask)
                                            {
                                                if (_NWO_Account.NWO_DevLogging)
                                                    AddText("Slot is available for task, lets try to start one.");

                                                int iTaskDur = 0;
                                                switch ((slot + 1))
                                                {
                                                    case 1:
                                                        #region Profession Leveling Slot 1
                                                        if (curCharacter.ProffessionLeveling_Slot1)
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling Start");

                                                            if (curCharacter.ProfLeveling_Slot1.ProfessionLevelingInfo.Count > 0)
                                                            {
                                                                if (StartTaskLeveling(curCharacter.ProfLeveling_Slot1, out iTaskDur))
                                                                {
                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                    {
                                                                        AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                        AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                    }

                                                                    curCharacter.AddTaskRunTime(iTaskDur, slot);

                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                        AddText("Profession Leveling Start");

                                                                    break;
                                                                }
                                                            }

                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling End");
                                                        }
                                                        #endregion

                                                        #region Start Task Slot 1
                                                        if (curCharacter.Slot1.Tasks.Count > 0)
                                                        {
                                                            if (StartTask(curCharacter.Slot1, out iTaskDur))
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                {
                                                                    AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                    AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                }

                                                                curCharacter.AddTaskRunTime(iTaskDur, slot);
                                                            }
                                                            else
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                    AddText("Not adding iTaskDur to curCharacter Duration Array");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText(String.Format("{0} has no tasks available.", slot + 1));
                                                        }
                                                        #endregion
                                                        break;
                                                    case 2:
                                                        #region Profession Leveling Slot 2
                                                        if (curCharacter.ProffessionLeveling_Slot2)
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling Start");

                                                            if (curCharacter.ProfLeveling_Slot2.ProfessionLevelingInfo.Count > 0)
                                                            {
                                                                if (StartTaskLeveling(curCharacter.ProfLeveling_Slot2, out iTaskDur))
                                                                {
                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                    {
                                                                        AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                        AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                    }

                                                                    curCharacter.AddTaskRunTime(iTaskDur, slot);

                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                        AddText("Profession Leveling End");

                                                                    break;
                                                                }
                                                            }

                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling Start");
                                                        }
                                                        #endregion

                                                        #region Start Task Slot 2
                                                        if (curCharacter.Slot2.Tasks.Count > 0)
                                                        {
                                                            if (StartTask(curCharacter.Slot2, out iTaskDur))
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                {
                                                                    AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                    AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                }

                                                                curCharacter.AddTaskRunTime(iTaskDur, slot);
                                                            }
                                                            else
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                    AddText("Not adding iTaskDur to curCharacter Duration Array");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText(String.Format("{0} has no tasks available.", slot + 1));
                                                        }
                                                        #endregion
                                                        break;
                                                    case 3:
                                                        #region Profession Leveling Slot 3
                                                        if (curCharacter.ProffessionLeveling_Slot3)
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling Start");

                                                            if (curCharacter.ProfLeveling_Slot3.ProfessionLevelingInfo.Count > 0)
                                                            {
                                                                if (StartTaskLeveling(curCharacter.ProfLeveling_Slot3, out iTaskDur))
                                                                {
                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                    {
                                                                        AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                        AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                    }

                                                                    curCharacter.AddTaskRunTime(iTaskDur, slot);

                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                        AddText("Profession Leveling End");

                                                                    break;
                                                                }
                                                            }

                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling Start");
                                                        }
                                                        #endregion

                                                        #region Start Task Slot 3
                                                        if (curCharacter.Slot3.Tasks.Count > 0)
                                                        {
                                                            if (StartTask(curCharacter.Slot3, out iTaskDur))
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                {
                                                                    AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                    AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                }

                                                                curCharacter.AddTaskRunTime(iTaskDur, slot);
                                                            }
                                                            else
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                    AddText("Not adding iTaskDur to curCharacter Duration Array");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText(String.Format("{0} has no tasks available.", slot + 1));
                                                        }
                                                        #endregion
                                                        break;
                                                    case 4:
                                                        #region Profession Leveling Slot 4
                                                        if (curCharacter.ProffessionLeveling_Slot4)
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling Start");

                                                            if (curCharacter.ProfLeveling_Slot4.ProfessionLevelingInfo.Count > 0)
                                                            {
                                                                if (StartTaskLeveling(curCharacter.ProfLeveling_Slot4, out iTaskDur))
                                                                {
                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                    {
                                                                        AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                        AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                    }

                                                                    curCharacter.AddTaskRunTime(iTaskDur, slot);

                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                        AddText("Profession Leveling Start");

                                                                    break;
                                                                }
                                                            }

                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling End");
                                                        }
                                                        #endregion

                                                        #region Start Task Slot 4
                                                        if (curCharacter.Slot4.Tasks.Count > 0)
                                                        {
                                                            if (StartTask(curCharacter.Slot4, out iTaskDur))
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                {
                                                                    AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                    AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                }

                                                                curCharacter.AddTaskRunTime(iTaskDur, slot);
                                                            }
                                                            else
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                    AddText("Not adding iTaskDur to curCharacter Duration Array");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText(String.Format("{0} has no tasks available.", slot + 1));
                                                        }
                                                        #endregion
                                                        break;
                                                    case 5:
                                                        #region Profession Leveling Slot 5
                                                        if (curCharacter.ProffessionLeveling_Slot5)
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling Start");

                                                            if (curCharacter.ProfLeveling_Slot5.ProfessionLevelingInfo.Count > 0)
                                                            {
                                                                if (StartTaskLeveling(curCharacter.ProfLeveling_Slot5, out iTaskDur))
                                                                {
                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                    {
                                                                        AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                        AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                    }

                                                                    curCharacter.AddTaskRunTime(iTaskDur, slot);

                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                        AddText("Profession Leveling End");

                                                                    break;
                                                                }
                                                            }

                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling Start");
                                                        }
                                                        #endregion

                                                        #region Start Task Slot 5
                                                        if (curCharacter.Slot5.Tasks.Count > 0)
                                                        {
                                                            if (StartTask(curCharacter.Slot5, out iTaskDur))
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                {
                                                                    AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                    AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                }

                                                                curCharacter.AddTaskRunTime(iTaskDur, slot);
                                                            }
                                                            else
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                    AddText("Not adding iTaskDur to curCharacter Duration Array");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText(String.Format("{0} has no tasks available.", slot + 1));
                                                        }
                                                        #endregion
                                                        break;
                                                    case 6:
                                                        #region Profession Leveling Slot 6
                                                        if (curCharacter.ProffessionLeveling_Slot6)
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling Start");

                                                            if (curCharacter.ProfLeveling_Slot6.ProfessionLevelingInfo.Count > 0)
                                                            {
                                                                if (StartTaskLeveling(curCharacter.ProfLeveling_Slot6, out iTaskDur))
                                                                {
                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                    {
                                                                        AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                        AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                    }

                                                                    curCharacter.AddTaskRunTime(iTaskDur, slot);

                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                        AddText("Profession Leveling End");

                                                                    break;
                                                                }
                                                            }

                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling Start");
                                                        }
                                                        #endregion

                                                        #region Start Task Slot 6
                                                        if (curCharacter.Slot6.Tasks.Count > 0)
                                                        {
                                                            if (StartTask(curCharacter.Slot6, out iTaskDur))
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                {
                                                                    AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                    AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                }

                                                                curCharacter.AddTaskRunTime(iTaskDur, slot);
                                                            }
                                                            else
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                    AddText("Not adding iTaskDur to curCharacter Duration Array");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText(String.Format("{0} has no tasks available.", slot + 1));
                                                        }
                                                        #endregion
                                                        break;
                                                    case 7:
                                                        #region Profession Leveling Slot 7
                                                        if (curCharacter.ProffessionLeveling_Slot7)
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling Start");

                                                            if (curCharacter.ProfLeveling_Slot7.ProfessionLevelingInfo.Count > 0)
                                                            {
                                                                if (StartTaskLeveling(curCharacter.ProfLeveling_Slot7, out iTaskDur))
                                                                {
                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                    {
                                                                        AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                        AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                    }

                                                                    curCharacter.AddTaskRunTime(iTaskDur, slot);

                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                        AddText("Profession Leveling End");

                                                                    break;
                                                                }
                                                            }

                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling Start");
                                                        }
                                                        #endregion

                                                        #region Start Task Slot 7
                                                        if (curCharacter.Slot7.Tasks.Count > 0)
                                                        {
                                                            if (StartTask(curCharacter.Slot7, out iTaskDur))
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                {
                                                                    AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                    AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                }

                                                                curCharacter.AddTaskRunTime(iTaskDur, slot);
                                                            }
                                                            else
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                    AddText("Not adding iTaskDur to curCharacter Duration Array");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText(String.Format("{0} has no tasks available.", slot + 1));
                                                        }
                                                        #endregion
                                                        break;
                                                    case 8:
                                                        #region Profession Leveling Slot 8
                                                        if (curCharacter.ProffessionLeveling_Slot8)
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling Start");

                                                            if (curCharacter.ProfLeveling_Slot8.ProfessionLevelingInfo.Count > 0)
                                                            {
                                                                if (StartTaskLeveling(curCharacter.ProfLeveling_Slot8, out iTaskDur))
                                                                {
                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                    {
                                                                        AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                        AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                    }

                                                                    curCharacter.AddTaskRunTime(iTaskDur, slot);

                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                        AddText("Profession Leveling End");

                                                                    break;
                                                                }
                                                            }

                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling Start");
                                                        }
                                                        #endregion

                                                        #region Start Task Slot 8
                                                        if (curCharacter.Slot8.Tasks.Count > 0)
                                                        {
                                                            if (StartTask(curCharacter.Slot8, out iTaskDur))
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                {
                                                                    AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                    AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                }

                                                                curCharacter.AddTaskRunTime(iTaskDur, slot);
                                                            }
                                                            else
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                    AddText("Not adding iTaskDur to curCharacter Duration Array");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText(String.Format("{0} has no tasks available.", slot + 1));
                                                        }
                                                        #endregion
                                                        break;
                                                    case 9:
                                                        #region Profession Leveling Slot 9
                                                        if (curCharacter.ProffessionLeveling_Slot9)
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling Start");

                                                            if (curCharacter.ProfLeveling_Slot9.ProfessionLevelingInfo.Count > 0)
                                                            {
                                                                if (StartTaskLeveling(curCharacter.ProfLeveling_Slot9, out iTaskDur))
                                                                {
                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                    {
                                                                        AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                        AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                    }

                                                                    curCharacter.AddTaskRunTime(iTaskDur, slot);

                                                                    if (_NWO_Account.NWO_DevLogging)
                                                                        AddText("Profession Leveling End");

                                                                    break;
                                                                }
                                                            }

                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText("Profession Leveling Start");
                                                        }
                                                        #endregion

                                                        #region Start Task Slot 9
                                                        if (curCharacter.Slot9.Tasks.Count > 0)
                                                        {
                                                            if (StartTask(curCharacter.Slot9, out iTaskDur))
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                {
                                                                    AddText("Adding iTaskDur to curCharacter Duration Array");
                                                                    AddText(String.Format("Duration time: {0}", TaskDurationTimeString(iTaskDur)));
                                                                }

                                                                curCharacter.AddTaskRunTime(iTaskDur, slot);
                                                            }
                                                            else
                                                            {
                                                                if (_NWO_Account.NWO_DevLogging)
                                                                    AddText("Not adding iTaskDur to curCharacter Duration Array");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (_NWO_Account.NWO_DevLogging)
                                                                AddText(String.Format("{0} has no tasks available.", slot + 1));
                                                        }
                                                        #endregion
                                                        break;
                                                    default:
                                                        if (_NWO_Account.NWO_DevLogging)
                                                            AddText("We some how went into the default for the switch statement. This should never happen!!!!!");
                                                        break;
                                                }

                                            }
                                            else
                                            {
                                                if (_NWO_Account.NWO_DevLogging)
                                                {
                                                    AddText("Not sure what happen, we probably should never reach this point.");
                                                    if (NWO_ProfessionScreen.ErrorMessage != String.Empty)
                                                        AddText(NWO_ProfessionScreen.ErrorMessage);
                                                }
                                            }
                                            #endregion

                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        if (_NWO_Account.NWO_DevLogging)
                                        {
                                            if (!_NWO_Account.NWO_AutoCalcWaitTime)
                                                AddText("Character tasks not complete. Going to next character.");
                                            else
                                                AddText("AutoCalcWaitTime set to true.. Why are we here.");
                                        }
                                        else if (_NWO_Account.NWO_VerboseLogging)
                                            AddText("Characters has no completed tasks. Skipping Character!!");
                                    }
                                }
                                else
                                {
                                    if (_NWO_Account.NWO_DevLogging)
                                        AddText("Character has no tasks to run. Skip!!");
                                    else if (_NWO_Account.NWO_VerboseLogging)
                                        AddText("Characters Task Cycle is empty. Skipping Character!!");
                                }

                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Navigate back to the Character Screen.");

                                NWO_Navigate(NWO_URLs.NWO_Character_Url);

                                WaitForPageToLoad();

                                #region Additional Tasks.
                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Checking for Additional Tasks.");

                                #region Refine Astral Diamonds
                                if (curCharacter.RefineAstralDiamonds)
                                {
                                    RefineAstralDiamonds();
                                }
                                #endregion

                                #region Sell Items/Open Boxes
                                SellItems(curCharacter.Inventory.NWO_Sell_Junk, curCharacter.Inventory.NWO_Sell_NonMagicalEquipment, false, curCharacter.Inventory.NWO_OpenBoxes);
                                #endregion

                                #endregion

                                #region Start Sell Bot Running
                                StartBotSelling();
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            #region Click Disconnect button
                            isDiscon = ClickDisconnectButton();

                            if (isDiscon)
                            {
                                _NWO_isLoggedIn = false;

                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Attempting to login again.");
                            }
                            else
                            {
                                _NWO_isLoggedIn = false;
                                NWO_IsRunning = false;

                                if (_NWO_Account.NWO_DevLogging)
                                    AddText("Unable to close the disconnect screen. Stoping Bot.");
                                else
                                    AddText("Stoping Bot.");
                            }

                            WaitForPageToLoad();
                            #endregion
                        }

                        if (_NWO_isLoggedIn)
                            WaitForNextCheck();
                    }
                    while (_NWO_isLoggedIn && NWO_IsRunning);
                    #endregion
                }

            } while (NWO_IsRunning);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddText(NWO_WebBrowser.Url.ToString());
        }
    }
}
