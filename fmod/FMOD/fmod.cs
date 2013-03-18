namespace FMOD
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    public class fmod
    {
        private List<SoundChannel> m_channels;
        private int m_deviceIndex;
        private static List<string> m_deviceList = new List<string>();
        private static int m_refCount = 0;
        private _System m_system = null;
        private static List<_System> m_systems = new List<_System>();

        private fmod(_System system, int deviceIndex)
        {
            this.m_system = system;
            this.m_deviceIndex = deviceIndex;
            this.m_channels = new List<SoundChannel>();
        }

        private static void CreateDeviceList()
        {
            if (m_deviceList.Count <= 0)
            {
                _System system = null;
                Factory.System_Create(ref system);
                int numdrivers = 0;
                system.getNumDrivers(ref numdrivers);
                if (numdrivers > 0)
                {
                    system.getNumDrivers(ref numdrivers);
                    StringBuilder name = new StringBuilder(0x100);
                    for (int i = 0; i < numdrivers; i++)
                    {
                        GUID guid = new GUID();
                        system.getDriverInfo(i, name, name.Capacity, ref guid);
                        m_deviceList.Add(name.ToString());
                    }
                }
                system.release();
            }
        }

        private void ERRCHECK(RESULT result)
        {
            if (result != RESULT.OK)
            {
                throw new Exception(string.Format("Sound system error ({0})\n\n{1}", result, Error.String(result)));
            }
        }

        public static fmod GetInstance(int deviceIndex)
        {
            if (m_systems.Count == 0)
            {
                _System system = null;
                CreateDeviceList();
                if (m_deviceList.Count > 0)
                {
                    int num;
                    for (num = 0; num < m_deviceList.Count; num++)
                    {
                        if (Factory.System_Create(ref system) == RESULT.ERR_OUTPUT_INIT)
                        {
                            m_deviceList.RemoveRange(num, m_deviceList.Count - num);
                            break;
                        }
                        m_systems.Add(system);
                    }
                    string path = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), "sound.params");
                    int numbuffers = 0;
                    uint filebuffersize = 0x8000;
                    if (File.Exists(path))
                    {
                        string str2;
                        StreamReader reader = new StreamReader(path);
                        while ((str2 = reader.ReadLine()) != null)
                        {
                            string[] strArray = str2.Split(new char[] { '=' });
                            string str3 = strArray[0].Trim().ToLower();
                            if (str3 != null)
                            {
                                if (!(str3 == "dspbuffercount"))
                                {
                                    if (str3 == "streambuffersize")
                                    {
                                        goto Label_0146;
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        numbuffers = Convert.ToInt32(strArray[1].Trim());
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            goto Label_0162;
                        Label_0146:
                            try
                            {
                                filebuffersize = Convert.ToUInt32(strArray[1].Trim());
                            }
                            catch
                            {
                            }
                        Label_0162:;
                        }
                        reader.Close();
                        reader.Dispose();
                    }
                    for (num = 0; num < m_systems.Count; num++)
                    {
                        system = m_systems[num];
                        system.setDriver(num);
                        if (numbuffers > 0)
                        {
                            system.setDSPBufferSize(0x400, numbuffers);
                        }
                        system.init(0x20, INITFLAG.NORMAL, IntPtr.Zero);
                        system.setStreamBufferSize(filebuffersize, TIMEUNIT.RAWBYTES);
                    }
                }
            }
            if (deviceIndex == -1)
            {
                deviceIndex = 0;
            }
            if (deviceIndex < m_systems.Count)
            {
                m_refCount++;
                return new fmod(m_systems[deviceIndex], deviceIndex);
            }
            return new fmod(null, -1);
        }

        public static string[] GetSoundDeviceList()
        {
            CreateDeviceList();
            return m_deviceList.ToArray();
        }

        private string GetSoundName(Sound sound)
        {
            StringBuilder name = new StringBuilder(0x100);
            sound.getName(name, name.Capacity);
            return name.ToString().Trim();
        }

        public SoundChannel LoadSound(string fileName)
        {
            return this.LoadSound(fileName, null);
        }

        public SoundChannel LoadSound(string fileName, SoundChannel existingChannel)
        {
            if (this.m_system == null)
            {
                return null;
            }
            if (!((fileName != null) && File.Exists(fileName)))
            {
                return null;
            }
            Sound sound = null;
            this.ERRCHECK(this.m_system.createSound(fileName, MODE.ACCURATETIME | MODE._2D | MODE.HARDWARE | MODE.CREATESTREAM, ref sound));
            if (existingChannel == null)
            {
                existingChannel = new SoundChannel(sound);
                this.m_channels.Add(existingChannel);
            }
            else
            {
                existingChannel.Sound = sound;
            }
            return existingChannel;
        }

        public object[] LoadSoundStats(string fileName)
        {
            if (!((fileName != null) && File.Exists(fileName)))
            {
                return null;
            }
            Sound sound = null;
            uint length = 0;
            this.ERRCHECK(this.m_system.createSound(fileName, MODE.ACCURATETIME | MODE._2D | MODE.HARDWARE | MODE.CREATESTREAM, ref sound));
            sound.getLength(ref length, TIMEUNIT.MS);
            string soundName = this.GetSoundName(sound);
            sound.release();
            return new object[] { soundName, length };
        }

        public void Play(SoundChannel soundChannel)
        {
            if (this.m_system == null)
            {
                throw new Exception("Cannot play a sound with no valid sound device");
            }
            this.PlaySound(soundChannel);
        }

        public void Play(SoundChannel soundChannel, bool paused)
        {
            this.PlaySound(soundChannel, paused);
        }

        private void PlaySound(SoundChannel soundChannel)
        {
            if (soundChannel.Sound != null)
            {
                soundChannel.WaitOnFade();
                Channel channel = null;
                if (soundChannel.Channel == null)
                {
                    this.ERRCHECK(this.m_system.playSound(CHANNELINDEX.FREE, soundChannel.Sound, false, ref channel));
                    soundChannel.Channel = channel;
                }
                else if (soundChannel.Paused)
                {
                    soundChannel.Paused = false;
                }
                else
                {
                    this.ERRCHECK(this.m_system.playSound(CHANNELINDEX.REUSE, soundChannel.Sound, false, ref channel));
                    soundChannel.Channel = channel;
                }
            }
        }

        private void PlaySound(SoundChannel soundChannel, bool paused)
        {
            if ((soundChannel != null) && (soundChannel.Sound != null))
            {
                soundChannel.WaitOnFade();
                Channel channel = null;
                if (soundChannel.Channel == null)
                {
                    this.ERRCHECK(this.m_system.playSound(CHANNELINDEX.FREE, soundChannel.Sound, paused, ref channel));
                    soundChannel.Channel = channel;
                }
                else
                {
                    bool flag = false;
                    soundChannel.Channel.getPaused(ref flag);
                    if (flag)
                    {
                        if (!paused)
                        {
                            soundChannel.Channel.setPaused(false);
                        }
                    }
                    else
                    {
                        this.ERRCHECK(this.m_system.playSound(CHANNELINDEX.REUSE, soundChannel.Sound, paused, ref channel));
                        soundChannel.Channel = channel;
                    }
                }
            }
        }

        public void ReleaseSound(SoundChannel soundChannel)
        {
            if (soundChannel != null)
            {
                if (this.m_channels.Contains(soundChannel))
                {
                    this.m_channels.Remove(soundChannel);
                }
                soundChannel.Dispose();
            }
        }

        public void Shutdown()
        {
            foreach (SoundChannel channel in this.m_channels)
            {
                channel.Dispose();
            }
            if ((this.m_system != null) && (--m_refCount == 0))
            {
                foreach (_System system in m_systems)
                {
                    this.ERRCHECK(system.release());
                }
                m_systems.Clear();
            }
        }

        public void Stop(SoundChannel soundChannel)
        {
            if ((soundChannel != null) && (soundChannel.Channel != null))
            {
                soundChannel.Channel.stop();
                soundChannel.CancelFades();
            }
        }

        public void Stop(SoundChannel soundChannel, int fadeDuration)
        {
            if (((soundChannel != null) && (soundChannel.Channel != null)) && ((fadeDuration != 0) && soundChannel.IsPlaying))
            {
                soundChannel.ImmediateFade(fadeDuration);
                soundChannel.WaitOnFade();
                soundChannel.Channel.stop();
                soundChannel.CancelFades();
            }
        }

        public int DeviceIndex
        {
            get
            {
                return this.m_deviceIndex;
            }
            set
            {
                if (value == -1)
                {
                    value = 0;
                }
                if (value < m_systems.Count)
                {
                    this.m_deviceIndex = value;
                    this.m_system = m_systems[value];
                }
            }
        }

        public _System SystemObject
        {
            get
            {
                return this.m_system;
            }
        }
    }
}

