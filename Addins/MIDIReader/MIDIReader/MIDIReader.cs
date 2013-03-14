namespace MIDIReader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class MIDIReader : IAddIn, ILoadable, IPlugIn
    {
        private XmlNode m_dataNode;
        private const int MIDI_START_NOTE_ID = 0x15;

        public bool Execute(EventSequence sequence)
        {
            if (sequence == null)
            {
                throw new Exception("A sequence is required to be open for this add-in.");
            }
            if (sequence.Audio == null)
            {
                throw new Exception("A sequence with MIDI audio is required for this add-in.");
            }
            MIDIFile midiFile = new MIDIFile(Path.Combine(Paths.AudioPath, sequence.Audio.FileName));
            if (!midiFile.IsValid)
            {
                throw new Exception("This sequence does not use a valid MIDI file for its audio.");
            }
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            foreach (MIDITrack track in midiFile.Tracks)
            {
                foreach (MIDIEvent event2 in track.GetEventsOfType(MIDIEventType.NoteOn))
                {
                    dictionary[event2.Param1] = event2.Param2;
                }
            }
            bool[] enabledKeyMap = new bool[0x58];
            foreach (int num in dictionary.Keys)
            {
                enabledKeyMap[num - 0x15] = true;
            }
            List<KeyChannelMapping> list = new List<KeyChannelMapping>();
            foreach (XmlNode node in this.m_dataNode.SelectNodes("Keys/*"))
            {
                list.Add(new KeyChannelMapping(node));
            }
            MIDIReaderDialog dialog = new MIDIReaderDialog(sequence, enabledKeyMap, midiFile);
            dialog.Mappings = list.ToArray();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(this.m_dataNode, "Keys");
                foreach (KeyChannelMapping mapping in dialog.Mappings)
                {
                    mapping.SaveToXml(emptyNodeAlways);
                }
                TranscribeProgressDialog dialog2 = new TranscribeProgressDialog(midiFile.TrackCount, sequence.Audio.Duration / sequence.EventPeriod);
                dialog2.Show();
                dialog2.Refresh();
                int duration = sequence.Audio.Duration;
                byte[] noteStates = new byte[0x58];
                foreach (MIDITrack track in midiFile.Tracks)
                {
                    dialog2.StepTrack();
                    track.Reset();
                    int milliseconds = 0;
                    for (int i = 0; milliseconds < duration; i++)
                    {
                        track.ReadTo(milliseconds, noteStates);
                        foreach (Key key in dialog.KeyChannelMapping.Keys)
                        {
                            int index = key.Id - 0x15;
                            foreach (int num7 in dialog.KeyChannelMapping[key])
                            {
                                byte num6 = (byte) (noteStates[index] * 2);
                                if (num6 > 0)
                                {
                                    sequence.EventValues[num7, i] = Math.Min(Math.Max(num6, sequence.MinimumLevel), sequence.MaximumLevel);
                                }
                            }
                        }
                        milliseconds += sequence.EventPeriod;
                    }
                }
                dialog2.Close();
                dialog2.Dispose();
                dialog.Dispose();
                return true;
            }
            dialog.Dispose();
            return false;
        }

        public void Loading(XmlNode dataNode)
        {
            this.m_dataNode = dataNode;
        }

        public void Unloading()
        {
        }

        public string Author
        {
            get
            {
                return "K.C. Oaks";
            }
        }

        public LoadableDataLocation DataLocationPreference
        {
            get
            {
                return LoadableDataLocation.Sequence;
            }
        }

        public string Description
        {
            get
            {
                return "Creates event data from a MIDI file";
            }
        }

        public string Name
        {
            get
            {
                return "MIDI reader";
            }
        }
    }
}

