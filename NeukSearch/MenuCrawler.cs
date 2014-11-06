using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace NeukSearch
{
    class MenuCrawler
    {

        public static bool crawl(IntPtr hwnd)
        {
            AutomationElementCollection menus = MenuExplorer.getRootMenus(hwnd);

            // menu가 없는 window는 return false
            if (menus == null)
                return false;

            List<Menu> menulist = crawlMenus(menus, null, hwnd);


            
            MenuManager.Instance.MenuSet.Add(hwnd, menulist);

            return true;
        }


        // recursive function
        private static List<Menu> crawlMenus(AutomationElementCollection menus, Menu parent, IntPtr hwnd)
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

                if (parent != null)
                    tmpMenu.Route.AddRange(parent.Route);

                tmpMenu.Route.Add(i++);

                // expand 성공시
                if (MenuExplorer.expandMenu(menu))
                {
                    AutomationElementCollection submenus = MenuExplorer.getSubMenus(menu);

                    tmpMenu.Attr = MenuAttr.ExpandableMenu;
                    tmpMenu.Descendents = crawlMenus(submenus, tmpMenu, hwnd);

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
