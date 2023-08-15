using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Gecko;
using Gecko.DOM;

namespace NWO_AutoProfessions
{
    public static class NWO_Defines
    {
        public static class AdditionalTasks
        {
            public static readonly String RefineAstralDiamonds                          = "Refine Astral Diamonds";
            public static readonly String SellItems                                     = "Sell Items from Inventory";
            public static readonly String OpenChests                                    = "Open Non-LockBox Chests";
            public static readonly String SellItems_Junk                                = "Sell Items: Junk Items";
            public static readonly String SellItems_NonMagicalEquipment                 = "Sell Items: Non-Magical Equipment";

            public static readonly String ProfessionLeveling                            = "{0} Profession Leveling";
        }

        public static class ArrayLists
        {
            private static ArrayList _ProfessionInformation;
            private static ArrayList _Craftsmen;

            public static ArrayList ProfessionInformation
            {
                get
                {
                    if (_ProfessionInformation == null)
                    {
                        _ProfessionInformation = new ArrayList();
                        ReadProfessionFilesIn();
                    }

                    return _ProfessionInformation;
                }
            }

            public static ArrayList Craftsmen
            {
                get
                {
                    if (_Craftsmen == null)
                    {
                        _Craftsmen = new ArrayList();
                        ReadInCraftsmen();
                    }

                    return _Craftsmen;
                }
            }

            private static void ReadProfessionFilesIn()
            {
                String[] profFileList = Directory.GetFiles(@"Config\\Professions");
                StreamReader sr;

                foreach (String profFile in profFileList)
                {
                    NWO_Profession curProf = new NWO_Profession();
                    sr = new StreamReader(profFile);

                    curProf.Name = profFile.Substring(profFile.IndexOf("ions") + 5, profFile.IndexOf(".txt") - (profFile.IndexOf("ions") + 5));

                    int MonthS = Convert.ToInt32(sr.ReadLine());
                    int dayS = Convert.ToInt32(sr.ReadLine());
                    int MonthE = Convert.ToInt32(sr.ReadLine());
                    int dayE = Convert.ToInt32(sr.ReadLine());
                    curProf.DateStart = new DateTime(2013, MonthS, dayS);
                    curProf.DateEnd = new DateTime(2013, MonthE, dayE);

                    String temp = String.Empty;
                    int count = 0;
                    while (!sr.EndOfStream)
                    {
                        temp = sr.ReadLine();
                        if (temp == String.Empty)
                        {
                            curProf.Rare = count;
                            sr.ReadLine();
                        }
                        else
                        {
                            curProf.TaskNameList.Add(temp);
                            curProf.TaskNameURLList.Add(sr.ReadLine());
                        }
                        count++;
                    }

                    _ProfessionInformation.Add(curProf);

                    sr.Close();
                }
            }

            private static void ReadInCraftsmen()
            {
                String[] craftmenFileList = Directory.GetFiles(@"Config\Professions\Craftsmen");
                StreamReader sr;

                foreach (String craftmenFile in craftmenFileList)
                {
                    NWO_Craftsmen craftMen = new NWO_Craftsmen();

                    sr = new StreamReader(craftmenFile);

                    craftMen.Profession = craftmenFile.Substring((craftmenFile.IndexOf("smen") + 5), craftmenFile.IndexOf(".txt") - (craftmenFile.IndexOf("smen") + 5));
                    craftMen.Rank1Common = sr.ReadLine();
                    craftMen.Rank2Common = sr.ReadLine();
                    craftMen.Rank3Common = sr.ReadLine();
                    craftMen.Rank3Uncommon = sr.ReadLine();
                    craftMen.Rank3Rare = sr.ReadLine();
                    craftMen.Rank3Epic = sr.ReadLine();

                    _Craftsmen.Add(craftMen);

                    sr.Close();
                }
            }
        }

        public static class Delegates
        {
            public delegate void NWO_Callback();
            public delegate void NWO_CallbackText(String text);
            public delegate void NWO_CallbackWebBrowserText(GeckoWebBrowser nwo_browser, String text);
            public delegate void NWO_CallbackTextInt32(String text, Int32 num);
            public delegate void NWO_CallbackTextInt64(String text, Int64 num);

