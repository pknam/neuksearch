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

        // main form instance
        // form 생성시 넣어줘야 함
        private Form1 _formInstance = null;
        public Form1 FormInstance
        {
            get
            {
                return _formInstance;
            }
            set
            {
                this._formInstance = value;
            }
        }

        private static MenuManager _instance = null;

        public static MenuManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MenuManager();
                    _instance.MenuSet = new Dictionary<IntPtr, List<Menu>>();
                    _instance.FormInstance = null;
                }
                return _instance;
            }
        }

        

        
        

        public List<Menu> search(string input)
        {
            List<Menu> result = new List<Menu>();
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

                    if (tmpMenu.Name.ToLower().Contains(input) && tmpMenu.Attr != MenuAttr.ExpandableMenu)
                    {
                        result.Add(tmpMenu);
                    }

                    if (tmpMenu.Descendents != null)
                    {
                        foreach (Menu m in tmpMenu.Descendents.GetRange(1, tmpMenu.Descendents.Count-1).Reverse<Menu>())
                            s.Push(m);
                    }
                }
            }
            
            

            return result;
        }

        
    }
}