using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomTextBox
{
    class CustomButton : System.Windows.Forms.Button
    {
        public CustomButton()
        {
            this.BackColor = System.Drawing.Color.White;
            this.MouseHover += CustomButton_MouseHover;
            this.MouseLeave += CustomButton_MouseLeave;
        }

        void CustomButton_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = System.Drawing.Color.White;
        }

        private void CustomButton_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = System.Drawing.Color.SkyBlue;
        }
    }
}