            public delegate Boolean NWO_BoolCallback();
            public delegate Boolean NWO_BoolCallbackWebBrowser(GeckoWebBrowser nwo_browser);
            public delegate Boolean NWO_BoolCallbackWebBrowserText(GeckoWebBrowser nwo_browser, String text);
            public delegate Boolean NWO_BoolCallbackWeBrowserInt32(GeckoWebBrowser nwo_browser, Int32 num);
            public delegate Boolean NWO_BoolCallbackWeBrowserInt64(GeckoWebBrowser nwo_browser, Int64 num);
            public delegate Boolean NWO_BoolCallbacknsIDOMNode(nsIDOMNode nwo_domObject);

            public delegate Int32 NWO_Int32CallbackWebBrowser(GeckoWebBrowser nwo_browser);
            public delegate Int64 NWO_Int64CallbackWebBrowser(GeckoWebBrowser nwo_browser);
            public delegate Int32 NWO_Int32CallbackWebBrowserString(GeckoWebBrowser nwo_browswer, String text);

            public delegate GeckoDivElement NWO_DivEleCallBackWebBrowser(GeckoWebBrowser nwo_browser);
            public delegate GeckoDivElement NWO_DivEleCallBackWebBrowserDivElement(GeckoWebBrowser nwo_browser, GeckoDivElement item);
            public delegate GeckoDivElement NWO_DivEleCallBackWebBrowserInt32(GeckoWebBrowser nwo_browser, Int32 item);

            public delegate GeckoHtmlElement NWO_HtmlEleCallBackWebBrowser(GeckoWebBrowser nwo_browser);
        }

        public static class Inventory
        {
            public static class ItemInfo
            {
                public static readonly String NotUsable                                 = "red";
                public static readonly String Empty                                     = "empty";

                public static class Type
                {
                    public static readonly String Armor                                 = "Upgrade";
                    public static readonly String Weapon                                = "Weapon";
                    public static readonly String Gem                                   = "Gem"; // Enchants/
                    public static readonly String Gem_Food                              = "ProgressionFood"; // Enchant Refine Food(Peals)
                    public static readonly String Generic                               = "Generic"; // Marks
                    public static readonly String None                                  = "None"; // HorseToken
                    public static readonly String IdentifyScroll                        = "IdentifyScroll";
                    public static readonly String RewardPack                            = "RewardPack"; // chests/Lockboxes/
                    public static readonly String Disabled                              = "disabled";
                    public static readonly String DyePack                               = "DyePack";
                    public static readonly String Quest                                 = "Mission";
                    public static readonly String Quest_Starter                         = "MissionGrant";
                    public static readonly String Lockboxes                             = "Lockbox";
                    public static readonly String Chests                                = "Chest";

                    public static class Consumables
                    {
                        public static readonly String Consumable                        = "Device"; // Skill Consumables(Lockpicking)/Mounts/Potions/Exp Scrolls/Injury Kits/Fireworks/
                        
                        public static readonly String Rank1                             = "T1";
                        public static readonly String Rank2                             = "T2";
                        public static readonly String Rank3                             = "T3";
                        public static readonly String Rank4                             = "T4";

                        public static class Potions
                        {
                            public static readonly String Potion                        = "Potion";
                            public static readonly String Tidespan                      = "Blue";
                            public static readonly String Healing                       = "Red";
                            public static readonly String YellowGreen                   = "Yellowgreen";
                        }
                    }
                }

                public static class Quality
                {
                    public static readonly String Junk                                  = "Junk";
                    public static readonly String Non_Magical                           = "Bronze";
                    public static readonly String Magical                               = "Silver";
                    public static readonly String Rare                                  = "Gold";
                    public static readonly String Epic                                  = "Special";
                }
            }
        }

        public static class PageInfo
        {
            public static class Inventory
            {
                public static readonly String Items                                     = "icon-slot";
                public static readonly String Bags_Page                                 = "page-inventory-bags";
                public static readonly String Professions_Tab                           = "inventory-professions";
                public static readonly String Companions_Tab                            = "inventory-companions";
                public static readonly String Bag_Headers                               = "bag-header";
                public static readonly String Buttons                                   = "button";
                public static readonly String Button_Disabled                           = "disabled";
                public static readonly String Confirm                                   = "Confirm";
                public static readonly String Cancel                                    = "Cancel";
                public static readonly String Back                                      = "Back";
                public static readonly String OK                                        = "OK";

