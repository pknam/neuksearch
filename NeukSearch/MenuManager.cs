using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace NeukSearch
{

    // singleton
    class MenuManager
    {
        private MenuManager() { }
        
        



        // executable image path는 sqlite db에 넣을 때 구해서 넣을것

        // key : hwnd
        // val : menu list
        public Dictionary<IntPtr, List<Menu>> MenuSet { get; set; }

        private static MenuManager _instance = null;


        public static MenuManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MenuManager();
                    _instance.MenuSet = new Dictionary<IntPtr, List<Menu>>();
                }
                return _instance;
            }
        }

        

        
        

        public List<string> search(string input)
        {
            List<string> result = new List<string>();
            Stack<Menu> s = new Stack<Menu>();


            // 모든 window의 menu를 검사
            foreach (var key in MenuManager.Instance.MenuSet.Keys)
            {
                // window 하나에 대한 menu tree search
                foreach (Menu m in MenuManager.Instance.MenuSet[key].Reverse<Menu>())
                    s.Push(m);

                while (s.Count > 0)
                {
                    Menu tmpMenu = s.Pop();

                    if (tmpMenu.Name.ToLower().Contains(input))
                    {
                        string path = tmpMenu.Name;


                        // parent 탐색용
                        Menu t = tmpMenu;

                        while (t.Parent != null)
                        {
                            t = t.Parent;

                            if (t.Parent != null)
                                path = " -> " + t.Name + " -> " + path;
                            else
                                path = t.Name + " -> " + path;
                        }

                        foreach (var node in tmpMenu.Route)
                        {
                            path += ", " + node;
                        }



                        result.Add(path);

                    }

                    if (tmpMenu.Descendents != null)
                    {
                        foreach (Menu m in tmpMenu.Descendents.Reverse<Menu>())
                            s.Push(m);
                    }
                }
            }
            
            

            return result;
        }

        
    }
}