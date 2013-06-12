using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using FMOD;

namespace VixenPlus
{
    internal class MusicPlayer
    {
        public delegate void OnSongChange(string songName);

        private readonly fmod _fmod;
        private readonly List<Audio> _playlist;
        private readonly Preference2 _preferences = Preference2.GetInstance();
        private readonly System.Timers.Timer _songTimer;
        private readonly List<Audio> _songs;
        private readonly XmlDocument _xmlDocument;
        private int _narrativeInterval;
        private Audio _narrativeSong;
        private int _songCounter;
        private SoundChannel _soundChannel;

        public MusicPlayer()
        {
            var integer = _preferences.GetInteger("SoundDevice");
            _fmod = (integer > 0) ? fmod.GetInstance(integer) : fmod.GetInstance(-1);
            _playlist = new List<Audio>();
            _songTimer = new System.Timers.Timer();
            _songTimer.Elapsed += SongTimerElapsed;
            var path = Path.Combine(Paths.AudioPath, "MusicPlayer.data");
            if (File.Exists(path))
            {
                _xmlDocument = new XmlDocument();
                _xmlDocument.Load(path);
            }
            else
            {
                _xmlDocument = Xml.CreateXmlDocument("MusicPlayer");
                Xml.SetAttribute(Xml.SetValue(_xmlDocument.DocumentElement, "Songs", string.Empty), "shuffle", false.ToString());
                var node = Xml.SetValue(_xmlDocument.DocumentElement, "Narrative", string.Empty);
                Xml.SetAttribute(node, "enabled", false.ToString());
                Xml.SetAttribute(node, "filename", string.Empty);
                Xml.SetAttribute(node, "interval", "2");
            }
            _songs = new List<Audio>();
            LoadAudioData();
        }

        public uint CurrentSongLength
        {
            get { return ((_soundChannel == null) ? 0 : _soundChannel.SoundLength); }
        }

        public string CurrentSongName
        {
            get { return ((_soundChannel == null) ? "(null)" : _soundChannel.SoundName); }
        }

        public bool IsPlaying
        {
            get { return ((_soundChannel != null) && _soundChannel.IsPlaying); }
        }

        public int SongCount
        {
            get { return _songs.Count; }
        }

        public event OnSongChange SongChange;

        public void GeneratePlaylist()
        {
            _playlist.Clear();
            var songNode = _xmlDocument.SelectSingleNode("//MusicPlayer/Songs");
            if (songNode != null && songNode.Attributes != null && bool.Parse(songNode.Attributes["shuffle"].Value))
            {
                var list = new List<XmlNode>();
                var allSongsNode = _xmlDocument.SelectNodes("//MusicPlayer/Songs/*");
                if (allSongsNode != null)
                {
                    list.AddRange(allSongsNode.Cast<XmlNode>());
                }
                var random = new Random();
                while (list.Count > 0)
                {
                    var index = random.Next(list.Count);
                    _playlist.Add(new Audio(list[index]));
                    list.RemoveAt(index);
                }
            }
            else
            {
                var allSongsNode = _xmlDocument.SelectNodes("//MusicPlayer/Songs/*");
                if (allSongsNode != null)
                {
                    foreach (XmlNode node2 in allSongsNode)
                    {
                        _playlist.Add(new Audio(node2));
                    }
                }
            }
        }

        private void LoadAudioData()
        {
            _songs.Clear();
            var allSongsNode = _xmlDocument.SelectNodes("//MusicPlayer/Songs/*");
            if (allSongsNode == null) {
                return;
            }
            foreach (XmlNode node in allSongsNode)
            {
                _songs.Add(new Audio(node));
            }
        }

        private void LogAudio(Audio song)
        {
            if (_preferences.GetBoolean("LogAudioMusicPlayer"))
            {
                Host.LogAudio("Music player", string.Empty, song.FileName, song.Duration);
            }
        }

        private void SongTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Host.BeginInvoke(new MethodInvoker(NextSong), new object[0]);
        }

