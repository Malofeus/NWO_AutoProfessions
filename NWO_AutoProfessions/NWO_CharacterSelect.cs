using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gecko;
using Gecko.DOM;

namespace NWO_AutoProfessions
{
    public static class NWO_CharacterSelect
    {
        private static String _errorMessage                     = String.Empty;
        private static String _characterName                    = String.Empty;
        private static String _characterLevel                   = String.Empty;
        private static String _characterClass                   = String.Empty;
        private static String _characterRace                    = String.Empty;

        internal static Boolean CheckForCharacterSelectScreen(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            GeckoNodeCollection charButtons = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.CharacterSelect.Screen);
            if (charButtons.Count > 0)
                return true;

            _errorMessage = "Character Select Screen not found.";
            return false;
        }

        internal static Boolean PressCharacterButton(GeckoWebBrowser curBrowser, String charName)
        {
            GeckoElementCollection charNames = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.CharacterSelect.Characters);
            foreach (GeckoNode cName in charNames)
            {
                if (cName.TextContent.Contains(charName))
                {
                    GeckoNodeCollection charInfo = cName.ChildNodes;

                    _characterName = charInfo[1].TextContent;
                    _characterLevel = charInfo[3].TextContent;
                    _characterClass = charInfo[5].TextContent;
                    _characterRace = charInfo[7].TextContent;

                    GeckoButtonElement charButton = new GeckoButtonElement(cName.DomObject);

                    if (charButton != null)
                    {
                        charButton.Click();

                        return true;
                    }
                    else
                    {
                        _errorMessage = "Unable to create Character Button Object.";
                        return false;
                    }
                }
            }

            _errorMessage = "Unable to find Character Name, Please check spelling.";
            return false;
        }

        internal static String ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                if (value != _errorMessage)
                    _errorMessage = value;
            }
        }

        internal static String CharacterName
        {
            get
            {
                return _characterName;
            }
            set
            {
                if (_characterName != value)
                    _characterName = value;
            }
        }

        internal static String CharacterClass
        {
            get
            {
                return _characterClass;
            }
            set
            {
                if (_characterClass != value)
                    _characterClass = value;
            }
        }

        internal static String CharacterLevel
        {
            get
            {
                return _characterLevel;
            }
            set
            {
                if (_characterLevel != value)
                    _characterLevel = value;
            }
        }

        internal static String CharacterRace
        {
            get
            {
                return _characterRace;
            }
            set
            {
                if (_characterRace != value)
                    _characterRace = value;
            }
        }
    }
}
