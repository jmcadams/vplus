using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using FMOD;

namespace Vixen
{
	internal class MusicPlayer
	{
		public delegate void OnSongChange(string songName);

		private readonly XmlDocument m_doc;
		private readonly fmod m_fmod;
		private readonly List<Audio> m_playlist;
		private readonly Preference2 m_preferences = Preference2.GetInstance();
		private readonly System.Timers.Timer m_songTimer;
		private readonly List<Audio> m_songs;
		private int m_narrativeInterval;
		private Audio m_narrativeSong;
		private int m_songCounter;
		private SoundChannel m_soundChannel;

		public MusicPlayer()
		{
			int integer = m_preferences.GetInteger("SoundDevice");
			m_fmod = (integer > 0) ? fmod.GetInstance(integer) : fmod.GetInstance(-1);
			m_playlist = new List<Audio>();
			m_songTimer = new System.Timers.Timer();
			m_songTimer.Elapsed += m_songTimer_Elapsed;
			string path = Path.Combine(Paths.AudioPath, "MusicPlayer.data");
			if (File.Exists(path))
			{
				m_doc = new XmlDocument();
				m_doc.Load(path);
			}
			else
			{
				m_doc = Xml.CreateXmlDocument("MusicPlayer");
				bool flag2 = false;
				Xml.SetAttribute(Xml.SetValue(m_doc.DocumentElement, "Songs", string.Empty), "shuffle", flag2.ToString());
				XmlNode node = Xml.SetValue(m_doc.DocumentElement, "Narrative", string.Empty);
				Xml.SetAttribute(node, "enabled", false.ToString());
				Xml.SetAttribute(node, "filename", string.Empty);
				Xml.SetAttribute(node, "interval", "2");
			}
			m_songs = new List<Audio>();
			LoadAudioData();
		}

		public uint CurrentSongLength
		{
			get { return ((m_soundChannel == null) ? 0 : m_soundChannel.SoundLength); }
		}

		public string CurrentSongName
		{
			get { return ((m_soundChannel == null) ? "(null)" : m_soundChannel.SoundName); }
		}

		public bool IsPlaying
		{
			get { return ((m_soundChannel != null) && m_soundChannel.IsPlaying); }
		}

		public int SongCount
		{
			get { return m_songs.Count; }
		}

		public event OnSongChange SongChange;

		public void GeneratePlaylist()
		{
			m_playlist.Clear();
			if (bool.Parse(m_doc.SelectSingleNode("//MusicPlayer/Songs").Attributes["shuffle"].Value))
			{
				var list = new List<XmlNode>();
				foreach (XmlNode node in m_doc.SelectNodes("//MusicPlayer/Songs/*"))
				{
					list.Add(node);
				}
				var random = new Random();
				while (list.Count > 0)
				{
					int index = random.Next(list.Count);
					m_playlist.Add(new Audio(list[index]));
					list.RemoveAt(index);
				}
			}
			else
			{
				foreach (XmlNode node2 in m_doc.SelectNodes("//MusicPlayer/Songs/*"))
				{
					m_playlist.Add(new Audio(node2));
				}
			}
		}

		private void LoadAudioData()
		{
			m_songs.Clear();
			foreach (XmlNode node in m_doc.SelectNodes("//MusicPlayer/Songs/*"))
			{
				m_songs.Add(new Audio(node));
			}
		}

		private void LogAudio(Audio song)
		{
			if (m_preferences.GetBoolean("LogAudioMusicPlayer"))
			{
				Host.LogAudio("Music player", string.Empty, song.FileName, song.Duration);
			}
		}

		private void m_songTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Host.BeginInvoke(new MethodInvoker(NextSong), new object[0]);
		}

		private void NextSong()
		{
			Audio narrativeSong;
			m_songTimer.Enabled = false;
			m_fmod.ReleaseSound(m_soundChannel);
			m_soundChannel = null;
			if ((m_narrativeSong != null) && (++m_songCounter == m_narrativeInterval))
			{
				narrativeSong = m_narrativeSong;
				m_songCounter = 0;
			}
			else
			{
				if (m_playlist.Count == 0)
				{
					GeneratePlaylist();
				}
				narrativeSong = m_playlist[0];
				m_playlist.RemoveAt(0);
			}
			LogAudio(narrativeSong);
			m_soundChannel = m_fmod.LoadSound(Path.Combine(Paths.AudioPath, narrativeSong.FileName), m_soundChannel);
			if (SongChange != null)
			{
				SongChange(narrativeSong.Name);
			}
			m_songTimer.Interval = m_soundChannel.SoundLength;
			m_fmod.Play(m_soundChannel);
			m_songTimer.Enabled = true;
		}

		public DialogResult ShowDialog()
		{
			var dialog = new MusicPlayerDialog(m_fmod);
			dialog.Shuffle = bool.Parse(m_doc.SelectSingleNode("//MusicPlayer/Songs").Attributes["shuffle"].Value);
			dialog.Songs = m_songs.ToArray();
			XmlNode node = m_doc.SelectSingleNode("//MusicPlayer/Narrative");
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
				XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(m_doc.DocumentElement, "Songs");
				Xml.SetAttribute(emptyNodeAlways, "shuffle", dialog.Shuffle.ToString());
				foreach (Audio audio in dialog.Songs)
				{
					emptyNodeAlways.AppendChild(audio.SaveToXml(m_doc));
				}
				node = Xml.GetEmptyNodeAlways(m_doc.DocumentElement, "Narrative");
				Xml.SetAttribute(node, "enabled", dialog.NarrativeSongEnabled.ToString());
				Xml.SetAttribute(node, "interval", dialog.NarrativeSongInterval.ToString());
				Audio narrativeSong = dialog.NarrativeSong;
				if (narrativeSong != null)
				{
					node.AppendChild(narrativeSong.SaveToXml(m_doc));
				}
				m_doc.Save(Path.Combine(Paths.AudioPath, "MusicPlayer.data"));
				LoadAudioData();
				GeneratePlaylist();
			}
			return result;
		}

		public void Start()
		{
			if (m_soundChannel == null)
			{
				m_songCounter = 0;
				m_narrativeSong = null;
				XmlNode node = m_doc.SelectSingleNode("//MusicPlayer/Narrative");
				bool result = false;
				bool.TryParse(node.Attributes["enabled"].Value, out result);
				if (result)
				{
					m_narrativeInterval = Convert.ToInt32(node.Attributes["interval"].Value);
					m_narrativeSong = new Audio(node.SelectSingleNode("*"));
				}
				NextSong();
			}
		}

		public void Stop()
		{
			if (m_soundChannel != null)
			{
				if (m_preferences.GetBoolean("EnableMusicFade"))
				{
					m_fmod.Stop(m_soundChannel, m_preferences.GetInteger("MusicFadeDuration"));
				}
				else
				{
					m_fmod.Stop(m_soundChannel);
				}
				m_fmod.ReleaseSound(m_soundChannel);
				m_soundChannel = null;
				m_songTimer.Enabled = false;
			}
		}
	}
}