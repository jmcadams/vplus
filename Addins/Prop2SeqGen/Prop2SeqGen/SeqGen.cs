namespace Prop2SeqGen
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using VixenPlus;

    public class SeqGen : IAddIn, ILoadable, IPlugIn
    {
        private const string FILL_FILE = "prop-2_sequencer_fill.txt";
        private const int HIGH_BIT = 0x8000;
        private int m_threshold = 0;
        private const string TEMPLATE_FILE = "prop-2_sequencer_main.txt";
        private const int WORD_SIZE = 0x10;

        private List<EventTableRecord> BuildEventTable(EventSequence sequence)
        {
            int totalEventPeriods = sequence.TotalEventPeriods;
            EventTableRecord item = null;
            List<EventTableRecord> list = new List<EventTableRecord>();
            for (int i = 0; i < totalEventPeriods; i++)
            {
                ushort num = 0;
                for (int j = 0; j < 0x10; j++)
                {
                    num = (ushort) (num >> 1);
                    num = (ushort) (num | ((sequence.EventValues[j, i] >= this.m_threshold) ? 0x8000 : 0));
                }
                if ((item != null) && (num == item.Value))
                {
                    item.EventPeriodCount = (ushort) (item.EventPeriodCount + 1);
                }
                else
                {
                    item = new EventTableRecord(num);
                    list.Add(item);
                }
            }
            if ((list.Count > 0) && (list[list.Count - 1].Value == 0))
            {
                list.RemoveAt(list.Count - 1);
            }
            return list;
        }

        public bool Execute(EventSequence sequence)
        {
            string path = Path.Combine(Paths.AddinPath, "prop-2_sequencer_main.txt");
            if (!File.Exists(path))
            {
                MessageBox.Show("Template file is missing.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            string str2 = Path.Combine(Paths.AddinPath, "prop-2_sequencer_fill.txt");
            if (!File.Exists(str2))
            {
                MessageBox.Show("Option file is missing.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            List<AudioSelection> audioOptions = this.LoadAudioSelections(str2);
            frmAddin addin = new frmAddin(audioOptions);
            if (addin.ShowDialog() == DialogResult.OK)
            {
                string str3;
                Dictionary<string, object> fillValues = new Dictionary<string, object>();
                this.LoadFillValues(str2, fillValues);
                fillValues["filename"] = Path.GetFileName(addin.FileName);
                fillValues["date"] = string.Format("{0} {1} {2}", DateTime.Today.ToString("dd"), DateTime.Today.ToString("MMM").ToUpper(), DateTime.Today.ToString("yyyy"));
                if (addin.TriggerLevelHigh)
                {
                    fillValues["active"] = "1";
                    fillValues["not_active"] = "0";
                }
                else
                {
                    fillValues["active"] = "0";
                    fillValues["not_active"] = "1";
                }
                fillValues["event_period"] = sequence.EventPeriod.ToString();
                this.m_threshold = (addin.Threshold * 0xff) / 100;
                List<EventTableRecord> list2 = this.BuildEventTable(sequence);
                StreamReader reader = new StreamReader(path);
                StreamWriter writer = new StreamWriter(addin.FileName);
                while ((str3 = reader.ReadLine()) != null)
                {
                    object obj2;
                    int num3;
                    int index = str3.IndexOf('<');
                    int num2 = str3.IndexOf('>');
                    if (((index == -1) || (num2 == -1)) || (index >= num2))
                    {
                        goto Label_0411;
                    }
                    string key = str3.Substring(index + 1, (num2 - index) - 1).Trim().ToLower();
                    str3 = str3.Remove(index, (num2 - index) + 1);
                    string str6 = key;
                    if (str6 == null)
                    {
                        goto Label_03DA;
                    }
                    if (!(str6 == "reset_audio"))
                    {
                        if (str6 == "audio_play")
                        {
                            goto Label_02B3;
                        }
                        if (str6 == "events_table")
                        {
                            goto Label_02FD;
                        }
                        goto Label_03DA;
                    }
                    if (fillValues.TryGetValue(string.Format("reset_audio_{0}", addin.AudioDeviceIndex), out obj2))
                    {
                        str3 = str3.Insert(index, obj2.ToString());
                        writer.WriteLine(str3);
                    }
                    goto Label_041D;
                Label_02B3:
                    if (fillValues.TryGetValue(string.Format("audio_play_{0}", addin.AudioDeviceIndex), out obj2))
                    {
                        str3 = str3.Insert(index, obj2.ToString());
                        writer.WriteLine(str3);
                    }
                    goto Label_041D;
                Label_02FD:
                    num3 = (addin.AudioDeviceIndex < audioOptions.Count) ? audioOptions[addin.AudioDeviceIndex].MaxRecords : list2.Count;
                    num3 = Math.Min(num3, list2.Count);
                    EventTableRecord local1 = list2[num3 - 1];
                    local1.Value = (ushort) (local1.Value | 0x8000);
                    if (num3 < list2.Count)
                    {
                        MessageBox.Show("The sequence data is more than can be stored.\nAll values will be written, this message is for your information only.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    for (int i = 0; i < list2.Count; i++)
                    {
                        foreach (string str5 in list2[i].GetRecordStrings())
                        {
                            writer.WriteLine(str5);
                        }
                    }
                    goto Label_041D;
                Label_03DA:
                    if (fillValues.TryGetValue(key, out obj2))
                    {
                        str3 = str3.Insert(index, obj2.ToString());
                        writer.WriteLine(str3);
                    }
                    goto Label_041D;
                Label_0411:
                    writer.WriteLine(str3);
                Label_041D:;
                }
                reader.Close();
                writer.Close();
                if (addin.OpenFile)
                {
                    Process process = new Process();
                    process.StartInfo.FileName = addin.FileName;
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                }
                else
                {
                    MessageBox.Show(string.Format("{0} has been written to\n{1}", Path.GetFileName(addin.FileName), Path.GetDirectoryName(addin.FileName)), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            return false;
        }

        private List<AudioSelection> LoadAudioSelections(string fillFileName)
        {
            string str;
            List<AudioSelection> list = new List<AudioSelection>();
            bool flag = false;
            StreamReader reader = new StreamReader(fillFileName);
            while ((str = reader.ReadLine()) != null)
            {
                if (str.Trim().ToLower() == "selections:")
                {
                    flag = true;
                    break;
                }
            }
            try
            {
                if (!flag)
                {
                    return list;
                }
                while (((str = reader.ReadLine()) != null) && ((str = str.Trim()).Length > 0))
                {
                    list.Add(new AudioSelection(str.Split(new char[] { ',' })));
                }
            }
            finally
            {
                reader.Close();
            }
            return list;
        }

        private void LoadFillValues(string fillFileName, Dictionary<string, object> fillValues)
        {
            string str;
            StreamReader reader = new StreamReader(fillFileName);
            while ((str = reader.ReadLine()) != null)
            {
                str = str.Trim();
                if ((str.Length > 0) && (str[0] == '<'))
                {
                    string str2;
                    StringBuilder builder;
                    string str3 = str.Substring(1, str.IndexOf('>') - 1).Trim();
                    fillValues[str3] = builder = new StringBuilder();
                    while (((str = reader.ReadLine()) != null) && (((str2 = str.Trim()).Length <= 0) || (str2[0] != '<')))
                    {
                        builder.AppendLine(str);
                    }
                }
            }
            reader.Close();
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
                return "Creates a sequence for the EFX-TEK Prop-2 controller.";
            }
        }

        public string Name
        {
            get
            {
                return "EFX-TEK Prop-2 Sequence Generator";
            }
        }
    }
}

