using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gecko;
using Gecko.DOM;

namespace NWO_AutoProfessions
{
    public static class NWO_ProfessionScreen
    {
        private static String _errorMessage;
        private static String _taskDurationTime;

        internal static Int32 GetNumberActiveSlots(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;
            Int32 _numValidSlots = 0;

            GeckoNodeCollection slotParent = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Profession._professionSlots);
            if (slotParent.Count > 0)
            {
                GeckoNodeCollection slots = slotParent[0].ChildNodes;
                for (int i = 0; i < slots.Count; i++)
                {
                    if (!((GeckoHtmlElement)slots[i]).InnerHtml.Contains("disabled"))
                    {
                        _numValidSlots++;
                    }
                }

                return _numValidSlots;
            }

            _errorMessage = "Unable to find Profession Slots for count.";
            return -1;
        }

        internal static Boolean CheckTaskStartedSuccessfully(GeckoWebBrowser curBrowser)
        {
            GeckoElementCollection messageBox = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.MessageLoc);
            foreach (GeckoHtmlElement message in messageBox)
            {
                if (message.ClassName.Contains(NWO_Defines.PageInfo.InfoMessage))
                    return true;
                if (message.ClassName.Contains(NWO_Defines.PageInfo.ErrorMessage))
                    return false;
            }

