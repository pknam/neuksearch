using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NeukSearch
{
    using System.IO;
    using System.Diagnostics;
    using System.Reflection;
    class CustomView : System.Windows.Forms.Form
    {
        public CustomButton RefreshButton { get; set; }
        public CustomButton SettingsButton { get; set; }
        public CustomTextBox SearchTextBox { get; set; }

        public CustomView()
        {
            this.BackColor = System.Drawing.Color.AntiqueWhite;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            
            ControlInit();

            this.Activated += Form_GotFocus;
            this.Deactivate += Form_LostFocus;
        }

        private void Form_LostFocus(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(260, 40);

            SearchTextBox.AutoSize = false;
            SearchTextBox.Location = new System.Drawing.Point(5, 5);
            SearchTextBox.Size = new System.Drawing.Size(140, 30);
            RefreshButton.Location = new System.Drawing.Point(SearchTextBox.Width + 10, 4);
            RefreshButton.Size = new System.Drawing.Size(50, 32);
            SettingsButton.Location = new System.Drawing.Point(RefreshButton.Width + SearchTextBox.Width + 13, 4);
            SettingsButton.Size = new System.Drawing.Size(50, 32);
        }

        private void Form_GotFocus(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(460, 40);

            SearchTextBox.AutoSize = false;
            SearchTextBox.Location = new System.Drawing.Point(5, 5);
            SearchTextBox.Size = new System.Drawing.Size(340, 30);
            RefreshButton.Location = new System.Drawing.Point(SearchTextBox.Width + 10, 4);
            RefreshButton.Size = new System.Drawing.Size(50, 32);
            SettingsButton.Location = new System.Drawing.Point(RefreshButton.Width + SearchTextBox.Width + 13, 4);
            SettingsButton.Size = new System.Drawing.Size(50, 32);

            //SearchTextBox.EditBox.BringToFront();
            SearchTextBox.SetCursor();
            
            //SearchTextBox.Focus();
            //SearchTextBox.EditBox.Focus();
            //SearchTextBox.EditBox.Select(0, 1);
            //SearchTextBox.Select();
            //this.ActiveControl = SearchTextBox.EditBox;
            //SearchTextBox.EditBox.SelectAll();
        }

        private void ControlInit()
        {
            RefreshButton = new CustomButton();
            SettingsButton = new CustomButton();
            SearchTextBox = new CustomTextBox();

            //임시경로
            //나중에수정해야함
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string project_path = path.Substring(0, path.Length-10);
            RefreshButton.BackgroundImage = System.Drawing.Image.FromFile(project_path + "\\icon\\refresh.png");
            RefreshButton.BackgroundImageLayout = ImageLayout.Center;
            SettingsButton.BackgroundImage = System.Drawing.Image.FromFile(project_path + "\\icon\\settings.png");
            SettingsButton.BackgroundImageLayout = ImageLayout.Center;

            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.SettingsButton);
        }
    }
}
