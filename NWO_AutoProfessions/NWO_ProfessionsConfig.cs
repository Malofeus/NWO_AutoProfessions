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
using System.Reflection;

namespace NWO_AutoProfessions
{
    public partial class NWO_ProfessionsConfig : Form
    {
        private ArrayList                                           _CharacterList;

        private String                                              _AutoProfessionFileLoc;

        private Version                                             _CurVersion;

        public NWO_ProfessionsConfig()
        {
            InitializeComponent();

            _CharacterList              = new ArrayList();

            PopulateAdditionalTaskComboBox();
        }

        public ArrayList CharacterList
        {
            get
            {
                return _CharacterList;
            }
            set
            {
                _CharacterList = value;
                if (_CharacterList.Count > 0)
                {
                    PopulateCharacterComboBox();
                    UpdateListViews();
                    UpdateAdditionalTasksListView();

                    foreach (NWO_Character nwoChar in _CharacterList)
                    {
                        UpdateTaskInformation(nwoChar.Slot1);
                        UpdateTaskInformation(nwoChar.Slot2);
                        UpdateTaskInformation(nwoChar.Slot3);
                        UpdateTaskInformation(nwoChar.Slot4);
                        UpdateTaskInformation(nwoChar.Slot5);
                        UpdateTaskInformation(nwoChar.Slot6);
                        UpdateTaskInformation(nwoChar.Slot7);
                        UpdateTaskInformation(nwoChar.Slot8);
                        UpdateTaskInformation(nwoChar.Slot9);
                    }
                }
            }
        }

        public String AutoProfessionFileLoc
        {
            get
            {
                return _AutoProfessionFileLoc;
            }
            set
            {
                _AutoProfessionFileLoc = value;
            }
        }

        public Version CurVersion
        {
            set
            {
                _CurVersion = value;
            }
        }

        private NWO_Task GetTaskInfo()
        {
            NWO_Asset tempAsset = new NWO_Asset();

            tempAsset.Rank1Common = NWO_OptionalAssets_Rank1Common_CheckBox.Checked;
            tempAsset.Rank2Common = NWO_OptionalAssets_Rank2Common_CheckBox.Checked;
            tempAsset.Rank3Common = NWO_OptionalAssets_Rank3Common_CheckBox.Checked;
            tempAsset.Rank3Uncommon = NWO_OptionalAssets_Rank3Uncommon_CheckBox.Checked;
            tempAsset.Rank3Rare = NWO_OptionalAssets_Rank3Rare_CheckBox.Checked;
            tempAsset.Rank3Epic = NWO_OptionalAssets_Rank3Epic_CheckBox.Checked;
            tempAsset.Rank1Common_Quantity = Convert.ToInt32(NWO_OptionalAssets_Rank1Common_TextBox.Text);
            tempAsset.Rank2Common_Quantity = Convert.ToInt32(NWO_OptionalAssets_Rank2Common_TextBox.Text);
            tempAsset.Rank3Common_Quantity = Convert.ToInt32(NWO_OptionalAssets_Rank3Common_TextBox.Text);
            tempAsset.Rank3Uncommon_Quantity = Convert.ToInt32(NWO_OptionalAssets_Rank3Uncommon_TextBox.Text);
            tempAsset.Rank3Rare_Quantity = Convert.ToInt32(NWO_OptionalAssets_Rank3Rare_TextBox.Text);
            tempAsset.Rank3Epic_Quantity = Convert.ToInt32(NWO_OptionalAssets_Rank3Epic_TextBox.Text);

            NWO_Task tempTask = new NWO_Task();
            tempTask.TaskProfession = NWO_Profession_ComboBox.SelectedItem.ToString();
            if (NWO_Task_ComboBox.SelectedItem.ToString() != String.Empty && !NWO_Task_ComboBox.SelectedItem.ToString().Contains("Rares:"))
            {
                tempTask.TaskFullName = NWO_Task_ComboBox.SelectedItem.ToString();
                foreach (NWO_Profession prof in NWO_Defines.ArrayLists.ProfessionInformation)
                {
                    if (tempTask.TaskProfession == prof.Name)
                    {
                        if (NWO_Task_ComboBox.SelectedIndex >= prof.Rare)
                        {
                            tempTask.TaskURL = prof.TaskNameURLList[NWO_Task_ComboBox.SelectedIndex - 2].ToString();
                            tempTask.Rare = true;
                        }
                        else
                        {
                            tempTask.TaskURL = prof.TaskNameURLList[NWO_Task_ComboBox.SelectedIndex].ToString();
                            tempTask.Rare = false;
                        }
                        break;
                    }
                }
            }
            tempTask.AmntToDo = Convert.ToInt32(NWO_RepeatTaskNum_TextBox.Text);

            tempTask.Assets = tempAsset;

            return tempTask;
        }

