namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ChannelGroup
    {
        private IntPtr channelgroupraw;

        public RESULT addDSP(DSP dsp, ref DSPConnection dspconnection)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSPConnection connection = null;
            try
            {
                oK = FMOD_ChannelGroup_AddDSP(this.channelgroupraw, dsp.getRaw(), ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (dspconnection == null)
                {
                    connection = new DSPConnection();
                    connection.setRaw(ptr);
                    dspconnection = connection;
                }
                else
                {
                    dspconnection.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT addGroup(ChannelGroup group)
        {
            return FMOD_ChannelGroup_AddGroup(this.channelgroupraw, group.getRaw());
        }

        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_AddDSP(IntPtr channelgroup, IntPtr dsp, ref IntPtr dspconnection);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_AddGroup(IntPtr channelgroup, IntPtr group);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_GetChannel(IntPtr channelgroup, int index, ref IntPtr channel);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_GetDSPHead(IntPtr channelgroup, ref IntPtr dsp);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_GetGroup(IntPtr channelgroup, int index, ref IntPtr group);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_GetMute(IntPtr channelgroup, ref int mute);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_GetName(IntPtr channelgroup, StringBuilder name, int namelen);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_GetNumChannels(IntPtr channelgroup, ref int numchannels);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_GetNumGroups(IntPtr channelgroup, ref int numgroups);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_GetParentGroup(IntPtr channelgroup, ref IntPtr group);
        [DllImport("fmodex", EntryPoint="FMOD_ChannelGroup_GetPaused")]
        private static extern RESULT FMOD_ChannelGroup_GetPaused_32(IntPtr channelgroup, ref int paused);
        [DllImport("fmodex64", EntryPoint="FMOD_ChannelGroup_GetPaused")]
        private static extern RESULT FMOD_ChannelGroup_GetPaused_64(IntPtr channelgroup, ref int paused);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_GetPitch(IntPtr channelgroup, ref float pitch);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_GetSpectrum(IntPtr channelgroup, [MarshalAs(UnmanagedType.LPArray)] float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_GetSystemObject(IntPtr channelgroup, ref IntPtr system);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_GetUserData(IntPtr channelgroup, ref IntPtr userdata);
        [DllImport("fmodex", EntryPoint="FMOD_ChannelGroup_GetVolume")]
        private static extern RESULT FMOD_ChannelGroup_GetVolume_32(IntPtr channelgroup, ref float volume);
        [DllImport("fmodex64", EntryPoint="FMOD_ChannelGroup_GetVolume")]
        private static extern RESULT FMOD_ChannelGroup_GetVolume_64(IntPtr channelgroup, ref float volume);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_GetWaveData(IntPtr channelgroup, [MarshalAs(UnmanagedType.LPArray)] float[] wavearray, int numvalues, int channeloffset);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_Override3DAttributes(IntPtr channelgroup, ref VECTOR pos, ref VECTOR vel);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_OverrideFrequency(IntPtr channelgroup, float frequency);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_OverrideMute(IntPtr channelgroup, bool mute);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_OverridePan(IntPtr channelgroup, float pan);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_OverridePaused(IntPtr channelgroup, bool paused);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_OverrideReverbProperties(IntPtr channelgroup, ref REVERB_CHANNELPROPERTIES prop);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_OverrideSpeakerMix(IntPtr channelgroup, float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_OverrideVolume(IntPtr channelgroup, float volume);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_Release(IntPtr channelgroup);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_SetMute(IntPtr channelgroup, int mute);
        [DllImport("fmodex", EntryPoint="FMOD_ChannelGroup_SetPaused")]
        private static extern RESULT FMOD_ChannelGroup_SetPaused_32(IntPtr channelgroup, int paused);
        [DllImport("fmodex64", EntryPoint="FMOD_ChannelGroup_SetPaused")]
        private static extern RESULT FMOD_ChannelGroup_SetPaused_64(IntPtr channelgroup, int paused);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_SetPitch(IntPtr channelgroup, float pitch);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_ChannelGroup_SetUserData(IntPtr channelgroup, IntPtr userdata);
        [DllImport("fmodex", EntryPoint="FMOD_ChannelGroup_SetVolume")]
        private static extern RESULT FMOD_ChannelGroup_SetVolume_32(IntPtr channelgroup, float volume);
        [DllImport("fmodex64", EntryPoint="FMOD_ChannelGroup_SetVolume")]
        private static extern RESULT FMOD_ChannelGroup_SetVolume_64(IntPtr channelgroup, float volume);
        [DllImport("fmodex", EntryPoint="FMOD_ChannelGroup_Stop")]
        private static extern RESULT FMOD_ChannelGroup_Stop_32(IntPtr channelgroup);
        [DllImport("fmodex64", EntryPoint="FMOD_ChannelGroup_Stop")]
        private static extern RESULT FMOD_ChannelGroup_Stop_64(IntPtr channelgroup);
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

        public RESULT getMute(ref bool mute)
        {
            int num = 0;
            RESULT result = FMOD_ChannelGroup_GetMute(this.channelgroupraw, ref num);
            mute = num != 0;
            return result;
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

        public RESULT getParentGroup(ref ChannelGroup group)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            ChannelGroup group2 = null;
            try
            {
                oK = FMOD_ChannelGroup_GetParentGroup(this.channelgroupraw, ref ptr);
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

        public RESULT getPaused(ref bool paused)
        {
            RESULT result;
            int num = 0;
            if (VERSION.platform == Platform.X64)
            {
                result = FMOD_ChannelGroup_GetPaused_64(this.channelgroupraw, ref num);
            }
            else
            {
                result = FMOD_ChannelGroup_GetPaused_32(this.channelgroupraw, ref num);
            }
            paused = num != 0;
            return result;
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

        public RESULT getSystemObject(ref _System system)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            _System system2 = null;
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
                    system2 = new _System();
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
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_ChannelGroup_GetVolume_64(this.channelgroupraw, ref volume);
            }
            return FMOD_ChannelGroup_GetVolume_32(this.channelgroupraw, ref volume);
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

        public RESULT overridePan(float pan)
        {
            return FMOD_ChannelGroup_OverridePan(this.channelgroupraw, pan);
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

        public RESULT setMute(bool mute)
        {
            return FMOD_ChannelGroup_SetMute(this.channelgroupraw, mute ? 1 : 0);
        }

        public RESULT setPaused(bool paused)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_ChannelGroup_SetPaused_64(this.channelgroupraw, paused ? 1 : 0);
            }
            return FMOD_ChannelGroup_SetPaused_32(this.channelgroupraw, paused ? 1 : 0);
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
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_ChannelGroup_SetVolume_64(this.channelgroupraw, volume);
            }
            return FMOD_ChannelGroup_SetVolume_32(this.channelgroupraw, volume);
        }

        public RESULT stop()
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_ChannelGroup_Stop_64(this.channelgroupraw);
            }
            return FMOD_ChannelGroup_Stop_32(this.channelgroupraw);
        }
    }
}

