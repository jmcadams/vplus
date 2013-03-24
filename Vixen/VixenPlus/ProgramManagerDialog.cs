using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Vixen.Dialogs;

namespace Vixen
{
	internal partial class ProgramManagerDialog : Form
	{
		private readonly StringFormat m_clipFormat = new StringFormat(StringFormatFlags.NoWrap);
		private readonly int m_executionContextHandle;
		private readonly IExecution m_executionInterface;
		private readonly Font m_programBoldFont;
		private readonly SolidBrush m_programBrush;
		private readonly Font m_programSmallFont;
		private bool m_dirty;
		private object m_dragSource;
		private bool m_internal;

		public ProgramManagerDialog(Host host)
		{
			var dialog = new ProgressDialog();
			InitializeComponent();
			pictureBoxRunProgram.Image = pictureBoxRun.Image;
			m_executionInterface = (IExecution) Interfaces.Available["IExecution"];
			m_executionContextHandle = m_executionInterface.RequestContext(true, false, null);
			m_executionInterface.SetSynchronousProgramChangeHandler(m_executionContextHandle, ProgramChanged);
			m_programBrush = new SolidBrush(Color.FromArgb(0xc2, 0xd3, 0xfc));
			m_programBoldFont = new Font(listBoxPrograms.Font.FontFamily, 12f, FontStyle.Bold);
			m_programSmallFont = new Font(listBoxPrograms.Font.FontFamily, 8f);
			dialog.Show();
			Cursor = Cursors.WaitCursor;
			try
			{
				foreach (string str in Directory.GetFiles(Paths.SequencePath))
				{
					dialog.Message = "Loading sequence " + Path.GetFileNameWithoutExtension(str);
					try
					{
						listBoxSequences.Items.Add(new EventSequenceStub(str, false));
					}
					catch
					{
					}
				}
				dialog.Hide();
				foreach (string str in Directory.GetFiles(Paths.ProgramPath, "*.vpr"))
				{
					var item = new SequenceProgram(str);
					listBoxPrograms.Items.Add(item);
				}
			}
			finally
			{
				Cursor = Cursors.Default;
				GC.Collect();
			}
		}

		private void addEditRemove_AddClick(object sender, EventArgs e)
		{
			var dialog = new TextQueryDialog("New Program", "Name for the new program", string.Empty);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				var item = new SequenceProgram();
				item.Name = dialog.Response;
				listBoxPrograms.Items.Add(item);
				m_dirty = true;
			}
			dialog.Dispose();
		}

