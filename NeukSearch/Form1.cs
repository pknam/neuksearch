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

namespace NeukSearch
{
    public partial class Form1 : Form
    {
        MenuManager mng;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ProcessOpenEvent._instance.run();
            //WindowHookNet windowhook = WindowHookNet.Instance;
            //windowhook.WindowCreated += windowhook_WindowCreated;
            mng = MenuManager.Instance;

            

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

            return new IntPtr(0);
        }

        void windowhook_WindowCreated(object aSender, WindowHookEventArgs aArgs)
        {
            //string exePath = obj["ExecutablePath"].ToString();
            //int pid = int.Parse(obj["ProcessId"].ToString());

            IntPtr hwnd = aArgs.Handle;//Pid2Hwnd(pid);




            if (MenuCrawler.crawl(hwnd, ""))
            {
                MessageBox.Show("add");
            }
            //else
            //{
            //    MessageBox.Show("no menu");
            //}
        }

        private void tbInput_TextChanged(object sender, EventArgs e)
        {
            List<Menu> searchResult = mng.search(tbInput.Text);


            listBox1.DataSource = searchResult;
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
    }
}