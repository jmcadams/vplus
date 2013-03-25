namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;

    public class Channel
    {
        private IntPtr channelraw;

        public RESULT addDSP(DSP dsp, ref DSPConnection dspconnection)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSPConnection connection = null;
            try
            {
                oK = FMOD_Channel_AddDSP(this.channelraw, dsp.getRaw(), ref ptr);
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

        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_AddDSP(IntPtr channel, IntPtr dsp, ref IntPtr dspconnection);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Get3DAttributes(IntPtr channel, ref VECTOR pos, ref VECTOR vel);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Get3DConeOrientation(IntPtr channel, ref VECTOR orientation);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Get3DConeSettings(IntPtr channel, ref float insideconeangle, ref float outsideconeangle, ref float outsidevolume);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Get3DCustomRolloff(IntPtr channel, ref IntPtr points, ref int numpoints);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Get3DDopplerLevel(IntPtr channel, ref float level);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Get3DMinMaxDistance(IntPtr channel, ref float mindistance, ref float maxdistance);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Get3DOcclusion(IntPtr channel, ref float directOcclusion, ref float reverbOcclusion);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Get3DPanLevel(IntPtr channel, ref float level);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Get3DSpread(IntPtr channel, ref float angle);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetAudibility(IntPtr channel, ref float audibility);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetChannelGroup(IntPtr channel, ref IntPtr channelgroup);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetCurrentSound(IntPtr channel, ref IntPtr sound);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetDelay(IntPtr channel, DELAYTYPE delaytype, ref uint delayhi, ref uint delaylo);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetDSPHead(IntPtr channel, ref IntPtr dsp);
        [DllImport("fmodex", EntryPoint="FMOD_Channel_GetFrequency")]
        private static extern RESULT FMOD_Channel_GetFrequency_32(IntPtr channel, ref float frequency);
        [DllImport("fmodex64", EntryPoint="FMOD_Channel_GetFrequency")]
        private static extern RESULT FMOD_Channel_GetFrequency_64(IntPtr channel, ref float frequency);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetIndex(IntPtr channel, ref int index);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetInputChannelMix(IntPtr channel, ref float levels, int numlevels);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetLoopCount(IntPtr channel, ref int loopcount);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetLoopPoints(IntPtr channel, ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetMode(IntPtr channel, ref MODE mode);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetMute(IntPtr channel, ref int mute);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetPan(IntPtr channel, ref float pan);
        [DllImport("fmodex", EntryPoint="FMOD_Channel_GetPaused")]
        private static extern RESULT FMOD_Channel_GetPaused_32(IntPtr channel, ref int paused);
        [DllImport("fmodex64", EntryPoint="FMOD_Channel_GetPaused")]
        private static extern RESULT FMOD_Channel_GetPaused_64(IntPtr channel, ref int paused);
        [DllImport("fmodex", EntryPoint="FMOD_Channel_GetPosition")]
        private static extern RESULT FMOD_Channel_GetPosition_32(IntPtr channel, ref uint position, TIMEUNIT postype);
        [DllImport("fmodex64", EntryPoint="FMOD_Channel_GetPosition")]
        private static extern RESULT FMOD_Channel_GetPosition_64(IntPtr channel, ref uint position, TIMEUNIT postype);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetPriority(IntPtr channel, ref int priority);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetReverbProperties(IntPtr channel, ref REVERB_CHANNELPROPERTIES prop);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetSpeakerLevels(IntPtr channel, SPEAKER speaker, [MarshalAs(UnmanagedType.LPArray)] float[] levels, int numlevels);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetSpeakerMix(IntPtr channel, ref float frontleft, ref float frontright, ref float center, ref float lfe, ref float backleft, ref float backright, ref float sideleft, ref float sideright);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetSpectrum(IntPtr channel, [MarshalAs(UnmanagedType.LPArray)] float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetSystemObject(IntPtr channel, ref IntPtr system);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetUserData(IntPtr channel, ref IntPtr userdata);
        [DllImport("fmodex", EntryPoint="FMOD_Channel_GetVolume")]
        private static extern RESULT FMOD_Channel_GetVolume_32(IntPtr channel, ref float volume);
        [DllImport("fmodex64", EntryPoint="FMOD_Channel_GetVolume")]
        private static extern RESULT FMOD_Channel_GetVolume_64(IntPtr channel, ref float volume);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_GetWaveData(IntPtr channel, [MarshalAs(UnmanagedType.LPArray)] float[] wavearray, int numvalues, int channeloffset);
        [DllImport("fmodex", EntryPoint="FMOD_Channel_IsPlaying")]
        private static extern RESULT FMOD_Channel_IsPlaying_32(IntPtr channel, ref bool isplaying);
        [DllImport("fmodex64", EntryPoint="FMOD_Channel_IsPlaying")]
        private static extern RESULT FMOD_Channel_IsPlaying_64(IntPtr channel, ref bool isplaying);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_IsVirtual(IntPtr channel, ref bool isvirtual);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Set3DAttributes(IntPtr channel, ref VECTOR pos, ref VECTOR vel);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Set3DConeOrientation(IntPtr channel, ref VECTOR orientation);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Set3DConeSettings(IntPtr channel, float insideconeangle, float outsideconeangle, float outsidevolume);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Set3DCustomRolloff(IntPtr channel, ref VECTOR points, int numpoints);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Set3DDopplerLevel(IntPtr channel, float level);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Set3DMinMaxDistance(IntPtr channel, float mindistance, float maxdistance);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Set3DOcclusion(IntPtr channel, float directOcclusion, float reverbOcclusion);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Set3DPanLevel(IntPtr channel, float level);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_Set3DSpread(IntPtr channel, float angle);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_SetCallback(IntPtr channel, CHANNEL_CALLBACKTYPE type, CHANNEL_CALLBACK callback, int command);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_SetChannelGroup(IntPtr channel, IntPtr channelgroup);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_SetDelay(IntPtr channel, DELAYTYPE delaytype, uint delayhi, uint delaylo);
        [DllImport("fmodex", EntryPoint="FMOD_Channel_SetFrequency")]
        private static extern RESULT FMOD_Channel_SetFrequency_32(IntPtr channel, float frequency);
        [DllImport("fmodex64", EntryPoint="FMOD_Channel_SetFrequency")]
        private static extern RESULT FMOD_Channel_SetFrequency_64(IntPtr channel, float frequency);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_SetInputChannelMix(IntPtr channel, ref float levels, int numlevels);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_SetLoopCount(IntPtr channel, int loopcount);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_SetLoopPoints(IntPtr channel, uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_SetMode(IntPtr channel, MODE mode);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_SetMute(IntPtr channel, int mute);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_SetPan(IntPtr channel, float pan);
        [DllImport("fmodex", EntryPoint="FMOD_Channel_SetPaused")]
        private static extern RESULT FMOD_Channel_SetPaused_32(IntPtr channel, int paused);
        [DllImport("fmodex64", EntryPoint="FMOD_Channel_SetPaused")]
        private static extern RESULT FMOD_Channel_SetPaused_64(IntPtr channel, int paused);
        [DllImport("fmodex", EntryPoint="FMOD_Channel_SetPosition")]
        private static extern RESULT FMOD_Channel_SetPosition_32(IntPtr channel, uint position, TIMEUNIT postype);
        [DllImport("fmodex64", EntryPoint="FMOD_Channel_SetPosition")]
        private static extern RESULT FMOD_Channel_SetPosition_64(IntPtr channel, uint position, TIMEUNIT postype);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_SetPriority(IntPtr channel, int priority);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_SetReverbProperties(IntPtr channel, ref REVERB_CHANNELPROPERTIES prop);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_SetSpeakerLevels(IntPtr channel, SPEAKER speaker, float[] levels, int numlevels);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_SetSpeakerMix(IntPtr channel, float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Channel_SetUserData(IntPtr channel, IntPtr userdata);
        [DllImport("fmodex", EntryPoint="FMOD_Channel_SetVolume")]
        private static extern RESULT FMOD_Channel_SetVolume_32(IntPtr channel, float volume);
        [DllImport("fmodex64", EntryPoint="FMOD_Channel_SetVolume")]
        private static extern RESULT FMOD_Channel_SetVolume_64(IntPtr channel, float volume);
        [DllImport("fmodex", EntryPoint="FMOD_Channel_Stop")]
        private static extern RESULT FMOD_Channel_Stop_32(IntPtr channel);
        [DllImport("fmodex64", EntryPoint="FMOD_Channel_Stop")]
        private static extern RESULT FMOD_Channel_Stop_64(IntPtr channel);
        public RESULT get3DAttributes(ref VECTOR pos, ref VECTOR vel)
        {
            return FMOD_Channel_Get3DAttributes(this.channelraw, ref pos, ref vel);
        }

        public RESULT get3DConeOrientation(ref VECTOR orientation)
        {
            return FMOD_Channel_Get3DConeOrientation(this.channelraw, ref orientation);
        }

        public RESULT get3DConeSettings(ref float insideconeangle, ref float outsideconeangle, ref float outsidevolume)
        {
            return FMOD_Channel_Get3DConeSettings(this.channelraw, ref insideconeangle, ref outsideconeangle, ref outsidevolume);
        }

        public RESULT get3DCustomRolloff(ref IntPtr points, ref int numpoints)
        {
            return FMOD_Channel_Get3DCustomRolloff(this.channelraw, ref points, ref numpoints);
        }

        public RESULT get3DDopplerLevel(ref float level)
        {
            return FMOD_Channel_Get3DDopplerLevel(this.channelraw, ref level);
        }

        public RESULT get3DMinMaxDistance(ref float mindistance, ref float maxdistance)
        {
            return FMOD_Channel_Get3DMinMaxDistance(this.channelraw, ref mindistance, ref maxdistance);
        }

        public RESULT get3DOcclusion(ref float directOcclusion, ref float reverbOcclusion)
        {
            return FMOD_Channel_Get3DOcclusion(this.channelraw, ref directOcclusion, ref reverbOcclusion);
        }

        public RESULT get3DPanLevel(ref float level)
        {
            return FMOD_Channel_Get3DPanLevel(this.channelraw, ref level);
        }

        public RESULT get3DSpread(ref float angle)
        {
            return FMOD_Channel_Get3DSpread(this.channelraw, ref angle);
        }

        public RESULT getAudibility(ref float audibility)
        {
            return FMOD_Channel_GetAudibility(this.channelraw, ref audibility);
        }

        public RESULT getChannelGroup(ref ChannelGroup channelgroup)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            ChannelGroup group = null;
            try
            {
                oK = FMOD_Channel_GetChannelGroup(this.channelraw, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (channelgroup == null)
                {
                    group = new ChannelGroup();
                    group.setRaw(ptr);
                    channelgroup = group;
                }
                else
                {
                    channelgroup.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT getCurrentSound(ref Sound sound)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            Sound sound2 = null;
            try
            {
                oK = FMOD_Channel_GetCurrentSound(this.channelraw, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (sound == null)
                {
                    sound2 = new Sound();
                    sound2.setRaw(ptr);
                    sound = sound2;
                }
                else
                {
                    sound.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT getDelay(DELAYTYPE delaytype, ref uint delayhi, ref uint delaylo)
        {
            return FMOD_Channel_GetDelay(this.channelraw, delaytype, ref delayhi, ref delaylo);
        }

        public RESULT getDSPHead(ref DSP dsp)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSP dsp2 = null;
            try
            {
                oK = FMOD_Channel_GetDSPHead(this.channelraw, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                dsp2 = new DSP();
                dsp2.setRaw(ptr);
                dsp = dsp2;
            }
            return oK;
        }

        public RESULT getFrequency(ref float frequency)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Channel_GetFrequency_64(this.channelraw, ref frequency);
            }
            return FMOD_Channel_GetFrequency_32(this.channelraw, ref frequency);
        }

        public RESULT getIndex(ref int index)
        {
            return FMOD_Channel_GetIndex(this.channelraw, ref index);
        }

        public RESULT getInputChannelMix(ref float levels, int numlevels)
        {
            return FMOD_Channel_GetInputChannelMix(this.channelraw, ref levels, numlevels);
        }

        public RESULT getLoopCount(ref int loopcount)
        {
            return FMOD_Channel_GetLoopCount(this.channelraw, ref loopcount);
        }

        public RESULT getLoopPoints(ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype)
        {
            return FMOD_Channel_GetLoopPoints(this.channelraw, ref loopstart, loopstarttype, ref loopend, loopendtype);
        }

        public RESULT getMode(ref MODE mode)
        {
            return FMOD_Channel_GetMode(this.channelraw, ref mode);
        }

        public RESULT getMute(ref bool mute)
        {
            int num = 0;
            RESULT result = FMOD_Channel_GetMute(this.channelraw, ref num);
            mute = num != 0;
            return result;
        }

        public RESULT getPan(ref float pan)
        {
            return FMOD_Channel_GetPan(this.channelraw, ref pan);
        }

        public RESULT getPaused(ref bool paused)
        {
            RESULT result;
            int num = 0;
            if (VERSION.platform == Platform.X64)
            {
                result = FMOD_Channel_GetPaused_64(this.channelraw, ref num);
            }
            else
            {
                result = FMOD_Channel_GetPaused_32(this.channelraw, ref num);
            }
            paused = num != 0;
            return result;
        }

        public RESULT getPosition(ref uint position, TIMEUNIT postype)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Channel_GetPosition_64(this.channelraw, ref position, postype);
            }
            return FMOD_Channel_GetPosition_32(this.channelraw, ref position, postype);
        }

        public RESULT getPriority(ref int priority)
        {
            return FMOD_Channel_GetPriority(this.channelraw, ref priority);
        }

        public IntPtr getRaw()
        {
            return this.channelraw;
        }

        public RESULT getReverbProperties(ref REVERB_CHANNELPROPERTIES prop)
        {
            return FMOD_Channel_GetReverbProperties(this.channelraw, ref prop);
        }

        public RESULT getSpeakerLevels(SPEAKER speaker, float[] levels, int numlevels)
        {
            return FMOD_Channel_GetSpeakerLevels(this.channelraw, speaker, levels, numlevels);
        }

        public RESULT getSpeakerMix(ref float frontleft, ref float frontright, ref float center, ref float lfe, ref float backleft, ref float backright, ref float sideleft, ref float sideright)
        {
            return FMOD_Channel_GetSpeakerMix(this.channelraw, ref frontleft, ref frontright, ref center, ref lfe, ref backleft, ref backright, ref sideleft, ref sideright);
        }

        public RESULT getSpectrum(float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype)
        {
            return FMOD_Channel_GetSpectrum(this.channelraw, spectrumarray, numvalues, channeloffset, windowtype);
        }

        public RESULT getSystemObject(ref _System system)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            _System system2 = null;
            try
            {
                oK = FMOD_Channel_GetSystemObject(this.channelraw, ref ptr);
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
            return FMOD_Channel_GetUserData(this.channelraw, ref userdata);
        }

        public RESULT getVolume(ref float volume)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Channel_GetVolume_64(this.channelraw, ref volume);
            }
            return FMOD_Channel_GetVolume_32(this.channelraw, ref volume);
        }

        public RESULT getWaveData(float[] wavearray, int numvalues, int channeloffset)
        {
            return FMOD_Channel_GetWaveData(this.channelraw, wavearray, numvalues, channeloffset);
        }

        public RESULT isPlaying(ref bool isplaying)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Channel_IsPlaying_64(this.channelraw, ref isplaying);
            }
            return FMOD_Channel_IsPlaying_32(this.channelraw, ref isplaying);
        }

        public RESULT isVirtual(ref bool isvirtual)
        {
            return FMOD_Channel_IsVirtual(this.channelraw, ref isvirtual);
        }

        public RESULT set3DAttributes(ref VECTOR pos, ref VECTOR vel)
        {
            return FMOD_Channel_Set3DAttributes(this.channelraw, ref pos, ref vel);
        }

        public RESULT set3DConeOrientation(ref VECTOR orientation)
        {
            return FMOD_Channel_Set3DConeOrientation(this.channelraw, ref orientation);
        }

        public RESULT set3DConeSettings(float insideconeangle, float outsideconeangle, float outsidevolume)
        {
            return FMOD_Channel_Set3DConeSettings(this.channelraw, insideconeangle, outsideconeangle, outsidevolume);
        }

        public RESULT set3DCustomRolloff(ref VECTOR points, int numpoints)
        {
            return FMOD_Channel_Set3DCustomRolloff(this.channelraw, ref points, numpoints);
        }

        public RESULT set3DDopplerLevel(float level)
        {
            return FMOD_Channel_Set3DDopplerLevel(this.channelraw, level);
        }

        public RESULT set3DMinMaxDistance(float mindistance, float maxdistance)
        {
            return FMOD_Channel_Set3DMinMaxDistance(this.channelraw, mindistance, maxdistance);
        }

        public RESULT set3DOcclusion(float directOcclusion, float reverbOcclusion)
        {
            return FMOD_Channel_Set3DOcclusion(this.channelraw, directOcclusion, reverbOcclusion);
        }

        public RESULT set3DPanLevel(float level)
        {
            return FMOD_Channel_Set3DPanLevel(this.channelraw, level);
        }

        public RESULT set3DSpread(float angle)
        {
            return FMOD_Channel_Set3DSpread(this.channelraw, angle);
        }

        public RESULT setCallback(CHANNEL_CALLBACKTYPE type, CHANNEL_CALLBACK callback, int command)
        {
            return FMOD_Channel_SetCallback(this.channelraw, type, callback, command);
        }

        public RESULT setChannelGroup(ChannelGroup channelgroup)
        {
            return FMOD_Channel_SetChannelGroup(this.channelraw, channelgroup.getRaw());
        }

        public RESULT setDelay(DELAYTYPE delaytype, uint delayhi, uint delaylo)
        {
            return FMOD_Channel_SetDelay(this.channelraw, delaytype, delayhi, delaylo);
        }

        public RESULT setFrequency(float frequency)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Channel_SetFrequency_64(this.channelraw, frequency);
            }
            return FMOD_Channel_SetFrequency_32(this.channelraw, frequency);
        }

        public RESULT setInputChannelMix(ref float levels, int numlevels)
        {
            return FMOD_Channel_SetInputChannelMix(this.channelraw, ref levels, numlevels);
        }

        public RESULT setLoopCount(int loopcount)
        {
            return FMOD_Channel_SetLoopCount(this.channelraw, loopcount);
        }

        public RESULT setLoopPoints(uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype)
        {
            return FMOD_Channel_SetLoopPoints(this.channelraw, loopstart, loopstarttype, loopend, loopendtype);
        }

        public RESULT setMode(MODE mode)
        {
            return FMOD_Channel_SetMode(this.channelraw, mode);
        }

        public RESULT setMute(bool mute)
        {
            return FMOD_Channel_SetMute(this.channelraw, mute ? 1 : 0);
        }

        public RESULT setPan(float pan)
        {
            return FMOD_Channel_SetPan(this.channelraw, pan);
        }

        public RESULT setPaused(bool paused)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Channel_SetPaused_64(this.channelraw, paused ? 1 : 0);
            }
            return FMOD_Channel_SetPaused_32(this.channelraw, paused ? 1 : 0);
        }

        public RESULT setPosition(uint position, TIMEUNIT postype)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Channel_SetPosition_64(this.channelraw, position, postype);
            }
            return FMOD_Channel_SetPosition_32(this.channelraw, position, postype);
        }

        public RESULT setPriority(int priority)
        {
            return FMOD_Channel_SetPriority(this.channelraw, priority);
        }

        public void setRaw(IntPtr channel)
        {
            this.channelraw = new IntPtr();
            this.channelraw = channel;
        }

        public RESULT setReverbProperties(ref REVERB_CHANNELPROPERTIES prop)
        {
            return FMOD_Channel_SetReverbProperties(this.channelraw, ref prop);
        }

        public RESULT setSpeakerLevels(SPEAKER speaker, float[] levels, int numlevels)
        {
            return FMOD_Channel_SetSpeakerLevels(this.channelraw, speaker, levels, numlevels);
        }

        public RESULT setSpeakerMix(float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright)
        {
            return FMOD_Channel_SetSpeakerMix(this.channelraw, frontleft, frontright, center, lfe, backleft, backright, sideleft, sideright);
        }

        public RESULT setUserData(IntPtr userdata)
        {
            return FMOD_Channel_SetUserData(this.channelraw, userdata);
        }

        public RESULT setVolume(float volume)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Channel_SetVolume_64(this.channelraw, volume);
            }
            return FMOD_Channel_SetVolume_32(this.channelraw, volume);
        }

        public RESULT stop()
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Channel_Stop_64(this.channelraw);
            }
            return FMOD_Channel_Stop_32(this.channelraw);
        }
    }
}

