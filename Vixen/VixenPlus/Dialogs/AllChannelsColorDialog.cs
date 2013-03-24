using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace VixenPlus.Dialogs
{
	public partial class AllChannelsColorDialog : Form
	{
		private readonly Dictionary<int, Color> _colorsInUse;
		private readonly Preference2 _preferences;
		private readonly SolidBrush _solidBrush;
		private Color _dragColor;

		public AllChannelsColorDialog(List<Channel> channels)
		{
			InitializeComponent();
			foreach (Channel channel in channels)
			{
				listBoxChannels.Items.Add(channel.Clone());
			}
			_colorsInUse = new Dictionary<int, Color>();
			_solidBrush = new SolidBrush(Color.White);
			foreach (Channel channel in channels)
			{
				if (!_colorsInUse.ContainsKey(channel.Color.ToArgb()))
				{
					listBoxColorsInUse.Items.Add(channel.Color);
					_colorsInUse.Add(channel.Color.ToArgb(), channel.Color);
				}
			}
			_preferences = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences;
			string[] strArray = _preferences.GetString("CustomColors").Split(new[] {','});
			var numArray = new int[strArray.Length];
			for (int i = 0; i < strArray.Length; i++)
			{
				numArray[i] = int.Parse(strArray[i]);
			}
			colorDialog.CustomColors = numArray;
		}

		public List<Color> ChannelColors
		{
			get
			{
				var list = new List<Color>();
				foreach (Channel channel in listBoxChannels.Items)
				{
					list.Add(channel.Color);
				}
				return list;
			}
		}

		private void AllChannelsColorDialog_HelpButtonClicked(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			var dialog =
				new HelpDialog(
					"Drag colors from the color list onto the channel list.\n\nIf you have one channel selected, the color will apply to whatever channel you drop it on.\n\nIf you have multiple channels selected, the color will apply to all channels selected.");
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void buttonNewColor_Click(object sender, EventArgs e)
		{
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				if (_colorsInUse.ContainsKey(colorDialog.Color.ToArgb()))
				{
					MessageBox.Show("Color already exists in the list.", Vendor.ProductName, MessageBoxButtons.OK,
					                MessageBoxIcon.Asterisk);
				}
				else
				{
					listBoxColorsInUse.Items.Add(colorDialog.Color);
					_colorsInUse.Add(colorDialog.Color.ToArgb(), colorDialog.Color);
				}
				var strArray = new string[colorDialog.CustomColors.Length];
				for (int i = 0; i < strArray.Length; i++)
				{
					strArray[i] = colorDialog.CustomColors[i].ToString(CultureInfo.InvariantCulture);
				}
				_preferences.SetString("CustomColors", string.Join(",", strArray));
			}
		}


		private void listBoxChannels_DragDrop(object sender, DragEventArgs e)
		{
			var data = (Color) e.Data.GetData(typeof (Color));
			if (listBoxChannels.SelectedItems.Count > 1)
			{
				foreach (Channel channel in listBoxChannels.SelectedItems)
				{
					channel.Color = data;
				}
			}
			else
			{
				Point p = listBoxChannels.PointToClient(new Point(e.X, e.Y));
				((Channel) listBoxChannels.Items[listBoxChannels.IndexFromPoint(p)]).Color = data;
			}
			listBoxChannels.Refresh();
		}

		private void listBoxChannels_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof (Color)))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		private void listBoxChannels_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index != -1)
			{
				var channel = (Channel) listBoxChannels.Items[e.Index];
				e.Graphics.FillRectangle(Brushes.White, e.Bounds);
				var rect = new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Height - 4, e.Bounds.Height - 4);
				e.Graphics.DrawRectangle((e.State & DrawItemState.Selected) != DrawItemState.None ? Pens.Black : Pens.White, rect);
				rect.X += 2;
				rect.Y += 2;
				rect.Width -= 3;
				rect.Height -= 3;
				e.Graphics.FillRectangle(channel.Brush, rect);
				_solidBrush.Color = ((channel.Color.R + channel.Color.G) + channel.Color.B) < 100 ? Color.White : Color.Black;
				e.Graphics.DrawString(channel.Name, Font, _solidBrush, ((e.Bounds.X + e.Bounds.Height) + 2), (e.Bounds.Y + 3));
			}
		}

		private void listBoxColorsInUse_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index != -1)
			{
				_solidBrush.Color = (Color) listBoxColorsInUse.Items[e.Index];
				e.Graphics.FillRectangle(_solidBrush, e.Bounds);
			}
		}

		private void listBoxColorsInUse_MouseDown(object sender, MouseEventArgs e)
		{
			int num = listBoxColorsInUse.IndexFromPoint(e.Location);
			if (num != -1)
			{
				_dragColor = (Color) listBoxColorsInUse.Items[num];
			}
			else
			{
				_dragColor = Color.Empty;
			}
		}

		private void listBoxColorsInUse_MouseMove(object sender, MouseEventArgs e)
		{
			if (((MouseButtons & MouseButtons.Left) != MouseButtons.None) && (_dragColor != Color.Empty))
			{
				listBoxColorsInUse.DoDragDrop(_dragColor, DragDropEffects.Copy);
			}
		}
	}
}