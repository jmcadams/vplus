namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ChannelGroup
    {
        private IntPtr channelgroupraw;

        public RESULT addDSP(DSP dsp)
        {
            return FMOD_ChannelGroup_AddDSP(this.channelgroupraw, dsp.getRaw());
        }

        public RESULT addGroup(ChannelGroup group)
        {
            return FMOD_ChannelGroup_AddGroup(this.channelgroupraw, group.getRaw());
        }

        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_AddDSP(IntPtr channelgroup, IntPtr dsp);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_AddGroup(IntPtr channelgroup, IntPtr group);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_GetChannel(IntPtr channelgroup, int index, ref IntPtr channel);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_GetDSPHead(IntPtr channelgroup, ref IntPtr dsp);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_GetGroup(IntPtr channelgroup, int index, ref IntPtr group);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_GetName(IntPtr channelgroup, StringBuilder name, int namelen);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_GetNumChannels(IntPtr channelgroup, ref int numchannels);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_GetNumGroups(IntPtr channelgroup, ref int numgroups);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_GetPitch(IntPtr channelgroup, ref float pitch);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_GetSpectrum(IntPtr channelgroup, [MarshalAs(UnmanagedType.LPArray)] float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_GetSystemObject(IntPtr channelgroup, ref IntPtr system);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_GetUserData(IntPtr channelgroup, ref IntPtr userdata);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_GetVolume(IntPtr channelgroup, ref float volume);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_GetWaveData(IntPtr channelgroup, [MarshalAs(UnmanagedType.LPArray)] float[] wavearray, int numvalues, int channeloffset);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_Override3DAttributes(IntPtr channelgroup, ref VECTOR pos, ref VECTOR vel);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_OverrideFrequency(IntPtr channelgroup, float frequency);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_OverrideMute(IntPtr channelgroup, bool mute);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_OverridePan(IntPtr channelgroup, float pan);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_OverridePaused(IntPtr channelgroup, bool paused);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_OverrideReverbProperties(IntPtr channelgroup, ref REVERB_CHANNELPROPERTIES prop);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_OverrideSpeakerMix(IntPtr channelgroup, float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_OverrideVolume(IntPtr channelgroup, float volume);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_Release(IntPtr channelgroup);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_SetPitch(IntPtr channelgroup, float pitch);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_SetUserData(IntPtr channelgroup, IntPtr userdata);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_SetVolume(IntPtr channelgroup, float volume);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_ChannelGroup_Stop(IntPtr channelgroup);
        public RESULT getChannel(int index, ref Channel channel)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            Channel channel2 = null;
            try
            {
                oK = FMOD_ChannelGroup_GetChannel(this.channelgroupraw, index, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (channel == null)
                {
                    channel2 = new Channel();
                    channel2.setRaw(ptr);
                    channel = channel2;
                }
                else
                {
                    channel.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT getDSPHead(ref DSP dsp)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSP dsp2 = null;
            try
            {
                oK = FMOD_ChannelGroup_GetDSPHead(this.channelgroupraw, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (dsp == null)
                {
                    dsp2 = new DSP();
                    dsp2.setRaw(ptr);
                    dsp = dsp2;
                }
                else
                {
                    dsp.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT getGroup(int index, ref ChannelGroup group)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            ChannelGroup group2 = null;
            try
            {
                oK = FMOD_ChannelGroup_GetGroup(this.channelgroupraw, index, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (group == null)
                {
                    group2 = new ChannelGroup();
                    group2.setRaw(ptr);
                    group = group2;
                }
                else
                {
                    group.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT getName(StringBuilder name, int namelen)
        {
            return FMOD_ChannelGroup_GetName(this.channelgroupraw, name, namelen);
        }

        public RESULT getNumChannels(ref int numchannels)
        {
            return FMOD_ChannelGroup_GetNumChannels(this.channelgroupraw, ref numchannels);
        }

        public RESULT getNumGroups(ref int numgroups)
        {
            return FMOD_ChannelGroup_GetNumGroups(this.channelgroupraw, ref numgroups);
        }

        public RESULT getPitch(ref float pitch)
        {
            return FMOD_ChannelGroup_GetPitch(this.channelgroupraw, ref pitch);
        }

        public IntPtr getRaw()
        {
            return this.channelgroupraw;
        }

        public RESULT getSpectrum(float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype)
        {
            return FMOD_ChannelGroup_GetSpectrum(this.channelgroupraw, spectrumarray, numvalues, channeloffset, windowtype);
        }

        public RESULT getSystemObject(ref System system)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            System system2 = null;
            try
            {
                oK = FMOD_ChannelGroup_GetSystemObject(this.channelgroupraw, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (system == null)
                {
                    system2 = new System();
                    system2.setRaw(ptr);
                    system = system2;
                }
                else
                {
                    system.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT getUserData(ref IntPtr userdata)
        {
            return FMOD_ChannelGroup_GetUserData(this.channelgroupraw, ref userdata);
        }

        public RESULT getVolume(ref float volume)
        {
            return FMOD_ChannelGroup_GetVolume(this.channelgroupraw, ref volume);
        }

        public RESULT getWaveData(float[] wavearray, int numvalues, int channeloffset)
        {
            return FMOD_ChannelGroup_GetWaveData(this.channelgroupraw, wavearray, numvalues, channeloffset);
        }

        public RESULT override3DAttributes(ref VECTOR pos, ref VECTOR vel)
        {
            return FMOD_ChannelGroup_Override3DAttributes(this.channelgroupraw, ref pos, ref vel);
        }

        public RESULT overrideFrequency(float frequency)
        {
            return FMOD_ChannelGroup_OverrideFrequency(this.channelgroupraw, frequency);
        }

        public RESULT overrideMute(bool mute)
        {
            return FMOD_ChannelGroup_OverrideMute(this.channelgroupraw, mute);
        }

        public RESULT overridePan(float pan)
        {
            return FMOD_ChannelGroup_OverridePan(this.channelgroupraw, pan);
        }

        public RESULT overridePaused(bool paused)
        {
            return FMOD_ChannelGroup_OverridePaused(this.channelgroupraw, paused);
        }

        public RESULT overrideReverbProperties(ref REVERB_CHANNELPROPERTIES prop)
        {
            return FMOD_ChannelGroup_OverrideReverbProperties(this.channelgroupraw, ref prop);
        }

        public RESULT overrideSpeakerMix(float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright)
        {
            return FMOD_ChannelGroup_OverrideSpeakerMix(this.channelgroupraw, frontleft, frontright, center, lfe, backleft, backright, sideleft, sideright);
        }

        public RESULT overrideVolume(float volume)
        {
            return FMOD_ChannelGroup_OverrideVolume(this.channelgroupraw, volume);
        }

        public RESULT release()
        {
            return FMOD_ChannelGroup_Release(this.channelgroupraw);
        }

        public RESULT setPitch(float pitch)
        {
            return FMOD_ChannelGroup_SetPitch(this.channelgroupraw, pitch);
        }

        public void setRaw(IntPtr channelgroup)
        {
            this.channelgroupraw = new IntPtr();
            this.channelgroupraw = channelgroup;
        }

        public RESULT setUserData(IntPtr userdata)
        {
            return FMOD_ChannelGroup_SetUserData(this.channelgroupraw, userdata);
        }

        public RESULT setVolume(float volume)
        {
            return FMOD_ChannelGroup_SetVolume(this.channelgroupraw, volume);
        }

        public RESULT stop()
        {
            return FMOD_ChannelGroup_Stop(this.channelgroupraw);
        }
    }
}

