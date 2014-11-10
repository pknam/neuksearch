﻿using System;
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

                    // 클릭했는데 비활성화된 menu일 때
                    if(!selected.invoke())
                    {
                        // beep sound
                        System.Media.SystemSounds.Beep.Play();

                        // 검색 폼 활성화
                        this.Activate();
                        tbInput.Focus();
                    }
                    break;
            }
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            

            Rectangle imageRect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Height, e.Bounds.Height);
            Rectangle stringRect = new Rectangle(e.Bounds.X + 20, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

            e.Graphics.DrawIcon(((Menu)listBox1.Items[e.Index]).icon, imageRect);
            e.Graphics.DrawString(listBox1.Items[e.Index].ToString(),
                e.Font, Brushes.Black, stringRect, StringFormat.GenericDefault);

            e.DrawFocusRectangle();
        }
    }
}