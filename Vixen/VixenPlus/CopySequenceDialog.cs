using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace VixenPlus
{
	internal sealed partial class CopySequenceDialog : Form
	{
		private readonly int _itemsShowing;
		private EventSequence _destSequence;
		private bool _internalUpdate;
		private int _knownTop = -1;
		private EventSequence _sourceSequence;

		public CopySequenceDialog()
		{
			InitializeComponent();
			Cursor = Cursors.WaitCursor;
			try
			{
				foreach (string str in Directory.GetFiles(Paths.SequencePath, "*.vix"))
				{
					try
					{
						var item = new EventSequence(str);
						comboBoxSourceSequence.Items.Add(item);
						comboBoxDestSequence.Items.Add(item);
					}
					catch
					{
					}
				}
			}
			finally
			{
				Cursor = Cursors.Default;
			}
			int num = (listViewMapping.Width - 22) >> 1;
			listViewMapping.Columns[0].Width = num;
			listViewMapping.Columns[1].Width = num;
			comboBoxDestChannels.Width = num;
			comboBoxDestChannels.Left = num + listViewMapping.Left;
			_itemsShowing = listViewMapping.Height/17;
		}

		private void buttonApply_Click(object sender, EventArgs e)
		{
			_sourceSequence = (EventSequence) comboBoxSourceSequence.SelectedItem;
			_destSequence = (EventSequence) comboBoxDestSequence.SelectedItem;
			if (_sourceSequence == null)
			{
				MessageBox.Show("Source program must be selected", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else if (_destSequence == null)
			{
				MessageBox.Show("Destination program must be selected", Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Hand);
			}
			else
			{
				Cursor = Cursors.WaitCursor;
				listViewMapping.BeginUpdate();
				listViewMapping.Items.Clear();
				foreach (Channel channel in _sourceSequence.Channels)
				{
					listViewMapping.Items.Add(channel.Name).SubItems.Add("none");
				}
				listViewMapping.EndUpdate();
				comboBoxDestChannels.Items.Clear();
				comboBoxDestChannels.Items.Add("none");
				comboBoxDestChannels.Items.AddRange(_destSequence.Channels.ToArray());
				var comparer = new CaseInsensitiveComparer();
				foreach (ListViewItem item in listViewMapping.Items)
				{
					foreach (object obj2 in comboBoxDestChannels.Items)
					{
						if ((obj2 is Channel) && (comparer.Compare(item.Text, ((Channel) obj2).Name) == 0))
						{
							item.Tag = obj2;
							item.SubItems[1].Text = ((Channel) obj2).Name;
						}
					}
				}
				Cursor = Cursors.Default;
				buttonOK.Enabled = true;
			}
		}

		private void buttonAutoMap_Click(object sender, EventArgs e)
		{
			if (CheckSequences())
			{
				int num = Math.Min(_sourceSequence.ChannelCount, _destSequence.ChannelCount);
				listViewMapping.BeginUpdate();
				for (int i = 0; i < num; i++)
				{
					listViewMapping.Items[i].Tag = _destSequence.Channels[i];
					listViewMapping.Items[i].SubItems[1].Text = _destSequence.Channels[i].Name;
				}
				listViewMapping.EndUpdate();
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			try
			{
				if (checkBoxSequenceLength.Checked && (_destSequence.Time < _sourceSequence.Time))
				{
					_destSequence.Time = _sourceSequence.Time;
				}
				int num4 = (_sourceSequence.EventValues.GetLength(1) > _destSequence.EventValues.GetLength(1))
					           ? _destSequence.EventValues.GetLength(1)
					           : _sourceSequence.EventValues.GetLength(1);
				foreach (ListViewItem item in listViewMapping.Items)
				{
					if (item.Tag != null)
					{
						int index = item.Index;
						int num2 = _destSequence.Channels.IndexOf((Channel) item.Tag);
						if ((num2 != -1) && (num2 < _destSequence.EventValues.GetLength(0)))
						{
							for (int i = 0; i < num4; i++)
							{
								_destSequence.EventValues[num2, i] = _sourceSequence.EventValues[index, i];
							}
						}
					}
				}
				if (checkBoxChannelDefs.Checked)
				{
					foreach (ListViewItem item in listViewMapping.Items)
					{
						if (item.Tag != null)
						{
							Channel channel = _sourceSequence.Channels[item.Index];
							var tag = (Channel) item.Tag;
							tag.Color = channel.Color;
							tag.Name = channel.Name;
						}
					}
				}
				_destSequence.Save();
				MessageBox.Show("Channels have been copied.\nDestination sequence has been saved.", Vendor.ProductName,
				                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void buttonReset_Click(object sender, EventArgs e)
		{
			if (CheckSequences())
			{
				listViewMapping.BeginUpdate();
				foreach (ListViewItem item in listViewMapping.Items)
				{
					item.Tag = null;
					item.SubItems[1].Text = "none";
				}
				comboBoxDestChannels.SelectedIndex = 0;
				listViewMapping.EndUpdate();
			}
		}

		private bool CheckSequences()
		{
			if ((_sourceSequence == null) || (_destSequence == null))
			{
				MessageBox.Show("Please select both source and destination sequences first and apply your selection.",
				                Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			return true;
		}

		private void comboBoxDestChannels_Leave(object sender, EventArgs e)
		{
			comboBoxDestChannels.Visible = false;
		}

		private void comboBoxDestChannels_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_internalUpdate && (listViewMapping.SelectedItems.Count != 0))
			{
				ListViewItem item = listViewMapping.SelectedItems[0];
				if (comboBoxDestChannels.SelectedIndex == 0)
				{
					item.Tag = null;
					item.SubItems[1].Text = "none";
				}
				else
				{
					item.Tag = comboBoxDestChannels.SelectedItem;
					item.SubItems[1].Text = ((Channel) item.Tag).Name;
				}
			}
		}

		private void comboBoxSourceSequence_SelectedIndexChanged(object sender, EventArgs e)
		{
			buttonOK.Enabled = false;
		}

		private void CopySequenceDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			foreach (EventSequence sequence in comboBoxSourceSequence.Items)
			{
				sequence.Dispose();
			}
		}


		private void groupBox2_Leave(object sender, EventArgs e)
		{
			comboBoxDestChannels.Visible = false;
		}


		private void listViewMapping_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
			e.DrawDefault = true;
		}

		private void listViewMapping_DrawItem(object sender, DrawListViewItemEventArgs e)
		{
			e.DrawDefault = true;
			if ((listViewMapping.SelectedItems.Count > 0) && (listViewMapping.TopItem.Index != _knownTop))
			{
				int index = listViewMapping.SelectedItems[0].Index;
				if ((index < listViewMapping.TopItem.Index) || (index > (listViewMapping.TopItem.Index + _itemsShowing)))
				{
					comboBoxDestChannels.Visible = false;
				}
				else
				{
					comboBoxDestChannels.Top = listViewMapping.SelectedItems[0].Position.Y + listViewMapping.Top;
					comboBoxDestChannels.Visible = true;
				}
			}
			_knownTop = listViewMapping.TopItem.Index;
		}

		private void listViewMapping_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
		{
			e.DrawDefault = true;
		}

		private void listViewMapping_Enter(object sender, EventArgs e)
		{
			comboBoxDestChannels.Visible = listViewMapping.SelectedItems.Count > 0;
		}

		private void listViewMapping_Leave(object sender, EventArgs e)
		{
			comboBoxDestChannels.Visible = ActiveControl == comboBoxDestChannels;
		}

		private void listViewMapping_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listViewMapping.SelectedItems.Count > 0)
			{
				comboBoxDestChannels.Top = listViewMapping.SelectedItems[0].Position.Y + listViewMapping.Top;
				var tag = (Channel) listViewMapping.SelectedItems[0].Tag;
				_internalUpdate = true;
				if (tag == null)
				{
					comboBoxDestChannels.SelectedIndex = 0;
				}
				else
				{
					comboBoxDestChannels.SelectedItem = tag;
				}
				_internalUpdate = false;
				comboBoxDestChannels.Visible = true;
			}
			else
			{
				comboBoxDestChannels.Visible = false;
			}
		}
	}
}