using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NeukSearch
{
    class CustomView : System.Windows.Forms.Form
    {
        public CustomButton RefreshButton { get; set; }
        public CustomButton SettingsButton { get; set; }
        public CustomTextBox SearchTextBox { get; set; }

        public CustomView()
        {
            this.BackColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ControlInit();
            SearchTextBox.Focus();

            SearchTextBox.Enter += SearchTextBox_GotFocus;
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

        private void SearchTextBox_GotFocus(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(460, 40);

            SearchTextBox.AutoSize = false;
            SearchTextBox.Location = new System.Drawing.Point(5, 5);
            SearchTextBox.Size = new System.Drawing.Size(340, 30);
            RefreshButton.Location = new System.Drawing.Point(SearchTextBox.Width + 10, 4);
            RefreshButton.Size = new System.Drawing.Size(50, 32);
            SettingsButton.Location = new System.Drawing.Point(RefreshButton.Width + SearchTextBox.Width + 13, 4);
            SettingsButton.Size = new System.Drawing.Size(50, 32);
        }

        private void ControlInit()
        {
            RefreshButton = new CustomButton();
            SettingsButton = new CustomButton();
            SearchTextBox = new CustomTextBox();

            RefreshButton.BackgroundImage = System.Drawing.Image.FromFile("C:\\Users\\석준\\Google 드라이브\\source\\Visual Studio 2013\\Projects\\CustomTextBox\\CustomTextBox\\refresh.png");
            RefreshButton.BackgroundImageLayout = ImageLayout.Center;
            SettingsButton.BackgroundImage = System.Drawing.Image.FromFile("C:\\Users\\석준\\Google 드라이브\\source\\Visual Studio 2013\\Projects\\CustomTextBox\\CustomTextBox\\settings.png");
            SettingsButton.BackgroundImageLayout = ImageLayout.Center;

            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.SettingsButton);
        }
    }
}
