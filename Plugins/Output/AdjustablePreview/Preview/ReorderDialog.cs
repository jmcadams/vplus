using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using VixenPlus;

namespace Preview {
    internal partial class ReorderDialog : Form {

        private readonly Dictionary<int, List<uint>> _channelDictionary = new Dictionary<int, List<uint>>();


        public ReorderDialog(List<Channel> channels, Dictionary<int, List<uint>> channelDictionary) {
            InitializeComponent();
            ResetLabel();
            foreach (var pair in channelDictionary) {
                List<uint> list;
                _channelDictionary[pair.Key] = list = new List<uint>();
                list.AddRange(pair.Value);
            }
            var items = channels.ToArray();
            comboBoxFrom.Items.AddRange(items);
            comboBoxTo.Items.AddRange(items);
        }


        private void buttonClear_Click(object sender, EventArgs e) {
            if (comboBoxTo.SelectedIndex == -1) {
                MessageBox.Show("Please select a channel to clear.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else {
                _channelDictionary.Remove(comboBoxTo.SelectedIndex);
                MessageBox.Show(string.Format("Channel '{0}' has been cleared.", comboBoxTo.SelectedItem), Vendor.ProductName, MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }
        }


        private void buttonCopy_Click(object sender, EventArgs e) {
            timerFade.Stop();
            ResetLabel();
            if ((comboBoxFrom.SelectedIndex == -1) || (comboBoxTo.SelectedIndex == -1)) {
                MessageBox.Show("Please select channels to copy from and to.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else {
                List<uint> list;
                if (!_channelDictionary.TryGetValue(comboBoxFrom.SelectedIndex, out list)) {
                    MessageBox.Show(string.Format("{0} has no cells drawn.", comboBoxFrom.SelectedItem), Vendor.ProductName, MessageBoxButtons.OK,
                                    MessageBoxIcon.Hand);
                }
                else {
                    _channelDictionary[comboBoxTo.SelectedIndex] = list;
                    timerFade.Start();
                    lblChannelCopied.Text = "Channel Copied";
                    //MessageBox.Show(
                    //    string.Format("Channel '{0}' has been copied to channel '{1}'.", comboBoxFrom.SelectedItem, comboBoxTo.SelectedItem),
                    //    Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        private void ResetLabel() {
            lblChannelCopied.Text = "";
            lblChannelCopied.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
        }


        public Dictionary<int, List<uint>> ChannelDictionary {
            get { return _channelDictionary; }
        }

        private void timerFade_Tick(object sender, EventArgs e) {
            timerFade.Stop();
            for (var i = 1; i < 16; i++) {
                lblChannelCopied.ForeColor = Color.FromArgb(255 - (i * 16), Color.FromKnownColor(KnownColor.ControlText));
                Application.DoEvents();
                Thread.Sleep(20);
            }
            ResetLabel();
        }
    }
}
