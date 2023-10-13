using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Shared.Forms
{
    public class MyLabel : Label
    {
        public static Label Set(string Text = "", Font Font = null, Color ForeColor = new Color(), Color BackColor = new Color())
        {
            Label l = new Label();
            l.Text = Text;
            //l.Font = (Font == null) ? new Font("Calibri", 12) : Font;
            l.ForeColor = (ForeColor == new Color()) ? Color.Black : ForeColor;
            l.BackColor = (BackColor == new Color()) ? SystemColors.Control : BackColor;
            l.AutoSize = true;
            return l;
        }
    }
    public class MyButton : Button
    {
        public static Button Set(string Text = "", DialogResult dialogResult = DialogResult.None, int Width = 75, int Height = 25, Font Font = null, Color ForeColor = new Color(), Color BackColor = new Color())
        {
            Button b = new Button();
            b.Text = Text;
            b.DialogResult = dialogResult;
            b.Width = Width;
            b.Height = Height;
            //b.Font = (Font == null) ? new Font("Calibri", 12) : Font;
            b.ForeColor = (ForeColor == new Color()) ? Color.Black : ForeColor;
            b.BackColor = (BackColor == new Color()) ? SystemColors.Control : BackColor;
            b.UseVisualStyleBackColor = (b.BackColor == SystemColors.Control);
            return b;
        }
    }

    public partial class CountdownMessageBox : Form
    {
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.Focus();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            this.Focus();
        }

        private CountdownMessageBox()
        {
            this.panText = new FlowLayoutPanel();
            this.panButtons = new FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // panText
            // 
            this.panText.Parent = this;
            this.panText.AutoSize = true;
            this.panText.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.panText.Margin = new Padding(0, 20, 0, 20);
            this.panText.MaximumSize = new Size(500, 300);
            this.panText.MinimumSize = new Size(108, 50);
            this.panText.Size = new Size(108, 50);
            // 
            // panButtons
            // 
            this.panButtons.AutoSize = true;
            this.panButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.panButtons.FlowDirection = FlowDirection.RightToLeft;
            this.panButtons.Location = new Point(89, 89);
            this.panButtons.Margin = new Padding(0);
            this.panButtons.MaximumSize = new Size(580, 150);
            this.panButtons.MinimumSize = new Size(90, 0);
            this.panButtons.Size = new Size(90, 30);
            // 
            // SessionExpireMessageBox
            //
            this.AutoScaleDimensions = new SizeF(8F, 19F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(206, 133);
            this.Controls.Add(this.panButtons);
            this.Controls.Add(this.panText);
            this.Font = new Font("Calibri", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Margin = new Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new Size(168, 115);
            this.Name = "CountdownMessageBox";
            this.ShowIcon = false;
            //this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();
            this.Focus();
        }

        public static DialogResult Show(string format = "", string title = "", MessageBoxButtons type = MessageBoxButtons.OK, int countdown = 0)
        {
            int labelWidth = 0;
            int labelHeight = 0;
            int buttonWidth = 0;
            int buttonHeight = 0;
            int totalWidth = 0;
            int totalHeight = 0;

            CountdownMessageBox mb = new CountdownMessageBox();

            mb.Text = title;
            mb._format = format;

            // Timer
            mb._countdown = countdown;
            if (mb._countdown > 0)
            {
                mb._timer = new Timer();
                mb._timer.Interval = 1000;
                mb._timer.Tick += new EventHandler(mb.Timer_Tick);
            }

            // Label
            Label label = MyLabel.Set(mb.GetFormattedText());

            mb.panText.Location = new Point(9, 9);
            label.Name = "countdownLabel";
            mb.panText.Controls.Add(label);
            label.Location = new Point(200, 50);
            label.MaximumSize = new Size(480, 2000);

            label.MinimumSize = new Size(label.Width, 1);
            mb.panText.Height = label.Height;
            mb.panText.MinimumSize = new Size(label.Width, 0);
            mb.panText.MaximumSize = new Size(label.Width, 300);
            mb.panText.Margin = new Padding(0, 35, 0, 25);

            labelWidth = mb.panText.Width;
            labelHeight = mb.panText.Height;

            //Buttons
            List<Button> buttons = GetButtons(type);
            buttons.Reverse();
            foreach (Button b in buttons)
            {
                mb.panButtons.Controls.Add(b);
                b.Location = new Point(3, 3);
                b.TabIndex = buttons.FindIndex(i => i.Text == b.Text);
                b.Click += new EventHandler(mb.Button_Click);
            }
            buttonWidth = mb.panButtons.Width;
            buttonHeight = mb.panButtons.Height;

            //Set Widths
            if (buttonWidth > labelWidth)
            {
                label.MinimumSize = new Size(buttonWidth, 1);
                mb.panText.Height = label.Height;
                mb.panText.MinimumSize = new Size(label.Width, label.Height);
                mb.panText.MaximumSize = new Size(label.Width, 300);

                labelWidth = mb.panText.Width;
                labelHeight = mb.panText.Height;
            }
            totalWidth = labelWidth + 25;

            //Set Height
            totalHeight = labelHeight + buttonHeight;

            mb.panButtons.Location = new Point(totalWidth - buttonWidth, mb.panText.Location.Y + mb.panText.Height + 10);
            mb.Size = new Size(totalWidth + 25, totalHeight + 8);

            if (mb._countdown > 0)
            {
                mb._timer.Start();
            }

            mb.ShowDialog();
            return mb._result;
        }

        private static List<Button> GetButtons(MessageBoxButtons type)
        {
            List<Button> buttons = new List<Button>();

            if (type == MessageBoxButtons.OK || type == MessageBoxButtons.OKCancel)
            {
                buttons.Add(MyButton.Set("OK", DialogResult.OK));
            }
            
            if (type == MessageBoxButtons.YesNo || type == MessageBoxButtons.YesNoCancel)
            {
                buttons.Add(MyButton.Set("Yes", DialogResult.Yes));
            }
            
            if (type == MessageBoxButtons.YesNo || type == MessageBoxButtons.YesNoCancel)
            {
                buttons.Add(MyButton.Set("No", DialogResult.No));
            }

            if (type == MessageBoxButtons.OKCancel || type == MessageBoxButtons.YesNoCancel)
            {
                buttons.Add(MyButton.Set("Cancel", DialogResult.Cancel));
            }

            return buttons;
        }
        
        private string GetFormattedText()
        {
            return string.Format(_format, _countdown);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            _result = ((Button)sender).DialogResult;
            Close();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _countdown--;

            var labels = panText.Controls.Find("countdownLabel", true);
            Control countdownLabel = labels[0];
            countdownLabel.Text = GetFormattedText();

            if (_countdown == 0)
            {
                Close();
                _timer.Stop();
                return;
            }
        }

        private FlowLayoutPanel panText;
        private FlowLayoutPanel panButtons;
        private Timer _timer;
        private int _countdown;
        private string _format;
        private DialogResult _result = DialogResult.None;
    }
}
