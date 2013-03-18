namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class Sound
    {
        private IntPtr soundraw;

        public RESULT addSyncPoint(int offset, TIMEUNIT offsettype, string name, ref IntPtr point)
        {
            return FMOD_Sound_AddSyncPoint(this.soundraw, offset, offsettype, name, ref point);
        }

        public RESULT deleteSyncPoint(IntPtr point)
        {
            return FMOD_Sound_DeleteSyncPoint(this.soundraw, point);
        }

        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_AddSyncPoint(IntPtr sound, int offset, TIMEUNIT offsettype, string name, ref IntPtr point);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_DeleteSyncPoint(IntPtr sound, IntPtr point);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_Get3DConeSettings(IntPtr sound, ref float insideconeangle, ref float outsideconeangle, ref float outsidevolume);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_Get3DCustomRolloff(IntPtr sound, ref IntPtr points, ref int numpoints);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_Get3DMinMaxDistance(IntPtr sound, ref float min, ref float max);
        [DllImport("fmodex", EntryPoint="FMOD_Sound_GetDefaults")]
        private static extern RESULT FMOD_Sound_GetDefaults_32(IntPtr sound, ref float frequency, ref float volume, ref float pan, ref int priority);
        [DllImport("fmodex64", EntryPoint="FMOD_Sound_GetDefaults")]
        private static extern RESULT FMOD_Sound_GetDefaults_64(IntPtr sound, ref float frequency, ref float volume, ref float pan, ref int priority);
        [DllImport("fmodex", EntryPoint="FMOD_Sound_GetFormat")]
        private static extern RESULT FMOD_Sound_GetFormat_32(IntPtr sound, ref SOUND_TYPE type, ref SOUND_FORMAT format, ref int channels, ref int bits);
        [DllImport("fmodex64", EntryPoint="FMOD_Sound_GetFormat")]
        private static extern RESULT FMOD_Sound_GetFormat_64(IntPtr sound, ref SOUND_TYPE type, ref SOUND_FORMAT format, ref int channels, ref int bits);
        [DllImport("fmodex", EntryPoint="FMOD_Sound_GetLength")]
        private static extern RESULT FMOD_Sound_GetLength_32(IntPtr sound, ref uint length, TIMEUNIT lengthtype);
        [DllImport("fmodex64", EntryPoint="FMOD_Sound_GetLength")]
        private static extern RESULT FMOD_Sound_GetLength_64(IntPtr sound, ref uint length, TIMEUNIT lengthtype);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetLoopCount(IntPtr sound, ref int loopcount);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetLoopPoints(IntPtr sound, ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetMode(IntPtr sound, ref MODE mode);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetMusicChannelVolume(IntPtr sound, int channel, ref float volume);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetMusicNumChannels(IntPtr sound, ref int numchannels);
        [DllImport("fmodex", EntryPoint="FMOD_Sound_GetName")]
        private static extern RESULT FMOD_Sound_GetName_32(IntPtr sound, StringBuilder name, int namelen);
        [DllImport("fmodex64", EntryPoint="FMOD_Sound_GetName")]
        private static extern RESULT FMOD_Sound_GetName_64(IntPtr sound, StringBuilder name, int namelen);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetNumSubSounds(IntPtr sound, ref int numsubsounds);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetNumSyncPoints(IntPtr sound, ref int numsyncpoints);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetNumTags(IntPtr sound, ref int numtags, ref int numtagsupdated);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetOpenState(IntPtr sound, ref OPENSTATE openstate, ref uint percentbuffered, ref bool starving);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetSoundGroup(IntPtr sound, ref IntPtr soundgroup);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetSubSound(IntPtr sound, int index, ref IntPtr subsound);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetSyncPoint(IntPtr sound, int index, ref IntPtr point);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetSyncPointInfo(IntPtr sound, IntPtr point, StringBuilder name, int namelen, ref uint offset, TIMEUNIT offsettype);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetSystemObject(IntPtr sound, ref IntPtr system);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetTag(IntPtr sound, string name, int index, IntPtr tag);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetUserData(IntPtr sound, ref IntPtr userdata);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_GetVariations(IntPtr sound, ref float frequencyvar, ref float volumevar, ref float panvar);
        [DllImport("fmodex", EntryPoint="FMOD_Sound_Lock")]
        private static extern RESULT FMOD_Sound_Lock_32(IntPtr sound, uint offset, uint length, ref IntPtr ptr1, ref IntPtr ptr2, ref uint len1, ref uint len2);
        [DllImport("fmodex64", EntryPoint="FMOD_Sound_Lock")]
        private static extern RESULT FMOD_Sound_Lock_64(IntPtr sound, uint offset, uint length, ref IntPtr ptr1, ref IntPtr ptr2, ref uint len1, ref uint len2);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_ReadData(IntPtr sound, IntPtr buffer, uint lenbytes, ref uint read);
        [DllImport("fmodex", EntryPoint="FMOD_Sound_Release")]
        private static extern RESULT FMOD_Sound_Release_32(IntPtr sound);
        [DllImport("fmodex64", EntryPoint="FMOD_Sound_Release")]
        private static extern RESULT FMOD_Sound_Release_64(IntPtr sound);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_SeekData(IntPtr sound, uint pcm);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_Set3DConeSettings(IntPtr sound, float insideconeangle, float outsideconeangle, float outsidevolume);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_Set3DCustomRolloff(IntPtr sound, ref VECTOR points, int numpoints);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_Set3DMinMaxDistance(IntPtr sound, float min, float max);
        [DllImport("fmodex", EntryPoint="FMOD_Sound_SetDefaults")]
        private static extern RESULT FMOD_Sound_SetDefaults_32(IntPtr sound, float frequency, float volume, float pan, int priority);
        [DllImport("fmodex64", EntryPoint="FMOD_Sound_SetDefaults")]
        private static extern RESULT FMOD_Sound_SetDefaults_64(IntPtr sound, float frequency, float volume, float pan, int priority);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_SetLoopCount(IntPtr sound, int loopcount);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_SetLoopPoints(IntPtr sound, uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_SetMode(IntPtr sound, MODE mode);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_SetMusicChannelVolume(IntPtr sound, int channel, float volume);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_SetSoundGroup(IntPtr sound, IntPtr soundgroup);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_SetSubSound(IntPtr sound, int index, IntPtr subsound);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_SetSubSoundSentence(IntPtr sound, int[] subsoundlist, int numsubsounds);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_SetUserData(IntPtr sound, IntPtr userdata);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Sound_SetVariations(IntPtr sound, float frequencyvar, float volumevar, float panvar);
        [DllImport("fmodex", EntryPoint="FMOD_Sound_Unlock")]
        private static extern RESULT FMOD_Sound_Unlock_32(IntPtr sound, IntPtr ptr1, IntPtr ptr2, uint len1, uint len2);
        [DllImport("fmodex64", EntryPoint="FMOD_Sound_Unlock")]
        private static extern RESULT FMOD_Sound_Unlock_64(IntPtr sound, IntPtr ptr1, IntPtr ptr2, uint len1, uint len2);
        public RESULT get3DConeSettings(ref float insideconeangle, ref float outsideconeangle, ref float outsidevolume)
        {
            return FMOD_Sound_Get3DConeSettings(this.soundraw, ref insideconeangle, ref outsideconeangle, ref outsidevolume);
        }

        public RESULT get3DCustomRolloff(ref IntPtr points, ref int numpoints)
        {
            return FMOD_Sound_Get3DCustomRolloff(this.soundraw, ref points, ref numpoints);
        }

        public RESULT get3DMinMaxDistance(ref float min, ref float max)
        {
            return FMOD_Sound_Get3DMinMaxDistance(this.soundraw, ref min, ref max);
        }

        public RESULT getDefaults(ref float frequency, ref float volume, ref float pan, ref int priority)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Sound_GetDefaults_64(this.soundraw, ref frequency, ref volume, ref pan, ref priority);
            }
            return FMOD_Sound_GetDefaults_32(this.soundraw, ref frequency, ref volume, ref pan, ref priority);
        }

        public RESULT getFormat(ref SOUND_TYPE type, ref SOUND_FORMAT format, ref int channels, ref int bits)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Sound_GetFormat_64(this.soundraw, ref type, ref format, ref channels, ref bits);
            }
            return FMOD_Sound_GetFormat_32(this.soundraw, ref type, ref format, ref channels, ref bits);
        }

        public RESULT getLength(ref uint length, TIMEUNIT lengthtype)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Sound_GetLength_64(this.soundraw, ref length, lengthtype);
            }
            return FMOD_Sound_GetLength_32(this.soundraw, ref length, lengthtype);
        }

        public RESULT getLoopCount(ref int loopcount)
        {
            return FMOD_Sound_GetLoopCount(this.soundraw, ref loopcount);
        }

        public RESULT getLoopPoints(ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype)
        {
            return FMOD_Sound_GetLoopPoints(this.soundraw, ref loopstart, loopstarttype, ref loopend, loopendtype);
        }

        public RESULT getMode(ref MODE mode)
        {
            return FMOD_Sound_GetMode(this.soundraw, ref mode);
        }

        public RESULT getMusicChannelVolume(int channel, ref float volume)
        {
            return FMOD_Sound_GetMusicChannelVolume(this.soundraw, channel, ref volume);
        }

        public RESULT getMusicNumChannels(ref int numchannels)
        {
            return FMOD_Sound_GetMusicNumChannels(this.soundraw, ref numchannels);
        }

        public RESULT getName(StringBuilder name, int namelen)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Sound_GetName_64(this.soundraw, name, namelen);
            }
            return FMOD_Sound_GetName_32(this.soundraw, name, namelen);
        }

        public RESULT getNumSubSounds(ref int numsubsounds)
        {
            return FMOD_Sound_GetNumSubSounds(this.soundraw, ref numsubsounds);
        }

        public RESULT getNumSyncPoints(ref int numsyncpoints)
        {
            return FMOD_Sound_GetNumSyncPoints(this.soundraw, ref numsyncpoints);
        }

        public RESULT getNumTags(ref int numtags, ref int numtagsupdated)
        {
            return FMOD_Sound_GetNumTags(this.soundraw, ref numtags, ref numtagsupdated);
        }

        public RESULT getOpenState(ref OPENSTATE openstate, ref uint percentbuffered, ref bool starving)
        {
            return FMOD_Sound_GetOpenState(this.soundraw, ref openstate, ref percentbuffered, ref starving);
        }

        public IntPtr getRaw()
        {
            return this.soundraw;
        }

        public RESULT getSoundGroup(ref SoundGroup soundgroup)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            SoundGroup group = null;
            try
            {
                oK = FMOD_Sound_GetSoundGroup(this.soundraw, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (soundgroup == null)
                {
                    group = new SoundGroup();
                    group.setRaw(ptr);
                    soundgroup = group;
                }
                else
                {
                    soundgroup.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT getSubSound(int index, ref Sound subsound)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            Sound sound = null;
            try
            {
                oK = FMOD_Sound_GetSubSound(this.soundraw, index, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (subsound == null)
                {
                    sound = new Sound();
                    sound.setRaw(ptr);
                    subsound = sound;
                }
                else
                {
                    subsound.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT getSyncPoint(int index, ref IntPtr point)
        {
            return FMOD_Sound_GetSyncPoint(this.soundraw, index, ref point);
        }

        public RESULT getSyncPointInfo(IntPtr point, StringBuilder name, int namelen, ref uint offset, TIMEUNIT offsettype)
        {
            return FMOD_Sound_GetSyncPointInfo(this.soundraw, point, name, namelen, ref offset, offsettype);
        }

        public RESULT getSystemObject(ref _System system)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            _System system2 = null;
            try
            {
                oK = FMOD_Sound_GetSystemObject(this.soundraw, ref ptr);
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

        public RESULT getTag(string name, int index, ref TAG tag)
        {
            IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf((TAG) tag));
            RESULT result = FMOD_Sound_GetTag(this.soundraw, name, index, ptr);
            if (result == RESULT.OK)
            {
                tag = (TAG) Marshal.PtrToStructure(ptr, typeof(TAG));
            }
            return result;
        }

        public RESULT getUserData(ref IntPtr userdata)
        {
            return FMOD_Sound_GetUserData(this.soundraw, ref userdata);
        }

        public RESULT getVariations(ref float frequencyvar, ref float volumevar, ref float panvar)
        {
            return FMOD_Sound_GetVariations(this.soundraw, ref frequencyvar, ref volumevar, ref panvar);
        }

        public RESULT @lock(uint offset, uint length, ref IntPtr ptr1, ref IntPtr ptr2, ref uint len1, ref uint len2)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Sound_Lock_64(this.soundraw, offset, length, ref ptr1, ref ptr2, ref len1, ref len2);
            }
            return FMOD_Sound_Lock_32(this.soundraw, offset, length, ref ptr1, ref ptr2, ref len1, ref len2);
        }

        public RESULT readData(IntPtr buffer, uint lenbytes, ref uint read)
        {
            return FMOD_Sound_ReadData(this.soundraw, buffer, lenbytes, ref read);
        }

        public RESULT release()
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Sound_Release_64(this.soundraw);
            }
            return FMOD_Sound_Release_32(this.soundraw);
        }

        public RESULT seekData(uint pcm)
        {
            return FMOD_Sound_SeekData(this.soundraw, pcm);
        }

        public RESULT set3DConeSettings(float insideconeangle, float outsideconeangle, float outsidevolume)
        {
            return FMOD_Sound_Set3DConeSettings(this.soundraw, insideconeangle, outsideconeangle, outsidevolume);
        }

        public RESULT set3DCustomRolloff(ref VECTOR points, int numpoints)
        {
            return FMOD_Sound_Set3DCustomRolloff(this.soundraw, ref points, numpoints);
        }

        public RESULT set3DMinMaxDistance(float min, float max)
        {
            return FMOD_Sound_Set3DMinMaxDistance(this.soundraw, min, max);
        }

        public RESULT setDefaults(float frequency, float volume, float pan, int priority)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Sound_SetDefaults_64(this.soundraw, frequency, volume, pan, priority);
            }
            return FMOD_Sound_SetDefaults_32(this.soundraw, frequency, volume, pan, priority);
        }

        public RESULT setLoopCount(int loopcount)
        {
            return FMOD_Sound_SetLoopCount(this.soundraw, loopcount);
        }

        public RESULT setLoopPoints(uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype)
        {
            return FMOD_Sound_SetLoopPoints(this.soundraw, loopstart, loopstarttype, loopend, loopendtype);
        }

        public RESULT setMode(MODE mode)
        {
            return FMOD_Sound_SetMode(this.soundraw, mode);
        }

        public RESULT setMusicChannelVolume(int channel, float volume)
        {
            return FMOD_Sound_SetMusicChannelVolume(this.soundraw, channel, volume);
        }

        public void setRaw(IntPtr sound)
        {
            this.soundraw = new IntPtr();
            this.soundraw = sound;
        }

        public RESULT setSoundGroup(SoundGroup soundgroup)
        {
            return FMOD_Sound_SetSoundGroup(this.soundraw, soundgroup.getRaw());
        }

        public RESULT setSubSound(int index, Sound subsound)
        {
            IntPtr ptr = subsound.getRaw();
            return FMOD_Sound_SetSubSound(this.soundraw, index, ptr);
        }

        public RESULT setSubSoundSentence(int[] subsoundlist, int numsubsounds)
        {
            return FMOD_Sound_SetSubSoundSentence(this.soundraw, subsoundlist, numsubsounds);
        }

        public RESULT setUserData(IntPtr userdata)
        {
            return FMOD_Sound_SetUserData(this.soundraw, userdata);
        }

        public RESULT setVariations(float frequencyvar, float volumevar, float panvar)
        {
            return FMOD_Sound_SetVariations(this.soundraw, frequencyvar, volumevar, panvar);
        }

        public RESULT unlock(IntPtr ptr1, IntPtr ptr2, uint len1, uint len2)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_Sound_Unlock_64(this.soundraw, ptr1, ptr2, len1, len2);
            }
            return FMOD_Sound_Unlock_32(this.soundraw, ptr1, ptr2, len1, len2);
        }
    }
}

