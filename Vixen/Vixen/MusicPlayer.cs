namespace Vixen
{
    using FMOD;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Timers;
    using System.Windows.Forms;
    using System.Xml;

    internal class MusicPlayer
    {
        private XmlDocument m_doc;
        private fmod m_fmod;
        private int m_narrativeInterval;
        private Audio m_narrativeSong = null;
        private List<Audio> m_playlist;
        private Preference2 m_preferences = Preference2.GetInstance();
        private int m_songCounter;
        private List<Audio> m_songs;
        private System.Timers.Timer m_songTimer;
        private SoundChannel m_soundChannel = null;

        public event OnSongChange SongChange;

        public MusicPlayer()
        {
            int integer = this.m_preferences.GetInteger("SoundDevice");
            this.m_fmod = (integer > 0) ? fmod.GetInstance(integer) : fmod.GetInstance(-1);
            this.m_playlist = new List<Audio>();
            this.m_songTimer = new System.Timers.Timer();
            this.m_songTimer.Elapsed += new ElapsedEventHandler(this.m_songTimer_Elapsed);
            string path = Path.Combine(Paths.AudioPath, "MusicPlayer.data");
            if (File.Exists(path))
            {
                this.m_doc = new XmlDocument();
                this.m_doc.Load(path);
            }
            else
            {
                this.m_doc = Xml.CreateXmlDocument("MusicPlayer");
                bool flag2 = false;
                Xml.SetAttribute(Xml.SetValue(this.m_doc.DocumentElement, "Songs", string.Empty), "shuffle", flag2.ToString());
                XmlNode node = Xml.SetValue(this.m_doc.DocumentElement, "Narrative", string.Empty);
                Xml.SetAttribute(node, "enabled", false.ToString());
                Xml.SetAttribute(node, "filename", string.Empty);
                Xml.SetAttribute(node, "interval", "2");
            }
            this.m_songs = new List<Audio>();
            this.LoadAudioData();
        }

        public void GeneratePlaylist()
        {
            this.m_playlist.Clear();
            if (bool.Parse(this.m_doc.SelectSingleNode("//MusicPlayer/Songs").Attributes["shuffle"].Value))
            {
                List<XmlNode> list = new List<XmlNode>();
                foreach (XmlNode node in this.m_doc.SelectNodes("//MusicPlayer/Songs/*"))
                {
                    list.Add(node);
                }
                Random random = new Random();
                while (list.Count > 0)
                {
                    int index = random.Next(list.Count);
                    this.m_playlist.Add(new Audio(list[index]));
                    list.RemoveAt(index);
                }
            }
            else
            {
                foreach (XmlNode node2 in this.m_doc.SelectNodes("//MusicPlayer/Songs/*"))
                {
                    this.m_playlist.Add(new Audio(node2));
                }
            }
        }

        private void LoadAudioData()
        {
            this.m_songs.Clear();
            foreach (XmlNode node in this.m_doc.SelectNodes("//MusicPlayer/Songs/*"))
            {
                this.m_songs.Add(new Audio(node));
            }
        }

        private void LogAudio(Audio song)
        {
            if (this.m_preferences.GetBoolean("LogAudioMusicPlayer"))
            {
                Host.LogAudio("Music player", string.Empty, song.FileName, song.Duration);
            }
        }

        private void m_songTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Host.BeginInvoke(new MethodInvoker(this.NextSong), new object[0]);
        }

        private void NextSong()
        {
            Audio narrativeSong;
            this.m_songTimer.Enabled = false;
            this.m_fmod.ReleaseSound(this.m_soundChannel);
            this.m_soundChannel = null;
            if ((this.m_narrativeSong != null) && (++this.m_songCounter == this.m_narrativeInterval))
            {
                narrativeSong = this.m_narrativeSong;
                this.m_songCounter = 0;
            }
            else
            {
                if (this.m_playlist.Count == 0)
                {
                    this.GeneratePlaylist();
                }
                narrativeSong = this.m_playlist[0];
                this.m_playlist.RemoveAt(0);
            }
            this.LogAudio(narrativeSong);
            this.m_soundChannel = this.m_fmod.LoadSound(Path.Combine(Paths.AudioPath, narrativeSong.FileName), this.m_soundChannel);
            if (this.SongChange != null)
            {
                this.SongChange(narrativeSong.Name);
            }
            this.m_songTimer.Interval = this.m_soundChannel.SoundLength;
            this.m_fmod.Play(this.m_soundChannel);
            this.m_songTimer.Enabled = true;
        }

        public DialogResult ShowDialog()
        {
            MusicPlayerDialog dialog = new MusicPlayerDialog(this.m_fmod);
            dialog.Shuffle = bool.Parse(this.m_doc.SelectSingleNode("//MusicPlayer/Songs").Attributes["shuffle"].Value);
            dialog.Songs = this.m_songs.ToArray();
            XmlNode node = this.m_doc.SelectSingleNode("//MusicPlayer/Narrative");
            XmlNode node2 = node.SelectSingleNode("*");
            if (node2 != null)
            {
                bool flag;
                dialog.NarrativeSong = new Audio(node2);
                if (bool.TryParse(node.Attributes["enabled"].Value, out flag))
                {
                    dialog.NarrativeSongEnabled = flag;
                }
                else
                {
                    dialog.NarrativeSongEnabled = false;
                }
            }
            else
            {
                dialog.NarrativeSongEnabled = false;
            }
            dialog.NarrativeSongInterval = Convert.ToInt32(node.Attributes["interval"].Value);
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(this.m_doc.DocumentElement, "Songs");
                Xml.SetAttribute(emptyNodeAlways, "shuffle", dialog.Shuffle.ToString());
                foreach (Audio audio in dialog.Songs)
                {
                    emptyNodeAlways.AppendChild(audio.SaveToXml(this.m_doc));
                }
                node = Xml.GetEmptyNodeAlways(this.m_doc.DocumentElement, "Narrative");
                Xml.SetAttribute(node, "enabled", dialog.NarrativeSongEnabled.ToString());
                Xml.SetAttribute(node, "interval", dialog.NarrativeSongInterval.ToString());
                Audio narrativeSong = dialog.NarrativeSong;
                if (narrativeSong != null)
                {
                    node.AppendChild(narrativeSong.SaveToXml(this.m_doc));
                }
                this.m_doc.Save(Path.Combine(Paths.AudioPath, "MusicPlayer.data"));
                this.LoadAudioData();
                this.GeneratePlaylist();
            }
            return result;
        }

        public void Start()
        {
            if (this.m_soundChannel == null)
            {
                this.m_songCounter = 0;
                this.m_narrativeSong = null;
                XmlNode node = this.m_doc.SelectSingleNode("//MusicPlayer/Narrative");
                bool result = false;
                bool.TryParse(node.Attributes["enabled"].Value, out result);
                if (result)
                {
                    this.m_narrativeInterval = Convert.ToInt32(node.Attributes["interval"].Value);
                    this.m_narrativeSong = new Audio(node.SelectSingleNode("*"));
                }
                this.NextSong();
            }
        }

        public void Stop()
        {
            if (this.m_soundChannel != null)
            {
                if (this.m_preferences.GetBoolean("EnableMusicFade"))
                {
                    this.m_fmod.Stop(this.m_soundChannel, this.m_preferences.GetInteger("MusicFadeDuration"));
                }
                else
                {
                    this.m_fmod.Stop(this.m_soundChannel);
                }
                this.m_fmod.ReleaseSound(this.m_soundChannel);
                this.m_soundChannel = null;
                this.m_songTimer.Enabled = false;
            }
        }

        public uint CurrentSongLength
        {
            get
            {
                return ((this.m_soundChannel == null) ? 0 : this.m_soundChannel.SoundLength);
            }
        }

        public string CurrentSongName
        {
            get
            {
                return ((this.m_soundChannel == null) ? "(null)" : this.m_soundChannel.SoundName);
            }
        }

        public bool IsPlaying
        {
            get
            {
                return ((this.m_soundChannel != null) && this.m_soundChannel.IsPlaying);
            }
        }

        public int SongCount
        {
            get
            {
                return this.m_songs.Count;
            }
        }

        public delegate void OnSongChange(string songName);
    }
}

