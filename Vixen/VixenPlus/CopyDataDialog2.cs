using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Vixen
{
	internal partial class CopyDataDialog2 : Form
	{
		private readonly string[] m_programPaths = new[] {"Program/PlugInData"};

		private readonly string[] m_sequencePaths = new[]
			{"Program/Channels", "Program/EventValues", "Program/PlugInData", "Program/LoadableData"};

		private readonly XmlDocument m_sourceDoc;
		private XmlDocument m_destDoc;
		private NodeSource m_source;


		public CopyDataDialog2()
		{
			InitializeComponent();
			treeViewFrom.PathSeparator = "/";
			m_sourceDoc = new XmlDocument();
			m_destDoc = new XmlDocument();
		}

		private void AddNode(XmlNode node, TreeNode parentNode)
		{
			if (node is XmlElement)
			{
				TreeNode node2 = AddNodeFormatted(node, parentNode);
				foreach (XmlNode node3 in node.ChildNodes)
				{
					AddNode(node3, node2);
				}
			}
		}

		private TreeNode AddNodeFormatted(XmlNode node, TreeNode parentNode)
		{
			TreeNode node2;
			if (parentNode == null)
			{
				node2 = treeViewFrom.Nodes.Add(string.Empty);
				node2.Tag = node.Name;
			}
			else
			{
				node2 = parentNode.Nodes.Add(string.Empty);
				node2.Tag = string.Format("{0}/{1}", parentNode.Tag, node.Name);
			}
			if (node.Attributes["name"] != null)
			{
				node2.Text = node.Attributes["name"].Value;
				node2.Name = string.Format("[@name=\"{0}\"]", node2.Text);
				return node2;
			}
			if (node.Name == "Channel")
			{
				node2.Text = node.InnerText;
				node2.Name = string.Format("[contains(.,\"{0}\") and string-length(substring-after(.,\"{0}\")) = 0]", node2.Text);
				return node2;
			}
			node2.Text = node.Name;
			node2.Name = node.Name;
			return node2;
		}

		private void AddTreeNode(TreeNode node, List<TreeNode> nodes)
		{
			if (node.Checked)
			{
				nodes.Add(node);
			}
			foreach (TreeNode node2 in node.Nodes)
			{
				AddTreeNode(node2, nodes);
			}
		}

		private void buttonCopy_Click(object sender, EventArgs e)
		{
			if (m_sourceDoc.BaseURI == string.Empty)
			{
				MessageBox.Show("You need to select something to copy from.", Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Hand);
			}
			else if (m_destDoc.BaseURI == string.Empty)
			{
				MessageBox.Show("You need to select something to copy to.", Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Hand);
			}
			else if (m_sourceDoc.BaseURI == m_destDoc.BaseURI)
			{
				MessageBox.Show("Source and destination cannot be the same.", Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Hand);
			}
			else if (
				MessageBox.Show(
					"If the selected items already exist in the destination, they will be overwritten.\nClick 'Yes' to confirm that you approve of this.",
					Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No)
			{
				int count = 0;
				int num2 = 0;
				int num3 = 0;
				XmlNode node = m_destDoc.SelectSingleNode("//Program/Time");
				if (node != null)
				{
					count = m_destDoc.SelectNodes("//Program/Channels/*").Count;
					try
					{
						num2 = Convert.ToInt32(node.InnerText);
					}
					catch
					{
						num2 = 0;
					}
					try
					{
						num3 = Convert.ToInt32(m_destDoc.SelectSingleNode("//Program/EventPeriodInMilliseconds").InnerText);
					}
					catch
					{
						num3 = 0;
					}
				}
				var list = new List<string>();
				foreach (TreeNode node5 in GetAllNodes())
				{
					string str;
					list.Add(node5.Text);
					if (node5.Name != node5.Text)
					{
						str = node5.Tag + node5.Name;
					}
					else
					{
						str = node5.Tag.ToString();
					}
					XmlNode node2 = m_sourceDoc.SelectSingleNode("//" + str);
					if (node2 == null)
					{
						MessageBox.Show(
							"Error encountered when trying to find the specified node.\nThis is a program error due to the data.\nPlease visit the support forum to get this resolved.\n\nNode: " +
							str, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						return;
					}
					XmlNode oldChild = m_destDoc.SelectSingleNode(str);
					if (oldChild != null)
					{
						oldChild.ParentNode.RemoveChild(oldChild);
					}
					if ((node2.ParentNode != null) && (node2.ParentNode is XmlElement))
					{
						Xml.CloneNode(m_destDoc, node2.ParentNode, false).AppendChild(m_destDoc.ImportNode(node2, true));
					}
				}
				if ((list.Contains("EventPeriodInMilliseconds") || list.Contains("Time")) || list.Contains("EventValues"))
				{
					XmlNode node6 = m_destDoc.SelectSingleNode("//Program/Time");
					if (node6 != null)
					{
						int num6;
						int num4 = m_sourceDoc.SelectNodes("//Program/Channels/Channel").Count;
						int num5 = m_destDoc.SelectNodes("//Program/Channels/*").Count;
						try
						{
							num6 = Convert.ToInt32(node6.InnerText);
						}
						catch
						{
							num6 = 0;
						}
						if (num6 != 0)
						{
							byte[] buffer = Convert.FromBase64String(m_destDoc.SelectSingleNode("//Program/EventValues").InnerText);
							int num7 = int.Parse(m_sourceDoc.SelectSingleNode("//Program/EventPeriodInMilliseconds").InnerText);
							int num8 = int.Parse(m_destDoc.SelectSingleNode("//Program/EventPeriodInMilliseconds").InnerText);
							var num9 = (int) Math.Ceiling(((num5*num6)/((float) num8)));
							if (((num9 != 0) && (num9 < buffer.Length)) &&
							    (MessageBox.Show(
								    "This change requires that events are to be truncated in order to maintain data integrity.\nThis will cause data loss in the destination sequence.\n\nProceed?\n\nIf you select 'No', your change will still be in effect, but no data will be truncated.\nKnow that this will leave the destination sequence in an unusable state.",
								    Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No))
							{
								return;
							}
							if ((num9 != 0) && (num9 != buffer.Length))
							{
								var inArray = new byte[num9];
								int num10 = inArray.Length/num5;
								int num11 = buffer.Length/num4;
								int num12 = Math.Min(num4, num5);
								float num14 = Math.Max(num7, num8);
								float num15 = num14/(num7);
								float num16 = num14/(num8);
								var num17 = (int) Math.Min(((num11)/num15), ((num10)/num16));
								for (int i = 0; i < num12; i++)
								{
									for (float j = 0f; j < num17; j++)
									{
										float num18 = (i*num11) + (j*num15);
										float num19 = (i*num10) + (j*num16);
										byte num22 = 0;
										for (float k = 0f; k < num15; k++)
										{
											num22 = Math.Max(num22, buffer[(int) (num18 + k)]);
										}
										for (float m = 0f; m < num16; m++)
										{
											inArray[(int) (num19 + m)] = num22;
										}
									}
								}
								m_destDoc.SelectSingleNode("//Program/EventValues").InnerText = Convert.ToBase64String(inArray);
							}
						}
					}
				}
				int num24 = 0;
				foreach (XmlNode node7 in m_destDoc.SelectNodes("//Program/PlugInData/PlugIn"))
				{
					node7.Attributes["id"].Value = num24.ToString();
					num24++;
				}
				m_destDoc.Save(m_destDoc.BaseURI.Substring(8).Replace('/', '\\'));
				MessageBox.Show(labelTo.Text + " has been updated.", Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Asterisk);
			}
		}

		private void buttonFromFile_Click(object sender, EventArgs e)
		{
			openFileDialog.FileName = string.Empty;
			openFileDialog.Filter = string.Format("{0} data|*.{1}", Vendor.ProductName, Vendor.DataExtension);
			openFileDialog.DefaultExt = Vendor.DataExtension;
			openFileDialog.InitialDirectory = Paths.ImportExportPath;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					m_sourceDoc.Load(openFileDialog.FileName);
					XmlNode node = m_sourceDoc.SelectSingleNode("//Program");
					labelFrom.Text = "file: " + Path.GetFileName(openFileDialog.FileName);
					m_source = NodeSource.File;
					if (checkBoxShowAllNodes.Checked)
					{
						checkBoxShowAllNodes_CheckedChanged(null, null);
					}
					checkBoxShowAllNodes.Checked = true;
					checkBoxShowAllNodes.Enabled = false;
				}
				catch
				{
					MessageBox.Show("This does not appear to be a valid file.\nPlease choose another.", Vendor.ProductName,
					                MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}

		private void buttonFromProgram_Click(object sender, EventArgs e)
		{
			openFileDialog.FileName = string.Empty;
			openFileDialog.Filter = string.Format("{0} program|*.{1}", Vendor.ProductName, Vendor.ProgramExtension);
			openFileDialog.DefaultExt = Vendor.ProgramExtension;
			openFileDialog.InitialDirectory = Paths.ProgramPath;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					m_sourceDoc.Load(openFileDialog.FileName);
					XmlNode node = m_sourceDoc.SelectSingleNode("//Program");
					labelFrom.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
					m_source = NodeSource.Program;
					if (!checkBoxShowAllNodes.Checked)
					{
						checkBoxShowAllNodes_CheckedChanged(null, null);
					}
					checkBoxShowAllNodes.Checked = false;
					checkBoxShowAllNodes.Enabled = true;
				}
				catch
				{
					MessageBox.Show("This does not appear to be a valid file.\nPlease choose another.", Vendor.ProductName,
					                MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}

		private void buttonFromSequence_Click(object sender, EventArgs e)
		{
			openFileDialog.FileName = string.Empty;
			openFileDialog.Filter = string.Format("{0} sequence|*.{1}", Vendor.ProductName, Vendor.SequenceExtension);
			openFileDialog.DefaultExt = Vendor.SequenceExtension;
			openFileDialog.InitialDirectory = Paths.SequencePath;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					m_sourceDoc.Load(openFileDialog.FileName);
					XmlNode node = m_sourceDoc.SelectSingleNode("//Program");
					labelFrom.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
					m_source = NodeSource.Sequence;
					if (!checkBoxShowAllNodes.Checked)
					{
						checkBoxShowAllNodes_CheckedChanged(null, null);
					}
					checkBoxShowAllNodes.Checked = false;
					checkBoxShowAllNodes.Enabled = true;
				}
				catch
				{
					MessageBox.Show("This does not appear to be a valid file.\nPlease choose another.", Vendor.ProductName,
					                MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}

		private void buttontoFile_Click(object sender, EventArgs e)
		{
			saveFileDialog.FileName = string.Empty;
			saveFileDialog.Filter = string.Format("{0} data|*.{1}", Vendor.ProductName, Vendor.DataExtension);
			saveFileDialog.DefaultExt = Vendor.DataExtension;
			saveFileDialog.InitialDirectory = Paths.ImportExportPath;
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(saveFileDialog.FileName))
				{
					m_destDoc.Load(saveFileDialog.FileName);
				}
				else
				{
					m_destDoc = Xml.CreateXmlDocument("Program");
					m_destDoc.Save(saveFileDialog.FileName);
					m_destDoc.Load(saveFileDialog.FileName);
				}
				labelTo.Text = "file: " + Path.GetFileName(saveFileDialog.FileName);
			}
		}

		private void buttonToProgram_Click(object sender, EventArgs e)
		{
			openFileDialog.FileName = string.Empty;
			openFileDialog.Filter = string.Format("{0} program|*.{1}", Vendor.ProductName, Vendor.ProgramExtension);
			openFileDialog.DefaultExt = Vendor.ProgramExtension;
			openFileDialog.InitialDirectory = Paths.ProgramPath;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					m_destDoc.Load(openFileDialog.FileName);
					XmlNode node = m_destDoc.SelectSingleNode("//Program");
					labelTo.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
				}
				catch
				{
					MessageBox.Show("This does not appear to be a valid file.\nPlease choose another.", Vendor.ProductName,
					                MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}

		private void buttonToSequence_Click(object sender, EventArgs e)
		{
			openFileDialog.FileName = string.Empty;
			openFileDialog.Filter = string.Format("{0} sequence|*.{1}", Vendor.ProductName, Vendor.SequenceExtension);
			openFileDialog.DefaultExt = Vendor.SequenceExtension;
			openFileDialog.InitialDirectory = Paths.SequencePath;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					m_destDoc.Load(openFileDialog.FileName);
					XmlNode node = m_destDoc.SelectSingleNode("//Program");
					labelTo.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
				}
				catch
				{
					MessageBox.Show("This does not appear to be a valid file.\nPlease choose another.", Vendor.ProductName,
					                MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}

		private void checkBoxShowAllNodes_CheckedChanged(object sender, EventArgs e)
		{
			if (!checkBoxShowAllNodes.Checked)
			{
				ShowPrescribedNodes();
			}
			else
			{
				ShowAllNodes();
			}
			if (treeViewFrom.Nodes.Count == 1)
			{
				treeViewFrom.Nodes[0].Expand();
			}
		}


		private TreeNode[] GetAllNodes()
		{
			var nodes = new List<TreeNode>();
			foreach (TreeNode node in treeViewFrom.Nodes)
			{
				AddTreeNode(node, nodes);
			}
			return nodes.ToArray();
		}


		private void ShowAllNodes()
		{
			treeViewFrom.Nodes.Clear();
			treeViewFrom.BeginUpdate();
			AddNode(m_sourceDoc.SelectSingleNode("//Program"), null);
			treeViewFrom.EndUpdate();
		}

		private void ShowPrescribedNodes()
		{
			Array programPaths;
			treeViewFrom.Nodes.Clear();
			treeViewFrom.BeginUpdate();
			if (m_source == NodeSource.Program)
			{
				programPaths = m_programPaths;
			}
			else if (m_source == NodeSource.Sequence)
			{
				programPaths = m_sequencePaths;
			}
			else
			{
				return;
			}
			foreach (string str2 in programPaths)
			{
				if (m_sourceDoc.SelectSingleNode("//" + str2) != null)
				{
					string text = str2.Substring(str2.LastIndexOf('/') + 1);
					TreeNode parentNode = treeViewFrom.Nodes.Add(text);
					parentNode.Tag = str2;
					parentNode.Name = text;
					foreach (XmlNode node2 in m_sourceDoc.SelectNodes("//" + str2 + "/*"))
					{
						AddNodeFormatted(node2, parentNode);
					}
				}
			}
			treeViewFrom.EndUpdate();
		}

		private enum NodeSource
		{
			Sequence,
			Program,
			File
		}
	}
}