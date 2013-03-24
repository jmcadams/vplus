using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace VixenPlus
{
	internal partial class CopyDataDialog2 : Form
	{
		private readonly string[] _programPaths = new[] {"Program/PlugInData"};

		private readonly string[] _sequencePaths = new[]
			{"Program/Channels", "Program/EventValues", "Program/PlugInData", "Program/LoadableData"};

		private readonly XmlDocument _sourceDoc;
		private XmlDocument _destDoc;
		private NodeSource _source;


		public CopyDataDialog2()
		{
			InitializeComponent();
			treeViewFrom.PathSeparator = "/";
			_sourceDoc = new XmlDocument();
			_destDoc = new XmlDocument();
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
			if (node.Attributes != null && node.Attributes["name"] != null)
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
			if (_sourceDoc.BaseURI == string.Empty)
			{
				MessageBox.Show("You need to select something to copy from.", Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Hand);
			}
			else if (_destDoc.BaseURI == string.Empty)
			{
				MessageBox.Show("You need to select something to copy to.", Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Hand);
			}
			else if (_sourceDoc.BaseURI == _destDoc.BaseURI)
			{
				MessageBox.Show("Source and destination cannot be the same.", Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Hand);
			}
			else if (
				MessageBox.Show(
					"If the selected items already exist in the destination, they will be overwritten.\nClick 'Yes' to confirm that you approve of this.",
					Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No)
			{
				//XmlNode node = _destDoc.SelectSingleNode("//Program/Time");
				//if (node != null)
				//{
				//    try
				//    {
				//        Convert.ToInt32(_destDoc.SelectSingleNode("//Program/EventPeriodInMilliseconds").InnerText);
				//    }
				//    catch
				//    {
				//    }
				//}
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
					XmlNode node2 = _sourceDoc.SelectSingleNode("//" + str);
					if (node2 == null)
					{
						MessageBox.Show(
							"Error encountered when trying to find the specified node.\nThis is a program error due to the data.\nPlease visit the support forum to get this resolved.\n\nNode: " +
							str, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						return;
					}
					XmlNode oldChild = _destDoc.SelectSingleNode(str);
					if (oldChild != null && oldChild.ParentNode != null)
					{
						oldChild.ParentNode.RemoveChild(oldChild);
					}
					if (node2.ParentNode != null && node2.ParentNode is XmlElement)
					{
						Xml.CloneNode(_destDoc, node2.ParentNode, false).AppendChild(_destDoc.ImportNode(node2, true));
					}
				}
				if ((list.Contains("EventPeriodInMilliseconds") || list.Contains("Time")) || list.Contains("EventValues"))
				{
					XmlNode node6 = _destDoc.SelectSingleNode("//Program/Time");
					if (node6 != null)
					{
						XmlNodeList xmlNodeList = _sourceDoc.SelectNodes("//Program/Channels/Channel");
						if (xmlNodeList != null)
						{
							int num4 = xmlNodeList.Count;
							XmlNodeList selectNodes = _destDoc.SelectNodes("//Program/Channels/*");
							if (selectNodes != null)
							{
								int num5 = selectNodes.Count;
								int num6;
								try
								{
									num6 = Convert.ToInt32(node6.InnerText); //TODO: TryParse instead
								}
								catch
								{
									num6 = 0;
								}
								if (num6 != 0)
								{
									XmlNode selectSingleNode = _destDoc.SelectSingleNode("//Program/EventValues");
									if (selectSingleNode != null)
									{
										byte[] buffer = Convert.FromBase64String(selectSingleNode.InnerText);
										XmlNode singleNode = _sourceDoc.SelectSingleNode("//Program/EventPeriodInMilliseconds");
										if (singleNode != null)
										{
											int num7 = int.Parse(singleNode.InnerText);
											XmlNode xmlNode = _destDoc.SelectSingleNode("//Program/EventPeriodInMilliseconds");
											if (xmlNode != null)
											{
												int num8 = int.Parse(xmlNode.InnerText);
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
													selectSingleNode.InnerText = Convert.ToBase64String(inArray);
												}
											}
										}
									}
								}
							}
						}
					}
				}
				int num24 = 0;
				XmlNodeList nodeList = _destDoc.SelectNodes("//Program/PlugInData/PlugIn");
				if (nodeList != null)
				{
					foreach (XmlNode node7 in nodeList)
					{
						if (node7.Attributes != null)
						{
							node7.Attributes["id"].Value = num24.ToString(CultureInfo.InvariantCulture);
						}
						num24++;
					}
				}
				_destDoc.Save(_destDoc.BaseURI.Substring(8).Replace('/', '\\'));
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
					_sourceDoc.Load(openFileDialog.FileName);
					//XmlNode node = _sourceDoc.SelectSingleNode("//Program");
					labelFrom.Text = "file: " + Path.GetFileName(openFileDialog.FileName);
					_source = NodeSource.File;
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
					_sourceDoc.Load(openFileDialog.FileName);
					_sourceDoc.SelectSingleNode("//Program");
					labelFrom.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
					_source = NodeSource.Program;
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
					_sourceDoc.Load(openFileDialog.FileName);
					_sourceDoc.SelectSingleNode("//Program");
					labelFrom.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
					_source = NodeSource.Sequence;
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
					_destDoc.Load(saveFileDialog.FileName);
				}
				else
				{
					_destDoc = Xml.CreateXmlDocument("Program");
					_destDoc.Save(saveFileDialog.FileName);
					_destDoc.Load(saveFileDialog.FileName);
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
					_destDoc.Load(openFileDialog.FileName);
					_destDoc.SelectSingleNode("//Program");
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
					_destDoc.Load(openFileDialog.FileName);
					_destDoc.SelectSingleNode("//Program");
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


		private IEnumerable<TreeNode> GetAllNodes()
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
			AddNode(_sourceDoc.SelectSingleNode("//Program"), null);
			treeViewFrom.EndUpdate();
		}

		private void ShowPrescribedNodes()
		{
			Array programPaths;
			treeViewFrom.Nodes.Clear();
			treeViewFrom.BeginUpdate();
			if (_source == NodeSource.Program)
			{
				programPaths = _programPaths;
			}
			else if (_source == NodeSource.Sequence)
			{
				programPaths = _sequencePaths;
			}
			else
			{
				return;
			}
			foreach (string str2 in programPaths)
			{
				if (_sourceDoc.SelectSingleNode("//" + str2) != null)
				{
					string text = str2.Substring(str2.LastIndexOf('/') + 1);
					TreeNode parentNode = treeViewFrom.Nodes.Add(text);
					parentNode.Tag = str2;
					parentNode.Name = text;
					XmlNodeList xmlNodeList = _sourceDoc.SelectNodes("//" + str2 + "/*");
					if (xmlNodeList != null)
					{
						foreach (XmlNode node2 in xmlNodeList)
						{
							AddNodeFormatted(node2, parentNode);
						}
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