                public static class Item
                {
                    public static readonly String Information                           = "item";
                    public static readonly String Sell_To_Vendor                        = "Sell to Vendor";
                    public static readonly String Open_Item                             = "Open";
                }

                public static class Sell
                {
                    public static readonly String Sell_Quantity_TextBox                 = "input";
                    public static readonly String Sell_Quantity_Max_Label               = "attention";
                    public static readonly String Sell_Quantity_TextBox_Name            = "inventorySellQty";
                }

                public static class Currency
                {
                    public static readonly String Currency_Header                       = "bag-currency";
                    public static readonly String AstralDiamonds                        = "Astral Diamonds:";
                    public static readonly String RoughAstralDiamonds                   = "Rough Astral Diamonds:";
                    public static readonly String RoughAstralDiamonds_Button            = "input-field button light";
                    public static readonly String Money                                 = "Money:";
                    public static readonly String Zen                                   = "ZEN:";
                    public static readonly String Glory                                 = "Glory:";
                    public static readonly String ArdentCoin                            = "Ardent Coin:";
                    public static readonly String CelestialCoin                         = "Celestial Coin:";
                }
            }

            public static class Login
            {
                public static readonly String UndergoingMaintenance                     = "login-locked";
                public static readonly String UndergoingMaintenanceCheck                = "undergoing maintenance";
                public static readonly String Attr                                      = "Value";
                public static readonly String UserTextbox                               = "user";
                public static readonly String PasswordTextbox                           = "pass";
                public static readonly String LoginAttr                                 = "Login";
            }

            public static class Main
            {
                public static readonly String CharacterLinkClass                        = "char-list-name";
                public static readonly String All                                       = "nav-button mainNav";
                public static readonly String Sword_Coast_Adventure                     = "dungeons nav-dungeons";
                public static readonly String Character_Sheet                           = "charactersheet nav-charsheet";
                public static readonly String Inventory                                 = "Inventory";
                public static readonly String Professions                               = "professions nav-professions";
                public static readonly String Auction_House                             = "auctionhouse nav-auction";
                public static readonly String Exchange                                  = "exchange nav-exchange";
                public static readonly String Zen_Market                                = "zenmarket nav-zenmarket";
                public static readonly String Guild                                     = "guild nav-guild";
                public static readonly String Mail                                      = "mail nav-mail";
                public static readonly String Change_Character_Mobile                   = "secondary-nav mobileOnly";
                public static readonly String Change_Character                          = "Change Character";
                public static readonly String Logout                                    = "Log Out";
            }

            public static class Profession
            {
                public static readonly String _overviewTab                              = "tab subNav professions-overview";
                public static readonly String _tasksTab                                 = "tab subNav professions-tasks";
                public static readonly String _professionSlots                          = "professions-slots";
                public static readonly String _collectResults                           = "professions-rewards-modal";
                public static readonly String _professionTabs                           = "tab subNav";
                public static readonly String _rareUpdateBar                            = "Rare";       // This is how long till the rare tasks update.

                public static readonly String Tasks                                     = "task-list-entry";
                public static readonly String RareTasks                                 = "rare";
                public static readonly String HigherLevel                               = "higherlevel";
                public static readonly String UnmetRequirements                         = "unmet";
                public static readonly String _oddTasks                                 = "odd";
                public static readonly String _evenTasks                                = "even";
                public static readonly String _professionPage                           = "task-list-block";

                public static readonly String _checkProfessionPage                      = "page-nav tabs-fancy";
                public static readonly String _startTask                                = "input-field button epic";
                public static readonly String _assetSelect                              = "button";
                public static readonly String _assetOptional                            = "select-optional-item";
                public static readonly String _assetButton                              = "icon-info";
                public static readonly String _taskDuration                             = "task-duration-time";
                public static readonly String _taskDurationOverview                     = "bar-text";
                public static readonly String _professionLevelsText                     = "update-content-professions-1";

                public static class Supplies
                {
                    public static readonly String BuySuppliesButton                     = "Buy Supplies";
                    public static readonly String SuppliesInformation                   = "vendor-entry";

