using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gecko;
using Gecko.DOM;

namespace NWO_AutoProfessions
{
    public static class NWO_LoginScreen
    {
        private static String _errorMessage = String.Empty;

        internal static Boolean SetUserText(GeckoWebBrowser curBrowser, String userName)
        {
            _errorMessage = String.Empty;
            GeckoElementCollection _UserTextbox = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.TextBox);
            foreach (GeckoInputElement userTextBox in _UserTextbox)
            {
                if (userTextBox.Name.Contains(NWO_Defines.PageInfo.Login.UserTextbox))
                {
                    userTextBox.Value = userName;
                    return true;
                }
            }

            _errorMessage = "Unable to find and populate the User Textbox.";
            return false;
        }

        internal static Boolean SetPasswordText(GeckoWebBrowser curBrowser, String password)
        {
            _errorMessage = String.Empty;
            GeckoElementCollection _PasswordTextbox = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.TextBox);

            foreach (GeckoInputElement passwordTextbox in _PasswordTextbox)
            {
                if (passwordTextbox.Name.Contains(NWO_Defines.PageInfo.Login.PasswordTextbox))
                {
                    passwordTextbox.Value = password;
                    return true;
                }
            }
            
            _errorMessage = "Unable to find and populate the Password Textbox.";
            return false;
        }

        internal static Boolean PressLoginButton(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;
            String attr = String.Empty;

            GeckoElementCollection buttons = curBrowser.Document.GetElementsByName(NWO_Defines.PageInfo.Button);

            foreach (GeckoElement log in buttons)
            {
                attr = log.GetAttribute(NWO_Defines.PageInfo.Login.Attr);
                if (attr == NWO_Defines.PageInfo.Login.LoginAttr)
                {
                    GeckoButtonElement loginButton = new GeckoButtonElement(log.DomObject);

                    if ( loginButton != null )
                    {
                        loginButton.Click();

                        if (CheckInvalidLogin(curBrowser))
                        {
                            _errorMessage = "Invalid username or password.";
                            return false;
                        }

                        return true; ;
                    }
                }
            }

            _errorMessage = "Unable to find Login Button/Login Button Click failed.";
            return false;
        }

        internal static Boolean CheckForLoginScreen(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            GeckoElementCollection buttons = curBrowser.Document.GetElementsByName(NWO_Defines.PageInfo.Button);
            if (buttons.Count == 0)
                return true;

            return false;
        }

        internal static Boolean CheckForDisconnectScreen(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            GeckoElement disconScreen = curBrowser.Document.GetElementById(NWO_Defines.PageInfo.DisconnectedScreen);
            if (disconScreen != null)
            {
                if (disconScreen.TextContent.Contains("Disconnected"))
                {
                    _errorMessage = "Disconnected from gateway.";
                    return true;
                }
            }

            return false;
        }

        internal static Boolean ClickDisconnectScreen(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;

            GeckoElementCollection disconnectScreenButtons = curBrowser.Document.GetElementsByTagName(NWO_Defines.PageInfo.Button);
            foreach (GeckoElement closeButton in disconnectScreenButtons)
            {
                if (((GeckoHtmlElement)closeButton).InnerHtml.Contains("Close"))
                {
                    GeckoButtonElement button = new GeckoButtonElement(closeButton.DomObject);
                    button.Click();

                    return true;
                }
            }

            _errorMessage = "Unable to find the close button on Disconnect Screen.";
            return false;
        }

        private static Boolean CheckInvalidLogin(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;
            GeckoNodeCollection errorBoxes = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.ErrorMessage);

            foreach (GeckoNode node in errorBoxes)
            {
                if (node.TextContent.Contains("Invalid"))
                    return true;
            }

            return false;
        }

        internal static Boolean CheckUndergoingMaintenance(GeckoWebBrowser curBrowser)
        {
            _errorMessage = String.Empty;
            GeckoNodeCollection errorBoxes = curBrowser.Document.GetElementsByClassName(NWO_Defines.PageInfo.Login.UndergoingMaintenance);

            foreach (GeckoNode node in errorBoxes)
            {
                if (node.TextContent.Contains(NWO_Defines.PageInfo.Login.UndergoingMaintenanceCheck))
                {
                    _errorMessage = NWO_Defines.PageInfo.Login.UndergoingMaintenanceCheck;
                    return true;
                }
            }

            return false;
        }

        public static String ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
        }
    }
}
