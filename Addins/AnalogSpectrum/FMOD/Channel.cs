using System;
using System.Runtime.InteropServices;

namespace FMOD
{
    public class Channel
    {
        private IntPtr _channelRaw;

        public RESULT AddDsp(DSP dsp)
        {
            return FMOD_Channel_AddDSP(_channelRaw, dsp.getRaw());
        }

        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_AddDSP(IntPtr channel, IntPtr dsp);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_Get3DAttributes(IntPtr channel, ref VECTOR pos, ref VECTOR vel);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_Get3DConeOrientation(IntPtr channel, ref VECTOR orientation);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_Get3DConeSettings(IntPtr channel, ref float insideconeangle, ref float outsideconeangle, ref float outsidevolume);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_Get3DCustomRolloff(IntPtr channel, ref IntPtr points, ref int numpoints);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_Get3DMinMaxDistance(IntPtr channel, ref float mindistance, ref float maxdistance);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_Get3DOcclusion(IntPtr channel, ref float directOcclusion, ref float reverbOcclusion);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_Get3DSpread(IntPtr channel, ref float angle);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetAudibility(IntPtr channel, ref float audibility);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetChannelGroup(IntPtr channel, ref IntPtr channelgroup);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetCurrentSound(IntPtr channel, ref IntPtr sound);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetDelay(IntPtr channel, ref uint startdelay, ref uint enddelay);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetDSPHead(IntPtr channel, ref IntPtr dsp);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetFrequency(IntPtr channel, ref float frequency);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetIndex(IntPtr channel, ref int index);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetLoopCount(IntPtr channel, ref int loopcount);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetLoopPoints(IntPtr channel, ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetMode(IntPtr channel, ref MODE mode);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetMute(IntPtr channel, ref bool mute);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetPan(IntPtr channel, ref float pan);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetPaused(IntPtr channel, ref bool paused);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetPosition(IntPtr channel, ref uint position, TIMEUNIT postype);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetPriority(IntPtr channel, ref int priority);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetReverbProperties(IntPtr channel, ref REVERB_CHANNELPROPERTIES prop);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetSpeakerLevels(IntPtr channel, SPEAKER speaker, [MarshalAs(UnmanagedType.LPArray)] float[] levels, int numlevels);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetSpeakerMix(IntPtr channel, ref float frontleft, ref float frontright, ref float center, ref float lfe, ref float backleft, ref float backright, ref float sideleft, ref float sideright);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetSpectrum(IntPtr channel, [MarshalAs(UnmanagedType.LPArray)] float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetSystemObject(IntPtr channel, ref IntPtr system);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetUserData(IntPtr channel, ref IntPtr userdata);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetVolume(IntPtr channel, ref float volume);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_GetWaveData(IntPtr channel, [MarshalAs(UnmanagedType.LPArray)] float[] wavearray, int numvalues, int channeloffset);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_IsPlaying(IntPtr channel, ref bool isplaying);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_IsVirtual(IntPtr channel, ref bool isvirtual);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_Set3DAttributes(IntPtr channel, ref VECTOR pos, ref VECTOR vel);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_Set3DConeOrientation(IntPtr channel, ref VECTOR orientation);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_Set3DConeSettings(IntPtr channel, float insideconeangle, float outsideconeangle, float outsidevolume);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_Set3DCustomRolloff(IntPtr channel, ref VECTOR points, int numpoints);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_Set3DMinMaxDistance(IntPtr channel, float mindistance, float maxdistance);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_Set3DOcclusion(IntPtr channel, float directOcclusion, float reverbOcclusion);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_Set3DSpread(IntPtr channel, float angle);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetCallback(IntPtr channel, CHANNEL_CALLBACKTYPE type, CHANNEL_CALLBACK callback, int command);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetChannelGroup(IntPtr channel, IntPtr channelgroup);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetDelay(IntPtr channel, uint startdelay, uint enddelay);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetFrequency(IntPtr channel, float frequency);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetLoopCount(IntPtr channel, int loopcount);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetLoopPoints(IntPtr channel, uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetMode(IntPtr channel, MODE mode);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetMute(IntPtr channel, bool mute);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetPan(IntPtr channel, float pan);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetPaused(IntPtr channel, bool paused);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetPosition(IntPtr channel, uint position, TIMEUNIT postype);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetPriority(IntPtr channel, int priority);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetReverbProperties(IntPtr channel, ref REVERB_CHANNELPROPERTIES prop);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetSpeakerLevels(IntPtr channel, SPEAKER speaker, float[] levels, int numlevels);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetSpeakerMix(IntPtr channel, float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetUserData(IntPtr channel, IntPtr userdata);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_SetVolume(IntPtr channel, float volume);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_Channel_Stop(IntPtr channel);
        public RESULT Get3DAttributes(ref VECTOR pos, ref VECTOR vel)
        {
            return FMOD_Channel_Get3DAttributes(_channelRaw, ref pos, ref vel);
        }

        public RESULT get3DConeOrientation(ref VECTOR orientation)
        {
            return FMOD_Channel_Get3DConeOrientation(this._channelRaw, ref orientation);
        }

        public RESULT get3DConeSettings(ref float insideconeangle, ref float outsideconeangle, ref float outsidevolume)
        {
            return FMOD_Channel_Get3DConeSettings(this._channelRaw, ref insideconeangle, ref outsideconeangle, ref outsidevolume);
        }

        public RESULT get3DCustomRolloff(ref IntPtr points, ref int numpoints)
        {
            return FMOD_Channel_Get3DCustomRolloff(this._channelRaw, ref points, ref numpoints);
        }

        public RESULT get3DMinMaxDistance(ref float mindistance, ref float maxdistance)
        {
            return FMOD_Channel_Get3DMinMaxDistance(this._channelRaw, ref mindistance, ref maxdistance);
        }

        public RESULT get3DOcclusion(ref float directOcclusion, ref float reverbOcclusion)
        {
            return FMOD_Channel_Get3DOcclusion(this._channelRaw, ref directOcclusion, ref reverbOcclusion);
        }

        public RESULT get3DSpread(ref float angle)
        {
            return FMOD_Channel_Get3DSpread(this._channelRaw, ref angle);
        }

        public RESULT getAudibility(ref float audibility)
        {
            return FMOD_Channel_GetAudibility(this._channelRaw, ref audibility);
        }

        public RESULT getChannelGroup(ref ChannelGroup channelgroup)
        {
            RESULT oK;
            var ptr = new IntPtr();
	        try
            {
                oK = FMOD_Channel_GetChannelGroup(this._channelRaw, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (channelgroup == null)
                {
                    var group = new ChannelGroup();
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

        public RESULT GetCurrentSound(ref Sound sound)
        {
            RESULT oK;
            var ptr = new IntPtr();
	        try
            {
                oK = FMOD_Channel_GetCurrentSound(this._channelRaw, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (sound == null)
                {
                    var sound2 = new Sound();
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

        public RESULT GetDelay(ref uint startdelay, ref uint enddelay)
        {
            return FMOD_Channel_GetDelay(this._channelRaw, ref startdelay, ref enddelay);
        }

        public RESULT getDSPHead(ref DSP dsp)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSP dsp2 = null;
            try
            {
                oK = FMOD_Channel_GetDSPHead(this._channelRaw, ref ptr);
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
            return FMOD_Channel_GetFrequency(this._channelRaw, ref frequency);
        }

        public RESULT getIndex(ref int index)
        {
            return FMOD_Channel_GetIndex(this._channelRaw, ref index);
        }

        public RESULT getLoopCount(ref int loopcount)
        {
            return FMOD_Channel_GetLoopCount(this._channelRaw, ref loopcount);
        }

        public RESULT getLoopPoints(ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype)
        {
            return FMOD_Channel_GetLoopPoints(this._channelRaw, ref loopstart, loopstarttype, ref loopend, loopendtype);
        }

        public RESULT getMode(ref MODE mode)
        {
            return FMOD_Channel_GetMode(this._channelRaw, ref mode);
        }

        public RESULT getMute(ref bool mute)
        {
            return FMOD_Channel_GetMute(this._channelRaw, ref mute);
        }

        public RESULT getPan(ref float pan)
        {
            return FMOD_Channel_GetPan(this._channelRaw, ref pan);
        }

        public RESULT getPaused(ref bool paused)
        {
            return FMOD_Channel_GetPaused(this._channelRaw, ref paused);
        }

        public RESULT getPosition(ref uint position, TIMEUNIT postype)
        {
            return FMOD_Channel_GetPosition(this._channelRaw, ref position, postype);
        }

        public RESULT getPriority(ref int priority)
        {
            return FMOD_Channel_GetPriority(this._channelRaw, ref priority);
        }

        public IntPtr getRaw()
        {
            return this._channelRaw;
        }

        public RESULT getReverbProperties(ref REVERB_CHANNELPROPERTIES prop)
        {
            return FMOD_Channel_GetReverbProperties(this._channelRaw, ref prop);
        }

        public RESULT getSpeakerLevels(SPEAKER speaker, float[] levels, int numlevels)
        {
            return FMOD_Channel_GetSpeakerLevels(this._channelRaw, speaker, levels, numlevels);
        }

        public RESULT getSpeakerMix(ref float frontleft, ref float frontright, ref float center, ref float lfe, ref float backleft, ref float backright, ref float sideleft, ref float sideright)
        {
            return FMOD_Channel_GetSpeakerMix(this._channelRaw, ref frontleft, ref frontright, ref center, ref lfe, ref backleft, ref backright, ref sideleft, ref sideright);
        }

        public RESULT getSpectrum(float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype)
        {
            return FMOD_Channel_GetSpectrum(this._channelRaw, spectrumarray, numvalues, channeloffset, windowtype);
        }

        public RESULT getSystemObject(ref System system)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            System system2 = null;
            try
            {
                oK = FMOD_Channel_GetSystemObject(this._channelRaw, ref ptr);
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
            return FMOD_Channel_GetUserData(this._channelRaw, ref userdata);
        }

        public RESULT getVolume(ref float volume)
        {
            return FMOD_Channel_GetVolume(this._channelRaw, ref volume);
        }

        public RESULT getWaveData(float[] wavearray, int numvalues, int channeloffset)
        {
            return FMOD_Channel_GetWaveData(this._channelRaw, wavearray, numvalues, channeloffset);
        }

        public RESULT isPlaying(ref bool isplaying)
        {
            return FMOD_Channel_IsPlaying(this._channelRaw, ref isplaying);
        }

        public RESULT isVirtual(ref bool isvirtual)
        {
            return FMOD_Channel_IsVirtual(this._channelRaw, ref isvirtual);
        }

        public RESULT set3DAttributes(ref VECTOR pos, ref VECTOR vel)
        {
            return FMOD_Channel_Set3DAttributes(this._channelRaw, ref pos, ref vel);
        }

        public RESULT set3DConeOrientation(ref VECTOR orientation)
        {
            return FMOD_Channel_Set3DConeOrientation(this._channelRaw, ref orientation);
        }

        public RESULT set3DConeSettings(float insideconeangle, float outsideconeangle, float outsidevolume)
        {
            return FMOD_Channel_Set3DConeSettings(this._channelRaw, insideconeangle, outsideconeangle, outsidevolume);
        }

        public RESULT set3DCustomRolloff(ref VECTOR points, int numpoints)
        {
            return FMOD_Channel_Set3DCustomRolloff(this._channelRaw, ref points, numpoints);
        }

        public RESULT set3DMinMaxDistance(float mindistance, float maxdistance)
        {
            return FMOD_Channel_Set3DMinMaxDistance(this._channelRaw, mindistance, maxdistance);
        }

        public RESULT set3DOcclusion(float directOcclusion, float reverbOcclusion)
        {
            return FMOD_Channel_Set3DOcclusion(this._channelRaw, directOcclusion, reverbOcclusion);
        }

        public RESULT set3DSpread(float angle)
        {
            return FMOD_Channel_Set3DSpread(this._channelRaw, angle);
        }

        public RESULT setCallback(CHANNEL_CALLBACKTYPE type, CHANNEL_CALLBACK callback, int command)
        {
            return FMOD_Channel_SetCallback(this._channelRaw, type, callback, command);
        }

        public RESULT setChannelGroup(ChannelGroup channelgroup)
        {
            return FMOD_Channel_SetChannelGroup(this._channelRaw, channelgroup.getRaw());
        }

        public RESULT setDelay(uint startdelay, uint enddelay)
        {
            return FMOD_Channel_SetDelay(this._channelRaw, startdelay, enddelay);
        }

        public RESULT setFrequency(float frequency)
        {
            return FMOD_Channel_SetFrequency(this._channelRaw, frequency);
        }

        public RESULT setLoopCount(int loopcount)
        {
            return FMOD_Channel_SetLoopCount(this._channelRaw, loopcount);
        }

        public RESULT setLoopPoints(uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype)
        {
            return FMOD_Channel_SetLoopPoints(this._channelRaw, loopstart, loopstarttype, loopend, loopendtype);
        }

        public RESULT setMode(MODE mode)
        {
            return FMOD_Channel_SetMode(this._channelRaw, mode);
        }

        public RESULT setMute(bool mute)
        {
            return FMOD_Channel_SetMute(this._channelRaw, mute);
        }

        public RESULT setPan(float pan)
        {
            return FMOD_Channel_SetPan(this._channelRaw, pan);
        }

        public RESULT setPaused(bool paused)
        {
            return FMOD_Channel_SetPaused(this._channelRaw, paused);
        }

        public RESULT setPosition(uint position, TIMEUNIT postype)
        {
            return FMOD_Channel_SetPosition(this._channelRaw, position, postype);
        }

        public RESULT setPriority(int priority)
        {
            return FMOD_Channel_SetPriority(this._channelRaw, priority);
        }

        public void setRaw(IntPtr channel)
        {
            this._channelRaw = new IntPtr();
            this._channelRaw = channel;
        }

        public RESULT setReverbProperties(ref REVERB_CHANNELPROPERTIES prop)
        {
            return FMOD_Channel_SetReverbProperties(this._channelRaw, ref prop);
        }

        public RESULT setSpeakerLevels(SPEAKER speaker, float[] levels, int numlevels)
        {
            return FMOD_Channel_SetSpeakerLevels(this._channelRaw, speaker, levels, numlevels);
        }

        public RESULT setSpeakerMix(float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright)
        {
            return FMOD_Channel_SetSpeakerMix(this._channelRaw, frontleft, frontright, center, lfe, backleft, backright, sideleft, sideright);
        }

        public RESULT setUserData(IntPtr userdata)
        {
            return FMOD_Channel_SetUserData(this._channelRaw, userdata);
        }

        public RESULT setVolume(float volume)
        {
            return FMOD_Channel_SetVolume(this._channelRaw, volume);
        }

        public RESULT stop()
        {
            return FMOD_Channel_Stop(this._channelRaw);
        }
    }
}

