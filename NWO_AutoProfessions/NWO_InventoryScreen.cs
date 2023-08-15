using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gecko;
using Gecko.DOM;

namespace NWO_AutoProfessions
{
    public static class NWO_InventoryScreen
    {
        private static Boolean _SellJunk                = false;
        private static Boolean _SellNonMagical          = false;
        private static Boolean _SellMagicalNonClass     = false;
        private static Boolean _OpenBoxes               = false;

        private static String _errorMessage;

        public static String ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
        }

        public static Boolean SellJunk
        {
            get
            {
                return _SellJunk;
            }
            set
            {
                _SellJunk = value;
            }
        }

        public static Boolean SellNonMagical
        {
            get
            {
                return _SellNonMagical;
            }
            set
            {
                _SellNonMagical = value;
            }
        }

        public static Boolean SellMagicalNonClass
        {
            get
            {
                return _SellMagicalNonClass;
            }
            set
            {
                _SellMagicalNonClass = value;
            }
        }

        public static Boolean OpenBoxes
        {
            get
            {
                return _OpenBoxes;
            }
            set
            {
                _OpenBoxes = value;
            }
        }

        internal static Boolean ClickButton(nsIDOMNode itemDomObject)
        {
            GeckoButtonElement button = new GeckoButtonElement(itemDomObject);
            if (button != null)
            {
                button.Click();
                return true;
            }

            return false;
        }

        internal static Boolean CheckForInventoryBagScreen(GeckoWebBrowser curBrowser)
        {
            GeckoNodeCollection bagTab = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Bags_Page);
            if (bagTab.Count > 0)
                return true;

