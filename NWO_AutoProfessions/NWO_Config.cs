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

namespace NWO_AutoProfessions
{
    public partial class NWO_Config : Form
    {
        private String                          _NWO_AutoProfessionFileLoc;

        private Boolean                         _NWO_isRunning;

        private ArrayList                       _NWO_Characters;

        private readonly String                 _NWO_AutoProfessionFileLocOrig = @"Config\Cycles";

        public NWO_Config()
        {
            InitializeComponent();

            _NWO_Characters = new ArrayList();

            _NWO_isRunning = false;

            NWO_MaxTimeoutsReached_ComboBox.Items.Add("Stop Bot");
            NWO_MaxTimeoutsReached_ComboBox.SelectedIndex = 0;

            NWO_ForBotting_CheckBox.Enabled = false;
            NWO_ForBottingCharacter_ComboBox.Enabled = false;
        }

        public Boolean NWO_IsRunning
        {
            set
            {
                _NWO_isRunning = value;
                if (_NWO_isRunning)
                    DisableAccountBoxes();
            }
        }

        public NWO_Account NWO_AccountInfo
        {
            get
            {
                NWO_Account _NWO_AccountInfo = new NWO_Account();
                _NWO_AccountInfo.NWO_AccountNick = NWO_AccountNick_Textbox.Text.Substring(1);
                _NWO_AccountInfo.NWO_AppendLogFile = NWO_AppendLogFiles_CheckBox.Checked;
                _NWO_AccountInfo.NWO_AutoCalcWaitTime = NWO_AutoCalculateTimeBetweenDelays_CheckBox.Checked;
                _NWO_AccountInfo.NWO_AutoLogin = NWO_AutoLogin_CheckBox.Checked;
                _NWO_AccountInfo.NWO_AutoProfessionFileLoc = _NWO_AutoProfessionFileLoc;
                _NWO_AccountInfo.NWO_DevLogging = NWO_DevLogging_CheckBox.Checked;
                _NWO_AccountInfo.NWO_MaxLoginRetry = Convert.ToInt32(NWO_MaxTimeoutsAllowed_Textbox.Text);
                _NWO_AccountInfo.NWO_ForBotting = NWO_ForBotting_CheckBox.Checked;
                
                if (NWO_ForBottingCharacter_ComboBox.SelectedIndex > -1)
                    _NWO_AccountInfo.NWO_BottingCharacter = NWO_ForBottingCharacter_ComboBox.SelectedItem.ToString();
                else
                    _NWO_AccountInfo.NWO_BottingCharacter = String.Empty;

                if (NWO_AutoLogin_CheckBox.Checked)
                {
                    _NWO_AccountInfo.NWO_Password = NWO_Password_Textbox.Text;
                    _NWO_AccountInfo.NWO_UserName = NWO_Username_Textbox.Text;
                }
                else
                {
                    _NWO_AccountInfo.NWO_Password = String.Empty;
                    _NWO_AccountInfo.NWO_UserName = String.Empty;
                }
                _NWO_AccountInfo.NWO_VerboseLogging = NWO_VerboseLogging_Checkbox.Checked;
                _NWO_AccountInfo.NWO_WaitTimeBetweenChecks = Convert.ToInt32(NWO_TimeBetweenDelay_TextBox.Text);
                _NWO_AccountInfo.NWO_WaitTimeForRetry = Convert.ToInt32(NWO_WaitTimeBetweenLoginAttemts_TextBox.Text);
                _NWO_AccountInfo.NWO_WaitTimePageLoad = Convert.ToInt32(NWO_WaitTimePageLoading_TextBox.Text);
                _NWO_AccountInfo.NWO_CharacterList = _NWO_Characters;

                return _NWO_AccountInfo;
            }
            set
            {
                NWO_AccountNick_Textbox.Text = String.Format("@{0}", value.NWO_AccountNick);
                
                _NWO_Characters = value.NWO_CharacterList;

                if (_NWO_Characters.Count > 0)
                {
                    UpdateCharacterList();
                    UpdateBottingCharacterComboBox();

                    NWO_ForBotting_CheckBox.Enabled = true;
                }

                NWO_AppendLogFiles_CheckBox.Checked = value.NWO_AppendLogFile;
                NWO_AutoCalculateTimeBetweenDelays_CheckBox.Checked = value.NWO_AutoCalcWaitTime;
                NWO_AutoLogin_CheckBox.Checked = value.NWO_AutoLogin;
                _NWO_AutoProfessionFileLoc = value.NWO_AutoProfessionFileLoc;
                NWO_DevLogging_CheckBox.Checked = value.NWO_DevLogging;
                NWO_MaxTimeoutsAllowed_Textbox.Text = value.NWO_MaxLoginRetry.ToString();
                NWO_ForBotting_CheckBox.Checked = value.NWO_ForBotting;

                if (NWO_ForBotting_CheckBox.Checked)
                    NWO_ForBottingCharacter_ComboBox.Enabled = true;

                int curSel = NWO_ForBottingCharacter_ComboBox.FindStringExact(value.NWO_BottingCharacter);
                NWO_ForBottingCharacter_ComboBox.SelectedIndex = curSel;

                if (NWO_AutoLogin_CheckBox.Checked)
                {
                    NWO_Password_Textbox.Text = value.NWO_Password;
                    NWO_Username_Textbox.Text = value.NWO_UserName;
                    NWO_Password_Textbox.Enabled = true;
                    NWO_Username_Textbox.Enabled = true;
                }
                else
                {
                    NWO_Password_Textbox.Enabled = false;
                    NWO_Username_Textbox.Enabled = false;
                }
                NWO_VerboseLogging_Checkbox.Checked = value.NWO_VerboseLogging;
                NWO_TimeBetweenDelay_TextBox.Text = value.NWO_WaitTimeBetweenChecks.ToString();
                NWO_WaitTimeBetweenLoginAttemts_TextBox.Text = value.NWO_WaitTimeForRetry.ToString();
                NWO_WaitTimePageLoading_TextBox.Text = value.NWO_WaitTimePageLoad.ToString();
            }
        }
        