		private void addEditRemove_RemoveClick(object sender, EventArgs e)
		{
			if (listBoxPrograms.SelectedItem != null)
			{
				var selectedItem = (SequenceProgram) listBoxPrograms.SelectedItem;
				if (selectedItem != null)
				{
					listBoxPrograms.Items.RemoveAt(listBoxPrograms.SelectedIndex);
				}
				m_dirty = true;
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			SavePrograms();
		}

		private void buttonRadioAction_Click(object sender, EventArgs e)
		{
			if (radioButtonProgramPlugin.Checked)
			{
				var selectedItem = (SequenceProgram) listBoxPrograms.SelectedItem;
				if (selectedItem.EventSequences.Count == 0)
				{
					MessageBox.Show("You have no sequences in this program.\nThere must be at least one sequence defined.",
					                Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					m_dirty = true;
					var dialog = new PluginListDialog(selectedItem);
					dialog.ShowDialog();
					dialog.Dispose();
				}
			}
			else if (radioButtonProfile.Checked)
			{
				openFileDialog.DefaultExt = ".pro";
				openFileDialog.Filter = "Vixen Profile|*.pro";
				openFileDialog.FileName = string.Empty;
				openFileDialog.InitialDirectory = Paths.ProfilePath;
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					((SequenceProgram) listBoxPrograms.SelectedItem).Profile = new Profile(openFileDialog.FileName);
				}
			}
		}


		private void GetCrossFadeValue()
		{
			if (!m_internal &&
			    (((int) numericUpDownCrossFade.Value) != (listBoxPrograms.SelectedItem as SequenceProgram).CrossFadeLength))
			{
				(listBoxPrograms.SelectedItem as SequenceProgram).CrossFadeLength = (int) numericUpDownCrossFade.Value;
				m_dirty = true;
			}
		}

		private void groupBoxProgramPlugin_EnabledChanged(object sender, EventArgs e)
		{
			pictureBoxRunProgram.Visible = groupBoxProgramPlugin.Enabled;
		}

		//ComponentResourceManager manager = new ComponentResourceManager(typeof(ProgramManagerDialog));
		//this.pictureBoxRun.Image = (Image)manager.GetObject("pictureBoxRun.Image");
		//this.pictureBoxStop.Image = (Image)manager.GetObject("pictureBoxStop.Image");


		private void listBoxPrograms_DragDrop(object sender, DragEventArgs e)
		{
			int num = listBoxPrograms.IndexFromPoint(listBoxPrograms.PointToClient(new Point(e.X, e.Y)));
			var program = (SequenceProgram) listBoxPrograms.Items[num];
			var data = (EventSequenceStub) e.Data.GetData("Vixen.EventSequenceStub");
			program.EventSequences.Add(data);
			data.RetrieveSequence();
			if (num == listBoxPrograms.SelectedIndex)
			{
				listBoxProgramSequences.Items.Add(data);
				RefreshProgramListBox();
			}
			else
			{
				listBoxPrograms.SelectedIndex = num;
			}
			m_dirty = true;
		}

		private void listBoxPrograms_DragOver(object sender, DragEventArgs e)
		{
			int num = listBoxPrograms.IndexFromPoint(listBoxPrograms.PointToClient(new Point(e.X, e.Y)));
			e.Effect = ((e.Data.GetDataPresent("Vixen.EventSequenceStub") && (sender != m_dragSource)) && (num != -1))
				           ? DragDropEffects.Move
				           : DragDropEffects.None;
		}

		private void listBoxPrograms_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index != -1)
			{
				Brush brush = ((e.State & DrawItemState.Selected) > DrawItemState.None) ? m_programBrush : Brushes.White;
				e.Graphics.FillRectangle(brush, (e.Bounds.Left + 2), (e.Bounds.Top + 2), (e.Bounds.Width - 4), (e.Bounds.Height - 4));
				var program = (SequenceProgram) listBoxPrograms.Items[e.Index];
				if ((e.State & DrawItemState.Selected) > DrawItemState.None)
				{
					e.Graphics.DrawString(program.Name, m_programBoldFont, Brushes.Blue, (e.Bounds.Left + 5), (e.Bounds.Top + 7));
					e.Graphics.DrawString(string.Format("Length: {0}:{1:d2}", program.Length/0xea60, (program.Length%0xea60)/0x3e8),
					                      m_programSmallFont, Brushes.Black, (e.Bounds.Left + 10), (e.Bounds.Top + 0x21));
					int count = program.EventSequences.Count;
					if (count == 0)
					{
						e.Graphics.DrawString("Empty", m_programSmallFont, Brushes.Black, (e.Bounds.Left + 10), (e.Bounds.Top + 50));
					}
					else if (count == 1)
					{
						e.Graphics.DrawString("1 sequence", m_programSmallFont, Brushes.Black, (e.Bounds.Left + 10), (e.Bounds.Top + 50));
					}
					else
					{
						e.Graphics.DrawString(count.ToString() + " sequences", m_programSmallFont, Brushes.Black, (e.Bounds.Left + 10),
						                      (e.Bounds.Top + 50));
					}
				}
				else
				{
					e.Graphics.DrawString((sender as ListBox).Items[e.Index].ToString(), m_programBoldFont, Brushes.Black,
					                      (e.Bounds.Left + 5), (e.Bounds.Top + 7));
				}
			}
		}

		private void listBoxPrograms_KeyDown(object sender, KeyEventArgs e)
		{
			if (listBoxPrograms.SelectedItem != null)
			{
				if (((listBoxPrograms.SelectedItem) != null) && (e.KeyCode == Keys.Delete))
				{
					listBoxPrograms.Items.RemoveAt(listBoxPrograms.SelectedIndex);
				}
				m_dirty = true;
			}
		}

		private void listBoxPrograms_MouseDown(object sender, MouseEventArgs e)
		{
			int num = listBoxPrograms.IndexFromPoint(e.X, e.Y);
			listBoxProgramSequences.BeginUpdate();
			listBoxProgramSequences.Items.Clear();
			m_internal = true;
			if (num != -1)
			{
				var program = (SequenceProgram) listBoxPrograms.Items[num];
				listBoxProgramSequences.Items.AddRange(program.EventSequences.ToArray());
				if (program.UseSequencePluginData)
				{
					radioButtonSequencePlugin.Checked = true;
				}
				else if (program.Profile == null)
				{
					radioButtonProgramPlugin.Checked = true;
				}
				else
				{
					radioButtonProfile.Checked = true;
				}
				labelProgramName.Text = program.Name;
				numericUpDownCrossFade.Value = program.CrossFadeLength;
			}
			else
			{
				labelProgramName.Text = "No program selected";
				numericUpDownCrossFade.Value = 0M;
			}
			m_internal = false;
			listBoxProgramSequences.EndUpdate();
			groupBoxProgramPlugin.Enabled = addEditRemove.RemoveEnabled = listBoxPrograms.SelectedIndex != -1;
		}

		private void listBoxPrograms_MouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button == MouseButtons.Left) && (listBoxPrograms.SelectedItem != null))
			{
				m_dragSource = sender;
				base.DoDragDrop(listBoxPrograms.SelectedItem, DragDropEffects.Move);
			}
		}

		private void listBoxProgramSongs_DragDrop(object sender, DragEventArgs e)
		{
			EventSequenceStub data;
			if (m_dragSource == listBoxProgramSequences)
			{
				data = (EventSequenceStub) e.Data.GetData("Vixen.EventSequenceStub");
				int index = listBoxProgramSequences.Items.IndexOf(data);
				int num2 = listBoxProgramSequences.IndexFromPoint(listBoxProgramSequences.PointToClient(new Point(e.X, e.Y)));
				if (num2 == -1)
				{
					num2 = listBoxProgramSequences.Items.Count - 1;
				}
				if (index == num2)
				{
					return;
				}
				var selectedItem = (SequenceProgram) listBoxPrograms.SelectedItem;
				listBoxProgramSequences.Items.RemoveAt(index);
				selectedItem.EventSequences.RemoveAt(index);
				listBoxProgramSequences.Items.Insert(num2, data);
				selectedItem.EventSequences.Insert(num2, data);
			}
			else if (m_dragSource == listBoxSequences)
			{
				data = (EventSequenceStub) e.Data.GetData("Vixen.EventSequenceStub");
				((SequenceProgram) listBoxPrograms.SelectedItem).EventSequences.Add(data);
				listBoxProgramSequences.Items.Add(data);
				data.RetrieveSequence();
			}
			RefreshProgramListBox();
		}

		private void listBoxProgramSongs_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = ((e.Data.GetDataPresent("Vixen.EventSequenceStub") &&
			             ((m_dragSource == listBoxSequences) || (m_dragSource == listBoxProgramSequences))) &&
			            (listBoxPrograms.SelectedItem != null))
				           ? DragDropEffects.Move
				           : DragDropEffects.None;
		}

		private void listBoxProgramSongs_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index != -1)
			{
				Brush brush = ((e.State & DrawItemState.Selected) > DrawItemState.None) ? m_programBrush : Brushes.White;
				var rect = new RectangleF((e.Bounds.Left + 2), (e.Bounds.Top + 2), (e.Bounds.Width - 4), (e.Bounds.Height - 4));
				e.Graphics.FillRectangle(brush, rect);
				var stub = (EventSequenceStub) listBoxProgramSequences.Items[e.Index];
				if ((e.State & DrawItemState.Selected) > DrawItemState.None)
				{
					rect.X += 3f;
					rect.Width -= 3f;
					rect.Y += 5f;
					rect.Height -= 5f;
					e.Graphics.DrawString(stub.Name, m_programBoldFont, Brushes.Blue, rect, m_clipFormat);
					e.Graphics.DrawString("Length: " + stub.LengthString, m_programSmallFont, Brushes.Black, (e.Bounds.Left + 10),
					                      (e.Bounds.Top + 0x21));
					if (stub.AudioName != string.Empty)
					{
						rect.X += 5f;
						rect.Width -= 5f;
						rect.Y += 43f;
						rect.Height -= 45f;
						e.Graphics.DrawString("Audio: " + stub.AudioName, m_programSmallFont, Brushes.Black, rect, m_clipFormat);
					}
				}
				else
				{
					rect.X += 3f;
					rect.Width -= 3f;
					rect.Y += 5f;
					rect.Height -= 5f;
					e.Graphics.DrawString(stub.Name, m_programBoldFont, Brushes.Black, rect, m_clipFormat);
				}
			}
		}

		private void listBoxProgramSongs_KeyDown(object sender, KeyEventArgs e)
		{
			if (listBoxProgramSequences.SelectedItem != null)
			{
				var selectedItem = (EventSequenceStub) listBoxProgramSequences.SelectedItem;
				if ((selectedItem != null) && (e.KeyCode == Keys.Delete))
				{
					RemoveSongFromProgram(selectedItem);
				}
			}
		}

		private void listBoxProgramSongs_MouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button == MouseButtons.Left) && (listBoxProgramSequences.SelectedItem != null))
			{
				m_dragSource = sender;
				base.DoDragDrop(listBoxProgramSequences.SelectedItem, DragDropEffects.Move);
			}
		}

		private void listBoxSongs_DragDrop(object sender, DragEventArgs e)
		{
			RemoveSongFromProgram((EventSequenceStub) listBoxProgramSequences.SelectedItem);
		}

		private void listBoxSongs_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = (e.Data.GetDataPresent("Vixen.EventSequenceStub") && (m_dragSource == listBoxProgramSequences))
				           ? DragDropEffects.Move
				           : DragDropEffects.None;
		}

		private void listBoxSongs_MouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button == MouseButtons.Left) && (listBoxSequences.SelectedItem != null))
			{
				m_dragSource = sender;
				base.DoDragDrop(listBoxSequences.SelectedItem, DragDropEffects.Move);
			}
		}

		private void numericUpDownCrossFade_Leave(object sender, EventArgs e)
		{
			GetCrossFadeValue();
		}

		private void numericUpDownCrossFade_ValueChanged(object sender, EventArgs e)
		{
			GetCrossFadeValue();
		}

		private void pictureBoxRunProgram_Click(object sender, EventArgs e)
		{
			var selectedItem = listBoxPrograms.SelectedItem as SequenceProgram;
			if (selectedItem.UseSequencePluginData)
			{
				foreach (EventSequenceStub stub in selectedItem.EventSequences)
				{
					if (!VerifyOutputPlugins(stub.Sequence.PlugInData))
					{
						MessageBox.Show(
							string.Format("Sequence \"{0}\" does not have any output plugins setup and/or enabled.", stub.Name),
							Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						return;
					}
				}
			}
			else if (!VerifyOutputPlugins(selectedItem.PlugInData))
			{
				MessageBox.Show("Program does not have any output plugins setup and/or enabled.", Vendor.ProductName,
				                MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			GetCrossFadeValue();
			if (m_executionInterface.EngineStatus(m_executionContextHandle) != 0)
			{
				m_executionInterface.ExecuteStop(m_executionContextHandle);
				pictureBoxRunProgram.Image = pictureBoxRun.Image;
			}
			else
			{
				m_executionInterface.SetSynchronousContext(m_executionContextHandle, listBoxPrograms.SelectedItem as SequenceProgram);
				if (m_executionInterface.ExecutePlay(m_executionContextHandle))
				{
					pictureBoxRunProgram.Image = pictureBoxStop.Image;
				}
			}
		}

		private void ProgramChanged(ProgramChange changeType)
		{
			switch (changeType)
			{
				case ProgramChange.End:
					pictureBoxRunProgram.Image = pictureBoxRun.Image;
					break;
			}
		}

		private void ProgramManagerDialog_DragDrop(object sender, DragEventArgs e)
		{
			if (m_dragSource == listBoxPrograms)
			{
				listBoxPrograms.Items.RemoveAt(listBoxPrograms.SelectedIndex);
			}
			else if (m_dragSource == listBoxProgramSequences)
			{
				RemoveSongFromProgram((EventSequenceStub) listBoxProgramSequences.SelectedItem);
			}
			m_dirty = true;
		}

		private void ProgramManagerDialog_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = (m_dragSource != listBoxSequences) ? DragDropEffects.Move : DragDropEffects.None;
		}

		private void ProgramManagerDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (m_dirty)
			{
				switch (MessageBox.Show("Save changes?", Vendor.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
					)
				{
					case DialogResult.Cancel:
						e.Cancel = true;
						break;

					case DialogResult.Yes:
						SavePrograms();
						break;
				}
			}
			if (!e.Cancel)
			{
				m_executionInterface.ReleaseContext(m_executionContextHandle);
				foreach (SequenceProgram program in listBoxPrograms.Items)
				{
					program.Dispose();
				}
			}
		}

		private void radioButtonProgramPlugin_CheckedChanged(object sender, EventArgs e)
		{
			m_dirty = true;
			if (radioButtonProgramPlugin.Checked)
			{
				buttonRadioAction.Text = "Plugin Setup";
				buttonRadioAction.Visible = true;
				((SequenceProgram) listBoxPrograms.SelectedItem).Profile = null;
			}
			else if (radioButtonProfile.Checked)
			{
				buttonRadioAction.Text = "Select Profile";
				buttonRadioAction.Visible = true;
			}
			else if (radioButtonSequencePlugin.Checked)
			{
				buttonRadioAction.Visible = false;
				((SequenceProgram) listBoxPrograms.SelectedItem).Profile = null;
			}
			((SequenceProgram) listBoxPrograms.SelectedItem).UseSequencePluginData = radioButtonSequencePlugin.Checked;
		}

		private void RefreshProgramListBox()
		{
			listBoxPrograms.BeginUpdate();
			listBoxPrograms.Refresh();
			listBoxPrograms.EndUpdate();
			m_dirty = true;
		}

		private void RemoveSongFromProgram(EventSequenceStub eventSequenceStub)
		{
			listBoxProgramSequences.Items.Remove(eventSequenceStub);
			((SequenceProgram) listBoxPrograms.SelectedItem).EventSequences.Remove(eventSequenceStub);
			RefreshProgramListBox();
		}

		private void SavePrograms()
		{
			foreach (string str in Directory.GetFiles(Paths.ProgramPath))
			{
				File.Copy(str, Path.ChangeExtension(str, ".bak"));
			}
			string filePath = null;
			foreach (SequenceProgram program in listBoxPrograms.Items)
			{
				try
				{
					filePath = Path.Combine(Paths.ProgramPath, program.Name + ".vpr");
					program.SaveTo(filePath);
					File.Delete(Path.ChangeExtension(filePath, ".bak"));
				}
				catch (Exception exception)
				{
					string sourceFileName = Path.ChangeExtension(filePath, ".bak");
					try
					{
						File.Copy(sourceFileName, filePath);
						sourceFileName = null;
					}
					catch
					{
					}
					foreach (string str4 in Directory.GetFiles(Paths.ProgramPath, "*.bak"))
					{
						if (str4 != sourceFileName)
						{
							File.Delete(str4);
						}
					}
					if (sourceFileName == null)
					{
						MessageBox.Show(
							string.Format("Error while saving programs:\n\n{0}\n\nAffected file was successfully restored.",
							              exception.Message), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						MessageBox.Show(
							string.Format(
								"Error while saving programs:\n\n{0}\n\nAffected file could not be restored.  It was backed up to {1}.",
								exception.Message, sourceFileName), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
		}

		private void timer_Tick(object sender, EventArgs e)
		{
		}

		private bool VerifyOutputPlugins(SetupData setupData)
		{
			return (setupData.GetAllPluginData(SetupData.PluginType.Output, true).Count > 0);
		}
	}
}