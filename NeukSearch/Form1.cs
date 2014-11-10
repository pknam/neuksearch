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
            ProcessOpenEvent._instance.run();
            mng = MenuManager.Instance;


            if (!MenuCrawler.crawl(new IntPtr(0x002308B4)))
            {
                MessageBox.Show("메뉴 없음");
            }

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
                    Menu selected = listBox1.SelectedItem as Menu;
                    selected.invoke();
                    break;
            }
        }
    }
}