        private void DisableAccountBoxes()
        {
            NWO_AutoLogin_CheckBox.Enabled = false;
            NWO_Username_Textbox.Enabled = false;
            NWO_Password_Textbox.Enabled = false;
            NWO_AccountNick_Textbox.Enabled = false;
        }

        private void UpdateCharacterList()
        {
            NWO_Character_ListView.Items.Clear();
            foreach (NWO_Character character in _NWO_Characters)
            {
                NWO_Character_ListView.Items.Add(character.CharacterListName);
            }
        }

        private void UpdateBottingCharacterComboBox()
        {
            NWO_ForBottingCharacter_ComboBox.Items.Clear();
            foreach (NWO_Character character in _NWO_Characters)
            {
                NWO_ForBottingCharacter_ComboBox.Items.Add(character.NWO_CharacterName);
            }
        }

        private void NWO_AutoLogin_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (NWO_AutoLogin_CheckBox.Checked)
            {
                NWO_Username_Textbox.Enabled = true;
                NWO_Password_Textbox.Enabled = true;
            }
            else
            {
                NWO_Username_Textbox.Enabled = false;
                NWO_Password_Textbox.Enabled = false;
            }
        }

        private void NWO_Username_Textbox_Enter(object sender, EventArgs e)
        {
            if (NWO_Username_Textbox.Text == "Username")
                NWO_Username_Textbox.Text = String.Empty;
        }

        private void NWO_Username_Textbox_Leave(object sender, EventArgs e)
        {
            if (NWO_Username_Textbox.Text == String.Empty)
                NWO_Username_Textbox.Text = "Username";
        }

        private void NWO_Password_Textbox_Enter(object sender, EventArgs e)
        {
            if (NWO_Password_Textbox.Text == "Password")
                NWO_Password_Textbox.Text = String.Empty;
        }

        private void NWO_Password_Textbox_Leave(object sender, EventArgs e)
        {
            if (NWO_Password_Textbox.Text == String.Empty)
                NWO_Password_Textbox.Text = "Password";
        }

        private void NWO_AccountNick_Textbox_Enter(object sender, EventArgs e)
        {
            if (NWO_AccountNick_Textbox.Text == "@Account")
                NWO_AccountNick_Textbox.Text = "@";
        }

        private void NWO_AccountNick_Textbox_Leave(object sender, EventArgs e)
        {
            if (NWO_AccountNick_Textbox.Text == "@" || NWO_AccountNick_Textbox.Text == String.Empty)
                NWO_AccountNick_Textbox.Text = "@Account";
        }

        private void NWO_AccountNick_Textbox_TextChanged(object sender, EventArgs e)
        {
            String temp = NWO_AccountNick_Textbox.Text;
            if (temp.IndexOf("@") != 0)
            {
                if (!temp.Contains("@"))
                    temp = String.Format("@{0}", temp);
                else
                {
                    String sub = temp.Substring(0, temp.IndexOf("@"));
                    temp = temp.Replace(sub, "");
                    temp = String.Format("{0}{1}", temp, sub);
                }

                NWO_AccountNick_Textbox.Text = temp;
            }
        }

        private void NWO_Character_TextBox_Enter(object sender, EventArgs e)
        {
            if (NWO_Character_TextBox.Text == "Character Name")
                NWO_Character_TextBox.Text = String.Empty;
        }

        private void NWO_Character_TextBox_Leave(object sender, EventArgs e)
        {
            if (NWO_Character_TextBox.Text == String.Empty)
                NWO_Character_TextBox.Text = "Character Name";
        }

