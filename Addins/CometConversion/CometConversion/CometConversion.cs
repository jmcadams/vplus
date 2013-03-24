namespace CometConversion
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using VixenPlus;

    public class CometConversion : IAddIn, ILoadable, IPlugIn
    {
        public bool Execute(EventSequence sequence)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            OptionsDialog dialog2 = new OptionsDialog();
            try
            {
                dialog.Title = "Select Comet playlist file";
                dialog.Filter = "Comet playlist | *.cpl";
                StringBuilder builder = new StringBuilder();
                if ((dialog.ShowDialog() == DialogResult.OK) && (dialog2.ShowDialog() == DialogResult.OK))
                {
                    Color blackReplacement = dialog2.BlackReplacement;
                    XmlDocument document = new XmlDocument();
                    XmlDocument document2 = new XmlDocument();
                    document.Load(dialog.FileName);
                    foreach (XmlNode node2 in document.SelectNodes("//cometPlaylist/songs/song"))
                    {
                        string innerText;
                        if (File.Exists(node2.InnerText))
                        {
                            innerText = node2.InnerText;
                        }
                        else
                        {
                            innerText = Path.Combine(Path.GetDirectoryName(dialog.FileName), Path.GetFileName(node2.InnerText));
                            if (!File.Exists(innerText))
                            {
                                continue;
                            }
                        }
                        document2.Load(innerText);
                        EventSequence sequence2 = new EventSequence((Preference2)null);
                        sequence2.EventPeriod = dialog2.EventPeriod;
                        XmlNode node = document2.SelectSingleNode("//cometSong");
                        string path = string.Format("{0} - {1}.vix", Path.Combine(Paths.SequencePath, document.SelectSingleNode("//cometPlaylist").Attributes["playlistName"].Value), node.Attributes["songName"].Value);
                        sequence2.FileName = path;
                        XmlNodeList list = document.SelectNodes("//cometPlaylist/channels/channel");
                        sequence2.ChannelCount = list.Count;
                        int num = 0;
                        while (num < sequence2.ChannelCount)
                        {
                            int num6 = Convert.ToInt32(list[num].Attributes["color"].Value);
                            int num7 = (((num6 & 0xff) << 0x10) + (num6 & 0xff00)) + ((num6 & 0xff0000) >> 0x10);
                            sequence2.Channels[num].Color = (num7 != 0) ? Color.FromArgb(-16777216 + num7) : blackReplacement;
                            sequence2.Channels[num].Name = list[num].InnerText;
                            num++;
                        }
                        double num8 = Math.Ceiling((double) Convert.ToSingle(node.Attributes["songLength"].Value));
                        double num9 = Math.Ceiling((double) (Convert.ToSingle(document2.SelectSingleNode("(//cometSong/songData/interval)[last()]").Attributes["time"].Value) + (((float) sequence2.EventPeriod) / 1000f)));
                        sequence2.Time = (int) Math.Max(num8, num9);
                        string str2 = node.Attributes["MP3File"].Value;
                        if (!File.Exists(str2))
                        {
                            str2 = Path.Combine(Path.GetDirectoryName(dialog.FileName), Path.GetFileName(str2));
                        }
                        if (File.Exists(str2))
                        {
                            sequence2.Audio = new Audio(node.Attributes["songName"].Value, str2, sequence2.Time);
                            string str4 = Path.Combine(Paths.AudioPath, Path.GetFileName(str2));
                            if (!File.Exists(str4))
                            {
                                File.Copy(str2, str4);
                            }
                        }
                        float num3 = ((float) sequence2.EventPeriod) / 1000f;
                        foreach (XmlNode node3 in document2.SelectNodes("//cometSong/songData/interval"))
                        {
                            int num12;
                            float num10 = Convert.ToSingle(node3.Attributes["time"].Value);
                            float num11 = (node3.NextSibling != null) ? Convert.ToSingle(node3.NextSibling.Attributes["time"].Value) : ((float) sequence2.Time);
                            if (((num11 - num10) % num3) > 0f)
                            {
                                num12 = ((int) ((num11 - num10) / num3)) + 1;
                            }
                            else
                            {
                                num12 = (int) ((num11 - num10) / num3);
                            }
                            for (int i = (int) Math.Round((double) (num10 / num3), MidpointRounding.AwayFromZero); num12-- > 0; i++)
                            {
                                int num4 = 0;
                                foreach (XmlNode node4 in node3.SelectNodes("data"))
                                {
                                    int num5 = Convert.ToInt32(node4.InnerText);
                                    for (num = 0; num < 8; num++)
                                    {
                                        sequence2.EventValues[num4++, i] = ((num5 & 1) > 0) ? sequence2.MaximumLevel : sequence2.MinimumLevel;
                                        num5 = num5 >> 1;
                                    }
                                    if (num4 >= sequence2.ChannelCount)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        builder.Append(Path.GetFileName(path) + "\n");
                        sequence2.SaveTo(path);
                    }
                    MessageBox.Show("Sequences created:\n\n" + builder.ToString(), "Comet Conversion", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Comet Conversion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                dialog.Dispose();
                dialog2.Dispose();
            }
            return false;
        }

        public void Loading(XmlNode dataNode)
        {
        }

        public void Unloading()
        {
        }

        public string Author
        {
            get
            {
                return "Vixen and VixenPlus Developers";
            }
        }

        public LoadableDataLocation DataLocationPreference
        {
            get
            {
                return LoadableDataLocation.Application;
            }
        }

        public string Description
        {
            get
            {
                return "Converts Comet files to Vixen sequences";
            }
        }

        public string Name
        {
            get
            {
                return "Comet conversion";
            }
        }
    }
}

