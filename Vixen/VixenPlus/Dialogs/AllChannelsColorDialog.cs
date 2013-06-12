using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using Properties;

namespace VixenPlus.Dialogs {
    public partial class AllChannelsColorDialog : Form {
        private readonly Dictionary<int, Color> _colorsInUse;
        private readonly Preference2 _preferences;
        private readonly SolidBrush _solidBrush;
        private Color _dragColor;


        public AllChannelsColorDialog(List<Channel> channels) {
            InitializeComponent();
            foreach (var channel in channels) {
                listBoxChannels.Items.Add(channel.Clone());
            }
            _colorsInUse = new Dictionary<int, Color>();
            _solidBrush = new SolidBrush(Color.White);
            foreach (var channel in channels) {
                if (_colorsInUse.ContainsKey(channel.Color.ToArgb())) {
                    continue;
                }

                listBoxColorsInUse.Items.Add(channel.Color);
                _colorsInUse.Add(channel.Color.ToArgb(), channel.Color);
            }
            _preferences = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences;
            var strArray = _preferences.GetString("CustomColors").Split(new[] {','});
            var numArray = new int[strArray.Length];
            for (var i = 0; i < strArray.Length; i++) {
                numArray[i] = int.Parse(strArray[i]);
            }
            colorDialog.CustomColors = numArray;
        }


        public List<Color> ChannelColors {
            get {
                return (from Channel channel in listBoxChannels.Items
                        select channel.Color).ToList();
            }
        }


        private void AllChannelsColorDialog_HelpButtonClicked(object sender, CancelEventArgs e) {
            e.Cancel = true;
            const string helpText =
                "Drag colors from the color list onto the channel list." +
                "\n\nIf you have one channel selected, the color will apply to whatever channel you drop it on." +
                "\n\nIf you have multiple channels selected, the color will apply to all channels selected.";
            using (var dialog = new HelpDialog(helpText)) {
                dialog.ShowDialog();
            }
        }


        private void buttonNewColor_Click(object sender, EventArgs e) {
            if (colorDialog.ShowDialog() != DialogResult.OK) {
                return;
            }

            if (_colorsInUse.ContainsKey(colorDialog.Color.ToArgb())) {
                MessageBox.Show(Resources.ColorAlreadyExists, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else {
                listBoxColorsInUse.Items.Add(colorDialog.Color);
                _colorsInUse.Add(colorDialog.Color.ToArgb(), colorDialog.Color);
            }
            var strArray = new string[colorDialog.CustomColors.Length];
            for (var i = 0; i < strArray.Length; i++) {
                strArray[i] = colorDialog.CustomColors[i].ToString(CultureInfo.InvariantCulture);
            }
            _preferences.SetString("CustomColors", string.Join(",", strArray));
        }


        private void listBoxChannels_DragDrop(object sender, DragEventArgs e) {
            var data = (Color) e.Data.GetData(typeof (Color));
            if (listBoxChannels.SelectedItems.Count > 1) {
                foreach (Channel channel in listBoxChannels.SelectedItems) {
                    channel.Color = data;
                }
            }
            else {
                var p = listBoxChannels.PointToClient(new Point(e.X, e.Y));
                ((Channel) listBoxChannels.Items[listBoxChannels.IndexFromPoint(p)]).Color = data;
            }
            listBoxChannels.Refresh();
        }


        private void listBoxChannels_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(typeof (Color))) {
                e.Effect = DragDropEffects.Copy;
            }
        }


        private void listBoxChannels_DrawItem(object sender, DrawItemEventArgs e) {
            var listBox = sender as ListBox;
            if (e.Index == -1 || listBox == null) {
                return;
            }

            Channel.DrawItem(listBox, e, (Channel) listBoxChannels.Items[e.Index]);
        }


        private void listBoxColorsInUse_DrawItem(object sender, DrawItemEventArgs e) {
            if (e.Index == -1) {
                return;
            }
            _solidBrush.Color = (Color) listBoxColorsInUse.Items[e.Index];
            e.Graphics.FillRectangle(_solidBrush, e.Bounds);
        }


        private void listBoxColorsInUse_MouseDown(object sender, MouseEventArgs e) {
            var num = listBoxColorsInUse.IndexFromPoint(e.Location);
            _dragColor = num != -1 ? (Color) listBoxColorsInUse.Items[num] : Color.Empty;
        }


        private void listBoxColorsInUse_MouseMove(object sender, MouseEventArgs e) {
            if (((MouseButtons & MouseButtons.Left) != MouseButtons.None) && (_dragColor != Color.Empty)) {
                listBoxColorsInUse.DoDragDrop(_dragColor, DragDropEffects.Copy);
            }
        }
    }
}
