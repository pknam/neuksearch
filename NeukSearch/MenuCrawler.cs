using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Drawing;

namespace NeukSearch
{
    class MenuCrawler
    {

        public static bool crawl(IntPtr hwnd, string exePath)
        {
            Icon icon = Icon.ExtractAssociatedIcon(exePath);
            AutomationElementCollection menus = MenuExplorer.getRootMenus(hwnd);

            // menu가 없는 window는 return false
            if (menus == null)
                return false;


            // crawlMenus 재귀호출때마다 icon부르면 비효율적이라
            // 여기서 미리 icon 구해서 넘김
            List<Menu> menulist = crawlMenus(menus, null, hwnd, exePath, icon);


            
            MenuManager.Instance.MenuSet.Add(hwnd, menulist);

            return true;
        }


        // recursive function
        private static List<Menu> crawlMenus(AutomationElementCollection menus, Menu parent, IntPtr hwnd, string exePath, Icon icon)
        {
            List<Menu> menuList = new List<Menu>();
            int i = 0;


            foreach (AutomationElement menu in menus)
            {
                Menu tmpMenu = new Menu(MenuAttr.UnexpandableMenu);
                tmpMenu.Name = menu.Current.Name;
                tmpMenu.Parent = parent;
                tmpMenu.Route = new List<int>();
                tmpMenu.hwnd = hwnd;
                tmpMenu.exePath = exePath;
                tmpMenu.icon = icon;

                if (parent != null)
                    tmpMenu.Route.AddRange(parent.Route);

                tmpMenu.Route.Add(i++);

                // expand 성공시
                if (MenuExplorer.expandMenu(menu))
                {
                    AutomationElementCollection submenus = MenuExplorer.getSubMenus(menu);

                    tmpMenu.Attr = MenuAttr.ExpandableMenu;
                    tmpMenu.Descendents = crawlMenus(submenus, tmpMenu, hwnd, exePath, icon);

                    MenuExplorer.collapseMenu(menu);
                }
                else // expand 실패시
                {
                    tmpMenu.Name = menu.Current.Name;
                    tmpMenu.Descendents = null;
                }

                menuList.Add(tmpMenu);


            }

            return menuList;

        }
    }
}
