using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CommonControls;

using VixenPlus.Properties;

namespace VixenPlus.Dialogs {
    internal partial class AboutDialog : Form {
        private readonly System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer {Interval = 25};
        private int _creditsTop;
        private const int CreditsMargin = 5;
        private const int CreditScollSize = 1;


        public AboutDialog() {
            InitializeComponent();
            pbIcon.Image = new Icon(Resources.VixenPlus, new Size(64,64)).ToBitmap();

            // Make sure the okay button is always on top since it may get covered by the credits.
            Controls.SetChildIndex(btnOkay, 0);

            _creditsTop = Height;
            _timer.Tick += TimerTick;

            Text = Resources.About + Vendor.ProductName;

            lblName.Text = Vendor.ProductName;
            lblDescription.Text = Vendor.ProductDescription;
            lblVersion.Text = string.Format(Resources.FormattedVersion, Utils.GetVersion());
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
            var credits = new StringBuilder();
            credits.AppendLine(Resources.InspiredBy).AppendLine("K.C. Oaks and Vixen 2.x\n");
            credits.AppendLine(Resources.WrittenBy).AppendLine("John McAdams\nAKA Mac E Bo Bo\n");
            credits.AppendLine(Resources.TestedBy).AppendLine(Resources.BetaTesters);
            credits.AppendLine(Resources.Exterminators).AppendLine(Resources.Buggos);
            credits.AppendLine(Resources.TranslatedBy).AppendLine(Resources.Translators);
            credits.AppendLine(Resources.DedicatedTo).AppendLine(Resources.MyFriends).AppendLine(Resources.ThankYou);
            credits.AppendLine(Resources.OpenSource).AppendLine("FMOD Sound System by:\nFirelight Technologies\n");
            credits.AppendLine("Nutcracker adapted from xLights\nwhich was written by:\nSean Meighan\nMatt Brown\n");
            credits.AppendLine("J1Sys Plugin by: Joshual 1 Systems\nSee: J1Sys License.txt\n");
            credits.AppendLine("7zip (7zr.exe) by Igor Pavlov\nSee: http://www.7-zip.org/license.txt \n");
            credits.AppendLine("Cyotek Color Picker controls library\nSee: http://cyotek.com/blog/tag/colorpicker");
            credits.AppendLine(Resources.Lutefisk);

            // This is how we get the correct height of the credits regardless of how 
            // long they get, used this instead of Graphics.measureString to be lightweight.
            lblCredits.AutoSize = true;
            lblCredits.Text = credits.ToString();
            var size = lblCredits.Size;
            lblCredits.AutoSize = false;
            size.Width = Width - CreditsMargin * 2;
            lblCredits.Size = size;

            UpdateVisibility(false);
            lblCredits.Location = new Point(CreditsMargin, _creditsTop);

            _timer.Start();
        }


        private void TimerTick(object sender, EventArgs e) {
            _creditsTop -= CreditScollSize;
            if (_creditsTop + lblCredits.Height < 0) {
                _creditsTop = Height;
            }
            lblCredits.Location = new Point(CreditsMargin, _creditsTop);
        }


        private void AboutDialog_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Stop();
            _timer.Tick -= TimerTick;
            _timer.Dispose();
        }


        private void lblCredits_Click(object sender, EventArgs e) {
            _timer.Stop();
            UpdateVisibility(true);
        }


        private void UpdateVisibility(bool isNormal) {
            btnCredits.Visible = isNormal;
            lblCredits.Visible = !isNormal;
            lblDescription.Visible = isNormal;
            lblName.Visible = isNormal;
            lblVersion.Visible = isNormal;
            llblURL.Visible = isNormal;
        }
    }
}
