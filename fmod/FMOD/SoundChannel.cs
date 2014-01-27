using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;

namespace FMOD
{
    public class SoundChannel : IDisposable
    {
        private const int FADE_TIMER_INTERVAL = 100;
        private FMOD.Channel m_channel = null;
        private float m_channelVolume = 1f;
        private float m_entryFadeDelta = 0f;
        private float m_exitFadeDelta = 0f;
        private float m_exitFadeThreshold = 0f;
        private global::System.Timers.Timer m_fadeTimer = new global::System.Timers.Timer(100.0);
        private FadeTimerState m_fadeTimerState = FadeTimerState.Inactive;
        private float m_normalFrequency;
        private FMOD.Sound m_sound = null;
        private uint m_soundLength = 0;
        private string m_soundName = string.Empty;

        public SoundChannel(FMOD.Sound sound)
        {
            this.m_fadeTimer.Elapsed += new ElapsedEventHandler(this.fadeTimer_Elapsed);
            this.Sound = sound;
        }

        public void CancelFades()
        {
            this.m_fadeTimer.Enabled = false;
            this.m_fadeTimerState = FadeTimerState.Inactive;
            this.m_entryFadeDelta = 0f;
            this.m_exitFadeDelta = 0f;
            this.m_exitFadeThreshold = 0f;
        }

        public void Dispose()
        {
            if (this.m_fadeTimer.Enabled)
            {
                this.m_fadeTimer.Enabled = false;
            }
            bool isplaying = false;
            if (this.m_channel != null)
            {
                this.m_channel.isPlaying(ref isplaying);
                if (isplaying)
                {
                    this.m_channel.stop();
                }
                this.m_channel = null;
            }
            if (this.m_sound != null)
            {
                this.m_sound.release();
                this.m_sound = null;
            }
            GC.SuppressFinalize(this);
        }

        private void fadeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            switch (this.m_fadeTimerState)
            {
                case FadeTimerState.Inactive:
                    this.m_fadeTimer.Enabled = false;
                    break;

                case FadeTimerState.Entry:
                    if (this.m_channelVolume >= 1f)
                    {
                        if (this.m_exitFadeDelta != 0f)
                        {
                            this.m_fadeTimerState = FadeTimerState.Threshold;
                        }
                        else
                        {
                            this.m_fadeTimerState = FadeTimerState.Inactive;
                        }
                        break;
                    }
                    if ((1f - this.m_channelVolume) >= this.m_entryFadeDelta)
                    {
                        this.Volume = this.m_channelVolume + this.m_entryFadeDelta;
                        break;
                    }
                    this.Volume = 1f;
                    break;

                case FadeTimerState.Threshold:
                    if ((this.m_exitFadeThreshold != 0f) && (this.Position >= this.m_exitFadeThreshold))
                    {
                        this.m_fadeTimerState = FadeTimerState.Exit;
                    }
                    break;

                case FadeTimerState.Exit:
                    if (this.m_channelVolume <= 0f)
                    {
                        this.m_fadeTimerState = FadeTimerState.Inactive;
                        break;
                    }
                    if (this.m_channelVolume >= -this.m_exitFadeDelta)
                    {
                        this.Volume = this.m_channelVolume + this.m_exitFadeDelta;
                        break;
                    }
                    this.Volume = 0f;
                    break;
            }
        }

        ~SoundChannel()
        {
            this.Dispose();
        }

        private string GetSoundName(FMOD.Sound sound)
        {
            //original implementation did not return all characters
            //StringBuilder name = new StringBuilder(0x100);
            //sound.getName(name, name.Capacity);

            //begin custom implementation
            string name = "";
            var tagCount = 0;
            var tagsUpdated = 0;
            sound.getNumTags(ref tagCount, ref tagsUpdated);
            TAG tag = new TAG();
            for (var i = 0; i < tagCount; i++) {
                sound.getTag(null, i, ref tag);
                if (tag.name == "TIT2") {
                    name = Marshal.PtrToStringAnsi(tag.data);
                    break;
                }
            }
            return name;
        }

