using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Drawing;

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
        public Icon icon { get; set; }          // json으로 변환할 때 이건 빼도 됨
        public string exePath { get; set; }     // json->menu 변환할 때 이걸로 icon구해서 집어넣기
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

        public bool invoke()
        {
            AutomationElementCollection menus = MenuExplorer.getRootMenus(hwnd);

            // invalid hwnd일 때
            // 윈도우가 닫혔을 때
            if(menus == null)
            {
                // 목록에서 삭제
                MenuManager.Instance.MenuSet.Remove(hwnd);
                MenuManager.Instance.FormInstance.refreshSearchResult();

                return false;
            }

            if(this.Attr == MenuAttr.ExpandableMenu)
            {
                return false;
            }

            foreach(int i in Route.GetRange(0, Route.Count - 1))
            {
                AutomationElement menu = menus[i];

                MenuExplorer.expandMenu(menu);
                menus = MenuExplorer.getSubMenus(menu);
            }

            AutomationElement targetMenu = menus[Route[Route.Count - 1]];

            
            try
            {
                InvokePattern settingMenuInvoke = targetMenu.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                settingMenuInvoke.Invoke();
            }
            catch(Exception)
            { // 비활성화된 menu일 때, expandable menu일 때
                return false;
            }

            return true;
        }

        // listbox에서 뽑아가는 값
        public override string ToString()
        {
            Menu tmpMenu = Parent;
            string line = Name;

            while (tmpMenu != null)
            {
                line = tmpMenu.Name + " → " + line;

                tmpMenu = tmpMenu.Parent;
            }

            return line;
        }
    }
}
