using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace NeukSearch
{
    [Flags]
    public enum MenuAttr
    {
        ExpandableMenu,
        UnexpandableMenu
    }

    class Menu
    {
        public IntPtr hwnd { get; set; }
        public string Name { get; set; }
        public Menu Parent { get; set; }
        public List<Menu> Descendents { get; set; }
        public MenuAttr Attr { get; set; }
        public List<int> Route { get; set; }    // root부터 자신까지 거치는 메뉴들의 index list


        public Menu(MenuAttr expandable = MenuAttr.UnexpandableMenu)
        {
            this.Name = "";
            this.Parent = null;
            this.Descendents = null;
            this.Attr = expandable;
        }

        public void invoke()
        {
            AutomationElementCollection menus = MenuExplorer.getRootMenus(hwnd);

            foreach(int i in Route.GetRange(0, Route.Count - 1))
            {
                AutomationElement menu = menus[i];

                MenuExplorer.expandMenu(menu);
                menus = MenuExplorer.getSubMenus(menu);
            }

            AutomationElement targetMenu = menus[Route[Route.Count - 1]];

            InvokePattern settingMenuInvoke = targetMenu.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            settingMenuInvoke.Invoke();
        }

    }
}
