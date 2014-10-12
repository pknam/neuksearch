using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace NeukSearch
{
    class MenuManager
    {
        // 여러 window들의 menu를 담는 List멤버 추가할것


        // 임시
        List<Menu> aaMenus;

        // window handle을 이용해 타겟 선택
        public bool crawl(IntPtr hwnd)
        {
            AutomationElement aeDesktop = AutomationElement.RootElement;
            AutomationElement aeForm = aeDesktop.FindFirst(TreeScope.Children,
                    new PropertyCondition(AutomationElement.NativeWindowHandleProperty, hwnd.ToInt32()));



            AutomationElementCollection menubars = aeForm.FindAll(TreeScope.Children,
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuBar));


            // 메뉴 없음
            if (menubars.Count == 0)
                return false;


            AutomationElement menuitem = menubars[0];
            AutomationElementCollection menus = menuitem.FindAll(TreeScope.Children,
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem));


            // 임시
            aaMenus = crawlMenus(menus, null);

            return true;
        }

        public List<string> search(string input)
        {
            List<string> result = new List<string>();
            Stack<Menu> s = new Stack<Menu>();


            foreach(Menu m in aaMenus.Reverse<Menu>())
                s.Push(m);

            while(s.Count > 0)
            {
                Menu tmpMenu = s.Pop();

                if (tmpMenu.Name.ToLower().Contains(input))
                {
                    string path = tmpMenu.Name;

                    // parent 탐색용
                    Menu t = tmpMenu;

                    while(t.Parent != null)
                    {
                        t = t.Parent;

                        if(t.Parent != null)
                            path = " -> " + t.Name + " -> " + path;
                        else
                            path = t.Name + " -> " + path;
                    }

                    result.Add(path);
                }

                if (tmpMenu.Descendents != null)
                {
                    foreach (Menu m in tmpMenu.Descendents.Reverse<Menu>())
                        s.Push(m);
                }
            }
            
            

            return result;
        }

        // recursive function
        private List<Menu> crawlMenus(AutomationElementCollection menus, Menu parent)
        {
            List<Menu> menuList = new List<Menu>();


            foreach (AutomationElement menu in menus)
            {
                Menu tmpMenu = new Menu(MenuAttr.UnexpandableMenu);
                tmpMenu.Name = menu.Current.Name;
                tmpMenu.Parent = parent;
                

                try
                {
                    ExpandCollapsePattern fileECPat = menu.GetCurrentPattern(ExpandCollapsePattern.Pattern)
                        as ExpandCollapsePattern;

                    // expand가 불가능한 경우 exception
                    fileECPat.Expand();


                    AutomationElementCollection submenus = menu.FindAll(TreeScope.Descendants,
                        Condition.TrueCondition);


                    tmpMenu.Attr = MenuAttr.ExpandableMenu;
                    tmpMenu.Descendents = crawlMenus(submenus, tmpMenu);


                    fileECPat.Collapse();
                }
                catch (InvalidOperationException e)
                {
                    tmpMenu.Name = menu.Current.Name;
                    tmpMenu.Descendents = null;
                    


                    //Console.WriteLine("not expandable. Error : " + e.Message);
                }
                catch(Exception e)
                {
                    ;
                }
                finally
                {
                    menuList.Add(tmpMenu);
                }



            }

            return menuList;

        }
    }
}