using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace NeukSearch
{
    class MenuExplorer
    {
        public static AutomationElementCollection getSubMenus(AutomationElement menu)
        {
            AutomationElementCollection submenus = menu.FindAll(TreeScope.Descendants, Condition.TrueCondition);

            return submenus;
        }

        // expand 성공시 true
        //        실패시 false
        public static bool expandMenu(AutomationElement menu)
        {
            ExpandCollapsePattern fileECPat;

            try
            {
                fileECPat = menu.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;

                // expand가 불가능한 경우 exception
                fileECPat.Expand();
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            return true;
        }

        public static void collapseMenu(AutomationElement menu)
        {
            ExpandCollapsePattern fileECPat;

            fileECPat = menu.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
            fileECPat.Collapse();
        }

        // 실패 시 return null
        public static AutomationElementCollection getRootMenus(IntPtr hwnd)
        {
            AutomationElement aeDesktop = AutomationElement.RootElement;
            AutomationElement aeForm = aeDesktop.FindFirst(TreeScope.Children,
                    new PropertyCondition(AutomationElement.NativeWindowHandleProperty, hwnd.ToInt32()));

            // invalid hwnd
            if (aeForm == null)
                return null;

            AutomationElementCollection menubars = aeForm.FindAll(TreeScope.Children,
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuBar));

            // 메뉴 없음
            if (menubars.Count == 0)
                return null;

            AutomationElement menuitem = menubars[0];
            AutomationElementCollection menus = menuitem.FindAll(TreeScope.Children,
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem));

            return menus;
        }
    }
}
 