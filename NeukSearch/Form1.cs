using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

using Newtonsoft.Json;
using System.Data.SQLite;

namespace NeukSearch
{
    public partial class Form1 : Form
    {
        MenuManager mng;
        private KeyboardHook keyboardhook;

        public Form1()
        {
            CustomView view = new CustomView();
            view.Show();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ProcessOpenEvent._instance.run();
            mng = MenuManager.Instance;
            mng.FormInstance = this;

            WindowHookNet windowhook = WindowHookNet.Instance;
            windowhook.WindowCreated += windowhook_WindowCreated;
            windowhook.WindowDestroyed += windowhook_WindowDestroy;

            keyboardhook = new KeyboardHook();
            keyboardhook.KeyPressed += keyboardhook_KeyPressed;
            keyboardhook.RegisterHotKey(NeukSearch.ModifierKeys.Control | NeukSearch.ModifierKeys.Shift, Keys.C);

        }

        void keyboardhook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            Win32.SetForegroundWindow(this.Handle);
            tbInput.Focus();
        }

        private IntPtr Pid2Hwnd(int pid)
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();

            foreach (var process in processes)
            {
                if (process.Id == pid)
                {
                    return process.MainWindowHandle;
                }
            }

            return IntPtr.Zero;
        }

        void windowhook_WindowDestroy(object sender, WindowHookEventArgs aArgs)
        {
            if (mng.MenuSet.Keys.Contains(aArgs.Handle))
            {
                mng.MenuSet.Remove(aArgs.Handle);
                this.refreshSearchResult();
            }
        }

        void windowhook_WindowCreated(object aSender, WindowHookEventArgs aArgs)
        {
            OverlayForm overlay = new OverlayForm();
            MenuSqliteHelper sqlite = MenuSqliteHelper._instance;
            //var reader = await sqlite.GetMenuDataByPath(aArgs.ExecutablePath);

            AutomationElementCollection menus = MenuExplorer.getRootMenus(aArgs.Handle);

            if (menus != null)
            {
                SQLiteDataReader reader = sqlite.GetMenuDataByPath(aArgs.ExecutablePath);
                if (!reader.HasRows)
                {
                    overlay.Show();
                    Win32.ShowWindow(aArgs.Handle, Win32.ShowWindowCommands.SW_RESTORE);
                    //데이터 없는경우
                    List<Menu> menulist = MenuCrawler.crawl(aArgs.Handle, aArgs.ExecutablePath);
                    overlay.Close();

                    if (menulist != null)
                    {
                        MenuSqliteHelper._instance.SetMenuData(menulist);
                        this.refreshSearchResult();
                    }
                    else
                    {
                        //MessageBox.Show("no menu");
                    }
                }
                else
                {
                    //데이터 있는경우
                    DataTable table = new DataTable();
                    table.Load(reader);

                    List<Menu> json_menulist = JsonUtil.Json2MenuList(Util.Base64Decode(table.Rows[0][1].ToString()), aArgs.Handle);
                    MenuManager.Instance.MenuSet.Add(aArgs.Handle, json_menulist);
                    this.refreshSearchResult();
                }
            }
        }

        public void refreshSearchResult()
        {
            List<Menu> searchResult = mng.search(tbInput.Text);
            listBox1.DataSource = searchResult;
        }

        private void tbInput_TextChanged(object sender, EventArgs e)
        {
            this.refreshSearchResult();
        }

        private void tbInput_KeyDown(object sender, KeyEventArgs e)
        {

            switch(e.KeyCode)
            {
                case Keys.Down:
                    if (listBox1.SelectedIndex < listBox1.Items.Count - 1)
                        listBox1.SetSelected(listBox1.SelectedIndex + 1, true);
                    break;

                case Keys.Up:
                    if (listBox1.SelectedIndex > 0)
                        listBox1.SetSelected(listBox1.SelectedIndex - 1, true);
                    break;

                case Keys.Enter:
                    if (listBox1.SelectedIndex < 0)
                        break;

                    Menu selected = listBox1.SelectedItem as Menu;

                    // 메뉴 클릭 실패시
                    if(!selected.invoke())
                    {
                        // beep sound
                        //System.Media.SystemSounds.Beep.Play();

                        // 검색 폼 활성화
                        this.Activate();
                        tbInput.Focus();
                    }
                    else
                        e.Handled = true;

                    break;
            }
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            e.DrawBackground();

            Rectangle imageRect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Height, e.Bounds.Height);
            Rectangle stringRect = new Rectangle(e.Bounds.X + 20, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

            e.Graphics.DrawIcon(((Menu)listBox1.Items[e.Index]).icon, imageRect);
            e.Graphics.DrawString(listBox1.Items[e.Index].ToString(),
                e.Font, Brushes.Black, stringRect, StringFormat.GenericDefault);

            e.DrawFocusRectangle();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            WindowHookNet hook = WindowHookNet.Instance;
            hook.WindowCreated -= windowhook_WindowCreated;
            hook.WindowDestroyed -= windowhook_WindowDestroy;

        }
    }
}