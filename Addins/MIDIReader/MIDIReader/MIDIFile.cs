namespace MIDIReader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    internal class MIDIFile
    {
        private List<Chunk> m_chunks;
        private string m_filePath;
        private MThd m_header;
        private List<MIDITrack> m_tracks = null;

        public MIDIFile(string filePath)
        {
            this.m_filePath = filePath;
            this.m_chunks = new List<Chunk>();
            Stream fileStream = File.Open(filePath, FileMode.Open);
            byte[] buffer = new byte[4];
            fileStream.Read(buffer, 0, 4);
            fileStream.Seek(0L, SeekOrigin.Begin);
            if (Encoding.ASCII.GetString(buffer) == "MThd")
            {
                try
                {
                    while (fileStream.Position < fileStream.Length)
                    {
                        Chunk chunk;
                        fileStream.Read(buffer, 0, 4);
                        string id = Encoding.ASCII.GetString(buffer);
                        if (id == "MThd")
                        {
                            chunk = new MThd(fileStream);
                        }
                        else if (id == "MTrk")
                        {
                            chunk = new MTrk(fileStream);
                        }
                        else
                        {
                            chunk = new Chunk(id, fileStream);
                        }
                        this.m_chunks.Add(chunk);
                    }
                }
                finally
                {
                    fileStream.Close();
                }
            }
            else
            {
                fileStream.Close();
                fileStream.Dispose();
            }
            this.m_header = this.IsValid ? ((MThd) this.m_chunks[0]) : null;
        }

        public void DebugDump(string filePath)
        {
            foreach (MIDITrack track in this.Tracks)
            {
                track.ReadAllEvents();
            }
            Stream stream = new FileStream(filePath, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);
            try
            {
                writer.WriteLine("Dump of " + this.m_filePath);
                writer.WriteLine("Delta ticks per quarter: " + this.m_header.PulsesPerQuarterNote.ToString());
                writer.WriteLine("Microseconds per quarter note: " + this.MicrosPerQuarterNote.ToString());
                writer.WriteLine("Calculated delta ticks/millisecond: " + (((1.0 / ((double) this.m_header.PulsesPerQuarterNote)) * (((double) this.MicrosPerQuarterNote) / 1000.0))).ToString());
                writer.WriteLine();
                for (int i = 0; i < this.m_tracks.Count; i++)
                {
                    writer.WriteLine("Track " + ((i + 1)).ToString());
                    this.m_tracks[i].DumpTo(writer, "{0,-10} {1,-15:F4} {2,-20} {3}");
                    writer.WriteLine();
                }
            }
            finally
            {
                writer.Close();
                writer.Dispose();
            }
        }

        public MIDIFormat Format
        {
            get
            {
                if (this.m_header == null)
                {
                    return MIDIFormat.None;
                }
                return this.m_header.Format;
            }
        }

        public int FramesPerSecond
        {
            get
            {
                if (this.m_header == null)
                {
                    return 0;
                }
                return this.m_header.FramesPerSecond;
            }
        }

        public bool IsValid
        {
            get
            {
                if (this.m_chunks.Count == 0)
                {
                    return false;
                }
                return (this.m_chunks[0] is MThd);
            }
        }

        public int MicrosPerQuarterNote
        {
            get
            {
                if (this.m_tracks.Count < 2)
                {
                    return 0;
                }
                return this.m_tracks[1].MicrosPerQuarter;
            }
        }

        public int PulsesPerQuarterNote
        {
            get
            {
                if (this.m_header == null)
                {
                    return 0;
                }
                return this.m_header.PulsesPerQuarterNote;
            }
        }

        public int SubFramesPerFrame
        {
            get
            {
                if (this.m_header == null)
                {
                    return 0;
                }
                return this.m_header.SubFramesPerFrame;
            }
        }

        public int TrackCount
        {
            get
            {
                if (this.m_header == null)
                {
                    return 0;
                }
                return this.m_header.TrackCount;
            }
        }

        public List<MIDITrack> Tracks
        {
            get
            {
                if (this.m_tracks == null)
                {
                    this.m_tracks = new List<MIDITrack>();
                    foreach (Chunk chunk in this.m_chunks)
                    {
                        if (chunk is MTrk)
                        {
                            this.m_tracks.Add(new MIDITrack((MTrk) chunk, this.m_header));
                        }
                    }
                }
                return this.m_tracks;
            }
        }

        public bool UsesSMPTETiming
        {
            get
            {
                if (this.m_header == null)
                {
                    return false;
                }
                return this.m_header.UsesSMPTETiming;
            }
        }
    }
}