        private void NWO_LoadQuickRoute_Button_Click(object sender, EventArgs e)
        {
            if (NWO_Character_ListView.SelectedIndex > -1)
            {
                NWO_Character tempCharacter = (NWO_Character)_NWO_Characters[NWO_Character_ListView.SelectedIndex];

                if (tempCharacter.NWO_CycleFileLocation == String.Empty)
                {
                    if (!Directory.Exists(_NWO_AutoProfessionFileLoc))
                        Directory.CreateDirectory(_NWO_AutoProfessionFileLoc);

                    if (_NWO_AutoProfessionFileLoc == _NWO_AutoProfessionFileLocOrig)
                        _NWO_AutoProfessionFileLoc = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), _NWO_AutoProfessionFileLoc);
                }
                else
                {
                    if (!Directory.Exists(tempCharacter.NWO_CycleFileLocation))
                    {
                        tempCharacter.NWO_CycleFileLocation = String.Empty;
                        if (!Directory.Exists(_NWO_AutoProfessionFileLoc))
                        {
                            if (_NWO_AutoProfessionFileLoc != _NWO_AutoProfessionFileLocOrig)
                                _NWO_AutoProfessionFileLoc = _NWO_AutoProfessionFileLocOrig;

                            if (!Directory.Exists(_NWO_AutoProfessionFileLoc))
                                Directory.CreateDirectory(_NWO_AutoProfessionFileLoc);
                        }
                    }
                    else
                        _NWO_AutoProfessionFileLoc = tempCharacter.NWO_CycleFileLocation;
                }

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Auto Profession|*.ap";
                ofd.FilterIndex = 0;
                ofd.InitialDirectory = _NWO_AutoProfessionFileLoc;
                ofd.Multiselect = false;
                ofd.AddExtension = false;

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    tempCharacter.NWO_CycleFileLocation = ofd.FileName.Remove(ofd.FileName.IndexOf(ofd.SafeFileName));
                    tempCharacter.NWO_CycleFileName = ofd.SafeFileName;

                    if (_NWO_AutoProfessionFileLoc != tempCharacter.NWO_CycleFileLocation)
                        _NWO_AutoProfessionFileLoc = tempCharacter.NWO_CycleFileLocation;

                    _NWO_Characters[NWO_Character_ListView.SelectedIndex] = tempCharacter;
                }

                UpdateCharacterList();
            }
        }

        private void NWO_ClearRecent_Button_Click(object sender, EventArgs e)
        {
            if (NWO_Character_ListView.SelectedIndex > -1)
            {
                ((NWO_Character)_NWO_Characters[NWO_Character_ListView.SelectedIndex]).NWO_CycleFileName = String.Empty;

                UpdateCharacterList();
            }
        }

        private void NWO_AddCharacter_Button_Click(object sender, EventArgs e)
        {
            if (NWO_Character_TextBox.Text != "Character Name" && NWO_Character_TextBox.Text != String.Empty)
            {
                NWO_Character newCharacter = new NWO_Character();
                newCharacter.NWO_CharacterName = NWO_Character_TextBox.Text;
                _NWO_Characters.Add(newCharacter);

                UpdateCharacterList();
                UpdateBottingCharacterComboBox();

                if (!NWO_ForBotting_CheckBox.Enabled)
                {
                    NWO_ForBotting_CheckBox.Enabled = true;
                    if (NWO_ForBotting_CheckBox.Checked)
                        NWO_ForBottingCharacter_ComboBox.Enabled = true;
                }
            }
        }

        private void NWO_RemoveCharacter_Button_Click(object sender, EventArgs e)
        {
            if (NWO_Character_ListView.SelectedIndex > -1)
            {
                _NWO_Characters.RemoveAt(NWO_Character_ListView.SelectedIndex);

                UpdateCharacterList();
                UpdateBottingCharacterComboBox();

                if (_NWO_Characters.Count > 0)
                {
                    if (!NWO_ForBotting_CheckBox.Enabled)
                    {
                        NWO_ForBotting_CheckBox.Enabled = true;
                        if (NWO_ForBotting_CheckBox.Checked)
                            NWO_ForBottingCharacter_ComboBox.Enabled = true;
                    }
                }
                else
                {
                    if (NWO_ForBotting_CheckBox.Enabled)
                    {
                        NWO_ForBotting_CheckBox.Enabled = false;
                        NWO_ForBottingCharacter_ComboBox.Enabled = false;
                    }
                }
            }
        }

        private void NWO_ForBotting_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (NWO_ForBotting_CheckBox.Checked)
                NWO_ForBottingCharacter_ComboBox.Enabled = true;
            else
                NWO_ForBottingCharacter_ComboBox.Enabled = false;
        }
    }
}