        private void ClearSlotInformation(Int32 slotNum)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                switch (slotNum)
                {
                    case 1:
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot1 = new NWO_ProfessionLevelingCollection();
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot1 = false;
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1 = new NWO_TaskCollection();
                        break;
                    case 2:
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot2 = new NWO_ProfessionLevelingCollection();
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot2 = false;
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2 = new NWO_TaskCollection();
                        break;
                    case 3:
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot3 = new NWO_ProfessionLevelingCollection();
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot3 = false;
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3 = new NWO_TaskCollection();
                        break;
                    case 4:
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot4 = new NWO_ProfessionLevelingCollection();
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot4 = false;
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4 = new NWO_TaskCollection();
                        break;
                    case 5:
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot5 = new NWO_ProfessionLevelingCollection();
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot5 = false;
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5 = new NWO_TaskCollection();
                        break;
                    case 6:
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot6 = new NWO_ProfessionLevelingCollection();
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot6 = false;
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6 = new NWO_TaskCollection();
                        break;
                    case 7:
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot7 = new NWO_ProfessionLevelingCollection();
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot7 = false;
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7 = new NWO_TaskCollection();
                        break;
                    case 8:
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot8 = new NWO_ProfessionLevelingCollection();
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot8 = false;
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8 = new NWO_TaskCollection();
                        break;
                    case 9:
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot9 = new NWO_ProfessionLevelingCollection();
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot9 = false;
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9 = new NWO_TaskCollection();
                        break;
                }
            }
        }

        private void UpdateTaskInformation(NWO_TaskCollection curTaskList)
        {
            if (_CurVersion.Major > 1 || _CurVersion.Minor > 3)
            {
                // Need to go through each task and update whether it is a rare task or not.
                foreach (NWO_Task nwoTask in curTaskList.Tasks)
                {
                    foreach (NWO_Profession prof in NWO_Defines.ArrayLists.ProfessionInformation)
                    {
                        if (nwoTask.TaskProfession == prof.Name)
                        {
                            if (prof.TaskNameList.IndexOf(nwoTask.TaskFullName, 0) >= prof.Rare)
                                nwoTask.Rare = true;
                            else
                                nwoTask.Rare = false;
                        }
                    }
                }
            }
        }

        private void MoveTaskUp(Int32 taskLoc, ArrayList tasks)
        {
            NWO_Task curTask = (NWO_Task)tasks[taskLoc];
            NWO_Task aboveTask = (NWO_Task)tasks[taskLoc - 1];

            tasks[taskLoc - 1] = curTask;
            tasks[taskLoc] = aboveTask;
        }

        private void MoveTaskDown(Int32 taskLoc, ArrayList tasks)
        {
            NWO_Task curTask = (NWO_Task)tasks[taskLoc];
            NWO_Task downTask = (NWO_Task)tasks[taskLoc + 1];

            tasks[taskLoc + 1] = curTask;
            tasks[taskLoc] = downTask;
        }

        private void PopulateCharacterComboBox()
        {
            NWO_CharacterList_ComboBox.Items.Clear();
            foreach (NWO_Character character in _CharacterList)
            {
                NWO_CharacterList_ComboBox.Items.Add(character.NWO_CharacterName);
            }

            NWO_CharacterList_ComboBox.SelectedIndex = 0;
        }

        private void PopulateAssets(String profession)
        {
            foreach (NWO_Craftsmen craftsmen in NWO_Defines.ArrayLists.Craftsmen)
            {
                if (craftsmen.Profession.Contains(profession))
                {
                    NWO_OptionalAssets_Rank1Common_CheckBox.Text = craftsmen.Rank1Common;
                    NWO_OptionalAssets_Rank2Common_CheckBox.Text = craftsmen.Rank2Common;
                    NWO_OptionalAssets_Rank3Common_CheckBox.Text = craftsmen.Rank3Common;
                    NWO_OptionalAssets_Rank3Uncommon_CheckBox.Text = craftsmen.Rank3Uncommon;
                    NWO_OptionalAssets_Rank3Rare_CheckBox.Text = craftsmen.Rank3Rare;
                    NWO_OptionalAssets_Rank3Epic_CheckBox.Text = craftsmen.Rank3Epic;

                    break;
                }
            }

            NWO_OptionalAssets_Rank1Common_TextBox.Text = "0";
            NWO_OptionalAssets_Rank2Common_TextBox.Text = "0";
            NWO_OptionalAssets_Rank3Common_TextBox.Text = "0";
            NWO_OptionalAssets_Rank3Uncommon_TextBox.Text = "0";
            NWO_OptionalAssets_Rank3Rare_TextBox.Text = "0";
            NWO_OptionalAssets_Rank3Epic_TextBox.Text = "0";
        }

        private void PopulateAdditionalTaskComboBox()
        {
            NWO_AdditionalTask_ComboBox.Items.Clear();
            NWO_AdditionalTask_ComboBox.Items.Add(NWO_Defines.AdditionalTasks.RefineAstralDiamonds);
            NWO_AdditionalTask_ComboBox.Items.Add(NWO_Defines.AdditionalTasks.SellItems);
            NWO_AdditionalTask_ComboBox.Items.Add(NWO_Defines.AdditionalTasks.OpenChests);
        }

        private void ClearListViewsItems()
        {
            NWO_SlotTask_ListView_1.Items.Clear();
            NWO_SlotTask_ListView_2.Items.Clear();
            NWO_SlotTask_ListView_3.Items.Clear();
            NWO_SlotTask_ListView_4.Items.Clear();
            NWO_SlotTask_ListView_5.Items.Clear();
            NWO_SlotTask_ListView_6.Items.Clear();
            NWO_SlotTask_ListView_7.Items.Clear();
            NWO_SlotTask_ListView_8.Items.Clear();
            NWO_SlotTask_ListView_9.Items.Clear();
        }

        private void ClearListViewsSelected(int slot)
        {
            if (slot != 1)
                NWO_SlotTask_ListView_1.ClearSelected();
            if (slot != 2)
                NWO_SlotTask_ListView_2.ClearSelected();
            if (slot != 3)
                NWO_SlotTask_ListView_3.ClearSelected();
            if (slot != 4)
                NWO_SlotTask_ListView_4.ClearSelected();
            if (slot != 5)
                NWO_SlotTask_ListView_5.ClearSelected();
            if (slot != 6)
                NWO_SlotTask_ListView_6.ClearSelected();
            if (slot != 7)
                NWO_SlotTask_ListView_7.ClearSelected();
            if (slot != 8)
                NWO_SlotTask_ListView_8.ClearSelected();
            if (slot != 9)
                NWO_SlotTask_ListView_9.ClearSelected();
        }

        private void ClearSlotCheckboxes()
        {
            NWO_Slot1_CheckBox.Checked = false;
            NWO_Slot2_CheckBox.Checked = false;
            NWO_Slot3_CheckBox.Checked = false;
            NWO_Slot4_CheckBox.Checked = false;
            NWO_Slot5_CheckBox.Checked = false;
            NWO_Slot6_CheckBox.Checked = false;
            NWO_Slot7_CheckBox.Checked = false;
            NWO_Slot8_CheckBox.Checked = false;
            NWO_Slot9_CheckBox.Checked = false;
        }

        private void UpdateListViews()
        {
            ClearListViewsItems();
            if (_CharacterList.Count == 0 || NWO_CharacterList_ComboBox.SelectedIndex == -1)
            {
                return;
            }

            #region Slot 1
            if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot1)
            {
                NWO_AdvanceSlot1_Button.Enabled = true;
                NWO_SlotTask_ListView_1.Enabled = false;

                NWO_SlotTask_ListView_1.Items.Add(((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot1.DisplayInfo);
            }
            else
            {
                NWO_SlotTask_ListView_1.Enabled = true;
                if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks.Count > 0)
                    NWO_AdvanceSlot1_Button.Enabled = false;
                else
                    NWO_AdvanceSlot1_Button.Enabled = true;

                foreach (NWO_Task task in ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks)
                {
                    NWO_SlotTask_ListView_1.Items.Add(task.DisplayName);
                }
                NWO_RepeatSlot_Checkbox_1.Checked = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.RepeatTaskCycle;
            }
            #endregion

            #region Slot 2
            if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot2)
            {
                NWO_AdvanceSlot2_Button.Enabled = true;
                NWO_SlotTask_ListView_2.Enabled = false;

                NWO_SlotTask_ListView_2.Items.Add(((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot2.DisplayInfo);
            }
            else
            {
                NWO_SlotTask_ListView_2.Enabled = true;
                if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks.Count > 0)
                    NWO_AdvanceSlot2_Button.Enabled = false;
                else
                    NWO_AdvanceSlot2_Button.Enabled = true;

                foreach (NWO_Task task in ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks)
                {
                    NWO_SlotTask_ListView_2.Items.Add(task.DisplayName);
                }
                NWO_RepeatSlot_Checkbox_2.Checked = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.RepeatTaskCycle;
            }
            #endregion

            #region Slot 3
            if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot3)
            {
                NWO_AdvanceSlot3_Button.Enabled = true;
                NWO_SlotTask_ListView_3.Enabled = false;

                NWO_SlotTask_ListView_3.Items.Add(((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot3.DisplayInfo);
            }
            else
            {
                NWO_SlotTask_ListView_3.Enabled = true;
                if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks.Count > 0)
                    NWO_AdvanceSlot3_Button.Enabled = false;
                else
                    NWO_AdvanceSlot3_Button.Enabled = true;

                foreach (NWO_Task task in ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks)
                {
                    NWO_SlotTask_ListView_3.Items.Add(task.DisplayName);
                }
                NWO_RepeatSlot_Checkbox_3.Checked = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.RepeatTaskCycle;
            }
            #endregion

            #region Slot 4
            if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot4)
            {
                NWO_AdvanceSlot4_Button.Enabled = true;
                NWO_SlotTask_ListView_4.Enabled = false;

                NWO_SlotTask_ListView_4.Items.Add(((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot4.DisplayInfo);
            }
            else
            {
                NWO_SlotTask_ListView_4.Enabled = true;
                if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks.Count > 0)
                    NWO_AdvanceSlot4_Button.Enabled = false;
                else
                    NWO_AdvanceSlot4_Button.Enabled = true;

                foreach (NWO_Task task in ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks)
                {
                    NWO_SlotTask_ListView_4.Items.Add(task.DisplayName);
                }
                NWO_RepeatSlot_Checkbox_4.Checked = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.RepeatTaskCycle;
            }
            #endregion

            #region Slot 5
            if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot5)
            {
                NWO_AdvanceSlot5_Button.Enabled = true;
                NWO_SlotTask_ListView_5.Enabled = false;

                NWO_SlotTask_ListView_5.Items.Add(((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot5.DisplayInfo);
            }
            else
            {
                NWO_SlotTask_ListView_5.Enabled = true;
                if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks.Count > 0)
                    NWO_AdvanceSlot5_Button.Enabled = false;
                else
                    NWO_AdvanceSlot5_Button.Enabled = true;

                foreach (NWO_Task task in ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks)
                {
                    NWO_SlotTask_ListView_5.Items.Add(task.DisplayName);
                }
                NWO_RepeatSlot_Checkbox_5.Checked = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.RepeatTaskCycle;
            }
            #endregion

            #region Slot 6
            if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot6)
            {
                NWO_AdvanceSlot6_Button.Enabled = true;
                NWO_SlotTask_ListView_6.Enabled = false;

                NWO_SlotTask_ListView_6.Items.Add(((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot6.DisplayInfo);
            }
            else
            {
                NWO_SlotTask_ListView_6.Enabled = true;
                if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks.Count > 0)
                    NWO_AdvanceSlot6_Button.Enabled = false;
                else
                    NWO_AdvanceSlot6_Button.Enabled = true;

                foreach (NWO_Task task in ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks)
                {
                    NWO_SlotTask_ListView_6.Items.Add(task.DisplayName);
                }
                NWO_RepeatSlot_Checkbox_6.Checked = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.RepeatTaskCycle;
            }
            #endregion

            #region Slot 7
            if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot7)
            {
                NWO_AdvanceSlot7_Button.Enabled = true;
                NWO_SlotTask_ListView_7.Enabled = false;

                NWO_SlotTask_ListView_7.Items.Add(((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot7.DisplayInfo);
            }
            else
            {
                NWO_SlotTask_ListView_7.Enabled = true;
                if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks.Count > 0)
                    NWO_AdvanceSlot7_Button.Enabled = false;
                else
                    NWO_AdvanceSlot7_Button.Enabled = true;

                foreach (NWO_Task task in ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks)
                {
                    NWO_SlotTask_ListView_7.Items.Add(task.DisplayName);
                }
                NWO_RepeatSlot_Checkbox_7.Checked = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.RepeatTaskCycle;
            }
            #endregion

            #region Slot 8
            if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot8)
            {

                NWO_AdvanceSlot8_Button.Enabled = true;
                NWO_SlotTask_ListView_8.Enabled = false;

                NWO_SlotTask_ListView_8.Items.Add(((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot8.DisplayInfo);
            }
            else
            {
                NWO_SlotTask_ListView_8.Enabled = true;
                if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks.Count > 0)
                    NWO_AdvanceSlot8_Button.Enabled = false;
                else
                    NWO_AdvanceSlot8_Button.Enabled = true;

                foreach (NWO_Task task in ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks)
                {
                    NWO_SlotTask_ListView_8.Items.Add(task.DisplayName);
                }
                NWO_RepeatSlot_Checkbox_8.Checked = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.RepeatTaskCycle;
            }
            #endregion

            #region Slot 9
            if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot9)
            {
                NWO_AdvanceSlot9_Button.Enabled = true;
                NWO_SlotTask_ListView_9.Enabled = false;

                NWO_SlotTask_ListView_9.Items.Add(((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot9.DisplayInfo);
            }
            else
            {
                NWO_SlotTask_ListView_9.Enabled = true;
                if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks.Count > 0)
                    NWO_AdvanceSlot9_Button.Enabled = false;
                else
                    NWO_AdvanceSlot9_Button.Enabled = true;

                foreach (NWO_Task task in ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks)
                {
                    NWO_SlotTask_ListView_9.Items.Add(task.DisplayName);
                }
                NWO_RepeatSlot_Checkbox_9.Checked = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.RepeatTaskCycle;
            }
            #endregion
        }

        private void UpdateAdditionalTasksListView()
        {
            NWO_AdditionalTasks_ListBox.Items.Clear();
            
            if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).RefineAstralDiamonds)
                NWO_AdditionalTasks_ListBox.Items.Add(NWO_Defines.AdditionalTasks.RefineAstralDiamonds);

            if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_Sell_Junk)
                NWO_AdditionalTasks_ListBox.Items.Add(NWO_Defines.AdditionalTasks.SellItems_Junk);

            if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_Sell_NonMagicalEquipment)
                NWO_AdditionalTasks_ListBox.Items.Add(NWO_Defines.AdditionalTasks.SellItems_NonMagicalEquipment);

            if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_OpenBoxes)
                NWO_AdditionalTasks_ListBox.Items.Add(NWO_Defines.AdditionalTasks.OpenChests);
        }

        private void SetSelected(NWO_Task task)
        {
            int curSel = NWO_Profession_ComboBox.FindStringExact(task.TaskProfession);
            if (curSel != -1)
                NWO_Profession_ComboBox.SelectedIndex = curSel;
            else
                NWO_Profession_ComboBox.SelectedIndex = 0;

            curSel = NWO_Task_ComboBox.FindStringExact(task.TaskFullName);
            if (curSel != -1)
                NWO_Task_ComboBox.SelectedIndex = curSel;
            else
                NWO_Task_ComboBox.SelectedIndex = 0;
        }

        private void NWO_CharacterList_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                UpdateListViews();
                UpdateAdditionalTasksListView();
            }
        }

        private void NWO_CheckAllSlots_Button_Click(object sender, EventArgs e)
        {
            NWO_RepeatSlot_Checkbox_1.Checked = true;
            NWO_RepeatSlot_Checkbox_2.Checked = true;
            NWO_RepeatSlot_Checkbox_3.Checked = true;
            NWO_RepeatSlot_Checkbox_4.Checked = true;
            NWO_RepeatSlot_Checkbox_5.Checked = true;
            NWO_RepeatSlot_Checkbox_6.Checked = true;
            NWO_RepeatSlot_Checkbox_7.Checked = true;
            NWO_RepeatSlot_Checkbox_8.Checked = true;
            NWO_RepeatSlot_Checkbox_9.Checked = true;
        }

        private void NWO_ClearRepeats_Button_Click(object sender, EventArgs e)
        {
            NWO_RepeatSlot_Checkbox_1.Checked = false;
            NWO_RepeatSlot_Checkbox_2.Checked = false;
            NWO_RepeatSlot_Checkbox_3.Checked = false;
            NWO_RepeatSlot_Checkbox_4.Checked = false;
            NWO_RepeatSlot_Checkbox_5.Checked = false;
            NWO_RepeatSlot_Checkbox_6.Checked = false;
            NWO_RepeatSlot_Checkbox_7.Checked = false;
            NWO_RepeatSlot_Checkbox_8.Checked = false;
            NWO_RepeatSlot_Checkbox_9.Checked = false;
        }

        private void NWO_RepeatSlot_Checkbox_1_CheckedChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.RepeatTaskCycle = NWO_RepeatSlot_Checkbox_1.Checked;
            }
        }

        private void NWO_RepeatSlot_Checkbox_2_CheckedChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.RepeatTaskCycle = NWO_RepeatSlot_Checkbox_2.Checked;
            }
        }

        private void NWO_RepeatSlot_Checkbox_3_CheckedChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.RepeatTaskCycle = NWO_RepeatSlot_Checkbox_3.Checked;
            }
        }

        private void NWO_RepeatSlot_Checkbox_4_CheckedChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.RepeatTaskCycle = NWO_RepeatSlot_Checkbox_4.Checked;
            }
        }

        private void NWO_RepeatSlot_Checkbox_5_CheckedChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.RepeatTaskCycle = NWO_RepeatSlot_Checkbox_5.Checked;
            }
        }

        private void NWO_RepeatSlot_Checkbox_6_CheckedChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.RepeatTaskCycle = NWO_RepeatSlot_Checkbox_6.Checked;
            }
        }

        private void NWO_RepeatSlot_Checkbox_7_CheckedChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.RepeatTaskCycle = NWO_RepeatSlot_Checkbox_7.Checked;
            }
        }

        private void NWO_RepeatSlot_Checkbox_8_CheckedChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.RepeatTaskCycle = NWO_RepeatSlot_Checkbox_8.Checked;
            }
        }

        private void NWO_RepeatSlot_Checkbox_9_CheckedChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.RepeatTaskCycle = NWO_RepeatSlot_Checkbox_9.Checked;
            }
        }

        private void NWO_SlotTask_ListView_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1 && NWO_SlotTask_ListView_1.SelectedIndex > -1 )
            {
                ClearListViewsSelected(1);

                SetSelected((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks[NWO_SlotTask_ListView_1.SelectedIndex]);

                NWO_RepeatTaskNum_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks[NWO_SlotTask_ListView_1.SelectedIndex]).AmntToDo.ToString();

                NWO_OptionalAssets_Rank1Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks[NWO_SlotTask_ListView_1.SelectedIndex]).Assets.Rank1Common;
                NWO_OptionalAssets_Rank1Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks[NWO_SlotTask_ListView_1.SelectedIndex]).Assets.Rank1Common_Quantity.ToString();

                NWO_OptionalAssets_Rank2Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks[NWO_SlotTask_ListView_1.SelectedIndex]).Assets.Rank2Common;
                NWO_OptionalAssets_Rank2Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks[NWO_SlotTask_ListView_1.SelectedIndex]).Assets.Rank2Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks[NWO_SlotTask_ListView_1.SelectedIndex]).Assets.Rank3Common;
                NWO_OptionalAssets_Rank3Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks[NWO_SlotTask_ListView_1.SelectedIndex]).Assets.Rank3Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Uncommon_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks[NWO_SlotTask_ListView_1.SelectedIndex]).Assets.Rank3Uncommon;
                NWO_OptionalAssets_Rank3Uncommon_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks[NWO_SlotTask_ListView_1.SelectedIndex]).Assets.Rank3Uncommon_Quantity.ToString();

                NWO_OptionalAssets_Rank3Rare_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks[NWO_SlotTask_ListView_1.SelectedIndex]).Assets.Rank3Rare;
                NWO_OptionalAssets_Rank3Rare_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks[NWO_SlotTask_ListView_1.SelectedIndex]).Assets.Rank3Rare_Quantity.ToString();

                NWO_OptionalAssets_Rank3Epic_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks[NWO_SlotTask_ListView_1.SelectedIndex]).Assets.Rank3Epic;
                NWO_OptionalAssets_Rank3Epic_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks[NWO_SlotTask_ListView_1.SelectedIndex]).Assets.Rank3Epic_Quantity.ToString();
            }
        }

        private void NWO_SlotTask_ListView_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1 && NWO_SlotTask_ListView_2.SelectedIndex > -1 )
            {
                ClearListViewsSelected(2);

                SetSelected((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks[NWO_SlotTask_ListView_2.SelectedIndex]);

                NWO_RepeatTaskNum_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks[NWO_SlotTask_ListView_2.SelectedIndex]).AmntToDo.ToString();

                NWO_OptionalAssets_Rank1Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks[NWO_SlotTask_ListView_2.SelectedIndex]).Assets.Rank1Common;
                NWO_OptionalAssets_Rank1Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks[NWO_SlotTask_ListView_2.SelectedIndex]).Assets.Rank1Common_Quantity.ToString();

                NWO_OptionalAssets_Rank2Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks[NWO_SlotTask_ListView_2.SelectedIndex]).Assets.Rank2Common;
                NWO_OptionalAssets_Rank2Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks[NWO_SlotTask_ListView_2.SelectedIndex]).Assets.Rank2Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks[NWO_SlotTask_ListView_2.SelectedIndex]).Assets.Rank3Common;
                NWO_OptionalAssets_Rank3Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks[NWO_SlotTask_ListView_2.SelectedIndex]).Assets.Rank3Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Uncommon_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks[NWO_SlotTask_ListView_2.SelectedIndex]).Assets.Rank3Uncommon;
                NWO_OptionalAssets_Rank3Uncommon_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks[NWO_SlotTask_ListView_2.SelectedIndex]).Assets.Rank3Uncommon_Quantity.ToString();

                NWO_OptionalAssets_Rank3Rare_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks[NWO_SlotTask_ListView_2.SelectedIndex]).Assets.Rank3Rare;
                NWO_OptionalAssets_Rank3Rare_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks[NWO_SlotTask_ListView_2.SelectedIndex]).Assets.Rank3Rare_Quantity.ToString();

                NWO_OptionalAssets_Rank3Epic_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks[NWO_SlotTask_ListView_2.SelectedIndex]).Assets.Rank3Epic;
                NWO_OptionalAssets_Rank3Epic_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks[NWO_SlotTask_ListView_2.SelectedIndex]).Assets.Rank3Epic_Quantity.ToString();
            }
        }

        private void NWO_SlotTask_ListView_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1 && NWO_SlotTask_ListView_3.SelectedIndex > -1 )
            {
                ClearListViewsSelected(3);

                SetSelected((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks[NWO_SlotTask_ListView_3.SelectedIndex]);

                NWO_RepeatTaskNum_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks[NWO_SlotTask_ListView_3.SelectedIndex]).AmntToDo.ToString();

                NWO_OptionalAssets_Rank1Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks[NWO_SlotTask_ListView_3.SelectedIndex]).Assets.Rank1Common;
                NWO_OptionalAssets_Rank1Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks[NWO_SlotTask_ListView_3.SelectedIndex]).Assets.Rank1Common_Quantity.ToString();

                NWO_OptionalAssets_Rank2Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks[NWO_SlotTask_ListView_3.SelectedIndex]).Assets.Rank2Common;
                NWO_OptionalAssets_Rank2Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks[NWO_SlotTask_ListView_3.SelectedIndex]).Assets.Rank2Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks[NWO_SlotTask_ListView_3.SelectedIndex]).Assets.Rank3Common;
                NWO_OptionalAssets_Rank3Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks[NWO_SlotTask_ListView_3.SelectedIndex]).Assets.Rank3Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Uncommon_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks[NWO_SlotTask_ListView_3.SelectedIndex]).Assets.Rank3Uncommon;
                NWO_OptionalAssets_Rank3Uncommon_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks[NWO_SlotTask_ListView_3.SelectedIndex]).Assets.Rank3Uncommon_Quantity.ToString();

                NWO_OptionalAssets_Rank3Rare_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks[NWO_SlotTask_ListView_3.SelectedIndex]).Assets.Rank3Rare;
                NWO_OptionalAssets_Rank3Rare_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks[NWO_SlotTask_ListView_3.SelectedIndex]).Assets.Rank3Rare_Quantity.ToString();

                NWO_OptionalAssets_Rank3Epic_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks[NWO_SlotTask_ListView_3.SelectedIndex]).Assets.Rank3Epic;
                NWO_OptionalAssets_Rank3Epic_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks[NWO_SlotTask_ListView_3.SelectedIndex]).Assets.Rank3Epic_Quantity.ToString();
            }
        }

        private void NWO_SlotTask_ListView_4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1 && NWO_SlotTask_ListView_4.SelectedIndex > -1 )
            {
                ClearListViewsSelected(4);

                SetSelected((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks[NWO_SlotTask_ListView_4.SelectedIndex]);

                NWO_RepeatTaskNum_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks[NWO_SlotTask_ListView_4.SelectedIndex]).AmntToDo.ToString();

                NWO_OptionalAssets_Rank1Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks[NWO_SlotTask_ListView_4.SelectedIndex]).Assets.Rank1Common;
                NWO_OptionalAssets_Rank1Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks[NWO_SlotTask_ListView_4.SelectedIndex]).Assets.Rank1Common_Quantity.ToString();

                NWO_OptionalAssets_Rank2Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks[NWO_SlotTask_ListView_4.SelectedIndex]).Assets.Rank2Common;
                NWO_OptionalAssets_Rank2Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks[NWO_SlotTask_ListView_4.SelectedIndex]).Assets.Rank2Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks[NWO_SlotTask_ListView_4.SelectedIndex]).Assets.Rank3Common;
                NWO_OptionalAssets_Rank3Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks[NWO_SlotTask_ListView_4.SelectedIndex]).Assets.Rank3Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Uncommon_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks[NWO_SlotTask_ListView_4.SelectedIndex]).Assets.Rank3Uncommon;
                NWO_OptionalAssets_Rank3Uncommon_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks[NWO_SlotTask_ListView_4.SelectedIndex]).Assets.Rank3Uncommon_Quantity.ToString();

                NWO_OptionalAssets_Rank3Rare_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks[NWO_SlotTask_ListView_4.SelectedIndex]).Assets.Rank3Rare;
                NWO_OptionalAssets_Rank3Rare_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks[NWO_SlotTask_ListView_4.SelectedIndex]).Assets.Rank3Rare_Quantity.ToString();

                NWO_OptionalAssets_Rank3Epic_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks[NWO_SlotTask_ListView_4.SelectedIndex]).Assets.Rank3Epic;
                NWO_OptionalAssets_Rank3Epic_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks[NWO_SlotTask_ListView_4.SelectedIndex]).Assets.Rank3Epic_Quantity.ToString();
            }
        }

        private void NWO_SlotTask_ListView_5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1 && NWO_SlotTask_ListView_5.SelectedIndex > -1 )
            {
                ClearListViewsSelected(5);

                SetSelected((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks[NWO_SlotTask_ListView_5.SelectedIndex]);

                NWO_RepeatTaskNum_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks[NWO_SlotTask_ListView_5.SelectedIndex]).AmntToDo.ToString();

                NWO_OptionalAssets_Rank1Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks[NWO_SlotTask_ListView_5.SelectedIndex]).Assets.Rank1Common;
                NWO_OptionalAssets_Rank1Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks[NWO_SlotTask_ListView_5.SelectedIndex]).Assets.Rank1Common_Quantity.ToString();

                NWO_OptionalAssets_Rank2Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks[NWO_SlotTask_ListView_5.SelectedIndex]).Assets.Rank2Common;
                NWO_OptionalAssets_Rank2Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks[NWO_SlotTask_ListView_5.SelectedIndex]).Assets.Rank2Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks[NWO_SlotTask_ListView_5.SelectedIndex]).Assets.Rank3Common;
                NWO_OptionalAssets_Rank3Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks[NWO_SlotTask_ListView_5.SelectedIndex]).Assets.Rank3Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Uncommon_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks[NWO_SlotTask_ListView_5.SelectedIndex]).Assets.Rank3Uncommon;
                NWO_OptionalAssets_Rank3Uncommon_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks[NWO_SlotTask_ListView_5.SelectedIndex]).Assets.Rank3Uncommon_Quantity.ToString();

                NWO_OptionalAssets_Rank3Rare_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks[NWO_SlotTask_ListView_5.SelectedIndex]).Assets.Rank3Rare;
                NWO_OptionalAssets_Rank3Rare_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks[NWO_SlotTask_ListView_5.SelectedIndex]).Assets.Rank3Rare_Quantity.ToString();

                NWO_OptionalAssets_Rank3Epic_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks[NWO_SlotTask_ListView_5.SelectedIndex]).Assets.Rank3Epic;
                NWO_OptionalAssets_Rank3Epic_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks[NWO_SlotTask_ListView_5.SelectedIndex]).Assets.Rank3Epic_Quantity.ToString();
            }
        }

        private void NWO_SlotTask_ListView_6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1 && NWO_SlotTask_ListView_6.SelectedIndex > -1 )
            {
                ClearListViewsSelected(6);

                SetSelected((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks[NWO_SlotTask_ListView_6.SelectedIndex]);

                NWO_RepeatTaskNum_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks[NWO_SlotTask_ListView_6.SelectedIndex]).AmntToDo.ToString();

                NWO_OptionalAssets_Rank1Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks[NWO_SlotTask_ListView_6.SelectedIndex]).Assets.Rank1Common;
                NWO_OptionalAssets_Rank1Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks[NWO_SlotTask_ListView_6.SelectedIndex]).Assets.Rank1Common_Quantity.ToString();

                NWO_OptionalAssets_Rank2Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks[NWO_SlotTask_ListView_6.SelectedIndex]).Assets.Rank2Common;
                NWO_OptionalAssets_Rank2Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks[NWO_SlotTask_ListView_6.SelectedIndex]).Assets.Rank2Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks[NWO_SlotTask_ListView_6.SelectedIndex]).Assets.Rank3Common;
                NWO_OptionalAssets_Rank3Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks[NWO_SlotTask_ListView_6.SelectedIndex]).Assets.Rank3Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Uncommon_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks[NWO_SlotTask_ListView_6.SelectedIndex]).Assets.Rank3Uncommon;
                NWO_OptionalAssets_Rank3Uncommon_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks[NWO_SlotTask_ListView_6.SelectedIndex]).Assets.Rank3Uncommon_Quantity.ToString();

                NWO_OptionalAssets_Rank3Rare_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks[NWO_SlotTask_ListView_6.SelectedIndex]).Assets.Rank3Rare;
                NWO_OptionalAssets_Rank3Rare_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks[NWO_SlotTask_ListView_6.SelectedIndex]).Assets.Rank3Rare_Quantity.ToString();

                NWO_OptionalAssets_Rank3Epic_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks[NWO_SlotTask_ListView_6.SelectedIndex]).Assets.Rank3Epic;
                NWO_OptionalAssets_Rank3Epic_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks[NWO_SlotTask_ListView_6.SelectedIndex]).Assets.Rank3Epic_Quantity.ToString();
            }
        }

        private void NWO_SlotTask_ListView_7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1 && NWO_SlotTask_ListView_7.SelectedIndex > -1 )
            {
                ClearListViewsSelected(7);

                SetSelected((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks[NWO_SlotTask_ListView_7.SelectedIndex]);

                NWO_RepeatTaskNum_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks[NWO_SlotTask_ListView_7.SelectedIndex]).AmntToDo.ToString();

                NWO_OptionalAssets_Rank1Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks[NWO_SlotTask_ListView_7.SelectedIndex]).Assets.Rank1Common;
                NWO_OptionalAssets_Rank1Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks[NWO_SlotTask_ListView_7.SelectedIndex]).Assets.Rank1Common_Quantity.ToString();

                NWO_OptionalAssets_Rank2Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks[NWO_SlotTask_ListView_7.SelectedIndex]).Assets.Rank2Common;
                NWO_OptionalAssets_Rank2Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks[NWO_SlotTask_ListView_7.SelectedIndex]).Assets.Rank2Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks[NWO_SlotTask_ListView_7.SelectedIndex]).Assets.Rank3Common;
                NWO_OptionalAssets_Rank3Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks[NWO_SlotTask_ListView_7.SelectedIndex]).Assets.Rank3Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Uncommon_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks[NWO_SlotTask_ListView_7.SelectedIndex]).Assets.Rank3Uncommon;
                NWO_OptionalAssets_Rank3Uncommon_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks[NWO_SlotTask_ListView_7.SelectedIndex]).Assets.Rank3Uncommon_Quantity.ToString();

                NWO_OptionalAssets_Rank3Rare_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks[NWO_SlotTask_ListView_7.SelectedIndex]).Assets.Rank3Rare;
                NWO_OptionalAssets_Rank3Rare_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks[NWO_SlotTask_ListView_7.SelectedIndex]).Assets.Rank3Rare_Quantity.ToString();

                NWO_OptionalAssets_Rank3Epic_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks[NWO_SlotTask_ListView_7.SelectedIndex]).Assets.Rank3Epic;
                NWO_OptionalAssets_Rank3Epic_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks[NWO_SlotTask_ListView_7.SelectedIndex]).Assets.Rank3Epic_Quantity.ToString();
            }
        }

        private void NWO_SlotTask_ListView_8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1 && NWO_SlotTask_ListView_8.SelectedIndex > -1)
            {
                ClearListViewsSelected(8);

                SetSelected((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks[NWO_SlotTask_ListView_8.SelectedIndex]);

                NWO_RepeatTaskNum_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks[NWO_SlotTask_ListView_8.SelectedIndex]).AmntToDo.ToString();

                NWO_OptionalAssets_Rank1Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks[NWO_SlotTask_ListView_8.SelectedIndex]).Assets.Rank1Common;
                NWO_OptionalAssets_Rank1Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks[NWO_SlotTask_ListView_8.SelectedIndex]).Assets.Rank1Common_Quantity.ToString();

                NWO_OptionalAssets_Rank2Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks[NWO_SlotTask_ListView_8.SelectedIndex]).Assets.Rank2Common;
                NWO_OptionalAssets_Rank2Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks[NWO_SlotTask_ListView_8.SelectedIndex]).Assets.Rank2Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks[NWO_SlotTask_ListView_8.SelectedIndex]).Assets.Rank3Common;
                NWO_OptionalAssets_Rank3Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks[NWO_SlotTask_ListView_8.SelectedIndex]).Assets.Rank3Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Uncommon_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks[NWO_SlotTask_ListView_8.SelectedIndex]).Assets.Rank3Uncommon;
                NWO_OptionalAssets_Rank3Uncommon_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks[NWO_SlotTask_ListView_8.SelectedIndex]).Assets.Rank3Uncommon_Quantity.ToString();

                NWO_OptionalAssets_Rank3Rare_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks[NWO_SlotTask_ListView_8.SelectedIndex]).Assets.Rank3Rare;
                NWO_OptionalAssets_Rank3Rare_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks[NWO_SlotTask_ListView_8.SelectedIndex]).Assets.Rank3Rare_Quantity.ToString();

                NWO_OptionalAssets_Rank3Epic_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks[NWO_SlotTask_ListView_8.SelectedIndex]).Assets.Rank3Epic;
                NWO_OptionalAssets_Rank3Epic_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks[NWO_SlotTask_ListView_8.SelectedIndex]).Assets.Rank3Epic_Quantity.ToString();
            }
        }

        private void NWO_SlotTask_ListView_9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1 && NWO_SlotTask_ListView_9.SelectedIndex > -1)
            {
                ClearListViewsSelected(9);

                SetSelected((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks[NWO_SlotTask_ListView_9.SelectedIndex]);

                NWO_RepeatTaskNum_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks[NWO_SlotTask_ListView_9.SelectedIndex]).AmntToDo.ToString();

                NWO_OptionalAssets_Rank1Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks[NWO_SlotTask_ListView_9.SelectedIndex]).Assets.Rank1Common;
                NWO_OptionalAssets_Rank1Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks[NWO_SlotTask_ListView_9.SelectedIndex]).Assets.Rank1Common_Quantity.ToString();

                NWO_OptionalAssets_Rank2Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks[NWO_SlotTask_ListView_9.SelectedIndex]).Assets.Rank2Common;
                NWO_OptionalAssets_Rank2Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks[NWO_SlotTask_ListView_9.SelectedIndex]).Assets.Rank2Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Common_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks[NWO_SlotTask_ListView_9.SelectedIndex]).Assets.Rank3Common;
                NWO_OptionalAssets_Rank3Common_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks[NWO_SlotTask_ListView_9.SelectedIndex]).Assets.Rank3Common_Quantity.ToString();

                NWO_OptionalAssets_Rank3Uncommon_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks[NWO_SlotTask_ListView_9.SelectedIndex]).Assets.Rank3Uncommon;
                NWO_OptionalAssets_Rank3Uncommon_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks[NWO_SlotTask_ListView_9.SelectedIndex]).Assets.Rank3Uncommon_Quantity.ToString();

                NWO_OptionalAssets_Rank3Rare_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks[NWO_SlotTask_ListView_9.SelectedIndex]).Assets.Rank3Rare;
                NWO_OptionalAssets_Rank3Rare_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks[NWO_SlotTask_ListView_9.SelectedIndex]).Assets.Rank3Rare_Quantity.ToString();

                NWO_OptionalAssets_Rank3Epic_CheckBox.Checked = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks[NWO_SlotTask_ListView_9.SelectedIndex]).Assets.Rank3Epic;
                NWO_OptionalAssets_Rank3Epic_TextBox.Text = ((NWO_Task)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks[NWO_SlotTask_ListView_9.SelectedIndex]).Assets.Rank3Epic_Quantity.ToString();
            }
        }

        private void NWO_Profession_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            NWO_Defines.PopulateComboBoxes.Tasks(NWO_Task_ComboBox, NWO_Profession_ComboBox.SelectedItem.ToString(), true);

            PopulateAssets(NWO_Profession_ComboBox.SelectedItem.ToString());
        }

        private void NWO_Add_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex == -1)
                return;

            NWO_Task tempTask = GetTaskInfo();

            if (NWO_Slot1_CheckBox.Checked)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks.Add(tempTask);
            if (NWO_Slot2_CheckBox.Checked)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks.Add(tempTask);
            if (NWO_Slot3_CheckBox.Checked)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks.Add(tempTask);
            if (NWO_Slot4_CheckBox.Checked)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks.Add(tempTask);
            if (NWO_Slot5_CheckBox.Checked)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks.Add(tempTask);
            if (NWO_Slot6_CheckBox.Checked)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks.Add(tempTask);
            if (NWO_Slot7_CheckBox.Checked)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks.Add(tempTask);
            if (NWO_Slot8_CheckBox.Checked)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks.Add(tempTask);
            if (NWO_Slot9_CheckBox.Checked)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks.Add(tempTask);

            UpdateListViews();
        }

        private void NWO_Remove_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex == -1)
                return;

            if (NWO_SlotTask_ListView_1.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks.RemoveAt(NWO_SlotTask_ListView_1.SelectedIndex);
            if (NWO_SlotTask_ListView_2.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks.RemoveAt(NWO_SlotTask_ListView_2.SelectedIndex);
            if (NWO_SlotTask_ListView_3.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks.RemoveAt(NWO_SlotTask_ListView_3.SelectedIndex);
            if (NWO_SlotTask_ListView_4.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks.RemoveAt(NWO_SlotTask_ListView_4.SelectedIndex);
            if (NWO_SlotTask_ListView_5.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks.RemoveAt(NWO_SlotTask_ListView_5.SelectedIndex);
            if (NWO_SlotTask_ListView_6.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks.RemoveAt(NWO_SlotTask_ListView_6.SelectedIndex);
            if (NWO_SlotTask_ListView_7.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks.RemoveAt(NWO_SlotTask_ListView_7.SelectedIndex);
            if (NWO_SlotTask_ListView_8.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks.RemoveAt(NWO_SlotTask_ListView_8.SelectedIndex);
            if (NWO_SlotTask_ListView_9.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks.RemoveAt(NWO_SlotTask_ListView_9.SelectedIndex);

            UpdateListViews();
        }

        private void NWO_Update_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex == -1)
                return;

            NWO_Task tempTask = GetTaskInfo();

            if (NWO_SlotTask_ListView_1.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks[NWO_SlotTask_ListView_1.SelectedIndex] = tempTask;
            if (NWO_SlotTask_ListView_2.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks[NWO_SlotTask_ListView_2.SelectedIndex] = tempTask;
            if (NWO_SlotTask_ListView_3.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks[NWO_SlotTask_ListView_3.SelectedIndex] = tempTask;
            if (NWO_SlotTask_ListView_4.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks[NWO_SlotTask_ListView_4.SelectedIndex] = tempTask;
            if (NWO_SlotTask_ListView_5.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks[NWO_SlotTask_ListView_5.SelectedIndex] = tempTask;
            if (NWO_SlotTask_ListView_6.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks[NWO_SlotTask_ListView_6.SelectedIndex] = tempTask;
            if (NWO_SlotTask_ListView_7.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks[NWO_SlotTask_ListView_7.SelectedIndex] = tempTask;
            if (NWO_SlotTask_ListView_8.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks[NWO_SlotTask_ListView_8.SelectedIndex] = tempTask;
            if (NWO_SlotTask_ListView_9.SelectedIndex > -1)
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks[NWO_SlotTask_ListView_9.SelectedIndex] = tempTask;

            UpdateListViews();
        }

        private void NWO_MoveUp_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex == -1)
                return;

            if (NWO_SlotTask_ListView_1.SelectedIndex > 0)
                MoveTaskUp(NWO_SlotTask_ListView_1.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks);
            if (NWO_SlotTask_ListView_2.SelectedIndex > 0)
                MoveTaskUp(NWO_SlotTask_ListView_2.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks);
            if (NWO_SlotTask_ListView_3.SelectedIndex > 0)
                MoveTaskUp(NWO_SlotTask_ListView_3.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks);
            if (NWO_SlotTask_ListView_4.SelectedIndex > 0)
                MoveTaskUp(NWO_SlotTask_ListView_4.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks);
            if (NWO_SlotTask_ListView_5.SelectedIndex > 0)
                MoveTaskUp(NWO_SlotTask_ListView_5.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks);
            if (NWO_SlotTask_ListView_6.SelectedIndex > 0)
                MoveTaskUp(NWO_SlotTask_ListView_6.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks);
            if (NWO_SlotTask_ListView_7.SelectedIndex > 0)
                MoveTaskUp(NWO_SlotTask_ListView_7.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks);
            if (NWO_SlotTask_ListView_8.SelectedIndex > 0)
                MoveTaskUp(NWO_SlotTask_ListView_8.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks);
            if (NWO_SlotTask_ListView_9.SelectedIndex > 0)
                MoveTaskUp(NWO_SlotTask_ListView_9.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks);

            UpdateListViews();
        }

        private void NWO_MoveDown_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex == -1)
                return;

            if (NWO_SlotTask_ListView_1.SelectedIndex > -1 && NWO_SlotTask_ListView_1.SelectedIndex < NWO_SlotTask_ListView_1.Items.Count - 1)
                MoveTaskDown(NWO_SlotTask_ListView_1.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks);
            if (NWO_SlotTask_ListView_2.SelectedIndex > -1 && NWO_SlotTask_ListView_2.SelectedIndex < NWO_SlotTask_ListView_2.Items.Count - 1)
                MoveTaskDown(NWO_SlotTask_ListView_2.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks);
            if (NWO_SlotTask_ListView_3.SelectedIndex > -1 && NWO_SlotTask_ListView_3.SelectedIndex < NWO_SlotTask_ListView_3.Items.Count - 1)
                MoveTaskDown(NWO_SlotTask_ListView_3.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks);
            if (NWO_SlotTask_ListView_4.SelectedIndex > -1 && NWO_SlotTask_ListView_4.SelectedIndex < NWO_SlotTask_ListView_4.Items.Count - 1)
                MoveTaskDown(NWO_SlotTask_ListView_4.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks);
            if (NWO_SlotTask_ListView_5.SelectedIndex > -1 && NWO_SlotTask_ListView_5.SelectedIndex < NWO_SlotTask_ListView_5.Items.Count - 1)
                MoveTaskDown(NWO_SlotTask_ListView_5.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks);
            if (NWO_SlotTask_ListView_6.SelectedIndex > -1 && NWO_SlotTask_ListView_6.SelectedIndex < NWO_SlotTask_ListView_6.Items.Count - 1)
                MoveTaskDown(NWO_SlotTask_ListView_6.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks);
            if (NWO_SlotTask_ListView_7.SelectedIndex > -1 && NWO_SlotTask_ListView_7.SelectedIndex < NWO_SlotTask_ListView_7.Items.Count - 1)
                MoveTaskDown(NWO_SlotTask_ListView_7.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks);
            if (NWO_SlotTask_ListView_8.SelectedIndex > -1 && NWO_SlotTask_ListView_8.SelectedIndex < NWO_SlotTask_ListView_8.Items.Count - 1)
                MoveTaskDown(NWO_SlotTask_ListView_8.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks);
            if (NWO_SlotTask_ListView_9.SelectedIndex > -1 && NWO_SlotTask_ListView_9.SelectedIndex < NWO_SlotTask_ListView_9.Items.Count - 1)
                MoveTaskDown(NWO_SlotTask_ListView_9.SelectedIndex, ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks);

            UpdateListViews();
        }

        private void NWO_Clear_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex == -1)
                return;

            if (MessageBox.Show("This will clear out ALL Task Information for ALL slots.  Is this OK?", "Warning", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                ClearSlotInformation(1);
                ClearSlotInformation(2);
                ClearSlotInformation(3);
                ClearSlotInformation(4);
                ClearSlotInformation(5);
                ClearSlotInformation(6);
                ClearSlotInformation(7);
                ClearSlotInformation(8);
                ClearSlotInformation(9);

                UpdateListViews();
            }
            //((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot1.Tasks.Clear();
            //((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot2.Tasks.Clear();
            //((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot3.Tasks.Clear();
            //((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot4.Tasks.Clear();
            //((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot5.Tasks.Clear();
            //((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot6.Tasks.Clear();
            //((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot7.Tasks.Clear();
            //((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot8.Tasks.Clear();
            //((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Slot9.Tasks.Clear();

            
        }

        private void NWO_ProfessionsConfig_Shown(object sender, EventArgs e)
        {
            NWO_Defines.PopulateComboBoxes.Profession(NWO_Profession_ComboBox);
        }

        private void NWO_AddAdditionalTask_Button_Click(object sender, EventArgs e)
        {
            if (NWO_AdditionalTask_ComboBox.SelectedItem.ToString() == NWO_Defines.AdditionalTasks.RefineAstralDiamonds)
            {
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).RefineAstralDiamonds = true;
            }

            if (NWO_AdditionalTask_ComboBox.SelectedItem.ToString() == NWO_Defines.AdditionalTasks.SellItems)
            {
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_Sell_Junk = NWO_AdditionalTasks_JunkSell_CheckBox.Checked;
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_Sell_NonMagicalEquipment = NWO_AdditionalTasks_NonMagicalSell_CheckBox.Checked;
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_Sell_MagicalNonClass = NWO_AdditionalTasks_MagicalSell_CheckBox.Checked;
            }

            if (NWO_AdditionalTask_ComboBox.SelectedItem.ToString() == NWO_Defines.AdditionalTasks.OpenChests)
            {
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_OpenBoxes = true;
            }

            UpdateAdditionalTasksListView();
        }

        private void NWO_DeleteAdditionalTask_Button_Click(object sender, EventArgs e)
        {
            if (NWO_AdditionalTask_ComboBox.SelectedItem.ToString() == NWO_Defines.AdditionalTasks.RefineAstralDiamonds)
            {
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).RefineAstralDiamonds = false;
            }

            if (NWO_AdditionalTask_ComboBox.SelectedItem.ToString() == NWO_Defines.AdditionalTasks.SellItems)
            {
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_Sell_Junk = false;
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_Sell_NonMagicalEquipment = false;
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_Sell_MagicalNonClass = false;
            }

            if (NWO_AdditionalTask_ComboBox.SelectedItem.ToString() == NWO_Defines.AdditionalTasks.OpenChests)
            {
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_OpenBoxes = false;
            }

            UpdateAdditionalTasksListView();
        }

        private void NWO_AdditionTasks_Update_Button_Click(object sender, EventArgs e)
        {
            if (NWO_AdditionalTask_ComboBox.SelectedItem.ToString() == NWO_Defines.AdditionalTasks.SellItems)
            {
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_Sell_Junk = NWO_AdditionalTasks_JunkSell_CheckBox.Checked;
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_Sell_NonMagicalEquipment = NWO_AdditionalTasks_NonMagicalSell_CheckBox.Checked;
                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_Sell_MagicalNonClass = NWO_AdditionalTasks_MagicalSell_CheckBox.Checked;
            }

            UpdateAdditionalTasksListView();
        }

        private void NWO_AdditionalTask_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            NWO_AdditionTasks_Update_Button.Enabled = false;
            NWO_AdditionalTasks_NonMagicalSell_CheckBox.Enabled = false;
            NWO_AdditionalTasks_NonMagicalSell_CheckBox.Visible = false;
            NWO_AdditionalTasks_JunkSell_CheckBox.Enabled = false;
            NWO_AdditionalTasks_JunkSell_CheckBox.Visible = false;
            NWO_AdditionalTasks_MagicalSell_CheckBox.Enabled = false;
            NWO_AdditionalTasks_MagicalSell_CheckBox.Visible = false;

            
            if (NWO_AdditionalTask_ComboBox.SelectedItem.ToString() == NWO_Defines.AdditionalTasks.RefineAstralDiamonds)
            {
            }

            if (NWO_AdditionalTask_ComboBox.SelectedItem.ToString() == NWO_Defines.AdditionalTasks.SellItems)
            {
                NWO_AdditionTasks_Update_Button.Enabled = true;
                NWO_AdditionalTasks_NonMagicalSell_CheckBox.Enabled = true;
                NWO_AdditionalTasks_NonMagicalSell_CheckBox.Visible = true;
                NWO_AdditionalTasks_JunkSell_CheckBox.Enabled = true;
                NWO_AdditionalTasks_JunkSell_CheckBox.Visible = true;
                NWO_AdditionalTasks_MagicalSell_CheckBox.Enabled = true;
                NWO_AdditionalTasks_MagicalSell_CheckBox.Visible = true;

                NWO_AdditionalTasks_JunkSell_CheckBox.Checked = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_Sell_Junk;
                NWO_AdditionalTasks_NonMagicalSell_CheckBox.Checked = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_Sell_NonMagicalEquipment;
                NWO_AdditionalTasks_MagicalSell_CheckBox.Checked = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).Inventory.NWO_Sell_MagicalNonClass;
            }

            if (NWO_AdditionalTask_ComboBox.SelectedItem.ToString() == NWO_Defines.AdditionalTasks.OpenChests)
            {
            }
        }

        private void NWO_SaveQueue_Button_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(_AutoProfessionFileLoc))
            {
                _AutoProfessionFileLoc = String.Format(@"{0}\Config\Cycles", Directory.GetCurrentDirectory());

                if (!Directory.Exists(_AutoProfessionFileLoc))
                    Directory.CreateDirectory(_AutoProfessionFileLoc);
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = _AutoProfessionFileLoc;
            sfd.Filter = "Auto Profession (*.ap)|*.ap";
            sfd.FilterIndex = 1;
            sfd.FileName = "TaskQueue";

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.OpenFile());

                sw.WriteLine(_CurVersion.ToString());

                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).WriteToConfig(sw, _CurVersion);

                sw.Close();
            }
        }

        private void NWO_LoadQueue_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = _AutoProfessionFileLoc;
            ofd.Filter = "Auto Profession (*.ap)|*.ap";
            ofd.FilterIndex = 1;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.OpenFile());

                Version tempVer = new Version(sr.ReadLine());

                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ReadFromConfig(sr, tempVer);

                sr.Close();

                UpdateListViews();
            }
        }

        private void NWO_AdvanceSlot1_Button_Click_1(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                NWO_ProfessionLeveling_ProfessionSelectConfig _professionLeveling_Profession = new NWO_ProfessionLeveling_ProfessionSelectConfig();
                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot1.Profession;

                if (_professionLeveling_Profession.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (_professionLeveling_Profession.Profession != ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot1.Profession && ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot1.Profession != String.Empty)
                    {
                        if (MessageBox.Show("The Profession Selected is Different from what you already have here.  Is this correct??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (MessageBox.Show("This will Erase the previous Profession Leveling information. Continue??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot1.ProfessionLevelingInfo.Clear();
                            }
                            else
                                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot1.Profession;
                        }
                        else
                            _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot1.Profession;
                    }

                    NWO_ProfessionLeveling_TasksConfig _professionLeveling_tasksConfig = new NWO_ProfessionLeveling_TasksConfig();
                    _professionLeveling_tasksConfig.Slot = 1;
                    _professionLeveling_tasksConfig.Version = _CurVersion;
                    _professionLeveling_tasksConfig.Profession = _professionLeveling_Profession.Profession;
                    _professionLeveling_tasksConfig.ProfessionLevelingCollection = (NWO_ProfessionLevelingCollection)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot1;

                    if (_professionLeveling_tasksConfig.ShowDialog() == DialogResult.OK)
                    {
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot1 = _professionLeveling_tasksConfig.ProfessionLevelingCollection;

                        if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot1.ProfessionLevelingInfo.Count > 0)
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot1 = true;
                        else
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot1 = false;
                    }
                    else
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot1 = false;

                    UpdateListViews();
                }
            }
        }

        private void NWO_AdvanceSlot2_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                NWO_ProfessionLeveling_ProfessionSelectConfig _professionLeveling_Profession = new NWO_ProfessionLeveling_ProfessionSelectConfig();
                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot2.Profession;

                if (_professionLeveling_Profession.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (_professionLeveling_Profession.Profession != ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot2.Profession && ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot2.Profession != String.Empty)
                    {
                        if (MessageBox.Show("The Profession Selected is Different from what you already have here.  Is this correct??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (MessageBox.Show("This will Erase the previous Profession Leveling information. Continue??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot2.ProfessionLevelingInfo.Clear();
                            }
                            else
                                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot2.Profession;
                        }
                        else
                            _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot2.Profession;
                    }

                    NWO_ProfessionLeveling_TasksConfig _professionLeveling_tasksConfig = new NWO_ProfessionLeveling_TasksConfig();
                    _professionLeveling_tasksConfig.Slot = 2;
                    _professionLeveling_tasksConfig.Version = _CurVersion;
                    _professionLeveling_tasksConfig.Profession = _professionLeveling_Profession.Profession;
                    _professionLeveling_tasksConfig.ProfessionLevelingCollection = (NWO_ProfessionLevelingCollection)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot2;

                    if (_professionLeveling_tasksConfig.ShowDialog() == DialogResult.OK)
                    {
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot2 = _professionLeveling_tasksConfig.ProfessionLevelingCollection;
                        if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot2.ProfessionLevelingInfo.Count > 0)
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot2 = true;
                        else
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot2 = false;
                    }
                    else
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot2 = false;

                    UpdateListViews();
                }
            }
        }

        private void NWO_AdvanceSlot3_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                NWO_ProfessionLeveling_ProfessionSelectConfig _professionLeveling_Profession = new NWO_ProfessionLeveling_ProfessionSelectConfig();
                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot3.Profession;

                if (_professionLeveling_Profession.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (_professionLeveling_Profession.Profession != ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot3.Profession && ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot3.Profession != String.Empty)
                    {
                        if (MessageBox.Show("The Profession Selected is Different from what you already have here.  Is this correct??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (MessageBox.Show("This will Erase the previous Profession Leveling information. Continue??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot3.ProfessionLevelingInfo.Clear();
                            }
                            else
                                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot3.Profession;
                        }
                        else
                            _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot3.Profession;
                    }

                    NWO_ProfessionLeveling_TasksConfig _professionLeveling_tasksConfig = new NWO_ProfessionLeveling_TasksConfig();
                    _professionLeveling_tasksConfig.Slot = 3;
                    _professionLeveling_tasksConfig.Version = _CurVersion;
                    _professionLeveling_tasksConfig.Profession = _professionLeveling_Profession.Profession;
                    _professionLeveling_tasksConfig.ProfessionLevelingCollection = (NWO_ProfessionLevelingCollection)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot3;

                    if (_professionLeveling_tasksConfig.ShowDialog() == DialogResult.OK)
                    {
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot3 = _professionLeveling_tasksConfig.ProfessionLevelingCollection;

                        if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot3.ProfessionLevelingInfo.Count > 0)
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot3 = true;
                        else
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot3 = false;
                    }
                    else
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot3 = false;

                    UpdateListViews();
                }
            }
        }

        private void NWO_AdvanceSlot4_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                NWO_ProfessionLeveling_ProfessionSelectConfig _professionLeveling_Profession = new NWO_ProfessionLeveling_ProfessionSelectConfig();
                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot4.Profession;

                if (_professionLeveling_Profession.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (_professionLeveling_Profession.Profession != ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot4.Profession && ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot4.Profession != String.Empty)
                    {
                        if (MessageBox.Show("The Profession Selected is Different from what you already have here.  Is this correct??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (MessageBox.Show("This will Erase the previous Profession Leveling information. Continue??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot4.ProfessionLevelingInfo.Clear();
                            }
                            else
                                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot4.Profession;
                        }
                        else
                            _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot4.Profession;
                    }

                    NWO_ProfessionLeveling_TasksConfig _professionLeveling_tasksConfig = new NWO_ProfessionLeveling_TasksConfig();
                    _professionLeveling_tasksConfig.Slot = 4;
                    _professionLeveling_tasksConfig.Version = _CurVersion;
                    _professionLeveling_tasksConfig.Profession = _professionLeveling_Profession.Profession;
                    _professionLeveling_tasksConfig.ProfessionLevelingCollection = (NWO_ProfessionLevelingCollection)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot4;

                    if (_professionLeveling_tasksConfig.ShowDialog() == DialogResult.OK)
                    {
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot4 = _professionLeveling_tasksConfig.ProfessionLevelingCollection;
                        if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot4.ProfessionLevelingInfo.Count > 0)
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot4 = true;
                        else
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot4 = false;
                    }
                    else
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot4 = false;

                    UpdateListViews();
                }
            }
        }

        private void NWO_AdvanceSlot5_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                NWO_ProfessionLeveling_ProfessionSelectConfig _professionLeveling_Profession = new NWO_ProfessionLeveling_ProfessionSelectConfig();
                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot5.Profession;

                if (_professionLeveling_Profession.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (_professionLeveling_Profession.Profession != ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot5.Profession && ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot5.Profession != String.Empty)
                    {
                        if (MessageBox.Show("The Profession Selected is Different from what you already have here.  Is this correct??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (MessageBox.Show("This will Erase the previous Profession Leveling information. Continue??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot5.ProfessionLevelingInfo.Clear();
                            }
                            else
                                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot5.Profession;
                        }
                        else
                            _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot5.Profession;
                    }

                    NWO_ProfessionLeveling_TasksConfig _professionLeveling_tasksConfig = new NWO_ProfessionLeveling_TasksConfig();
                    _professionLeveling_tasksConfig.Slot = 5;
                    _professionLeveling_tasksConfig.Version = _CurVersion;
                    _professionLeveling_tasksConfig.Profession = _professionLeveling_Profession.Profession;
                    _professionLeveling_tasksConfig.ProfessionLevelingCollection = (NWO_ProfessionLevelingCollection)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot5;

                    if (_professionLeveling_tasksConfig.ShowDialog() == DialogResult.OK)
                    {
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot5 = _professionLeveling_tasksConfig.ProfessionLevelingCollection;
                        if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot5.ProfessionLevelingInfo.Count > 0)
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot5 = true;
                        else
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot5 = false;
                    }
                    else
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot5 = false;

                    UpdateListViews();
                }
            }
        }

        private void NWO_AdvanceSlot6_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                NWO_ProfessionLeveling_ProfessionSelectConfig _professionLeveling_Profession = new NWO_ProfessionLeveling_ProfessionSelectConfig();
                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot6.Profession;

                if (_professionLeveling_Profession.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (_professionLeveling_Profession.Profession != ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot6.Profession && ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot6.Profession != String.Empty)
                    {
                        if (MessageBox.Show("The Profession Selected is Different from what you already have here.  Is this correct??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (MessageBox.Show("This will Erase the previous Profession Leveling information. Continue??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot6.ProfessionLevelingInfo.Clear();
                            }
                            else
                                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot6.Profession;
                        }
                        else
                            _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot6.Profession;
                    }

                    NWO_ProfessionLeveling_TasksConfig _professionLeveling_tasksConfig = new NWO_ProfessionLeveling_TasksConfig();
                    _professionLeveling_tasksConfig.Slot = 6;
                    _professionLeveling_tasksConfig.Version = _CurVersion;
                    _professionLeveling_tasksConfig.Profession = _professionLeveling_Profession.Profession;
                    _professionLeveling_tasksConfig.ProfessionLevelingCollection = (NWO_ProfessionLevelingCollection)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot6;

                    if (_professionLeveling_tasksConfig.ShowDialog() == DialogResult.OK)
                    {
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot6 = _professionLeveling_tasksConfig.ProfessionLevelingCollection;
                        if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot6.ProfessionLevelingInfo.Count > 0)
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot6 = true;
                        else
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot6 = false;
                    }
                    else
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot6 = false;

                    UpdateListViews();
                }
            }
        }

        private void NWO_AdvanceSlot7_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                NWO_ProfessionLeveling_ProfessionSelectConfig _professionLeveling_Profession = new NWO_ProfessionLeveling_ProfessionSelectConfig();
                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot7.Profession;

                if (_professionLeveling_Profession.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (_professionLeveling_Profession.Profession != ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot7.Profession && ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot7.Profession != String.Empty)
                    {
                        if (MessageBox.Show("The Profession Selected is Different from what you already have here.  Is this correct??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (MessageBox.Show("This will Erase the previous Profession Leveling information. Continue??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot7.ProfessionLevelingInfo.Clear();
                            }
                            else
                                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot7.Profession;
                        }
                        else
                            _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot7.Profession;
                    }

                    NWO_ProfessionLeveling_TasksConfig _professionLeveling_tasksConfig = new NWO_ProfessionLeveling_TasksConfig();
                    _professionLeveling_tasksConfig.Slot = 7;
                    _professionLeveling_tasksConfig.Version = _CurVersion;
                    _professionLeveling_tasksConfig.Profession = _professionLeveling_Profession.Profession;
                    _professionLeveling_tasksConfig.ProfessionLevelingCollection = (NWO_ProfessionLevelingCollection)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot7;

                    if (_professionLeveling_tasksConfig.ShowDialog() == DialogResult.OK)
                    {
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot7 = _professionLeveling_tasksConfig.ProfessionLevelingCollection;
                        if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot7.ProfessionLevelingInfo.Count > 0)
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot7 = true;
                        else
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot7 = false;
                    }
                    else
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot7 = false;

                    UpdateListViews();
                }
            }
        }

        private void NWO_AdvanceSlot8_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                NWO_ProfessionLeveling_ProfessionSelectConfig _professionLeveling_Profession = new NWO_ProfessionLeveling_ProfessionSelectConfig();
                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot8.Profession;

                if (_professionLeveling_Profession.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (_professionLeveling_Profession.Profession != ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot8.Profession && ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot8.Profession != String.Empty)
                    {
                        if (MessageBox.Show("The Profession Selected is Different from what you already have here.  Is this correct??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (MessageBox.Show("This will Erase the previous Profession Leveling information. Continue??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot8.ProfessionLevelingInfo.Clear();
                            }
                            else
                                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot8.Profession;
                        }
                        else
                            _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot8.Profession;
                    }

                    NWO_ProfessionLeveling_TasksConfig _professionLeveling_tasksConfig = new NWO_ProfessionLeveling_TasksConfig();
                    _professionLeveling_tasksConfig.Slot = 8;
                    _professionLeveling_tasksConfig.Version = _CurVersion;
                    _professionLeveling_tasksConfig.Profession = _professionLeveling_Profession.Profession;
                    _professionLeveling_tasksConfig.ProfessionLevelingCollection = (NWO_ProfessionLevelingCollection)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot8;

                    if (_professionLeveling_tasksConfig.ShowDialog() == DialogResult.OK)
                    {
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot8 = _professionLeveling_tasksConfig.ProfessionLevelingCollection;
                        if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot8.ProfessionLevelingInfo.Count > 0)
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot8 = true;
                        else
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot8 = false;
                    }
                    else
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot8 = false;

                    UpdateListViews();
                }
            }
        }

        private void NWO_AdvanceSlot9_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                NWO_ProfessionLeveling_ProfessionSelectConfig _professionLeveling_Profession = new NWO_ProfessionLeveling_ProfessionSelectConfig();
                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot9.Profession;

                if (_professionLeveling_Profession.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (_professionLeveling_Profession.Profession != ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot9.Profession && ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot9.Profession != String.Empty)
                    {
                        if (MessageBox.Show("The Profession Selected is Different from what you already have here.  Is this correct??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (MessageBox.Show("This will Erase the previous Profession Leveling information. Continue??", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot9.ProfessionLevelingInfo.Clear();
                            }
                            else
                                _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot9.Profession;
                        }
                        else
                            _professionLeveling_Profession.Profession = ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot9.Profession;
                    }

                    NWO_ProfessionLeveling_TasksConfig _professionLeveling_tasksConfig = new NWO_ProfessionLeveling_TasksConfig();
                    _professionLeveling_tasksConfig.Slot = 9;
                    _professionLeveling_tasksConfig.Version = _CurVersion;
                    _professionLeveling_tasksConfig.Profession = _professionLeveling_Profession.Profession;
                    _professionLeveling_tasksConfig.ProfessionLevelingCollection = (NWO_ProfessionLevelingCollection)((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot9;

                    if (_professionLeveling_tasksConfig.ShowDialog() == DialogResult.OK)
                    {
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot9 = _professionLeveling_tasksConfig.ProfessionLevelingCollection;
                        if (((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProfLeveling_Slot9.ProfessionLevelingInfo.Count > 0)
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot9 = true;
                        else
                            ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot9 = false;
                    }
                    else
                        ((NWO_Character)_CharacterList[NWO_CharacterList_ComboBox.SelectedIndex]).ProffessionLeveling_Slot9 = false;

                    UpdateListViews();
                }
            }
        }

        private void NWO_ClearSlot1_Button_Click(object sender, EventArgs e)
        {
            if ( NWO_CharacterList_ComboBox.SelectedIndex > -1 )
            {
                if (MessageBox.Show("This will clear out All slot 1 Task Information.  Is this OK?", "Warning", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    ClearSlotInformation(1);
                    
                    UpdateListViews();
                }
            }
        }

        private void NWO_ClearSlot2_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                if (MessageBox.Show("This will clear out All slot 2 Task Information.  Is this OK?", "Warning", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    ClearSlotInformation(2);

                    UpdateListViews();
                }
            }
        }

        private void NWO_ClearSlot3_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                if (MessageBox.Show("This will clear out All slot 3 Task Information.  Is this OK?", "Warning", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    ClearSlotInformation(3);

                    UpdateListViews();
                }
            }
        }

        private void NWO_ClearSlot4_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                if (MessageBox.Show("This will clear out All slot 4 Task Information.  Is this OK?", "Warning", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    ClearSlotInformation(4);

                    UpdateListViews();
                }
            }
        }

        private void NWO_ClearSlot5_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                if (MessageBox.Show("This will clear out All slot 5 Task Information.  Is this OK?", "Warning", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    ClearSlotInformation(5);

                    UpdateListViews();
                }
            }
        }

        private void NWO_ClearSlot6_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                if (MessageBox.Show("This will clear out All slot 6 Task Information.  Is this OK?", "Warning", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    ClearSlotInformation(6);

                    UpdateListViews();
                }
            }
        }

        private void NWO_ClearSlot7_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                if (MessageBox.Show("This will clear out All slot 7 Task Information.  Is this OK?", "Warning", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    ClearSlotInformation(7);

                    UpdateListViews();
                }
            }
        }

        private void NWO_ClearSlot8_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                if (MessageBox.Show("This will clear out All slot 8 Task Information.  Is this OK?", "Warning", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    ClearSlotInformation(8);

                    UpdateListViews();
                }
            }
        }

        private void NWO_ClearSlot9_Button_Click(object sender, EventArgs e)
        {
            if (NWO_CharacterList_ComboBox.SelectedIndex > -1)
            {
                if (MessageBox.Show("This will clear out All slot 9 Task Information.  Is this OK?", "Warning", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    ClearSlotInformation(9);

                    UpdateListViews();
                }
            }
        }
    }
}
