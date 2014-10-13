using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NeukSearch
{
    class Win32MenuManager
    {
        private List<NeukSearch.Menu> m_pRetMenuData { get; set; }
        private Process m_pProcess { get; set; }
        private IntPtr m_pMenu;

        public Win32MenuManager(Process process)
        {
            this.m_pProcess = process;
            m_pRetMenuData = new List<NeukSearch.Menu>();
            m_pMenu = Win32.GetMenu(process.MainWindowHandle);
        }

        public bool GetMenuData()
        {
            if (m_pMenu.ToInt32() == -1) { return false; }
            else
            {
                int main_cnt = Win32.GetMenuItemCount(m_pMenu);
                for (int i = 0; i < main_cnt; i++)
                {
                    StringBuilder main_menu_name = new StringBuilder(0x20);
                    Win32.GetMenuString(m_pMenu, (uint)i, main_menu_name, 0x20, Win32.MF_BYPOSITION);
                    //m_pRetMenuData.Add(GetMenuBuilder(NeukSearch.MenuAttr.ExpandableMenu, main_menu_name.ToString()));

                    GetRecursiveData(m_pMenu, i);
                }
                return true;
            }
        }

        private void GetRecursiveData(IntPtr menu, int pos)
        {
            IntPtr hSubMenu = Win32.GetSubMenu(menu, pos);
            int sub_cnt = Win32.GetMenuItemCount(hSubMenu);

            if (hSubMenu.ToInt32() != 0)
            {
                for (int j = 0; j < sub_cnt; j++)
                {
                    uint menu_id = Win32.GetMenuItemID(hSubMenu, j);
                    StringBuilder menu_name = new StringBuilder(0x20);
                    Win32.GetMenuString(hSubMenu, (uint)j, menu_name, 0x20, Win32.MF_BYPOSITION);
                    if (Win32.GetSubMenu(hSubMenu, j).ToInt32() != 0)
                    {
                        GetRecursiveData(hSubMenu, j);
                    }
                }
            }
            else { return; }
        }

        private NeukSearch.Menu GetMenuBuilder(NeukSearch.MenuAttr attr, string name, NeukSearch.Menu parent = null, List<NeukSearch.Menu> Descendents = null)
        {
            NeukSearch.Menu ret = new NeukSearch.Menu(attr);
            ret.Name = name; ret.Parent = parent; ret.Descendents = Descendents;

            return ret;
        }
    }
}