        private void NextSong()
        {
            Audio narrativeSong;
            _songTimer.Enabled = false;
            _fmod.ReleaseSound(_soundChannel);
            _soundChannel = null;
            if ((_narrativeSong != null) && (++_songCounter == _narrativeInterval))
            {
                narrativeSong = _narrativeSong;
                _songCounter = 0;
            }
            else
            {
                if (_playlist.Count == 0)
                {
                    GeneratePlaylist();
                }
                narrativeSong = _playlist[0];
                _playlist.RemoveAt(0);
            }
            LogAudio(narrativeSong);
            _soundChannel = _fmod.LoadSound(Path.Combine(Paths.AudioPath, narrativeSong.FileName), _soundChannel);
            if (SongChange != null)
            {
                SongChange(narrativeSong.Name);
            }
            _songTimer.Interval = _soundChannel.SoundLength;
            _fmod.Play(_soundChannel);
            _songTimer.Enabled = true;
        }

        public DialogResult ShowDialog()
        {
            var songNode = _xmlDocument.SelectSingleNode("//MusicPlayer/Songs");
            var dialog = new MusicPlayerDialog(_fmod)
                {
                    Shuffle = songNode != null && songNode.Attributes != null && bool.Parse(songNode.Attributes["shuffle"].Value),
                    Songs = _songs.ToArray()
                };
            var node = _xmlDocument.SelectSingleNode("//MusicPlayer/Narrative");
            if (node != null)
            {
                var node2 = node.SelectSingleNode("*");
                if (node2 != null)
                {
                    dialog.NarrativeSong = new Audio(node2);
                    if (node.Attributes != null)
                    {
                        bool flag;
                        dialog.NarrativeSongEnabled = bool.TryParse(node.Attributes["enabled"].Value, out flag) && flag;
                    }
                }
                else
                {
                    dialog.NarrativeSongEnabled = false;
                }
            }
            if (node != null && node.Attributes != null)
            {
                dialog.NarrativeSongInterval = Convert.ToInt32(node.Attributes["interval"].Value);
            }
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var emptyNodeAlways = Xml.GetEmptyNodeAlways(_xmlDocument.DocumentElement, "Songs");
                Xml.SetAttribute(emptyNodeAlways, "shuffle", dialog.Shuffle.ToString());
                foreach (var audio in dialog.Songs)
                {
                    emptyNodeAlways.AppendChild(audio.SaveToXml(_xmlDocument));
                }
                node = Xml.GetEmptyNodeAlways(_xmlDocument.DocumentElement, "Narrative");
                Xml.SetAttribute(node, "enabled", dialog.NarrativeSongEnabled.ToString());
                Xml.SetAttribute(node, "interval", dialog.NarrativeSongInterval.ToString(CultureInfo.InvariantCulture));
                var narrativeSong = dialog.NarrativeSong;
                if (narrativeSong != null)
                {
                    node.AppendChild(narrativeSong.SaveToXml(_xmlDocument));
                }
                _xmlDocument.Save(Path.Combine(Paths.AudioPath, "MusicPlayer.data"));
                LoadAudioData();
                GeneratePlaylist();
            }
            return result;
        }

        public void Start()
        {
            if (_soundChannel != null) {
                return;
            }
            _songCounter = 0;
            _narrativeSong = null;
            var node = _xmlDocument.SelectSingleNode("//MusicPlayer/Narrative");
            if (node != null && node.Attributes != null)
            {
                bool result;
                bool.TryParse(node.Attributes["enabled"].Value, out result);
                if (result)
                {
                    _narrativeInterval = Convert.ToInt32(node.Attributes["interval"].Value);
                    _narrativeSong = new Audio(node.SelectSingleNode("*"));
                }
            }
            NextSong();
        }

        public void Stop()
        {
            if (_soundChannel == null) {
                return;
            }
            if (_preferences.GetBoolean("EnableMusicFade"))
            {
                _fmod.Stop(_soundChannel, _preferences.GetInteger("MusicFadeDuration"));
            }
            else
            {
                _fmod.Stop(_soundChannel);
            }
            _fmod.ReleaseSound(_soundChannel);
            _soundChannel = null;
            _songTimer.Enabled = false;
        }
    }
}