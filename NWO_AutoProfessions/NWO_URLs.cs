using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gecko;

namespace NWO_AutoProfessions
{
    public static class NWO_URLs
    {
        private static readonly String                      _NWO_GateWay_URL                                        = "http://gateway.playneverwinter.com/";
        private static readonly String                      _NWO_Character_Select_URL                               = "#/characterselect";
        private static readonly String                      _NWO_Character_URL                                      = "#char({0}@{1})";
        private static readonly String                      _NWO_Professions_Overview_URL                           = "/professions";
        private static readonly String                      _NWO_Profession_Task_URL                                = "-tasks";
        private static readonly String                      _NWO_profession_Task_List_URL                           = "/{0}";
        private static readonly String                      _NWO_profession_Task_Select_URL                         = "/{0}";
        private static readonly String                      _NWO_profession_Collect_Reward_URL                      = "/collect-reward/{0}";
        private static readonly String                      _NWO_SWordcoast_Adventures_URL                          = "/adventures";
        private static readonly String                      _NWO_Character_Sheet_URL                                = "/charactersheet";
        private static readonly String                      _NWO_Inventory_URL                                      = "/inventory";
        private static readonly String                      _NWO_Inventory_Professions_URL                          = "-professions";
        private static readonly String                      _NWO_Inventory_Companions_URL                           = "-companions";
        //private static readonly String                      _NWO_Inventory_Item_URL                                 = "/item?item=ent.main.inventory.playerbags[{0}].slots[{1}]";
        //private static readonly String                      _NWO_Inventory_Item_Sell_URL                            = "/item-sell?item=ent.main.inventory.playerbags[{0}].slots[{1}]";
        //private static readonly String                      _NWO_Inventory_Item_Discard_URL                         = "/item-discard?item=ent.main.inventory.playerbags[{0}].slots[{1}]";
        //private static readonly String                      _NWO_Inventory_Item_Open_URL                            = "/item-open?item=ent.main.inventory.playerbags[{0}].slots[{1}]";
        private static readonly String                      _NWO_Professions_Vendory_URL                            = "/vendor";
        private static readonly String                      _NWO_Auction_House_URL                                  = "/auctionhouse";
        private static readonly String                      _NWO_Auction_House_Sell_URL                             = "-sell";
        private static readonly String                      _NWO_Auction_House_Buy_URL                              = "-buy";
        private static readonly String                      _NWO_Exchange_URL                                       = "/exchange";
        private static readonly String                      _NWO_Exchange_Sell_Zen_URL                              = "-sellzen";
        private static readonly String                      _NWO_Exchange_Listings_URL                              = "-listings";
        private static readonly String                      _NWO_Exchange_Log_URL                                   = "-log";
        private static readonly String                      _NWO_Zen_Market_URL                                     = "/zenmarket";
        private static readonly String                      _NWO_Mail_URL                                           = "/mail";
        private static readonly String                      _NWO_Mail_Compse_URL                                    = "-compose";

        private static String                               _NWO_Character_Name;
        private static String                               _NWO_Account_Nickname;
        private static String                               _NWO_Profession_Name;
        private static String                               _NWO_Task_Name;
        private static Int32                                _NWO_Slot_Num;
        //private static Int32                                _NWO_Bag_Num;
        //private static Int32                                _NWO_Bag_Slot_Num;

        internal static String NWO_Charcter_Name
        {
            set
            {
                if (value != _NWO_Character_Name && value != String.Empty)
                    _NWO_Character_Name = value;
            }
        }

        internal static String NWO_Account_Nickname
        {
            set
            {
                if (value != _NWO_Account_Nickname && value != String.Empty)
                    _NWO_Account_Nickname = value;
            }
        }

        internal static String NWO_Profession_Name
        {
            set
            {
                if (value != _NWO_Profession_Name && value != String.Empty)
                    _NWO_Profession_Name = value;
            }
        }

        internal static String NWO_Task_Name
        {
            set
            {
                if (value != _NWO_Task_Name && value != String.Empty)
                    _NWO_Task_Name = value;
            }
        }

        internal static Int32 NWO_Slot_Num
        {
            set
            {
                if (_NWO_Slot_Num != value && value > -1)
                    _NWO_Slot_Num = value;
            }
        }

        internal static String NWO_Gateway_URL
        {
            get
            {
                return _NWO_GateWay_URL;
            }
        }