                    public static class Type
                    {
                        public static readonly String Charcoal                          = "Charcoal";
                        public static readonly String RockSalt                          = "Rock Salt";
                        public static readonly String SpoolofThread                     = "Spool of Thread";
                        public static readonly String Porridge                          = "Porridge";
                        public static readonly String Solvent                           = "Solvent";
                        public static readonly String Brimstone                         = "Brimstone";
                        public static readonly String Coal                              = "Coal";
                        public static readonly String MoonseaSalt                       = "Moonsea Salt";
                        public static readonly String Quicksilver                       = "Quicksilver";
                        public static readonly String SpoolofSilkThread                 = "Spool of Silk Thread";
                    }

                    public static class Buy
                    {
                        public static readonly String Ok                                = "OK";
                        public static readonly String Cancel                            = "Cancel";
                        public static readonly String Purchase_Quantity_Textbox         = "input";
                        public static readonly String Purchase_Quantity_Max             = "attention";
                        public static readonly String Purchase_Quantity_TextBox_Name    = "inventoryBuyQty";
                    }
                }
            }

            public static class CharacterSelect
            {
                public static readonly String Screen                                    = "page-characterselect";
                public static readonly String Characters                                = "a";
            }

            public static readonly String Popup_Message_Box                             = "close-button closeNotification";
            public static readonly String MessageLoc                                    = "li";
            public static readonly String ErrorMessage                                  = "error";
            public static readonly String InfoMessage                                   = "Info";
            public static readonly String DisconnectedScreen                            = "modal_content";
            
            public static readonly String TextBox                                       = "input";
            public static readonly String Button                                        = "button";
            public static readonly String MobileBackButton                              = "nav-button mobile-back-button linkPreviousPage";
            public static readonly String MaxLabel                                      = "Max";
        }

        public static class PopulateComboBoxes
        {
            public static void Profession(System.Windows.Forms.ComboBox curBox)
            {
                curBox.Items.Clear();

                foreach (NWO_Profession prof in NWO_Defines.ArrayLists.ProfessionInformation)
                {
                    if ((prof.DateStart.Month <= DateTime.Today.Month && prof.DateStart.Day <= DateTime.Today.Day) &&
                        (prof.DateEnd.Day >= DateTime.Today.Month && prof.DateEnd.Day >= DateTime.Today.Day))
                            curBox.Items.Add(prof.Name);
                }

                curBox.SelectedIndex = 0;
            }

            public static void Tasks(System.Windows.Forms.ComboBox curBox, String curProfession, Boolean includeRares)
            {
                curBox.Items.Clear();

                foreach (NWO_Profession prof in NWO_Defines.ArrayLists.ProfessionInformation)
                {
                    if (curProfession == prof.Name)
                    {
                        int count = 0;
                        foreach (String taskN in prof.TaskNameList)
                        {
                            if (count == prof.Rare)
                            {
                                if (includeRares)
                                {
                                    curBox.Items.Add(String.Empty);
                                    curBox.Items.Add("--- Rares ---");
                                }
                                else
                                    break;
                            }
                            curBox.Items.Add(taskN);
                            count++;
                        }
                        break;
                    }
                }

                curBox.SelectedIndex = 0;
            }

            public static void Supplies(System.Windows.Forms.ComboBox curBox)
            {
                curBox.Items.Clear();

                curBox.Items.Add(NWO_Defines.PageInfo.Profession.Supplies.Type.Brimstone);
                curBox.Items.Add(NWO_Defines.PageInfo.Profession.Supplies.Type.Charcoal);
                curBox.Items.Add(NWO_Defines.PageInfo.Profession.Supplies.Type.Coal);
                curBox.Items.Add(NWO_Defines.PageInfo.Profession.Supplies.Type.MoonseaSalt);
                curBox.Items.Add(NWO_Defines.PageInfo.Profession.Supplies.Type.Porridge);
                curBox.Items.Add(NWO_Defines.PageInfo.Profession.Supplies.Type.Quicksilver);
                curBox.Items.Add(NWO_Defines.PageInfo.Profession.Supplies.Type.RockSalt);
                curBox.Items.Add(NWO_Defines.PageInfo.Profession.Supplies.Type.Solvent);
                curBox.Items.Add(NWO_Defines.PageInfo.Profession.Supplies.Type.SpoolofSilkThread);
                curBox.Items.Add(NWO_Defines.PageInfo.Profession.Supplies.Type.SpoolofThread);

                curBox.SelectedIndex = 0;
            }
        }
    }
}
