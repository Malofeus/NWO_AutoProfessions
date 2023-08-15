using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gecko;
using Gecko.DOM;

namespace NWO_AutoProfessions
{
    public static class NWO_CharacterScreen
    {
        private static String _errorMessage;

        internal static String ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                if (_errorMessage != value)
                    _errorMessage = value;
            }
        }

        private static Boolean ClickButton(GeckoWebBrowser curBrowser, String text)
        {
            _errorMessage = String.Empty;

            GeckoNodeCollection buttons = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Main.All);
            foreach (GeckoNode button in buttons)
            {
                if (button.TextContent.Contains(text))
                {
                    GeckoButtonElement cButton = new GeckoButtonElement(button.DomObject);
                    if (cButton != null)
                    {
                        cButton.Click();

                        return true;
                    }
                }
            }
            return false;
        }

        internal static Boolean CheckForCharacterScreen(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            GeckoNodeCollection buttons = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Main.All);

            if (buttons.Count > 0)
                return true;

            return false;
        }

        internal static Boolean ClickSwordCoastAdventureButton(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            if (!ClickButton(curBrowser, NWO_Defines.PageInfo.Main.Sword_Coast_Adventure))
            {
                _errorMessage = "Unable to Click Sword Coast Adventures Button.";
                return false;
            }

            return true;
        }

        internal static Boolean ClickCharacterSheetButton(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            if (!ClickButton(curBrowser, NWO_Defines.PageInfo.Main.Character_Sheet))
            {
               _errorMessage = "Unable to Click Character Sheet Button.";
               return false;
            }

            return true;
        }

        internal static Boolean ClickInventoryButton(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            if (!ClickButton(curBrowser, NWO_Defines.PageInfo.Main.Inventory))
            {
               _errorMessage = "Unable to Click Inventory Button.";
               return false;
            }

            return true;
        }

        internal static Boolean ClickProfessionButton(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            if (!ClickButton(curBrowser, NWO_Defines.PageInfo.Main.Professions))
            {
               _errorMessage = "Unable to Click Profession Button.";
               return false;
            }

            return true;
        }

        internal static Boolean ClickAuctionHouseButton(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            if (!ClickButton(curBrowser, NWO_Defines.PageInfo.Main.Auction_House))
            {
                _errorMessage = "Unable to Click Auction House Button.";
                return false;
            }

            return true;
        }

        internal static Boolean ClickExchangeButton(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            if (!ClickButton(curBrowser, NWO_Defines.PageInfo.Main.Exchange))
            {
               _errorMessage = "Unable to Click Exchange Button.";
               return false;
            }

            return true;
        }

        internal static Boolean ClickZenMarketButton(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            if (!ClickButton(curBrowser, NWO_Defines.PageInfo.Main.Zen_Market))
            {
                _errorMessage = "Unable to Click Zen Market Button.";
                return false;
            }

            return true;
        }

        internal static Boolean ClickGuildButton(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            if (!ClickButton(curBrowser, NWO_Defines.PageInfo.Main.Guild))
            {
                _errorMessage = "Unable to Click Guild Button.";
                return false;
            }

            return true;
        }

        internal static Boolean ClickMailButton(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            if (!ClickButton(curBrowser, NWO_Defines.PageInfo.Main.Mail))
            {
                _errorMessage = "Unable to Click Mail Button.";
                return false;
            }

            return true;
        }

        internal static Boolean ClickChangeCharacterButton(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            if (!ClickButton(curBrowser, NWO_Defines.PageInfo.Main.Change_Character))
            {
                _errorMessage = "Unable to Click Change Character Button.";
                return false;
            }

            return true;
        }

        internal static Boolean ClickLogoutButton(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            if (!ClickButton(curBrowser, NWO_Defines.PageInfo.Main.Logout))
            {
                _errorMessage = "Unable to Click Inventory Button.";
                return false;
            }

            return true;
        }

        internal static Boolean ClickChangeCharacter(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            GeckoNodeCollection buttons = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Main.Change_Character_Mobile);
            foreach (GeckoNode button in buttons)
            {
                GeckoNodeCollection mobilButtons = button.ChildNodes;
                bool found = false;
                foreach (GeckoNode mButton in mobilButtons)
                {
                    if (mButton.TextContent == NWO_Defines.PageInfo.Main.Change_Character)
                        found = true;

                    if (found)
                    {
                        GeckoButtonElement cButton = new GeckoButtonElement(mButton.DomObject);
                        if (cButton != null)
                        {
                            cButton.Click();
                            
                            return true;
                        }
                    }
                } 
            }
            
            return false;
        }

        internal static Boolean ClickLogOut(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            GeckoNodeCollection buttons = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Main.Change_Character_Mobile);
            foreach (GeckoNode button in buttons)
            {
                GeckoNodeCollection mobilButtons = button.ChildNodes;
                bool found = false;
                foreach (GeckoNode mButton in mobilButtons)
                {
                    if (mButton.TextContent == NWO_Defines.PageInfo.Main.Logout)
                        found = true;

                    if (found)
                    {
                        GeckoButtonElement cButton = new GeckoButtonElement(mButton.DomObject);
                        if (cButton != null)
                        {
                            cButton.Click();

                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