        internal static String NWO_Character_Select_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Gateway_URL, _NWO_Character_Select_URL);
            }
        }

        internal static String NWO_Character_Url
        {
            get
            {
                return String.Format("{0}{1}", NWO_Gateway_URL, String.Format(_NWO_Character_URL, _NWO_Character_Name, _NWO_Account_Nickname));
            }
        }

        internal static String NWO_Professions_Overview_Url
        {
            get
            {
                return String.Format("{0}{1}", NWO_Character_Url, _NWO_Professions_Overview_URL);
            }
        }

        internal static String NWO_Profession_Task_Tab_Url
        {
            get
            {
                return String.Format("{0}{1}", NWO_Professions_Overview_Url, _NWO_Profession_Task_URL);
            }
        }

        internal static String NWO_Profession_Task_List_Url
        {
            get
            {
                return String.Format("{0}{1}", NWO_Profession_Task_Tab_Url, String.Format(_NWO_profession_Task_List_URL, _NWO_Profession_Name));
            }
        }

        internal static String NWO_Profession_Task_Select_Url
        {
            get
            {
                return String.Format("{0}{1}", NWO_Profession_Task_List_Url, String.Format(_NWO_profession_Task_Select_URL, _NWO_Task_Name));
            }
        }

        internal static String NWO_ProfessionCollectReward_Url
        {
            get
            {
                return String.Format("{0}{1}", NWO_Professions_Overview_Url, String.Format(_NWO_profession_Collect_Reward_URL, _NWO_Slot_Num));
            }
        }

        internal static String NWO_Swordcoast_Adventures_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Character_Url, _NWO_SWordcoast_Adventures_URL);
            }
        }

        internal static String NWO_Character_Sheet_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Character_Url, _NWO_Character_Sheet_URL);
            }
        }

        internal static String NWO_Inventory_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Character_Url, _NWO_Inventory_URL);
            }
        }

        //private static String NWO_Inventory_Bag_Item_URL
        //{
        //    get
        //    {
        //        return String.Format(_NWO_Inventory_Item_URL, _NWO_Bag_Num, _NWO_Bag_Slot_Num);
        //    }
        //}

        //internal static String NWO_Inventory_Item_URL
        //{
        //    get
        //    {
        //        return String.Format("{0}{1}", NWO_Inventory_URL, NWO_Inventory_Bag_Item_URL);
        //    }
        //}

        internal static String NWO_Inventory_Professions_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Inventory_URL, _NWO_Inventory_Professions_URL);
            }
        }

        internal static String NWO_Inventory_Companions_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Inventory_URL, _NWO_Inventory_Companions_URL);
            }
        }

        internal static String NWO_Professions_Vendory_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Professions_Overview_Url, _NWO_Professions_Vendory_URL);
            }
        }

        internal static String NWO_Auction_House_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Character_Url, _NWO_Auction_House_URL);
            }
        }

        internal static String NWO_Auction_House_Sell_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Auction_House_URL, _NWO_Auction_House_Sell_URL);
            }
        }

        internal static String NWO_Auction_House_Buy_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Auction_House_URL, _NWO_Auction_House_Buy_URL);
            }
        }

        internal static String NWO_Exchange_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Character_Url, _NWO_Exchange_URL);
            }
        }

        internal static String NWO_Exchange_Sell_Zen_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Exchange_URL, _NWO_Exchange_Sell_Zen_URL);
            }
        }

        internal static String NWO_Exchange_Listings_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Exchange_URL, _NWO_Exchange_Listings_URL);
            }
        }

        internal static String NWO_Exchange_Log_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Exchange_URL, _NWO_Exchange_Log_URL);
            }
        }

        internal static String NWO_Zen_Market_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Character_Url, _NWO_Zen_Market_URL);
            }
        }

        internal static String NWO_Mail_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Character_Url, _NWO_Mail_URL);
            }
        }

        internal static String NWO_Mail_Compose_URL
        {
            get
            {
                return String.Format("{0}{1}", NWO_Mail_URL, _NWO_Mail_Compse_URL);
            }
        }

        internal static void NWO_Navigate(GeckoWebBrowser curBrowser, String url)
        {
            curBrowser.Navigate(url);
        }

        internal static Boolean NWO_CompareURLs(GeckoWebBrowser curBrowser, String url)
        {
            if (curBrowser.Url.ToString() == url)
                return true;

            return false;
        }
    }
}