            return true;
        }

        internal static Boolean CheckRareTaskAvailable(GeckoWebBrowser curBrowser, String taskName)
        {
            _errorMessage = String.Empty;

            GeckoNodeCollection taskList = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Profession.Tasks);
            if (taskList.Count > 0)
            {
                foreach (GeckoHtmlElement nTaskList in taskList)
                {
                    if (nTaskList.ClassName.Contains(NWO_Defines.PageInfo.Profession.RareTasks) && 
                        !nTaskList.ClassName.Contains(NWO_Defines.PageInfo.Profession.HigherLevel) && 
                        !nTaskList.ClassName.Contains(NWO_Defines.PageInfo.Profession.UnmetRequirements))
                    {
                        if (nTaskList.InnerHtml.Contains(taskName))
                            return true;
                    }
                }
            }
            
            return false;
        }

        internal static Boolean CheckProfessionPage(GeckoWebBrowser curBrowser)
        {
            GeckoNodeCollection slotParent = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Profession._professionSlots);
            if (slotParent.Count > 0)
                return true;

            return false;
        }

        internal static Boolean CheckSlotTaskComplete(GeckoWebBrowser curBrowser, Int32 slot)
        {
            _errorMessage = String.Empty;

            GeckoNodeCollection slotNodes;
            GeckoNodeCollection parentNode = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Profession._professionSlots);

            if (parentNode.Count > 0)
            {
                slotNodes = parentNode[0].ChildNodes;

                if (slotNodes.Count == 0 || slotNodes.Count < 9)
                {
                    _errorMessage = "Unable to Find the Individual Slot Nodes.";
                    return false;
                }

                if (slotNodes[slot].TextContent.Contains("Task Complete!"))
                    return true;
                else
                    return false;
            }

            _errorMessage = "Unable to Find the Individual Slot Nodes.";
            return false;
        }

        internal static Boolean CheckSlotTaskStartTask(GeckoWebBrowser curBrowser, Int32 slot)
        {
            _errorMessage = String.Empty;

            GeckoNodeCollection slotNodes;
            GeckoNodeCollection parentNode = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Profession._professionSlots);

            if (parentNode.Count > 0)
            {
                slotNodes = parentNode[0].ChildNodes;

                if (slotNodes.Count == 0 || slotNodes.Count < 9)
                {
                    _errorMessage = "Unable to Find the Individual Slot Nodes.";
                    return false;
                }

                if (slotNodes[slot].TextContent.Contains("Choose Task"))
                    return true;
                else
                    return false;
            }

            _errorMessage = "Unable to Find the Individual Slot Nodes.";
            return false;
        }

        internal static Boolean CheckSlotTaskNotComplete(GeckoWebBrowser curBrowser, Int32 slot)
        {
            _errorMessage = String.Empty;

            GeckoNodeCollection slotNodes;
            GeckoNodeCollection parentNode = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Profession._professionSlots);

            if (parentNode.Count > 0)
            {
                slotNodes = parentNode[0].ChildNodes;

                if (slotNodes.Count == 0 || slotNodes.Count < 9)
                {
                    _errorMessage = "Unable to Find the Individual Slot Nodes.";
                    return false;
                }

                if (slotNodes[slot].TextContent.Contains("Finish Now"))
                    return true;
                else
                    return false;
            }

            _errorMessage = "Unable to Find the Individual Slot Nodes.";
            return false;
        }

        internal static Boolean ClickTaskComplete(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            GeckoElementCollection buttons = curBrowser.Document.GetElementsByTagName("button");
            foreach (GeckoHtmlElement button in buttons)
            {
                if (button.InnerHtml.Contains("Collect Result"))
                {
                    if (button.OuterHtml.Contains("professionTaskCollectRewards"))
                    {
                        button.Click();
                        return true;
                    }
                }
            }

            _errorMessage = "Unable to Click Collect results.";
            return false;
        }

        internal static Boolean CheckForOptionalAssets(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            GeckoElementCollection buttons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Profession._assetSelect);
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].OuterHtml.ToString().Contains(NWO_Defines.PageInfo.Profession._assetOptional))
                    return true;
            }

            // No Optional Asset Slots.
            return false;
        }

        internal static Int32 GetNumberOptionalAssets(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;
            Int32 numOptionalAssets = 0;

            GeckoElementCollection buttons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Profession._assetSelect);
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].OuterHtml.ToString().Contains(NWO_Defines.PageInfo.Profession._assetOptional))
                    numOptionalAssets++;
            }

            return numOptionalAssets;
        }

        internal static Boolean CheckForOptionalAssetSpot(GeckoWebBrowser curBrowser, Int32 assetSlot)
        {
            _errorMessage = String.Empty;

            GeckoElementCollection buttons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Profession._assetSelect);
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].OuterHtml.ToString().Contains(String.Format("{0}/{1}", NWO_Defines.PageInfo.Profession._assetOptional, assetSlot)))
                    return true;
            }
            
            // The Optional Asset Slot we are looking for does not exist.
            return false;
        }

        internal static Boolean ClickSelectOptionalAssetSpot(GeckoWebBrowser curBrowser, Int32 assetSlot)
        {
            _errorMessage = String.Empty;

            GeckoElementCollection buttons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Profession._assetSelect);
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].OuterHtml.ToString().Contains(String.Format("{0}/{1}", NWO_Defines.PageInfo.Profession._assetOptional, assetSlot)))
                {
                    buttons[i].Click();

                    return true;
                }
            }

            return false;
        }

        internal static Boolean AssetSelect(GeckoWebBrowser curBrowser, String assetName)
        {
            _errorMessage = String.Empty;
            // Need to find the Asset and click it.  Then return;
            GeckoNodeCollection assets = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Profession._assetButton);
            foreach (GeckoNode asset in assets)
            {
                if (asset.TextContent.Contains(assetName) && !asset.TextContent.Contains("Select"))
                {
                    GeckoButtonElement selectAsset = new GeckoButtonElement(asset.DomObject);
                    selectAsset.Click();

                    return true;
                }
            }

            _errorMessage = String.Format("{0} not found in Asset List.", assetName);
            return false;
        }

        internal static Boolean StartTask(GeckoWebBrowser curBrowser, String taskURL)
        {
            _errorMessage = String.Empty;

            GeckoElementCollection buttons = curBrowser.Document.GetElementsByTagName("button");
            foreach (GeckoHtmlElement button in buttons)
            {
                if (button.InnerHtml.Contains("Start Task"))
                {
                    if (button.OuterHtml.Contains(taskURL) && button.OuterHtml.Contains("professionStartAssignment"))
                    {
                        button.Click();

                        return true;
                    }
                }
            }

            _errorMessage = "Unable to start task.";
            return false;
        }

        internal static Boolean GetTaskDuration(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;
            _taskDurationTime = String.Empty;

            GeckoNodeCollection times = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Profession._taskDuration);
            if (times.Count > 0)
            {
                _taskDurationTime = ((GeckoNode)times[0]).TextContent;
                return true;
            }

            _errorMessage = "Unable to find Task Duration.";
            return false;
        }

        internal static Boolean GetTaskDurationOverview(GeckoWebBrowser curBrowser, int slot)
        {
            _errorMessage = String.Empty;
            _taskDurationTime = String.Empty;

            GeckoNodeCollection times = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Profession._taskDurationOverview);
            if (times.Count > 0)
            {
                if (times.Count > slot)
                {
                    _taskDurationTime = ((GeckoNode)times[slot]).TextContent;

                    return true;
                }
            }

            _errorMessage = "Unable to find Task Duration.";
            return false;
        }

        internal static Boolean CheckStartTask(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            GeckoNodeCollection startButton = curBrowser.Document.GetElementsByClassName(String.Format("{0}{1}", NWO_Defines.PageInfo.Profession._startTask, " disabled"));
            if (startButton.Count > 0)
            {
                _errorMessage = "Task is not Available";
                return false;
            }

            return true;
        }

        internal static Int32 GetProfessionLevel(GeckoWebBrowser curBrowser, String prof)
        {
            _errorMessage = String.Empty;
            Int32 _profLevel = 0;

            GeckoElement profTaskInformation = curBrowser.Document.GetElementById(NWO_Defines.PageInfo.Profession._professionLevelsText);
            if ( profTaskInformation.ChildNodes.Count > 0 )
            {
                GeckoNode profTaskInfoChildren = profTaskInformation.ChildNodes[0];
                foreach (GeckoNode childs in profTaskInfoChildren.ChildNodes)
                {
                    if (childs.TextContent.Contains("Level") && !childs.TextContent.Contains("Max"))
                        _profLevel = Convert.ToInt32(childs.TextContent.Substring(childs.TextContent.IndexOf(" ")));

                    if (childs.TextContent.Contains(prof))
                        return _profLevel;
                }
            }

            _errorMessage = "Unable to find the Professions Information.";
            return -1;
        }

        internal static Boolean ClickBuySupplies(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            GeckoElementCollection ProfessionOverviewScreen = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Inventory.Buttons);

            foreach (GeckoElement _BuySupplies in ProfessionOverviewScreen)
            {
                if (((GeckoHtmlElement)_BuySupplies).InnerHtml.Contains(NWO_Defines.PageInfo.Profession.Supplies.BuySuppliesButton))
                {
                    GeckoButtonElement buySuppliesButton = new GeckoButtonElement(_BuySupplies.DomObject);
                    buySuppliesButton.Click();

                    return true;
                }
            }

            return false;
        }

        internal static Boolean ClickBuySupplyButton(GeckoWebBrowser curBrowser, String supplyName)
        {
            _errorMessage = String.Empty;

            GeckoNodeCollection _ProfessionSupplies = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Profession.Supplies.SuppliesInformation);

            foreach (GeckoNode _supplyItem in _ProfessionSupplies)
            {
                if (_supplyItem.ChildNodes.Count > 0)
                {
                    if (_supplyItem.ChildNodes[3].TextContent.Contains(supplyName))
                    {
                        GeckoButtonElement _buyButton = new GeckoButtonElement(_supplyItem.ChildNodes[5].ChildNodes[3].ChildNodes[7].DomObject);
                        _buyButton.Click();

                        return true;
                    }
                }
            }

            return false;
        }

        internal static Boolean CheckForBuyQuantityTextBox(GeckoWebBrowser curBrowser)
        {
            GeckoElementCollection textBoxes = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Profession.Supplies.Buy.Purchase_Quantity_Textbox);
            foreach (GeckoInputElement buyQty in textBoxes)
            {
                if (buyQty.Name.Contains(NWO_Defines.PageInfo.Profession.Supplies.Buy.Purchase_Quantity_TextBox_Name))
                {
                    return true;
                }
            }

            return false;
        }

        internal static Int32 SetBuyQuantityTextBox(GeckoWebBrowser curBrowser, Int32 qty)
        {
            Int32 retQty = -1;

            if (qty <= 0)
                return 0;

            GeckoElementCollection textBoxes = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Profession.Supplies.Buy.Purchase_Quantity_Textbox);
            foreach (GeckoInputElement buyQty in textBoxes)
            {
                if (buyQty.Name.Contains(NWO_Defines.PageInfo.Profession.Supplies.Buy.Purchase_Quantity_TextBox_Name))
                {
                    GeckoNodeCollection nodes = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Profession.Supplies.Buy.Purchase_Quantity_Max);
                    foreach (GeckoNode maxValue in nodes)
                    {
                        if (maxValue.TextContent.Contains(NWO_Defines.PageInfo.MaxLabel))
                        {
                            retQty = Convert.ToInt32(maxValue.TextContent.Substring(maxValue.TextContent.IndexOf(NWO_Defines.PageInfo.MaxLabel) + 4));
                            if (retQty > qty)
                                retQty = qty;

                            buyQty.Value = Convert.ToString(retQty);

                            return retQty;
                        }
                    }
                }
            }

            return retQty;
        }

        internal static Boolean ClickBuySupplyOkButton(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            GeckoElementCollection _professionBuyScreenButtons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Button);

            foreach (GeckoElement _OkButton in _professionBuyScreenButtons)
            {
                if (((GeckoHtmlElement)_OkButton).InnerHtml.Contains(NWO_Defines.PageInfo.Profession.Supplies.Buy.Ok))
                {
                    GeckoButtonElement okButton = new GeckoButtonElement(_OkButton.DomObject);
                    okButton.Click();

                    return true;
                }
            }

            return false;
        }

        internal static Boolean ClickMessageBox(GeckoWebBrowser curBrowser)
        {
            GeckoElementCollection inventoryButtons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Inventory.Buttons);
            foreach (GeckoElement button in inventoryButtons)
            {
                if (((GeckoHtmlElement)button).ClassName.Contains(NWO_Defines.PageInfo.Popup_Message_Box))
                {
                    GeckoButtonElement MessageBoxButton = new GeckoButtonElement(button.DomObject);
                    MessageBoxButton.Click();

                    return true;
                }
            }

            return false;
        }

        internal static Int32 GetRareResetTime(GeckoWebBrowser curBrowser)
        {
            GeckoNodeCollection inventoryItems = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Profession._rareUpdateBar);

            foreach (GeckoHtmlElement item in inventoryItems)
            {
                String tempStr = item.InnerHtml;
                tempStr = tempStr.Remove(0, tempStr.IndexOf("data-timer-length"));
                tempStr = tempStr.Remove(0, tempStr.IndexOf(">") + 1);
                tempStr = tempStr.Remove(tempStr.IndexOf("m"));
                return Convert.ToInt32(tempStr);
            }

            return 0;
        }

        public static String ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
        }

        public static String TaskDurationTime
        {
            get
            {
                return _taskDurationTime;
            }
        }
    }
}
