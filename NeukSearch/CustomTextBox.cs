using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomTextBox
{
    using System.Windows.Forms;
    using System.Drawing;
    public class CustomTextBox : Panel
    {
        private Color _NormalBorderColor = Color.Gray;
        private Color _FocusBorderColor = Color.SkyBlue;

        public TextBox EditBox;

        public CustomTextBox()
        {
            this.DoubleBuffered = true;
            this.Padding = new Padding(3);

            EditBox = new TextBox();
            EditBox.AutoSize = false;
            EditBox.BorderStyle = BorderStyle.None;
            EditBox.Dock = DockStyle.Fill;
            EditBox.Enter += new EventHandler(EditBox_Refresh);
            EditBox.Leave += new EventHandler(EditBox_Refresh);
            EditBox.Resize += new EventHandler(EditBox_Refresh);
            this.Controls.Add(EditBox);
        }

        private void EditBox_Refresh(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(SystemColors.Window);
            using (Pen borderPen = new Pen(this.EditBox.Focused ? _FocusBorderColor : _NormalBorderColor))
            {
                e.Graphics.DrawRectangle(borderPen, new Rectangle(0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1));
            }
            base.OnPaint(e);
        }
    }
}