            return false;
        }

        internal static Boolean CheckForInventoryProfessionsScreen(GeckoWebBrowser curBrowser)
        {
            throw new NotImplementedException();

            GeckoNodeCollection bagTab = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Professions_Tab);
            if (bagTab.Count > 0)
                return true;

            return false;
        }

        internal static Boolean CheckForInventoryCompanionsScreen(GeckoWebBrowser curBrowser)
        {
            throw new NotImplementedException();

            GeckoNodeCollection bagTab = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Companions_Tab);
            if (bagTab.Count > 0)
                return true;

            return false;
        }

        internal static Boolean CheckRefineAstralDiamondsButtonEnabled(GeckoWebBrowser curBrowser)
        {
            GeckoNodeCollection inventoryButtons = curBrowser.Document.GetElementsByClassName(String.Format("{0} {1}", NWO_Defines.PageInfo.Inventory.Currency.RoughAstralDiamonds_Button, "disabled"));
            if (inventoryButtons.Count > 0)
                return false;

            return true;
        }

        internal static Boolean ClickRefineAstralDiamonds(GeckoWebBrowser curBrowser)
        {
            GeckoNodeCollection inventoryButtons = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Currency.RoughAstralDiamonds_Button);
            if (inventoryButtons.Count > 0)
            {
                foreach (GeckoNode refine in inventoryButtons)
                {
                    if (refine.TextContent.ToString().Contains("Refine"))
                    {
                        GeckoNodeCollection refineButton = refine.ChildNodes;
                        foreach (GeckoNode rButton in refineButton)
                        {
                            if (rButton.NodeName.Contains("BUTTON"))
                            {
                                GeckoButtonElement aDiamondRefine = new GeckoButtonElement(rButton.DomObject);
                                if (aDiamondRefine != null)
                                {
                                    aDiamondRefine.Click();

                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        internal static Boolean CheckForInventorySellQuantityBox(GeckoWebBrowser curBrowser)
        {
            GeckoElementCollection textBoxes = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Inventory.Sell.Sell_Quantity_TextBox);
            foreach (GeckoInputElement sellQty in textBoxes)
            {
                if (sellQty.Name.Contains(NWO_Defines.PageInfo.Inventory.Sell.Sell_Quantity_TextBox_Name))
                {
                    return true;
                }
            }

            return false;
        }

        internal static Boolean SetInventorySellQuantityBox(GeckoWebBrowser curBrowser)
        {
            GeckoElementCollection textBoxes = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Inventory.Sell.Sell_Quantity_TextBox);
            foreach (GeckoInputElement sellQty in textBoxes)
            {
                if (sellQty.Name.Contains(NWO_Defines.PageInfo.Inventory.Sell.Sell_Quantity_TextBox_Name))
                {
                    GeckoNodeCollection nodes = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Sell.Sell_Quantity_Max_Label);
                    foreach (GeckoNode maxValue in nodes)
                    {
                        if (maxValue.TextContent.Contains(NWO_Defines.PageInfo.MaxLabel))
                        {
                            sellQty.Value = maxValue.TextContent.Substring(maxValue.TextContent.IndexOf(NWO_Defines.PageInfo.MaxLabel) + 4);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        internal static Boolean CheckForItemSelectScreen(GeckoWebBrowser curBrowser)
        {
            GeckoElementCollection ItemSelectScreenButtons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Inventory.Buttons);
             foreach (GeckoElement sellToVendorButton in ItemSelectScreenButtons)
             {
                 if (((GeckoHtmlElement)sellToVendorButton).OuterHtml.Contains(NWO_Defines.PageInfo.Inventory.Item.Sell_To_Vendor))
                     return true;
             }

             return false;
        }

        internal static Boolean ClickSellToVendorButton(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            GeckoElementCollection ItemSelectScreenButtons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Inventory.Buttons);
            foreach (GeckoElement sellToVendorButton in ItemSelectScreenButtons)
            {
                if (((GeckoHtmlElement)sellToVendorButton).InnerHtml.Contains(NWO_Defines.PageInfo.Inventory.Item.Sell_To_Vendor) && !((GeckoHtmlElement)sellToVendorButton).ClassName.Contains(NWO_Defines.PageInfo.Inventory.Button_Disabled))
                    return ClickButton(sellToVendorButton.DomObject);
                else if (((GeckoHtmlElement)sellToVendorButton).InnerHtml.Contains(NWO_Defines.PageInfo.Inventory.Item.Sell_To_Vendor) && ((GeckoHtmlElement)sellToVendorButton).ClassName.Contains(NWO_Defines.PageInfo.Inventory.Button_Disabled))
                {
                    _errorMessage = NWO_Defines.PageInfo.Inventory.Button_Disabled;
                    return false;
                }
            }

            return false;
        }

        internal static Boolean ClickConfirmButton(GeckoWebBrowser curBrowser)
        {
            GeckoElementCollection ItemSelectScreenButtons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Inventory.Buttons);
            foreach (GeckoElement confirmButton in ItemSelectScreenButtons)
            {
                if (((GeckoHtmlElement)confirmButton).InnerHtml.Contains(NWO_Defines.PageInfo.Inventory.Confirm))
                    return ClickButton(confirmButton.DomObject);
            }

            return false;
        }

        internal static Boolean ClickOkButton(GeckoWebBrowser curBrowser)
        {
            GeckoElementCollection ItemSelectScreenButtons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Inventory.Buttons);
            foreach (GeckoElement confirmButton in ItemSelectScreenButtons)
            {
                if (((GeckoHtmlElement)confirmButton).InnerHtml.Contains(NWO_Defines.PageInfo.Inventory.OK))
                    return ClickButton(confirmButton.DomObject);
            }

            return false;
        }

        internal static Boolean CheckForOpenButton(GeckoWebBrowser curBrowser)
        {
            GeckoElementCollection ItemSelectScreenButtons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Inventory.Buttons);
            foreach (GeckoElement openItemButton in ItemSelectScreenButtons)
            {
                if (((GeckoHtmlElement)openItemButton).InnerHtml.Contains(NWO_Defines.PageInfo.Inventory.Item.Open_Item) && !((GeckoHtmlElement)openItemButton).ClassName.Contains(NWO_Defines.PageInfo.Inventory.Button_Disabled))
                    return true;
                else if (((GeckoHtmlElement)openItemButton).InnerHtml.Contains(NWO_Defines.PageInfo.Inventory.Item.Open_Item) && ((GeckoHtmlElement)openItemButton).ClassName.Contains(NWO_Defines.PageInfo.Inventory.Button_Disabled))
                    return false;
            }

            return false;
        }

        internal static Boolean ClickOpenButton(GeckoWebBrowser curBrowser)
        {
            GeckoElementCollection ItemSelectScreenButtons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Inventory.Buttons);
            foreach (GeckoElement openItemButton in ItemSelectScreenButtons)
            {
                if (((GeckoHtmlElement)openItemButton).InnerHtml.Contains(NWO_Defines.PageInfo.Inventory.Item.Open_Item) && !((GeckoHtmlElement)openItemButton).ClassName.Contains(NWO_Defines.PageInfo.Inventory.Button_Disabled))
                    return ClickButton(openItemButton.DomObject);
                else if (((GeckoHtmlElement)openItemButton).InnerHtml.Contains(NWO_Defines.PageInfo.Inventory.Item.Open_Item) && ((GeckoHtmlElement)openItemButton).ClassName.Contains(NWO_Defines.PageInfo.Inventory.Button_Disabled))
                    return false;
            }

            return false;
        }

        internal static Boolean ClickBackButton(GeckoWebBrowser curBrowser)
        {
            GeckoElementCollection ItemSelectScreenButtons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Inventory.Buttons);
            foreach (GeckoElement backButton in ItemSelectScreenButtons)
            {
                if (((GeckoHtmlElement)backButton).InnerHtml.Contains(NWO_Defines.PageInfo.Inventory.Back))
                    return ClickButton(backButton.DomObject);
            }

            return false;
        }

        internal static Boolean ClickCancelButton(GeckoWebBrowser curBrowser)
        {
            GeckoElementCollection ItemSelectScreenButtons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Inventory.Buttons);
            foreach (GeckoElement cancelButton in ItemSelectScreenButtons)
            {
                if (((GeckoHtmlElement)cancelButton).InnerHtml.Contains(NWO_Defines.PageInfo.Inventory.Cancel))
                    return ClickButton(cancelButton.DomObject);
            }

            return false;
        }

        internal static Boolean CheckMessageBox(GeckoWebBrowser curBrowser)
        {
            GeckoElementCollection inventoryButtons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Inventory.Buttons);
            foreach (GeckoElement button in inventoryButtons)
            {
                if (((GeckoHtmlElement)button).ClassName.Contains(NWO_Defines.PageInfo.Popup_Message_Box))
                    return true;
            }

            return false;
        }

        internal static Boolean ClickMessageBox(GeckoWebBrowser curBrowser)
        {
            GeckoElementCollection inventoryButtons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Inventory.Buttons);
            foreach (GeckoElement button in inventoryButtons)
            {
                if (((GeckoHtmlElement)button).ClassName.Contains(NWO_Defines.PageInfo.Popup_Message_Box))
                    return ClickButton(button.DomObject);
            }

            return false;
        }

        internal static Int64 GetNumAstralDiamonds(GeckoWebBrowser curBrowser)
        {
            Int64 retValue = -1;
            GeckoNodeCollection characterCurrency = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Currency.Currency_Header);

            foreach (GeckoNode currencyType in characterCurrency)
            {
                if (currencyType.TextContent.Contains(NWO_Defines.PageInfo.Inventory.Currency.AstralDiamonds))
                {
                    String tempCur = currencyType.TextContent.Substring(currencyType.TextContent.IndexOf(NWO_Defines.PageInfo.Inventory.Currency.AstralDiamonds) + NWO_Defines.PageInfo.Inventory.Currency.AstralDiamonds.Length + 1);
                    tempCur = tempCur.Substring(0, tempCur.IndexOf("\n"));

                    if (tempCur.IndexOf(",") > -1)
                        tempCur = tempCur.Replace(",", "");

                    retValue = Convert.ToInt32(tempCur);
                }
            }

            return retValue;
        }

        internal static Int64 GetNumRoughAstralDiamonds(GeckoWebBrowser curBrowser)
        {
            Int64 retValue = -1;
            GeckoNodeCollection characterCurrency = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Currency.Currency_Header);

            foreach (GeckoNode currencyType in characterCurrency)
            {
                if (currencyType.TextContent.Contains(NWO_Defines.PageInfo.Inventory.Currency.RoughAstralDiamonds))
                {
                    String tempCur = currencyType.TextContent.Substring(currencyType.TextContent.IndexOf(NWO_Defines.PageInfo.Inventory.Currency.RoughAstralDiamonds) + NWO_Defines.PageInfo.Inventory.Currency.RoughAstralDiamonds.Length + 1);
                    tempCur = tempCur.Substring(0, tempCur.IndexOf("\n"));

                    if (tempCur.IndexOf(",") != -1)
                        tempCur = tempCur.Replace(",", "");

                    retValue = Convert.ToInt32(tempCur);
                }
            }

            return retValue;
        }

        internal static Int32 GetNumGold(GeckoWebBrowser curBrowser)
        {
            Int32 retValue = -1;
            GeckoNodeCollection characterCurrency = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Currency.Currency_Header);
            foreach (GeckoNode currencyType in characterCurrency)
            {
                if (currencyType.TextContent.Contains(NWO_Defines.PageInfo.Inventory.Currency.Money))
                {
                    String tempCur = currencyType.TextContent.Substring(currencyType.TextContent.IndexOf(NWO_Defines.PageInfo.Inventory.Currency.Money) + NWO_Defines.PageInfo.Inventory.Currency.Money.Length + 1);
                    if (tempCur.IndexOf("\n") == 0)
                        tempCur = tempCur.Remove(0, tempCur.IndexOf("\n") + 1);
                    
                    tempCur = tempCur.Substring(0, tempCur.IndexOf("\n"));

                    if (tempCur.IndexOf(",") > -1)
                        tempCur = tempCur.Replace(",", "");

                    retValue = Convert.ToInt32(tempCur);
                }
            }

            return retValue;
        }

        internal static Int32 GetNumSilver(GeckoWebBrowser curBrowser)
        {
            Int32 retValue = -1;
            GeckoNodeCollection characterCurrency = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Currency.Currency_Header);
            foreach (GeckoNode currencyType in characterCurrency)
            {
                if (currencyType.TextContent.Contains(NWO_Defines.PageInfo.Inventory.Currency.Money))
                {
                    String tempCur = currencyType.TextContent.Substring(currencyType.TextContent.IndexOf(NWO_Defines.PageInfo.Inventory.Currency.Money) + NWO_Defines.PageInfo.Inventory.Currency.Money.Length + 1);
                    if (tempCur.IndexOf("\n") == 0)
                        tempCur = tempCur.Remove(0, tempCur.IndexOf("\n") + 1);

                    tempCur = tempCur.Remove(0, tempCur.IndexOf("\n") + 1);

                    if (tempCur.IndexOf("\n") == 0)
                        tempCur = tempCur.Remove(0, tempCur.IndexOf("\n") + 1);

                    tempCur = tempCur.Substring(0, tempCur.IndexOf("\n"));

                    if (tempCur.IndexOf(",") > -1)
                        tempCur = tempCur.Replace(",", "");

                    retValue = Convert.ToInt32(tempCur);
                }
            }

            return retValue;
        }

        internal static Int32 GetNumCopper(GeckoWebBrowser curBrowser)
        {
            Int32 retValue = -1;
            GeckoNodeCollection characterCurrency = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Currency.Currency_Header);
            foreach (GeckoNode currencyType in characterCurrency)
            {
                if (currencyType.TextContent.Contains(NWO_Defines.PageInfo.Inventory.Currency.Money))
                {
                    String tempCur = currencyType.TextContent.Substring(currencyType.TextContent.IndexOf(NWO_Defines.PageInfo.Inventory.Currency.Money) + NWO_Defines.PageInfo.Inventory.Currency.Money.Length + 1);
                    if (tempCur.IndexOf("\n") == 0)
                        tempCur = tempCur.Remove(0, tempCur.IndexOf("\n") + 1);

                    tempCur = tempCur.Remove(0, tempCur.IndexOf("\n") + 1);

                    if (tempCur.IndexOf("\n") == 0)
                        tempCur = tempCur.Remove(0, tempCur.IndexOf("\n") + 1);

                    tempCur = tempCur.Remove(0, tempCur.IndexOf("\n") + 1);

                    if (tempCur.IndexOf("\n") == 0)
                        tempCur = tempCur.Remove(0, tempCur.IndexOf("\n") + 1);

                    tempCur = tempCur.Substring(0, tempCur.IndexOf("\n"));

                    if (tempCur.IndexOf(",") > -1)
                        tempCur = tempCur.Replace(",", "");

                    retValue = Convert.ToInt32(tempCur);
                }
            }

            return retValue;
        }

        internal static GeckoDivElement GetItem(GeckoWebBrowser curBrowser, Int32 count)
        {
            Int32 curItem = 0;
            GeckoNodeCollection inventoryItems = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Items);

            foreach (GeckoDivElement item in inventoryItems)
            {
                if (curItem == count)
                    return item;
            }

            return null;
        }

        internal static GeckoDivElement GetItem(GeckoWebBrowser curBrowser, GeckoDivElement _item)
        {
            GeckoNodeCollection inventoryItems = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Items);

            foreach (GeckoDivElement item in inventoryItems)
            {
                if (item != _item)
                {
                    if (_OpenBoxes)
                    {
                        if (CheckItemOpenableChest(item))
                        {
                            _SellMagicalNonClass = false;
                            _SellNonMagical = false;
                            _SellJunk = false;
                            return item;
                        }
                    }

                    if (_SellJunk)
                    {
                        if (CheckItemIsJunk(item))
                        {
                            _SellMagicalNonClass = false;
                            _SellNonMagical = false;
                            _OpenBoxes = false;
                            return item;
                        }
                    }

                    if (_SellNonMagical)
                    {
                        if (CheckItemNonMagical(item))
                        {
                            _SellMagicalNonClass = false;
                            _SellJunk = false;
                            _OpenBoxes = false;
                            return item;
                        }
                    }

                    if (_SellMagicalNonClass)
                    {
                        if (CheckItemMagicalNonClass(item))
                        {
                            _SellNonMagical = false;
                            _SellJunk = false;
                            _OpenBoxes = false;
                            return item;
                        }
                    }
                }
            }

            return null;
        }

        internal static GeckoDivElement GetBotItem(GeckoWebBrowser curBrowser)
        {
            GeckoNodeCollection inventoryItems = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Items);

            foreach (GeckoDivElement item in inventoryItems)
            {
                if (!item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Quality.Epic) && !item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Quality.Rare))
                {
                    if (item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Quality.Junk) && !item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Empty) &&
                        !item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.Armor) && !item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.Weapon) &&
                        !item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.Quest_Starter))
                        return item;

                    if (item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.Armor) || item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.Weapon) ||
                        item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.Disabled))
                        return item;

                    if (item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Quality.Non_Magical) && item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.Gem))
                        return item;

                    //if (item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.Consumables.Consumable))
                    //{
                    //    if (item.InnerHtml.Contains(NWO_Defines.Inventory.ItemInfo.Type.Consumables.Potions.Potion) &&
                    //        !item.InnerHtml.Contains(NWO_Defines.Inventory.ItemInfo.Type.Consumables.Potions.Healing))
                    //        return item;
                    //}
                }
            }

            return null;
        }

        private static Boolean CheckItemIsJunk(GeckoDivElement _item)
        {
            if (_item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Quality.Junk) && !_item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Empty) &&
                !_item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.Armor) && !_item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.Weapon) &&
                !_item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.Quest_Starter))
                return true;

            return false;
        }

        private static Boolean CheckItemNonMagical(GeckoDivElement _item)
        {
            if (!_item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Empty) &&
                (_item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Quality.Non_Magical) || _item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Quality.Junk)) &&
                (_item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.Armor) || _item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.Weapon)))
                return true;

            return false;
        }

        private static Boolean CheckItemMagicalNonClass(GeckoDivElement _item)
        {
            if (!_item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Empty) &&
                _item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Quality.Magical) && _item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.NotUsable) &&
                (_item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.Armor) || _item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.Weapon)))
                return true;

            return false;
        }

        private static Boolean CheckItemOpenableChest(GeckoDivElement _item)
        {
            if (!_item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Empty) &&
                _item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.RewardPack) &&
                !_item.ClassName.Contains(NWO_Defines.Inventory.ItemInfo.Type.Disabled) &&
                !((GeckoHtmlElement)_item).InnerHtml.Contains(NWO_Defines.Inventory.ItemInfo.Type.Lockboxes) &&
                ((GeckoHtmlElement)_item).InnerHtml.Contains(NWO_Defines.Inventory.ItemInfo.Type.Chests))
                return true;

            return false;
        }

        internal static GeckoDivElement GetJunkItem(GeckoWebBrowser curBrowser, GeckoDivElement _item)
        {
            GeckoNodeCollection inventoryItems = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Items);

            foreach (GeckoDivElement item in inventoryItems)
            {
                if (item != _item)
                {
                    if (CheckItemIsJunk(item))
                        return item;
                }
            }

            return null;
        }

        internal static GeckoDivElement GetNonMagicalItem(GeckoWebBrowser curBrowser, GeckoDivElement _item)
        {
            GeckoNodeCollection inventoryItems = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Items);

            foreach (GeckoDivElement item in inventoryItems)
            {
                if (item != _item)
                {
                    if (CheckItemNonMagical(item))
                        return item;
                }
            }

            return null;
        }

        internal static GeckoDivElement GetMagicalNonClass(GeckoWebBrowser curBrowser, GeckoDivElement _item)
        {
            GeckoNodeCollection inventoryItems = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Items);

            foreach (GeckoDivElement item in inventoryItems)
            {
                if (item != _item)
                {
                    if (CheckItemMagicalNonClass(item))
                        return item;
                }
            }

            return null;
        }

        internal static GeckoDivElement GetOpenableChests(GeckoWebBrowser curBrowser, GeckoDivElement _item)
        {
            GeckoNodeCollection inventoryItems = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Inventory.Items);

            foreach (GeckoDivElement item in inventoryItems)
            {
                if (item != _item)
                {
                    if (CheckItemOpenableChest(item)) 
                            return item;
                }
            }

            return null;
        }
    }
}
