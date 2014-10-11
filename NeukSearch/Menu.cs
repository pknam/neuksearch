using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Name { get; set; }
        public Menu Parent { get; set; }
        public List<Menu> Descendents { get; set; }
        public MenuAttr Attr { get; set; }


        public Menu(MenuAttr expandable)
        {
            this.Name = "";
            this.Parent = null;
            this.Descendents = null;
            this.Attr = MenuAttr.UnexpandableMenu;
        }
    }
}
