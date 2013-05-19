using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Properties;

namespace VixenPlus {
    internal partial class AboutDialog : Form {
        private readonly System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer { Interval = 50 };
        private int _creditsTop;

        public AboutDialog() {
            InitializeComponent();
            
            _creditsTop = Height;

            Text = Resources.About + Vendor.ProductName;

            lblName.Text = Vendor.ProductName;
            lblDescription.Text = Vendor.ProductDescription;

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = string.Format(Resources.FormattedVersion, version);

            llblURL.Text = Vendor.ProductURL;
        }


        public override sealed string Text {
            get { return base.Text; }
            set { base.Text = value; }
        }


        private void llblURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            llblURL.LinkVisited = true;
            Process.Start(Vendor.ProductURL);
        }


        private void AboutDialog_MouseClick(object sender, EventArgs e) {
            Controls.SetChildIndex(btnOkay, 0);
            var credits = new StringBuilder();
            credits.AppendLine("Inspired By:").AppendLine("K.C. Oaks and Vixen 2.x\n");
            credits.AppendLine("Written By:").AppendLine("John McAdams\nAKA Macebobo\nPronounced Mac E Bo Bo\n\n");
            credits.AppendLine("Beta Tested By:").AppendLine("Falcon\nPhoenix\nEagle\nOregonLights\nDirknerkle\n");
            credits.AppendLine("Supported By:").AppendLine("DIYChristmas.org").AppendLine("Thanks Guys && Gals!");
            credits.AppendLine("\n\n\n\n\n\nLutefisk for all!");
            _timer.Tick += TimerTick;
            _timer.Start();
            lblCredits.AutoSize = true;
            lblCredits.Text = credits.ToString();
            var size = lblCredits.Size;
            lblCredits.AutoSize = false;
            size.Width = Width - 10;
            lblCredits.Size = size;
            lblCredits.Visible = true;
            lblCredits.Location = new Point(5, _creditsTop);
            lblDescription.Visible = false;
            lblName.Visible = false;
            lblVersion.Visible = false;
            llblURL.Visible = false;
        }




        private void TimerTick(object sender, EventArgs e) {
            _creditsTop -= 2;
            if (_creditsTop + lblCredits.Height < 0) {
                _creditsTop = Height;
            }
            lblCredits.Location = new Point(5, _creditsTop);
        }

        private void AboutDialog_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}