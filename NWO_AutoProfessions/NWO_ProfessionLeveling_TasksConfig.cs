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
    public partial class NWO_ProfessionLeveling_TasksConfig : Form
    {
        private NWO_ProfessionLevelingCollection        _plCollection;
        private NWO_ProfessionLeveling                  _professionLeveling;

        private String                                  _profession;
        
        private Int32                                   _slotNum;

        private Version                                 _curVersion;

        private Boolean                                 _modified;

        public NWO_ProfessionLeveling_TasksConfig()
        {
            InitializeComponent();

            _plCollection = new NWO_ProfessionLevelingCollection();
            _professionLeveling = new NWO_ProfessionLeveling();

            _modified = false;

            NWO_Defines.PopulateComboBoxes.Supplies(NWO_PLSupplies_ComboBox);
        }

        public Int32 Slot
        {
            set
            {
                _slotNum = value;
            }
        }

        public String Profession
        {
            set
            {
                _profession = value;

                this.Text = String.Format("{0} - Slot {1} Tasks for Leveling", _profession, _slotNum);

                NWO_Defines.PopulateComboBoxes.Tasks(NWO_PLTask_ComboBox, _profession, false);

                PopulateAssets();
            }
        }

        public Version Version
        {
            set
            {
                _curVersion = value;
            }
        }

        public NWO_ProfessionLevelingCollection ProfessionLevelingCollection
        {
            get
            {
                return _plCollection;
            }
            set
            {
                _plCollection = value;

                UpdateProfessionLevelingCollectionListView();
                UpdateTaskListView();
                UpdateSuppliesListView();
            }
        }

        private NWO_Task GetTaskInfo()
        {
            NWO_Asset tempAsset = new NWO_Asset();

            tempAsset.Rank1Common = NWO_PLOptionalAssets_Rank1Common_CheckBox.Checked;
            tempAsset.Rank2Common = NWO_PLOptionalAssets_Rank2Common_CheckBox.Checked;
            tempAsset.Rank3Common = NWO_PLOptionalAssets_Rank3Common_CheckBox.Checked;
            tempAsset.Rank3Uncommon = NWO_PLOptionalAssets_Rank3Uncommon_CheckBox.Checked;
            tempAsset.Rank3Rare = NWO_PLOptionalAssets_Rank3Rare_CheckBox.Checked;
            tempAsset.Rank3Epic = NWO_PLOptionalAssets_Rank3Epic_CheckBox.Checked;
            tempAsset.Rank1Common_Quantity = Convert.ToInt32(NWO_PLOptionalAssets_Rank1Common_TextBox.Text);
            tempAsset.Rank2Common_Quantity = Convert.ToInt32(NWO_PLOptionalAssets_Rank2Common_TextBox.Text);
            tempAsset.Rank3Common_Quantity = Convert.ToInt32(NWO_PLOptionalAssets_Rank3Common_TextBox.Text);
            tempAsset.Rank3Uncommon_Quantity = Convert.ToInt32(NWO_PLOptionalAssets_Rank3Uncommon_TextBox.Text);
            tempAsset.Rank3Rare_Quantity = Convert.ToInt32(NWO_PLOptionalAssets_Rank3Rare_TextBox.Text);
            tempAsset.Rank3Epic_Quantity = Convert.ToInt32(NWO_PLOptionalAssets_Rank3Epic_TextBox.Text);

            NWO_Task tempTask = new NWO_Task();
            tempTask.TaskProfession = _profession;
            if (NWO_PLTask_ComboBox.SelectedItem.ToString() != String.Empty && !NWO_PLTask_ComboBox.SelectedItem.ToString().Contains("Rares:"))
            {
                tempTask.TaskFullName = NWO_PLTask_ComboBox.SelectedItem.ToString();
                foreach (NWO_Profession prof in NWO_Defines.ArrayLists.ProfessionInformation)
                {
                    if (tempTask.TaskProfession == prof.Name)
                    {
                        if (NWO_PLTask_ComboBox.SelectedIndex >= prof.Rare)
                        {
                            tempTask.TaskURL = prof.TaskNameURLList[NWO_PLTask_ComboBox.SelectedIndex - 2].ToString();
                            tempTask.Rare = true;
                        }
                        else
                        {
                            tempTask.TaskURL = prof.TaskNameURLList[NWO_PLTask_ComboBox.SelectedIndex].ToString();
                            tempTask.Rare = false;
                        }
                        break;
                    }
                }
            }
            tempTask.AmntToDo = Convert.ToInt32(NWO_PLRepeatTaskNum_TextBox.Text);

            tempTask.Assets = tempAsset;

            return tempTask;
        }

        private NWO_Supplies GetSupplyInfo()
        {
            NWO_Supplies retVal = new NWO_Supplies();
            retVal.Name = NWO_PLSupplies_ComboBox.SelectedItem.ToString();
            retVal.AmntToBuy = Convert.ToInt32(NWO_PLSuppliesAmntToPurchase_TextBox.Text);

            return retVal;
        }

        private void PopulateTaskInfo(NWO_Task curTask)
        {
            NWO_PLOptionalAssets_Rank1Common_CheckBox.Checked = curTask.Assets.Rank1Common;
            NWO_PLOptionalAssets_Rank2Common_CheckBox.Checked = curTask.Assets.Rank2Common;
            NWO_PLOptionalAssets_Rank3Common_CheckBox.Checked = curTask.Assets.Rank3Common;
            NWO_PLOptionalAssets_Rank3Uncommon_CheckBox.Checked = curTask.Assets.Rank3Uncommon;
            NWO_PLOptionalAssets_Rank3Rare_CheckBox.Checked = curTask.Assets.Rank3Rare;
            NWO_PLOptionalAssets_Rank3Epic_CheckBox.Checked = curTask.Assets.Rank3Epic;

            NWO_PLOptionalAssets_Rank1Common_TextBox.Text = Convert.ToString(curTask.Assets.Rank1Common_Quantity);
            NWO_PLOptionalAssets_Rank2Common_TextBox.Text = Convert.ToString(curTask.Assets.Rank2Common_Quantity);
            NWO_PLOptionalAssets_Rank3Common_TextBox.Text = Convert.ToString(curTask.Assets.Rank3Common_Quantity);
            NWO_PLOptionalAssets_Rank3Uncommon_TextBox.Text = Convert.ToString(curTask.Assets.Rank3Uncommon_Quantity);
            NWO_PLOptionalAssets_Rank3Rare_TextBox.Text = Convert.ToString(curTask.Assets.Rank3Rare_Quantity);
            NWO_PLOptionalAssets_Rank3Epic_TextBox.Text = Convert.ToString(curTask.Assets.Rank3Epic_Quantity);

            int curSel = NWO_PLTask_ComboBox.FindStringExact(curTask.TaskFullName);
            if (curSel < 0)
                curSel = 0;

            NWO_PLTask_ComboBox.SelectedIndex = curSel;

            NWO_PLRepeatTaskNum_TextBox.Text = Convert.ToString(curTask.AmntToDo);
        }

        private void PopulateSupplyInfo(NWO_Supplies curSupply)
        {
            int curSel = NWO_PLSupplies_ComboBox.FindStringExact(curSupply.Name);
            if (curSel < 0)
                curSel = 0;

            NWO_PLSupplies_ComboBox.SelectedIndex = curSel;

            NWO_PLSuppliesAmntToPurchase_TextBox.Text = curSupply.AmntToBuy.ToString();
        }

        private void PopulateAssets()
        {
            foreach (NWO_Craftsmen craftsmen in NWO_Defines.ArrayLists.Craftsmen)
            {
                if (craftsmen.Profession.Contains(_profession))
                {
                    NWO_PLOptionalAssets_Rank1Common_CheckBox.Text = craftsmen.Rank1Common;
                    NWO_PLOptionalAssets_Rank2Common_CheckBox.Text = craftsmen.Rank2Common;
                    NWO_PLOptionalAssets_Rank3Common_CheckBox.Text = craftsmen.Rank3Common;
                    NWO_PLOptionalAssets_Rank3Uncommon_CheckBox.Text = craftsmen.Rank3Uncommon;
                    NWO_PLOptionalAssets_Rank3Rare_CheckBox.Text = craftsmen.Rank3Rare;
                    NWO_PLOptionalAssets_Rank3Epic_CheckBox.Text = craftsmen.Rank3Epic;

                    break;
                }
            }

            NWO_PLOptionalAssets_Rank1Common_TextBox.Text = "0";
            NWO_PLOptionalAssets_Rank2Common_TextBox.Text = "0";
            NWO_PLOptionalAssets_Rank3Common_TextBox.Text = "0";
            NWO_PLOptionalAssets_Rank3Uncommon_TextBox.Text = "0";
            NWO_PLOptionalAssets_Rank3Rare_TextBox.Text = "0";
            NWO_PLOptionalAssets_Rank3Epic_TextBox.Text = "0";
        }

        private void UpdateSuppliesListView()
        {
            NWO_PLSupplies_ListView.Items.Clear();

            if (_professionLeveling.Supplies.Count > 0)
            {
                foreach (NWO_Supplies sup in _professionLeveling.Supplies)
                {
                    NWO_PLSupplies_ListView.Items.Add(sup.DisplayInfo);
                }
            }
        }

        private void UpdateTaskListView()
        {
            NWO_PLTasks_ListBox.Items.Clear();

            if (_professionLeveling.Tasks.Count > 0)
            {
                foreach (NWO_Task task in _professionLeveling.Tasks)
                {
                    NWO_PLTasks_ListBox.Items.Add(task.DisplayName);
                }
            }
        }

        private void UpdateProfessionLevelingCollectionListView()
        {
            NWO_PLCollection_ListView.Items.Clear();

            if (_plCollection.ProfessionLevelingInfo.Count > 0)
            {
                foreach (NWO_ProfessionLeveling pl in _plCollection.ProfessionLevelingInfo)
                {
                    NWO_PLCollection_ListView.Items.Add(pl.DisplayInformation);
                }
            }
        }

        private void NWO_PLSupplies_ListView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                NWO_PLSupplies_ListView.SelectedIndex = -1;
            }
        }

        private void NWO_PLSupplies_ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NWO_PLSupplies_ListView.SelectedIndex > -1)
            {
                PopulateSupplyInfo((NWO_Supplies)_professionLeveling.Supplies[NWO_PLSupplies_ListView.SelectedIndex]);
            }
        }

        private void NWO_PLSuppliesAdd_Button_Click(object sender, EventArgs e)
        {
            foreach (NWO_Supplies temp in _professionLeveling.Supplies)
            {
                if (temp.Name.Contains(NWO_PLSupplies_ComboBox.SelectedItem.ToString()))
                {
                    MessageBox.Show("Supply type already added, please update purchase amnt instead of adding a new one.");
                    return;
                }
            }

            NWO_Supplies curSupplies = GetSupplyInfo();

            _professionLeveling.Supplies.Add(curSupplies);

            UpdateSuppliesListView();

            _modified = true;
        }

        private void NWO_PLSuppliesDelete_Button_Click(object sender, EventArgs e)
        {
            if (NWO_PLSupplies_ListView.SelectedIndex > -1)
            {
                _professionLeveling.Supplies.RemoveAt(NWO_PLSupplies_ListView.SelectedIndex);

                UpdateSuppliesListView();

                _modified = true;
            }
        }

        private void NWO_PLSuppliesUpdate_Button_Click(object sender, EventArgs e)
        {
            if (NWO_PLSupplies_ListView.SelectedIndex > -1)
            {
                ((NWO_Supplies)_professionLeveling.Supplies[NWO_PLSupplies_ListView.SelectedIndex]).Name = NWO_PLSupplies_ComboBox.SelectedItem.ToString();
                ((NWO_Supplies)_professionLeveling.Supplies[NWO_PLSupplies_ListView.SelectedIndex]).AmntToBuy = Convert.ToInt32(NWO_PLSuppliesAmntToPurchase_TextBox.Text);

                UpdateSuppliesListView();

                _modified = true;
            }
        }

        private void NWO_PLSuppliesClear_Button_Click(object sender, EventArgs e)
        {
            _professionLeveling.Supplies.Clear();

            UpdateSuppliesListView();
        }

        private void NWO_PLTaskAdd_Button_Click(object sender, EventArgs e)
        {
            foreach (NWO_Task temp in _professionLeveling.Tasks)
            {
                if (temp.TaskFullName.Contains(NWO_PLTask_ComboBox.SelectedItem.ToString()))
                {
                    MessageBox.Show("Task Already added, please update task if you want to modify it.");
                    return;
                }
            }

            NWO_Task curTask = GetTaskInfo();

            _professionLeveling.Tasks.Add(curTask);

            UpdateTaskListView();

            _modified = true;
        }

        private void NWO_PLTaskRemove_Button_Click(object sender, EventArgs e)
        {
            if (NWO_PLTasks_ListBox.SelectedIndex > -1)
            {
                _professionLeveling.Tasks.RemoveAt(NWO_PLTasks_ListBox.SelectedIndex);

                UpdateTaskListView();

                _modified = true;
            }
        }

        private void NWO_PLTaskUpdate_Button_Click(object sender, EventArgs e)
        {
            if (NWO_PLTasks_ListBox.SelectedIndex > -1)
            {
                NWO_Task curTask = GetTaskInfo();

                _professionLeveling.Tasks[NWO_PLTasks_ListBox.SelectedIndex] = curTask;

                UpdateTaskListView();

                _modified = true;
            }
        }

        private void NWO_PLTaskMoveUp_Button_Click(object sender, EventArgs e)
        {
            if (NWO_PLTasks_ListBox.SelectedIndex > -1)
            {
                if (NWO_PLTasks_ListBox.SelectedIndex > 0)
                {
                    NWO_Task curTask = (NWO_Task)_professionLeveling.Tasks[NWO_PLTasks_ListBox.SelectedIndex];
                    _professionLeveling.Tasks[NWO_PLTasks_ListBox.SelectedIndex] = _professionLeveling.Tasks[NWO_PLTasks_ListBox.SelectedIndex - 1];
                    _professionLeveling.Tasks[NWO_PLTasks_ListBox.SelectedIndex - 1] = curTask;

                    UpdateTaskListView();

                    _modified = true;
                }
            }
        }

        private void NWO_PLTaskMoveDown_Button_Click(object sender, EventArgs e)
        {
            if (NWO_PLTasks_ListBox.SelectedIndex > -1)
            {
                if (NWO_PLTasks_ListBox.SelectedIndex != NWO_PLTasks_ListBox.Items.Count - 1)
                {
                    NWO_Task curTask = (NWO_Task)_professionLeveling.Tasks[NWO_PLTasks_ListBox.SelectedIndex];
                    _professionLeveling.Tasks[NWO_PLTasks_ListBox.SelectedIndex] = _professionLeveling.Tasks[NWO_PLTasks_ListBox.SelectedIndex + 1];
                    _professionLeveling.Tasks[NWO_PLTasks_ListBox.SelectedIndex + 1] = curTask;

                    UpdateTaskListView();

                    _modified = true;
                }
            }
        }

        private void NWO_PLTaskClear_Button_Click(object sender, EventArgs e)
        {
            _professionLeveling.Tasks.Clear();

            UpdateTaskListView();
        }

        private void NWO_PLTasks_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NWO_PLTasks_ListBox.SelectedIndex > -1)
            {
                PopulateTaskInfo((NWO_Task)_professionLeveling.Tasks[NWO_PLTasks_ListBox.SelectedIndex]);
            }
        }

        private void NWO_PLTasks_ListBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                NWO_PLTasks_ListBox.SelectedIndex = -1;
            }
        }

        private void NWO_PLAdd_Button_Click(object sender, EventArgs e)
        {
            foreach (NWO_ProfessionLeveling pl in _plCollection.ProfessionLevelingInfo)
            {
                if (pl.Start == Convert.ToInt32(NWO_PLStartLevel_Textbox.Text))
                {
                    MessageBox.Show("You already have a Profession Leveling that starts at this level.");
                    return;
                }
            }

            try
            {
                if (Convert.ToInt32(NWO_PLStartLevel_Textbox.Text) < 0 || Convert.ToInt32(NWO_PLStartLevel_Textbox.Text) > 20)
                    throw new FormatException();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid Start Level.(0-20)");
                return;
            }

            try
            {
                if (Convert.ToInt32(NWO_PLEndLevel_Textbox.Text) < 1 || Convert.ToInt32(NWO_PLEndLevel_Textbox.Text) > 20)
                    throw new FormatException();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid End Level.(1-20)");
                return;
            }

            _professionLeveling.Start = Convert.ToInt32(NWO_PLStartLevel_Textbox.Text);
            _professionLeveling.End = Convert.ToInt32(NWO_PLEndLevel_Textbox.Text);
            _professionLeveling.Profession = _profession;
            _professionLeveling.PurchaseSupplies = NWO_PLPurchase_Checkbox.Checked;

            _plCollection.ProfessionLevelingInfo.Add(_professionLeveling);

            _professionLeveling = new NWO_ProfessionLeveling();

            UpdateProfessionLevelingCollectionListView();
            UpdateTaskListView();
            UpdateSuppliesListView();

            _modified = false;
        }

        private void NWO_PLRemove_Button_Click(object sender, EventArgs e)
        {
            if (NWO_PLCollection_ListView.SelectedIndex > -1)
            {
                _plCollection.ProfessionLevelingInfo.RemoveAt(NWO_PLCollection_ListView.SelectedIndex);

                UpdateProfessionLevelingCollectionListView();
                UpdateTaskListView();
                UpdateSuppliesListView();

                _modified = false;
            }
        }

        private void NWO_PLUpdate_Button_Click(object sender, EventArgs e)
        {
            if (NWO_PLCollection_ListView.SelectedIndex > -1)
            {
                _professionLeveling.Start = Convert.ToInt32(NWO_PLStartLevel_Textbox.Text);
                _professionLeveling.End = Convert.ToInt32(NWO_PLEndLevel_Textbox.Text);
                _professionLeveling.Profession = _profession;
                _professionLeveling.PurchaseSupplies = NWO_PLPurchase_Checkbox.Checked;

                _plCollection.ProfessionLevelingInfo[NWO_PLCollection_ListView.SelectedIndex] = _professionLeveling;

                _professionLeveling = new NWO_ProfessionLeveling();

                UpdateProfessionLevelingCollectionListView();
                UpdateTaskListView();
                UpdateSuppliesListView();

                _modified = false;
            }
        }

        private void NWO_PLClear_Button_Click(object sender, EventArgs e)
        {
            _plCollection.ProfessionLevelingInfo.Clear();

            UpdateProfessionLevelingCollectionListView();
            UpdateTaskListView();
            UpdateSuppliesListView();

            _modified = false;
        }

        private void NWO_PLCollection_ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NWO_PLCollection_ListView.SelectedIndex > -1)
            {
                System.Windows.Forms.DialogResult tempResults;
                if (_professionLeveling.Tasks.Count > 0 && _modified)
                    tempResults = MessageBox.Show("You are about to load a Profession Leveling List, this will delete all you have done if it has not been added to the list already. Continue??", "Warning", MessageBoxButtons.YesNo);
                else
                    tempResults = System.Windows.Forms.DialogResult.Yes;

                if (tempResults == System.Windows.Forms.DialogResult.Yes)
                {
                    _professionLeveling = new NWO_ProfessionLeveling();
                    
                    _professionLeveling.End = ((NWO_ProfessionLeveling)_plCollection.ProfessionLevelingInfo[NWO_PLCollection_ListView.SelectedIndex]).End;
                    _professionLeveling.Start = ((NWO_ProfessionLeveling)_plCollection.ProfessionLevelingInfo[NWO_PLCollection_ListView.SelectedIndex]).Start;
                    _professionLeveling.PurchaseSupplies = ((NWO_ProfessionLeveling)_plCollection.ProfessionLevelingInfo[NWO_PLCollection_ListView.SelectedIndex]).PurchaseSupplies;
                    _professionLeveling.Profession = ((NWO_ProfessionLeveling)_plCollection.ProfessionLevelingInfo[NWO_PLCollection_ListView.SelectedIndex]).Profession;

                    NWO_Task tempT = GetTaskInfo();
                    foreach (NWO_Task task in ((NWO_ProfessionLeveling)_plCollection.ProfessionLevelingInfo[NWO_PLCollection_ListView.SelectedIndex]).Tasks)
                    {
                        PopulateTaskInfo(task);

                        NWO_Task tTask = new NWO_Task();
                        tTask = GetTaskInfo();
                        _professionLeveling.Tasks.Add(tTask);
                    }
                    PopulateTaskInfo(tempT);

                    NWO_Supplies tempS = GetSupplyInfo();
                    foreach (NWO_Supplies supp in ((NWO_ProfessionLeveling)_plCollection.ProfessionLevelingInfo[NWO_PLCollection_ListView.SelectedIndex]).Supplies)
                    {
                        PopulateSupplyInfo(supp);

                        NWO_Supplies tSupp = new NWO_Supplies();
                        tSupp = GetSupplyInfo();
                        _professionLeveling.Supplies.Add(tSupp);
                    }
                    PopulateSupplyInfo(tempS);

                    NWO_PLStartLevel_Textbox.Text = _professionLeveling.Start.ToString();
                    NWO_PLEndLevel_Textbox.Text = _professionLeveling.End.ToString();
                    NWO_PLPurchase_Checkbox.Checked = _professionLeveling.PurchaseSupplies;

                    UpdateTaskListView();
                    UpdateSuppliesListView();
                }
                else
                    NWO_PLCollection_ListView.SelectedIndex = -1;
            }
        }

        private void NWO_PLCollection_ListView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                NWO_PLCollection_ListView.SelectedIndex = -1;

                UpdateTaskListView();
                UpdateProfessionLevelingCollectionListView();
            }
        }

        private void NWO_PLSave_Button_Click(object sender, EventArgs e)
        {
            String _fileLoc = String.Format(@"{0}\Config\Profession Leveling\Scripts", Directory.GetCurrentDirectory());
            if (!Directory.Exists(_fileLoc))
            {
                if (!Directory.Exists(@"Config\Profession Leveling"))
                    Directory.CreateDirectory(@"Config\Profession Leveling");

                Directory.CreateDirectory(@"Config\Profession Leveling\Scripts");
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = _fileLoc;
            sfd.Filter = "PL Script (*.pls)|*.pls";
            sfd.FilterIndex = 1;
            sfd.FileName = "Profession Leveling Script";

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.OpenFile());

                sw.WriteLine(_curVersion.ToString());

                _plCollection.WriteToConfig(sw, _curVersion);

                sw.Close();
            }
        }

        private void NWO_PLLoad_Button_Click(object sender, EventArgs e)
        {
            String _fileLoc = String.Format(@"{0}\Config\Profession Leveling\Scripts", Directory.GetCurrentDirectory());
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = _fileLoc;
            ofd.Filter = "PL Script (*.pls)|*.pls";
            ofd.FilterIndex = 1;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.OpenFile());

                Version tempVer = new Version(sr.ReadLine());

                _plCollection.ReadFromConfig(sr, tempVer);

                sr.Close();

                UpdateProfessionLevelingCollectionListView();
                UpdateTaskListView();
                UpdateSuppliesListView();
            }
        }

        private void NWO_PLTaskListSave_Button_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"Config\Profession Leveling\Task Leveling Queue"))
            {
                if (!Directory.Exists(@"Config\Profession Leveling"))
                    Directory.CreateDirectory(@"Config\Profession Leveling");

                Directory.CreateDirectory(@"Config\Profession Leveling\Task Leveling Queue");
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @"Config\Profession Leveling\Task Leveling Queue";
            sfd.Filter = "PL Task Queues (*.pltq)|*.pltq";
            sfd.FilterIndex = 1;
            sfd.FileName = "Profession Leveling Task Queue";

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.OpenFile());

                sw.WriteLine(_curVersion.ToString());

                _professionLeveling.WriteToConfig(sw, _curVersion);

                sw.Close();
            }
        }

        private void NWO_PLTaskListLoad_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"Config\Profession Leveling\Task Leveling Queue";
            ofd.Filter = "PL Task Queues (*.pltq)|*.pltq";
            ofd.FilterIndex = 1;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.OpenFile());

                Version tempVer = new Version(sr.ReadLine());

                _professionLeveling.ReadFromConfig(sr, tempVer);

                sr.Close();

                UpdateTaskListView();
                UpdateSuppliesListView();
            }
        }

        private void NWO_PLSaveSupply_Button_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"Config\Profession Leveling\Supply Purchase Information"))
            {
                if (!Directory.Exists(@"Config\Profession Leveling"))
                    Directory.CreateDirectory(@"Config\Profession Leveling");

                Directory.CreateDirectory(@"Config\Profession Leveling\Supply Purchase Information");
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @"Config\Profession Leveling\Supply Purchase Information";
            sfd.Filter = "Supply Purchase Info (*.spi)|*.spi";
            sfd.FilterIndex = 1;
            sfd.FileName = "Supply Purchase Information";

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.OpenFile());

                sw.WriteLine(_curVersion.ToString());

                sw.WriteLine(_professionLeveling.Supplies.Count);

                foreach (NWO_Supplies tempSup in _professionLeveling.Supplies)
                {
                    tempSup.WriteToConfig(sw, _curVersion);
                }

                sw.Close();
            }
        }

        private void NWO_PLLoadSupply_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"Config\Profession Leveling\Supply Purchase Information";
            ofd.Filter = "Supply Purchase Info (*.spi)|*.spi";
            ofd.FilterIndex = 1;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.OpenFile());

                Version tempVer = new Version(sr.ReadLine());

                _professionLeveling.Supplies.Clear();

                int tempI = Convert.ToInt32(sr.ReadLine());

                for (int i = 0; i < tempI; i++)
                {
                    NWO_Supplies tempSup = new NWO_Supplies();
                    tempSup.ReadFromConfig(sr, tempVer);

                    _professionLeveling.Supplies.Add(tempSup);
                }

                sr.Close();

                UpdateSuppliesListView();
            }
        }
    }
}
