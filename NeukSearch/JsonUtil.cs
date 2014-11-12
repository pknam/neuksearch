using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Drawing;

namespace NeukSearch
{
    class JsonUtil
    {
        public static List<Menu> Json2MenuList(string input, IntPtr hwnd)
        {
            JArray ja = JArray.Parse(input);

            return JArray2Menulist(ja, null, hwnd);
        }

        private static List<Menu> JArray2Menulist(JArray ja, Menu parent, IntPtr hwnd)
        {
            List<Menu> menuList = new List<Menu>();


            foreach(JObject item in ja)
            {
                menuList.Add(JObject2Menu(item, parent, hwnd));
            }

            return menuList;
        }

        private static Menu JObject2Menu(JObject js, Menu parent, IntPtr hwnd)
        {
            Menu menu;

            if (js["Attr"].ToString().Equals("1"))
            {
                menu = new Menu(MenuAttr.UnexpandableMenu);
                menu.Descendents = null;
            }
            else
            {
                menu = new Menu(MenuAttr.ExpandableMenu);

                JArray ja = JArray.Parse(js["Descendents"].ToString());
                menu.Descendents = JArray2Menulist(ja, menu, hwnd);
            }

            menu.Name = js["Name"].ToString();
            menu.exePath = js["exePath"].ToString();
            menu.icon = Icon.ExtractAssociatedIcon(menu.exePath);
            menu.Parent = parent;
            menu.hwnd = hwnd;

            menu.Route= JsonConvert.DeserializeObject<List<int>>(js["Route"].ToString());


            return menu;
        }
    }
}