        public void ImmediateFade(int durationInSeconds)
        {
            if (durationInSeconds != 0)
            {
                this.m_exitFadeDelta = -1f / (durationInSeconds * 10f);
                this.m_exitFadeThreshold = this.Position + 100;
                this.m_fadeTimerState = FadeTimerState.Threshold;
                if (!this.m_fadeTimer.Enabled)
                {
                    this.m_fadeTimer.Enabled = true;
                }
            }
        }

        public void SetEntryFade(int durationInSeconds)
        {
            if (durationInSeconds != 0)
            {
                this.m_entryFadeDelta = 1f / (durationInSeconds * 10f);
            }
            else
            {
                this.m_entryFadeDelta = 0f;
            }
        }

        public void SetExitFade(int durationInSeconds)
        {
            if (durationInSeconds != 0)
            {
                this.m_exitFadeDelta = -1f / (durationInSeconds * 10f);
                this.m_exitFadeThreshold = this.m_soundLength - (durationInSeconds * 0x3e8);
            }
            else
            {
                this.m_exitFadeDelta = 0f;
                this.m_exitFadeThreshold = 0f;
            }
        }

        public void WaitOnFade()
        {
            while (this.m_fadeTimer.Enabled)
            {
                Thread.Sleep(100);
            }
        }

        public FMOD.Channel Channel
        {
            get
            {
                return this.m_channel;
            }
            set
            {
                this.m_fadeTimerState = FadeTimerState.Inactive;
                this.m_channel = value;
                if (this.m_entryFadeDelta != 0f)
                {
                    this.Volume = 0f;
                    this.m_fadeTimerState = FadeTimerState.Entry;
                    this.m_fadeTimer.Enabled = true;
                }
                if (!((this.m_exitFadeDelta == 0f) || this.m_fadeTimer.Enabled))
                {
                    this.m_fadeTimerState = FadeTimerState.Threshold;
                    this.m_fadeTimer.Enabled = true;
                }
                this.m_channel.getFrequency(ref this.m_normalFrequency);
            }
        }

        public float Frequency
        {
            get
            {
                if (this.m_channel == null)
                {
                    return 0f;
                }
                float frequency = 0f;
                this.m_channel.getFrequency(ref frequency);
                return frequency;
            }
            set
            {
                if (this.m_channel != null)
                {
                    this.m_channel.setFrequency(value * this.m_normalFrequency);
                }
            }
        }

        public bool IsPlaying
        {
            get
            {
                if ((this.m_channel == null) || (this.m_sound == null))
                {
                    return false;
                }
                bool isplaying = false;
                this.m_channel.isPlaying(ref isplaying);
                return isplaying;
            }
        }

        public bool Paused
        {
            get
            {
                if (this.m_channel != null)
                {
                    bool paused = false;
                    this.m_channel.getPaused(ref paused);
                    return paused;
                }
                return false;
            }
            set
            {
                if (this.m_channel != null)
                {
                    this.m_channel.setPaused(value);
                    if (this.m_fadeTimerState != FadeTimerState.Inactive)
                    {
                        this.m_fadeTimer.Enabled = !value;
                    }
                }
            }
        }

        public uint Position
        {
            get
            {
                uint position = 0;
                if (this.m_channel != null)
                {
                    this.m_channel.getPosition(ref position, TIMEUNIT.MS);
                }
                return position;
            }
            set
            {
                if (this.m_channel != null)
                {
                    this.m_channel.setPosition(value, TIMEUNIT.MS);
                }
            }
        }

        public FMOD.Sound Sound
        {
            get
            {
                return this.m_sound;
            }
            set
            {
                this.m_sound = value;
                this.m_sound.getLength(ref this.m_soundLength, TIMEUNIT.MS);
                this.m_soundName = this.GetSoundName(this.m_sound);
            }
        }

        public uint SoundLength
        {
            get
            {
                return this.m_soundLength;
            }
        }

        public string SoundName
        {
            get
            {
                return this.m_soundName;
            }
        }

        public float Volume
        {
            get
            {
                if (this.m_channel != null)
                {
                    this.m_channel.getVolume(ref this.m_channelVolume);
                    return this.m_channelVolume;
                }
                return 0f;
            }
            set
            {
                if (this.m_channel != null)
                {
                    this.m_channel.setVolume(value);
                    this.m_channelVolume = value;
                }
            }
        }

        private enum FadeTimerState
        {
            Inactive,
            Entry,
            Threshold,
            Exit
        }
    }
}

