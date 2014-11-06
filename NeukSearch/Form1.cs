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
            mng = MenuManager.Instance;


            if (!MenuCrawler.crawl(new IntPtr(0x002B0B72)))
            {
                MessageBox.Show("메뉴 없음");
            }

        }

        private void tbInput_TextChanged(object sender, EventArgs e)
        {
            List<string> searchResult = mng.search(tbInput.Text);

            string str = "";

            foreach (string s in searchResult)
            {
                str += s + "\r\n";
            }

            tbResult.Text = str;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // invoke test.
            // SUXEX
            MenuManager.Instance.MenuSet[new IntPtr(0x002B0B72)][5].Descendents[5].invoke();
        }
    }
}